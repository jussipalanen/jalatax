Imports JalaTax.Core.Audit

Namespace Rules

    ''' <summary>
    ''' Progressive tax: each bracket's rate applies only to the part of taxable income
    ''' from its min (inclusive) up to its max (exclusive). The total is rounded to
    ''' 2 decimals with halves rounded away from zero.
    ''' </summary>
    Public NotInheritable Class TaxBracketRule
        Implements ITaxRule

        Public ReadOnly Property Name As String Implements ITaxRule.Name
            Get
                Return "TaxBracketRule"
            End Get
        End Property

        Public Sub Apply(context As TaxCalculationContext) Implements ITaxRule.Apply
            ArgumentNullException.ThrowIfNull(context)

            Dim taxableIncome = context.TaxableIncome
            Dim totalTax = 0D

            For Each bracket In context.Configuration.TaxBrackets
                Dim upperBound = If(bracket.Max, Decimal.MaxValue)
                Dim taxedAmount = Math.Min(taxableIncome, upperBound) - bracket.Min
                If taxedAmount <= 0D Then
                    Exit For
                End If

                Dim tax = taxedAmount * bracket.Rate
                context.AddBracketTax(New BracketTax(bracket, taxedAmount, tax))
                totalTax += tax

                context.AuditLog.Record(
                    AuditEventType.RuleApplied,
                    $"Tax bracket {DescribeRange(bracket)} at {AuditFormat.Rate(bracket.Rate)}: " &
                    $"{AuditFormat.Amount(taxedAmount)} taxed, tax {AuditFormat.Amount(tax)}",
                    Name)
            Next

            context.CalculatedTax = Math.Round(totalTax, 2, MidpointRounding.AwayFromZero)

            If context.BracketTaxes.Count = 0 Then
                context.AuditLog.Record(AuditEventType.RuleApplied, "No taxable income, no bracket applied: tax 0.00", Name)
            End If
        End Sub

        Private Shared Function DescribeRange(bracket As Configuration.TaxBracket) As String
            If bracket.Max.HasValue Then
                Return $"{AuditFormat.Amount(bracket.Min)}–{AuditFormat.Amount(bracket.Max.Value)}"
            End If

            Return $"{AuditFormat.Amount(bracket.Min)}+"
        End Function

    End Class

End Namespace
