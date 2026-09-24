Imports JalaTax.Core.Audit

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

            Dim annualIncome = context.TaxCase.AnnualIncome
            Dim deductions = context.TaxCase.Deductions
            Dim basicDeduction = context.Configuration.BasicDeduction

            context.TaxableIncome = Math.Max(0D, annualIncome - deductions - basicDeduction)

            context.AuditLog.Record(
                AuditEventType.RuleApplied,
                $"Deductions applied: taxable income {DisplayFormat.Amount(context.TaxableIncome)} " &
                $"(income {DisplayFormat.Amount(annualIncome)} − deductions {DisplayFormat.Amount(deductions)} " &
                $"− basic deduction {DisplayFormat.Amount(basicDeduction)}, not below 0.00)",
                Name)
        End Sub

    End Class

End Namespace
