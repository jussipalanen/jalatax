Imports System.IO
Imports System.Text
Imports JalaTax.Core
Imports JalaTax.Core.Audit
Imports JalaTax.Core.Configuration
Imports JalaTax.Core.Services

''' <summary>
''' Command-line runner: processes every case in the example input (or the file given as the
''' first argument) and prints validation, rules, result and a summary.
''' </summary>
Friend Module Program

    Private Const Separator As String = "----------------------------------------"

    Private Enum ExitCode
        Success = 0
        ConfigurationError = 1
        InputError = 2
        UnexpectedError = 3
    End Enum

    Public Function Main(args As String()) As Integer
        Console.OutputEncoding = Encoding.UTF8
        Console.WriteLine($"{ApplicationInfo.Name} {ApplicationInfo.Version}")
        Console.WriteLine(Separator)

        Try
            Dim rulesPath = DataPath("rules.json")
            Dim casesPath = If(args.Length > 0, args(0), DataPath("example-taxpayer.json"))

            Dim service As New TaxCalculationService(New TaxRuleConfigurationLoader().Load(rulesPath))
            Dim outcomes = TaxCaseLoader.Load(casesPath).Select(Function(taxCase) service.Calculate(taxCase)).ToList()

            For Each outcome In outcomes
                PrintOutcome(outcome)
            Next

            Dim calculated = outcomes.Where(Function(outcome) outcome.IsSuccess).Count()
            Console.WriteLine()
            Console.WriteLine(Separator)
            Console.WriteLine($"Processed {outcomes.Count} cases: {calculated} calculated, {outcomes.Count - calculated} rejected.")
            Console.WriteLine("Demo calculation completed.")
            Return ExitCode.Success
        Catch ex As ConfigurationException
            PrintError("Configuration error", ex.Message)
            Return ExitCode.ConfigurationError
        Catch ex As InputDataException
            PrintError("Input error", ex.Message)
            Return ExitCode.InputError
        Catch ex As Exception
            ' Application boundary: anything unexpected is reported instead of crashing with a stack trace.
            PrintError("Unexpected error", ex.Message)
            Return ExitCode.UnexpectedError
        End Try
    End Function

    Private Sub PrintOutcome(outcome As CalculationOutcome)
        Console.WriteLine()
        Console.WriteLine($"Processing case: {outcome.TaxCase.TaxpayerId}")

        Console.WriteLine()
        Console.WriteLine("Validation")
        If outcome.Validation.IsValid Then
            PrintEntries(outcome, AuditEventType.ValidationPassed, "✓")
        Else
            For Each problem In outcome.Validation.Errors
                Console.WriteLine($"✗ {problem.Message}")
            Next
            Console.WriteLine()
            Console.WriteLine("Case rejected. No tax calculated.")
            Return
        End If

        Console.WriteLine()
        Console.WriteLine("Rules")
        PrintEntries(outcome, AuditEventType.RuleApplied, "✓")

        Dim result = outcome.Result
        Console.WriteLine()
        Console.WriteLine("Result")
        Console.WriteLine(Separator)
        PrintAmount("Annual income:", result.AnnualIncome)
        PrintAmount("Deductions:", result.Deductions)
        PrintAmount("Basic deduction:", result.BasicDeduction)
        PrintAmount("Taxable income:", result.TaxableIncome)
        PrintAmount("Calculated tax:", result.CalculatedTax)
    End Sub

    Private Sub PrintEntries(outcome As CalculationOutcome, eventType As AuditEventType, marker As String)
        For Each entry In outcome.AuditEntries.Where(Function(item) item.EventType = eventType)
            Console.WriteLine($"{marker} {entry.Description}")
        Next
    End Sub

    Private Sub PrintAmount(label As String, value As Decimal)
        Console.WriteLine($"{label,-18}{DisplayFormat.Amount(value),14}")
    End Sub

    Private Sub PrintError(title As String, message As String)
        Console.WriteLine()
        Console.Error.WriteLine($"{title}: {message}")
    End Sub

    Private Function DataPath(fileName As String) As String
        Return Path.Combine(AppContext.BaseDirectory, "data", fileName)
    End Function

End Module
