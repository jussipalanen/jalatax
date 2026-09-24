Imports System.Text.Json.Serialization

Namespace Configuration

    ''' <summary>
    ''' Configurable values for the fictional tax rules, loaded from rules.json.
    ''' </summary>
    Public Class TaxRuleConfiguration

        ''' <summary>Deducted from income in addition to the tax case's own deductions.</summary>
        <JsonRequired>
        Public Property BasicDeduction As Decimal

        ''' <summary>Brackets ordered from lowest to highest.</summary>
        <JsonRequired>
        Public Property TaxBrackets As List(Of TaxBracket) = New List(Of TaxBracket)()

    End Class

End Namespace
