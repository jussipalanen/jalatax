Namespace Validation

    ''' <summary>
    ''' A single validation failure.
    ''' </summary>
    Public NotInheritable Class ValidationError

        Public Sub New(propertyName As String, message As String)
            ArgumentException.ThrowIfNullOrWhiteSpace(propertyName)
            ArgumentException.ThrowIfNullOrWhiteSpace(message)

            Me.PropertyName = propertyName
            Me.Message = message
        End Sub

        ''' <summary>Name of the invalid property, for example "AnnualIncome".</summary>
        Public ReadOnly Property PropertyName As String

        Public ReadOnly Property Message As String

    End Class

End Namespace
