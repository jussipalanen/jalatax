Imports JalaTax.Core.Configuration
Imports JalaTax.Core.Models
Imports JalaTax.Core.Rules

''' <summary>
''' Shared fictional test data matching data/rules.json.
''' </summary>
Friend Module RuleTestData

    Friend Function CreateConfiguration(Optional basicDeduction As Decimal = 3000D) As TaxRuleConfiguration
        Return New TaxRuleConfiguration With {
            .BasicDeduction = basicDeduction,
            .TaxBrackets = New List(Of TaxBracket) From {
                New TaxBracket With {.Min = 0D, .Max = 20000D, .Rate = 0.1D},
                New TaxBracket With {.Min = 20000D, .Max = 50000D, .Rate = 0.2D},
                New TaxBracket With {.Min = 50000D, .Rate = 0.3D}
            }
        }
    End Function

    Friend Function CreateCase(annualIncome As Decimal, deductions As Decimal) As TaxCase
        Return New TaxCase With {.TaxpayerId = "DEMO-001", .AnnualIncome = annualIncome, .Deductions = deductions}
    End Function

    Friend Function CreateContextWithTaxableIncome(taxableIncome As Decimal) As TaxCalculationContext
        Return New TaxCalculationContext(CreateCase(0D, 0D), CreateConfiguration()) With {.TaxableIncome = taxableIncome}
    End Function

End Module
