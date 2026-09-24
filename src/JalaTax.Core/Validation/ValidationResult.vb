Namespace Validation

    ''' <summary>
    ''' Collects validation errors so that all problems can be reported at once.
    ''' </summary>
    Public NotInheritable Class ValidationResult

        Private ReadOnly _errors As New List(Of ValidationError)()

        Public ReadOnly Property Errors As IReadOnlyList(Of ValidationError)
            Get
                Return _errors.AsReadOnly()
            End Get
        End Property

        Public ReadOnly Property IsValid As Boolean
            Get
                Return _errors.Count = 0
            End Get
        End Property

        Public Sub AddError(propertyName As String, message As String)
            _errors.Add(New ValidationError(propertyName, message))
        End Sub

    End Class

End Namespace
