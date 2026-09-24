Imports System.Globalization
Imports JalaTax.Core.Rules

<TestClass>
Public Class TaxBracketRuleTests

    Private ReadOnly _rule As New TaxBracketRule()

    <TestMethod>
    Public Sub Apply_WhenTaxableIncomeSpansTwoBrackets_TaxesEachPartAtItsOwnRate()
        ' Arrange
        Dim context = CreateContextWithTaxableIncome(42500D)

        ' Act
        _rule.Apply(context)

        ' Assert
        Assert.AreEqual(6500D, context.CalculatedTax)
        Assert.HasCount(2, context.BracketTaxes)
        Assert.AreEqual(20000D, context.BracketTaxes(0).TaxedAmount)
        Assert.AreEqual(2000D, context.BracketTaxes(0).Tax)
        Assert.AreEqual(22500D, context.BracketTaxes(1).TaxedAmount)
        Assert.AreEqual(4500D, context.BracketTaxes(1).Tax)
    End Sub

    <TestMethod>
    <DataRow("0", "0")>
    <DataRow("10000", "1000")>
    <DataRow("19999.99", "2000.00")>
    <DataRow("20000", "2000")>
    <DataRow("20000.01", "2000.00")>
    <DataRow("49999.99", "8000.00")>
    <DataRow("50000", "8000")>
    <DataRow("60000", "11000")>
    Public Sub Apply_AtBracketBoundaries_ReturnsExpectedTax(taxableIncome As String, expectedTax As String)
        ' Arrange
        Dim context = CreateContextWithTaxableIncome(ParseAmount(taxableIncome))

        ' Act
        _rule.Apply(context)

        ' Assert
        Assert.AreEqual(ParseAmount(expectedTax), context.CalculatedTax)
    End Sub

    <TestMethod>
    Public Sub Apply_WhenIncomeIsExactlyOnBoundary_DoesNotTaxTheNextBracket()
        ' Arrange
        Dim context = CreateContextWithTaxableIncome(20000D)

        ' Act
        _rule.Apply(context)

        ' Assert
        Assert.HasCount(1, context.BracketTaxes)
    End Sub

    <TestMethod>
    Public Sub Apply_WhenIncomeIsInTopBracket_TaxesTheRemainderAtTopRate()
        ' Arrange
        Dim context = CreateContextWithTaxableIncome(1000000D)

        ' Act
        _rule.Apply(context)

        ' Assert
        Assert.HasCount(3, context.BracketTaxes)
        Assert.AreEqual(950000D, context.BracketTaxes(2).TaxedAmount)
        Assert.AreEqual(2000D + 6000D + 285000D, context.CalculatedTax)
    End Sub

    <TestMethod>
    Public Sub Apply_WhenTaxableIncomeIsZero_RecordsNoBrackets()
        ' Arrange
        Dim context = CreateContextWithTaxableIncome(0D)

        ' Act
        _rule.Apply(context)

        ' Assert
        Assert.AreEqual(0D, context.CalculatedTax)
        Assert.IsEmpty(context.BracketTaxes)
    End Sub

    <TestMethod>
    <DataRow("0.05", "0.01")>
    <DataRow("0.15", "0.02")>
    <DataRow("0.04", "0.00")>
    Public Sub Apply_WhenTaxEndsInHalfCent_RoundsAwayFromZero(taxableIncome As String, expectedTax As String)
        ' Arrange: 10 % of 0.05 is 0.005, which banker's rounding would turn into 0.00.
        Dim context = CreateContextWithTaxableIncome(ParseAmount(taxableIncome))

        ' Act
        _rule.Apply(context)

        ' Assert
        Assert.AreEqual(ParseAmount(expectedTax), context.CalculatedTax)
    End Sub

    Private Shared Function ParseAmount(value As String) As Decimal
        Return Decimal.Parse(value, CultureInfo.InvariantCulture)
    End Function

End Class
