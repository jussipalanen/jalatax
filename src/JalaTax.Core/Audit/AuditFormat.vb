Imports System.Globalization

Namespace Audit

    ''' <summary>
    ''' Formats numbers in audit descriptions the same way on every machine, regardless of the user's locale.
    ''' </summary>
    Friend Module AuditFormat

        Friend Function Amount(value As Decimal) As String
            Return value.ToString("N2", CultureInfo.InvariantCulture)
        End Function

        Friend Function Rate(value As Decimal) As String
            Return (value * 100D).ToString("0.##", CultureInfo.InvariantCulture) & " %"
        End Function

    End Module

End Namespace
