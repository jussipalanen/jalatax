Imports JalaTax.Core.Localization

''' <summary>
''' Texts of the console runner from Resources/ConsoleText.resx (English) and ConsoleText.fi.resx (Finnish).
''' </summary>
Friend Module ConsoleText

    Private ReadOnly Resources As New TextResources("JalaTax.ConsoleText", GetType(ConsoleText).Assembly)

    Friend Function [Get](name As String, ParamArray args As Object()) As String
        Return Resources.Get(name, args)
    End Function

End Module
