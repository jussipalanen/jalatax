Imports System.Globalization

<TestClass>
Public NotInheritable Class TestAssemblySetup

    Private Sub New()
    End Sub

    ''' <summary>
    ''' Tests expect English texts and formats unless they switch language themselves, whatever
    ''' the language of the machine that runs them.
    ''' </summary>
    <AssemblyInitialize>
    Public Shared Sub UseEnglishByDefault(context As TestContext)
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US")
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US")
        context.WriteLine("Default test language: English (en-US).")
    End Sub

End Class
