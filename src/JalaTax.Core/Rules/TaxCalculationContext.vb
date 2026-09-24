Imports JalaTax.Core.Audit
Imports JalaTax.Core.Configuration
Imports JalaTax.Core.Models

Namespace Rules

    ''' <summary>
    ''' Working state for one calculation: the input, the configuration, the values rules produce
    ''' and the audit log rules write to.
    ''' </summary>
    Public NotInheritable Class TaxCalculationContext

        Private ReadOnly _bracketTaxes As New List(Of BracketTax)()

        ''' <param name="auditLog">Log for rule events; a new log using the system clock when omitted.</param>
        Public Sub New(taxCase As TaxCase, configuration As TaxRuleConfiguration, Optional auditLog As AuditLog = Nothing)
            ArgumentNullException.ThrowIfNull(taxCase)
            ArgumentNullException.ThrowIfNull(configuration)

            Me.TaxCase = taxCase
            Me.Configuration = configuration
            Me.AuditLog = If(auditLog, New AuditLog(TimeProvider.System))
        End Sub

        Public ReadOnly Property TaxCase As TaxCase

        Public ReadOnly Property Configuration As TaxRuleConfiguration

        Public ReadOnly Property AuditLog As AuditLog

        Public Property TaxableIncome As Decimal

        Public Property CalculatedTax As Decimal

        ''' <summary>Tax per bracket, in bracket order; only brackets that tax some income are listed.</summary>
        Public ReadOnly Property BracketTaxes As IReadOnlyList(Of BracketTax)
            Get
                Return _bracketTaxes.AsReadOnly()
            End Get
        End Property

        Public Sub AddBracketTax(bracketTax As BracketTax)
            ArgumentNullException.ThrowIfNull(bracketTax)
            _bracketTaxes.Add(bracketTax)
        End Sub

    End Class

End Namespace
