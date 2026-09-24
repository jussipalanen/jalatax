Imports System.Text.Json.Serialization

Namespace Configuration

    ''' <summary>
    ''' Configurable values for the tax rules, loaded from a rules JSON file (for example data/rules.json).
    ''' </summary>
    Public Class TaxRuleConfiguration

        ''' <summary>
        ''' Optional display name of the rule set per language code, for example
        ''' { "fi": "Kuvitteelliset esimerkkisäännöt", "en": "Fictional demo rules" }.
        ''' </summary>
        Public Property Names As Dictionary(Of String, String) = New Dictionary(Of String, String)()

        ''' <summary>Deducted from income in addition to the tax case's own deductions.</summary>
        <JsonRequired>
        Public Property BasicDeduction As Decimal

        ''' <summary>Brackets ordered from lowest to highest.</summary>
        <JsonRequired>
        Public Property TaxBrackets As List(Of TaxBracket) = New List(Of TaxBracket)()

    End Class

End Namespace
