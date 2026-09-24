Imports System.Globalization
Imports JalaTax.Core.Localization

''' <summary>
''' Switches the JalaTax language for one test and restores the previous language afterwards.
''' </summary>
Friend NotInheritable Class LanguageScope
    Implements IDisposable

    Private ReadOnly _previous As CultureInfo = CultureInfo.CurrentUICulture

    Public Sub New(languageCode As String)
        Languages.Use(languageCode)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        CultureInfo.CurrentUICulture = _previous
    End Sub

End Class
