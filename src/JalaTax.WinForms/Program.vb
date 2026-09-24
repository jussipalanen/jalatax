Imports System.Windows.Forms
Imports JalaTax.Core.Localization

Friend Module Program

    ''' <param name="args">Optional "--lang fi|en" (Finnish is the default) and "--rules file.json".</param>
    <STAThread>
    Public Sub Main(args As String())
        Application.SetHighDpiMode(HighDpiMode.SystemAware)
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        ' Application boundary: unexpected errors are shown to the user instead of closing the app.
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException)
        AddHandler Application.ThreadException,
            Sub(sender, e)
                MessageBox.Show($"{UiText.Get("Error_Unexpected")}{Environment.NewLine}{e.Exception.Message}",
                                "JalaTax", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Sub

        Dim otherArguments = SelectLanguage(args)
        Application.Run(New MainForm() With {.InitialRuleSetFile = ReadRulesOption(otherArguments)})
    End Sub

    ''' <summary>Value of "--rules file.json", for example --rules rules-fi-2026.json; Nothing when absent.</summary>
    Private Function ReadRulesOption(args As IReadOnlyList(Of String)) As String
        Dim index = args.ToList().FindIndex(Function(argument) String.Equals(argument, "--rules", StringComparison.OrdinalIgnoreCase))
        Return If(index >= 0 AndAlso index + 1 < args.Count, args(index + 1), Nothing)
    End Function

    ''' <returns>The arguments other than the --lang option.</returns>
    Private Function SelectLanguage(args As String()) As IReadOnlyList(Of String)
        ' Messages about the --lang option itself are shown in the default language.
        Languages.Use(Languages.DefaultLanguage)

        Dim otherArguments As IReadOnlyList(Of String) = Nothing
        Try
            Languages.Use(Languages.FromCommandLine(args, otherArguments))
            Return otherArguments
        Catch ex As ArgumentException
            MessageBox.Show(UiText.Get("Error_InvalidLanguageOption", ex.Message), "JalaTax", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return Array.Empty(Of String)()
        End Try
    End Function

End Module
