Imports JalaTax.Core.Localization
Namespace Configuration

    ''' <summary>
    ''' Raised when the tax rule configuration cannot be loaded or contains invalid values.
    ''' </summary>
    Public NotInheritable Class ConfigurationException
        Inherits Exception

        Public Sub New()
            MyBase.New(CoreText.Get("Config_Invalid"))
        End Sub

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

        Public Sub New(message As String, errors As IEnumerable(Of String))
            MyBase.New(message)
            ArgumentNullException.ThrowIfNull(errors)
            Me.Errors = errors.ToList().AsReadOnly()
        End Sub

        ''' <summary>Individual problems found in the configuration; empty when the cause is a single failure.</summary>
        Public ReadOnly Property Errors As IReadOnlyList(Of String) = Array.Empty(Of String)()

    End Class

End Namespace
