Imports JalaTax.Core.Audit
Imports JalaTax.Core.Models
Imports JalaTax.Core.Rules
Imports JalaTax.Core.Validation

Namespace Services

    ''' <summary>
    ''' What happened to one tax case: either a result, or the validation errors that stopped the calculation,
    ''' together with the audit trail of every step.
    ''' </summary>
    Public NotInheritable Class CalculationOutcome

        Private Sub New(taxCase As TaxCase,
                        validation As ValidationResult,
                        result As TaxResult,
                        bracketTaxes As IReadOnlyList(Of BracketTax),
                        auditEntries As IReadOnlyList(Of AuditEntry))
            Me.TaxCase = taxCase
            Me.Validation = validation
            Me.Result = result
            Me.BracketTaxes = bracketTaxes
            Me.AuditEntries = auditEntries
        End Sub

        Public ReadOnly Property TaxCase As TaxCase

        Public ReadOnly Property Validation As ValidationResult

        ''' <summary>The calculated result; Nothing when validation failed.</summary>
        Public ReadOnly Property Result As TaxResult

        ''' <summary>Tax per bracket; empty when validation failed.</summary>
        Public ReadOnly Property BracketTaxes As IReadOnlyList(Of BracketTax)

        ''' <summary>Every processing step, in order.</summary>
        Public ReadOnly Property AuditEntries As IReadOnlyList(Of AuditEntry)

        Public ReadOnly Property IsSuccess As Boolean
            Get
                Return Result IsNot Nothing
            End Get
        End Property

        Friend Shared Function Succeeded(taxCase As TaxCase,
                                         validation As ValidationResult,
                                         result As TaxResult,
                                         bracketTaxes As IReadOnlyList(Of BracketTax),
                                         auditEntries As IReadOnlyList(Of AuditEntry)) As CalculationOutcome
            ArgumentNullException.ThrowIfNull(result)
            Return New CalculationOutcome(taxCase, validation, result, bracketTaxes, auditEntries)
        End Function

        Friend Shared Function Rejected(taxCase As TaxCase,
                                        validation As ValidationResult,
                                        auditEntries As IReadOnlyList(Of AuditEntry)) As CalculationOutcome
            Return New CalculationOutcome(taxCase, validation, Nothing, Array.Empty(Of BracketTax)(), auditEntries)
        End Function

    End Class

End Namespace
