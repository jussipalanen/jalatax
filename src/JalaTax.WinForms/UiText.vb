Imports JalaTax.Core.Localization

''' <summary>
''' Texts of the desktop app from Resources/UiText.resx (English) and UiText.fi.resx (Finnish).
''' </summary>
Friend Module UiText

    Private ReadOnly Resources As New TextResources("JalaTax.WinForms.UiText", GetType(UiText).Assembly)

    Friend Function [Get](name As String, ParamArray args As Object()) As String
        Return Resources.Get(name, args)
    End Function

End Module
