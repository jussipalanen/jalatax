Imports System.Windows.Forms

Friend Module Program

    <STAThread>
    Public Sub Main()
        Application.SetHighDpiMode(HighDpiMode.SystemAware)
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        ' Application boundary: unexpected errors are shown to the user instead of closing the app.
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException)
        AddHandler Application.ThreadException,
            Sub(sender, e)
                MessageBox.Show($"An unexpected error occurred:{Environment.NewLine}{e.Exception.Message}",
                                "JalaTax", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Sub

        Application.Run(New MainForm())
    End Sub

End Module
