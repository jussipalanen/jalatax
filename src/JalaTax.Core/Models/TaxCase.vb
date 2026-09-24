Imports System.Text.Json.Serialization

Namespace Models

    ''' <summary>
    ''' Input for one fictional tax calculation.
    ''' </summary>
    Public Class TaxCase

        <JsonRequired>
        Public Property TaxpayerId As String = String.Empty

        <JsonRequired>
        Public Property AnnualIncome As Decimal

        <JsonRequired>
        Public Property Deductions As Decimal

    End Class

End Namespace
