Namespace Services

    ''' <summary>
    ''' Raised when tax case input data cannot be loaded.
    ''' Invalid values inside a case are reported by validation instead.
    ''' </summary>
    Public NotInheritable Class InputDataException
        Inherits Exception

        Public Sub New()
            MyBase.New("The tax case input data is invalid.")
        End Sub

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

    End Class

End Namespace
