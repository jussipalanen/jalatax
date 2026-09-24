Namespace Models

    ''' <summary>
    ''' Outcome of processing one <see cref="TaxCase"/>.
    ''' </summary>
    Public NotInheritable Class TaxResult

        Public Sub New(taxpayerId As String,
                       annualIncome As Decimal,
                       deductions As Decimal,
                       basicDeduction As Decimal,
                       taxableIncome As Decimal,
                       calculatedTax As Decimal)
            ArgumentException.ThrowIfNullOrWhiteSpace(taxpayerId)

            Me.TaxpayerId = taxpayerId
            Me.AnnualIncome = annualIncome
            Me.Deductions = deductions
            Me.BasicDeduction = basicDeduction
            Me.TaxableIncome = taxableIncome
            Me.CalculatedTax = calculatedTax
        End Sub

        Public ReadOnly Property TaxpayerId As String

        Public ReadOnly Property AnnualIncome As Decimal

        ''' <summary>Deductions reported in the tax case.</summary>
        Public ReadOnly Property Deductions As Decimal

        ''' <summary>Basic deduction from configuration, applied in addition to <see cref="Deductions"/>.</summary>
        Public ReadOnly Property BasicDeduction As Decimal

        Public ReadOnly Property TaxableIncome As Decimal

        Public ReadOnly Property CalculatedTax As Decimal

    End Class

End Namespace
