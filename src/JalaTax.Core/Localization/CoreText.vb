Namespace Localization

    ''' <summary>
    ''' Texts of JalaTax.Core (validation, configuration and audit messages) from Resources/Strings.resx.
    ''' </summary>
    Friend Module CoreText

        Private ReadOnly Resources As New TextResources("JalaTax.Core.Strings", GetType(CoreText).Assembly)

        Friend Function [Get](name As String, ParamArray args As Object()) As String
            Return Resources.Get(name, args)
        End Function

    End Module

End Namespace
