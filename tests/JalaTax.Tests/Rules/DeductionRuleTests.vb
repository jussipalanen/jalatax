Imports JalaTax.Core.Rules

<TestClass>
Public Class DeductionRuleTests

    Private ReadOnly _rule As New DeductionRule()

    <TestMethod>
    Public Sub Apply_WhenDeductionsExist_ReducesIncomeByDeductionsAndBasicDeduction()
        ' Arrange
        Dim context As New TaxCalculationContext(CreateCase(45000D, 2500D), CreateConfiguration(basicDeduction:=3000D))

        ' Act
        _rule.Apply(context)

        ' Assert
        Assert.AreEqual(39500D, context.TaxableIncome)
    End Sub

    <TestMethod>
    Public Sub Apply_WhenBasicDeductionIsZero_ReducesIncomeByCaseDeductionsOnly()
        ' Arrange
        Dim context As New TaxCalculationContext(CreateCase(45000D, 2500D), CreateConfiguration(basicDeduction:=0D))

        ' Act
        _rule.Apply(context)

        ' Assert
        Assert.AreEqual(42500D, context.TaxableIncome)
    End Sub

    <TestMethod>
    <DataRow(1000, 5000)>
    <DataRow(3000, 0)>
    <DataRow(0, 0)>
    Public Sub Apply_WhenDeductionsReachOrExceedIncome_ReturnsZero(annualIncome As Integer, deductions As Integer)
        ' Arrange
        Dim context As New TaxCalculationContext(CreateCase(annualIncome, deductions), CreateConfiguration(basicDeduction:=3000D))

        ' Act
        _rule.Apply(context)

        ' Assert
        Assert.AreEqual(0D, context.TaxableIncome)
    End Sub

    <TestMethod>
    Public Sub Apply_WhenContextIsNothing_ThrowsArgumentNullException()
        ' Act and Assert
        Assert.ThrowsExactly(Of ArgumentNullException)(Sub() _rule.Apply(Nothing))
    End Sub

End Class
