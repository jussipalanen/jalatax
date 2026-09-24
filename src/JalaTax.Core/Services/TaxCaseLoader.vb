Imports JalaTax.Core.Localization
Imports JalaTax.Core.Models

Namespace Services

    ''' <summary>
    ''' Loads tax cases from a JSON array. Only the file structure is checked here;
    ''' the values of each case are checked by <see cref="Validation.TaxCaseValidator"/> when the case is processed.
    ''' </summary>
    Public NotInheritable Class TaxCaseLoader

        Private Const DataKey As String = "TaxCase"

        Private Sub New()
        End Sub

        ''' <exception cref="InputDataException">The file is missing, unreadable or not a list of tax cases.</exception>
        Public Shared Function Load(filePath As String) As IReadOnlyList(Of TaxCase)
            Return CheckEntries(JsonFileReader.Read(Of List(Of TaxCase))(filePath, DataKey, AddressOf CreateException))
        End Function

        ''' <exception cref="InputDataException">The JSON is not a list of tax cases.</exception>
        Public Shared Function Parse(json As String) As IReadOnlyList(Of TaxCase)
            Return CheckEntries(JsonFileReader.Parse(Of List(Of TaxCase))(json, DataKey, AddressOf CreateException))
        End Function

        Private Shared Function CheckEntries(taxCases As List(Of TaxCase)) As IReadOnlyList(Of TaxCase)
            If taxCases.Count = 0 Then
                Throw New InputDataException(CoreText.Get("TaxCase_NoCases"))
            End If

            If taxCases.Any(Function(taxCase) taxCase Is Nothing) Then
                Throw New InputDataException(CoreText.Get("TaxCase_EmptyEntries"))
            End If

            Return taxCases.AsReadOnly()
        End Function

        Private Shared Function CreateException(message As String, innerException As Exception) As Exception
            Return New InputDataException(message, innerException)
        End Function

    End Class

End Namespace
