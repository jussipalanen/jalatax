Imports JalaTax.Core.Configuration
Imports JalaTax.Core.Models

Namespace Rules

    ''' <summary>
    ''' Working state for one calculation: the input, the configuration and the values rules produce.
    ''' </summary>
    Public NotInheritable Class TaxCalculationContext

        Private ReadOnly _bracketTaxes As New List(Of BracketTax)()

        Public Sub New(taxCase As TaxCase, configuration As TaxRuleConfiguration)
            ArgumentNullException.ThrowIfNull(taxCase)
            ArgumentNullException.ThrowIfNull(configuration)

            Me.TaxCase = taxCase
            Me.Configuration = configuration
        End Sub

        Public ReadOnly Property TaxCase As TaxCase

        Public ReadOnly Property Configuration As TaxRuleConfiguration

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
