Imports JalaTax.Core.Models
Imports JalaTax.Core.Rules
Imports JalaTax.Core.Validation

Namespace Services

    ''' <summary>
    ''' What happened to one tax case: either a result, or the validation errors that stopped the calculation.
    ''' </summary>
    Public NotInheritable Class CalculationOutcome

        Private Sub New(taxCase As TaxCase, validation As ValidationResult, result As TaxResult, bracketTaxes As IReadOnlyList(Of BracketTax))
            Me.TaxCase = taxCase
            Me.Validation = validation
            Me.Result = result
            Me.BracketTaxes = bracketTaxes
        End Sub

        Public ReadOnly Property TaxCase As TaxCase

        Public ReadOnly Property Validation As ValidationResult

        ''' <summary>The calculated result; Nothing when validation failed.</summary>
        Public ReadOnly Property Result As TaxResult

        ''' <summary>Tax per bracket; empty when validation failed.</summary>
        Public ReadOnly Property BracketTaxes As IReadOnlyList(Of BracketTax)

        Public ReadOnly Property IsSuccess As Boolean
            Get
                Return Result IsNot Nothing
            End Get
        End Property

        Friend Shared Function Succeeded(taxCase As TaxCase, validation As ValidationResult, result As TaxResult, bracketTaxes As IReadOnlyList(Of BracketTax)) As CalculationOutcome
            ArgumentNullException.ThrowIfNull(result)
            Return New CalculationOutcome(taxCase, validation, result, bracketTaxes)
        End Function

        Friend Shared Function Rejected(taxCase As TaxCase, validation As ValidationResult) As CalculationOutcome
            Return New CalculationOutcome(taxCase, validation, Nothing, Array.Empty(Of BracketTax)())
        End Function

    End Class

End Namespace
