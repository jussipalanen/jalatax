Imports System.Globalization
Imports System.IO
Imports JalaTax.Core
Imports JalaTax.Core.Audit
Imports JalaTax.Core.Configuration
Imports JalaTax.Core.Localization
Imports JalaTax.Core.Services

''' <summary>
''' The optional rule set data/rules-fi-2026.json: the published 2026 state income tax scale
''' (Laki vuoden 2026 tuloveroasteikosta 1140/2025), simplified to state income tax only.
''' </summary>
<TestClass>
Public Class FinnishTaxScale2026Tests

    Private Shared ReadOnly Configuration As TaxRuleConfiguration =
        New TaxRuleConfigurationLoader().Load(Path.Combine(AppContext.BaseDirectory, "data", "rules-fi-2026.json"))

    Private ReadOnly _service As New TaxCalculationService(Configuration)

    <TestMethod>
    Public Sub Load_WhenFileIsRead_HasFiveBracketsAndNoBasicDeduction()
        ' Assert
        Assert.HasCount(5, Configuration.TaxBrackets)
        Assert.AreEqual(0D, Configuration.BasicDeduction)
        Assert.AreEqual(0.375D, Configuration.TaxBrackets(4).Rate)
        Assert.IsFalse(Configuration.TaxBrackets(4).Max.HasValue)
    End Sub

    <TestMethod>
    <DataRow("0", "0.00")>
    <DataRow("22000", "2780.80")>
    <DataRow("32600", "4794.80")>
    <DataRow("40100", "7063.55")>
    <DataRow("52100", "11053.55")>
    Public Sub Calculate_AtEachLowerLimit_MatchesThePublishedTaxAtLowerLimit(taxableIncome As String, publishedTax As String)
        ' Arrange: with no deductions, taxable income equals annual income.
        Dim taxCase = CreateCase(ParseAmount(taxableIncome), 0D)

        ' Act
        Dim outcome = _service.Calculate(taxCase)

        ' Assert
        Assert.AreEqual(ParseAmount(publishedTax), outcome.Result.CalculatedTax)
    End Sub

    <TestMethod>
    <DataRow("45000", "2500", "42500", "7861.55")>
    <DataRow("68000", "1200", "66800", "16566.05")>
    Public Sub Calculate_WithExampleCases_ReturnsStateIncomeTax(annualIncome As String, deductions As String, expectedTaxable As String, expectedTax As String)
        ' Arrange
        Dim taxCase = CreateCase(ParseAmount(annualIncome), ParseAmount(deductions))

        ' Act
        Dim outcome = _service.Calculate(taxCase)

        ' Assert
        Assert.AreEqual(ParseAmount(expectedTaxable), outcome.Result.TaxableIncome)
        Assert.AreEqual(ParseAmount(expectedTax), outcome.Result.CalculatedTax)
    End Sub

    <TestMethod>
    Public Sub RuleSetName_FollowsTheLanguage()
        ' Act and Assert
        Assert.AreEqual("State income tax scale 2026 (simplified)", DisplayText.RuleSetName(Configuration))
        Using New LanguageScope(Languages.Finnish)
            Assert.AreEqual("Valtion tuloveroasteikko 2026 (yksinkertaistettu)", DisplayText.RuleSetName(Configuration))
        End Using
    End Sub

    <TestMethod>
    Public Sub Calculate_WhenCaseIsValid_RecordsTheRuleSetInTheAuditTrail()
        ' Act
        Dim entries = _service.Calculate(CreateCase(45000D, 0D)).AuditEntries

        ' Assert
        Dim ruleSetEntry = entries.Single(Function(entry) entry.EventType = AuditEventType.RuleSetSelected)
        Assert.AreEqual("Rule set: State income tax scale 2026 (simplified)", ruleSetEntry.Description)
    End Sub

    <TestMethod>
    Public Sub RuleSetName_WhenOnlyAnotherLanguageIsNamed_UsesThatName()
        ' Arrange
        Dim configuration = CreateConfiguration()
        configuration.Names = New Dictionary(Of String, String) From {{"fi", "Vain suomeksi"}}

        ' Act and Assert
        Assert.AreEqual("Vain suomeksi", DisplayText.RuleSetName(configuration))
    End Sub

    <TestMethod>
    Public Sub RuleSetName_WhenNamesAreMissing_ReturnsUnnamedText()
        ' Arrange
        Dim configuration = CreateConfiguration()
        configuration.Names = Nothing

        ' Act and Assert
        Assert.AreEqual("Unnamed rule set", DisplayText.RuleSetName(configuration))
    End Sub

    Private Shared Function ParseAmount(value As String) As Decimal
        Return Decimal.Parse(value, CultureInfo.InvariantCulture)
    End Function

End Class
