Imports JalaTax.Core.Configuration

Namespace Rules

    ''' <summary>
    ''' The part of taxable income that falls inside one bracket, and the (unrounded) tax on it.
    ''' </summary>
    Public NotInheritable Class BracketTax

        Public Sub New(bracket As TaxBracket, taxedAmount As Decimal, tax As Decimal)
            ArgumentNullException.ThrowIfNull(bracket)

            Me.Bracket = bracket
            Me.TaxedAmount = taxedAmount
            Me.Tax = tax
        End Sub

        Public ReadOnly Property Bracket As TaxBracket

        Public ReadOnly Property TaxedAmount As Decimal

        Public ReadOnly Property Tax As Decimal

    End Class

End Namespace
