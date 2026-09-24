Imports JalaTax.Core.Audit
Imports JalaTax.Core.Configuration
Imports JalaTax.Core.Localization

''' <summary>
''' Localized names shared by the desktop app and the console runner, so both use the same wording.
''' </summary>
Public Module DisplayText

    Public ReadOnly Property TaxpayerId As String
        Get
            Return CoreText.Get("Label_TaxpayerId")
        End Get
    End Property

    Public ReadOnly Property AnnualIncome As String
        Get
            Return CoreText.Get("Label_AnnualIncome")
        End Get
    End Property

    Public ReadOnly Property Deductions As String
        Get
            Return CoreText.Get("Label_Deductions")
        End Get
    End Property

    Public ReadOnly Property BasicDeduction As String
        Get
            Return CoreText.Get("Label_BasicDeduction")
        End Get
    End Property

    Public ReadOnly Property TaxableIncome As String
        Get
            Return CoreText.Get("Label_TaxableIncome")
        End Get
    End Property

    Public ReadOnly Property CalculatedTax As String
        Get
            Return CoreText.Get("Label_CalculatedTax")
        End Get
    End Property

    ''' <summary>For example "Bracket 0.00–20,000.00 at 10 %".</summary>
    Public Function BracketLabel(taxBracket As TaxBracket) As String
        ArgumentNullException.ThrowIfNull(taxBracket)
        Return CoreText.Get("Label_Bracket", DisplayFormat.BracketRange(taxBracket), DisplayFormat.Rate(taxBracket.Rate))
    End Function

    ''' <summary>A readable name for the event, for example "Case loaded".</summary>
    Public Function EventName(eventType As AuditEventType) As String
        Return CoreText.Get($"Event_{eventType}")
    End Function

End Module
