Imports System.Windows.Forms
Imports JalaTax.Core.Localization

Friend Module Program

    ''' <param name="args">Optional "--lang fi|en"; Finnish is the default.</param>
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

        SelectLanguage(args)
        Application.Run(New MainForm())
    End Sub

    Private Sub SelectLanguage(args As String())
        ' Messages about the --lang option itself are shown in the default language.
        Languages.Use(Languages.DefaultLanguage)

        Try
            Dim otherArguments As IReadOnlyList(Of String) = Nothing
            Languages.Use(Languages.FromCommandLine(args, otherArguments))
        Catch ex As ArgumentException
            MessageBox.Show(UiText.Get("Error_InvalidLanguageOption", ex.Message), "JalaTax", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

End Module
