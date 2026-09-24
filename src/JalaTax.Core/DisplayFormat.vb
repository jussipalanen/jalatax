Imports System.Globalization
Imports JalaTax.Core.Configuration

''' <summary>
''' Formats amounts, rates and bracket ranges the same way in audit entries, the console and the
''' Windows Forms UI, regardless of the user's regional settings.
''' </summary>
Public Module DisplayFormat

    ''' <summary>For example 45,000.00.</summary>
    Public Function Amount(value As Decimal) As String
        Return value.ToString("N2", CultureInfo.InvariantCulture)
    End Function

    ''' <summary>For example 10 % or 12.5 %.</summary>
    Public Function Rate(value As Decimal) As String
        Return (value * 100D).ToString("0.##", CultureInfo.InvariantCulture) & " %"
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
