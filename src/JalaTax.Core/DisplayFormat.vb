Imports JalaTax.Core.Configuration
Imports JalaTax.Core.Localization

''' <summary>
''' Formats amounts, rates and bracket ranges the same way in audit entries, the console and the
''' Windows Forms UI. The format follows the application language (not the user's regional settings):
''' 45,000.00 in English and 45 000,00 in Finnish.
''' </summary>
Public Module DisplayFormat

    ''' <summary>For example 45,000.00 (English) or 45 000,00 (Finnish).</summary>
    Public Function Amount(value As Decimal) As String
        Return value.ToString("N2", Languages.NumberCulture())
    End Function

    ''' <summary>For example 10 % or 12.5 % (English), 12,5 % (Finnish).</summary>
    Public Function Rate(value As Decimal) As String
        Return (value * 100D).ToString("0.##", Languages.NumberCulture()) & " %"
    End Function

    ''' <summary>For example 0.00–20,000.00, or 50,000.00+ for the top bracket.</summary>
    Public Function BracketRange(bracket As TaxBracket) As String
        ArgumentNullException.ThrowIfNull(bracket)

        If bracket.Max.HasValue Then
            Return $"{Amount(bracket.Min)}–{Amount(bracket.Max.Value)}"
        End If

        Return $"{Amount(bracket.Min)}+"
    End Function

End Module
