Namespace Rules

    ''' <summary>
    ''' Taxable income = annual income − the case's deductions − the configured basic deduction, never below zero.
    ''' </summary>
    Public NotInheritable Class DeductionRule
        Implements ITaxRule

        Public ReadOnly Property Name As String Implements ITaxRule.Name
            Get
                Return "DeductionRule"
            End Get
        End Property

        Public Sub Apply(context As TaxCalculationContext) Implements ITaxRule.Apply
            ArgumentNullException.ThrowIfNull(context)

            Dim taxableIncome = context.TaxCase.AnnualIncome -
                                context.TaxCase.Deductions -
                                context.Configuration.BasicDeduction

            context.TaxableIncome = Math.Max(0D, taxableIncome)
        End Sub

    End Class

End Namespace
