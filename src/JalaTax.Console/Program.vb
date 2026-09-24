Imports System.IO
Imports System.Text
Imports JalaTax.Core
Imports JalaTax.Core.Audit
Imports JalaTax.Core.Configuration
Imports JalaTax.Core.Localization
Imports JalaTax.Core.Services

''' <summary>
''' Command-line runner: processes every case in the example input (or the file given as an
''' argument) and prints validation, rules, result and a summary.
''' Usage: JalaTax [--lang fi|en] [cases.json]. Finnish is the default language.
''' </summary>
Friend Module Program

    Private Const Separator As String = "----------------------------------------"

    Private Enum ExitCode
        Success = 0
        ConfigurationError = 1
        InputError = 2
        UnexpectedError = 3
        InvalidArguments = 4
    End Enum

    Public Function Main(args As String()) As Integer
        Console.OutputEncoding = Encoding.UTF8

        ' Messages about the --lang option itself are shown in the default language.
        Languages.Use(Languages.DefaultLanguage)

        Dim otherArguments As IReadOnlyList(Of String) = Nothing
        Try
            Languages.Use(Languages.FromCommandLine(args, otherArguments))
        Catch ex As ArgumentException
            Console.Error.WriteLine(ex.Message)
            Return ExitCode.InvalidArguments
        End Try

        Console.WriteLine($"{ApplicationInfo.Name} {ApplicationInfo.Version}")
        Console.WriteLine(Separator)

        Try
            Dim rulesPath = DataPath("rules.json")
            Dim casesPath = If(otherArguments.Count > 0, otherArguments(0), DataPath("example-taxpayer.json"))

            Dim service As New TaxCalculationService(New TaxRuleConfigurationLoader().Load(rulesPath))
            Dim outcomes = TaxCaseLoader.Load(casesPath).Select(Function(taxCase) service.Calculate(taxCase)).ToList()

            For Each outcome In outcomes
                PrintOutcome(outcome)
            Next

            Dim calculated = outcomes.Where(Function(outcome) outcome.IsSuccess).Count()
            Console.WriteLine()
            Console.WriteLine(Separator)
            Console.WriteLine(ConsoleText.Get("Console_Summary", outcomes.Count, calculated, outcomes.Count - calculated))
            Console.WriteLine(ConsoleText.Get("Console_Completed"))
            Return ExitCode.Success
        Catch ex As ConfigurationException
            PrintError(ConsoleText.Get("Console_ConfigError"), ex.Message)
            Return ExitCode.ConfigurationError
        Catch ex As InputDataException
            PrintError(ConsoleText.Get("Console_InputError"), ex.Message)
            Return ExitCode.InputError
        Catch ex As Exception
            ' Application boundary: anything unexpected is reported instead of crashing with a stack trace.
            PrintError(ConsoleText.Get("Console_UnexpectedError"), ex.Message)
            Return ExitCode.UnexpectedError
        End Try
    End Function

    Private Sub PrintOutcome(outcome As CalculationOutcome)
        Console.WriteLine()
        Console.WriteLine(ConsoleText.Get("Console_ProcessingCase", outcome.TaxCase.TaxpayerId))

        Console.WriteLine()
        Console.WriteLine(ConsoleText.Get("Console_Validation"))
        If outcome.Validation.IsValid Then
            PrintEntries(outcome, AuditEventType.ValidationPassed, "✓")
        Else
            For Each problem In outcome.Validation.Errors
                Console.WriteLine($"✗ {problem.Message}")
            Next
            Console.WriteLine()
            Console.WriteLine(ConsoleText.Get("Console_CaseRejected"))
            Return
        End If

        Console.WriteLine()
        Console.WriteLine(ConsoleText.Get("Console_Rules"))
        PrintEntries(outcome, AuditEventType.RuleApplied, "✓")

        Dim result = outcome.Result
        Console.WriteLine()
        Console.WriteLine(ConsoleText.Get("Console_Result"))
        Console.WriteLine(Separator)
        PrintAmount(DisplayText.AnnualIncome, result.AnnualIncome)
        PrintAmount(DisplayText.Deductions, result.Deductions)
        PrintAmount(DisplayText.BasicDeduction, result.BasicDeduction)
        PrintAmount(DisplayText.TaxableIncome, result.TaxableIncome)
        PrintAmount(DisplayText.CalculatedTax, result.CalculatedTax)
    End Sub

    Private Sub PrintEntries(outcome As CalculationOutcome, eventType As AuditEventType, marker As String)
        For Each entry In outcome.AuditEntries.Where(Function(item) item.EventType = eventType)
            Console.WriteLine($"{marker} {entry.Description}")
        Next
    End Sub

    Private Sub PrintAmount(label As String, value As Decimal)
        Console.WriteLine($"{label & ":",-18}{DisplayFormat.Amount(value),14}")
    End Sub

    Private Sub PrintError(title As String, message As String)
        Console.WriteLine()
        Console.Error.WriteLine($"{title}: {message}")
    End Sub

    Private Function DataPath(fileName As String) As String
        Return Path.Combine(AppContext.BaseDirectory, "data", fileName)
    End Function

End Module
