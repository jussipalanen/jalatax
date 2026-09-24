Imports JalaTax.Core.Models

<TestClass>
Public Class TaxResultTests

    <TestMethod>
    Public Sub Constructor_WhenValuesGiven_SetsAllProperties()
        ' Act
        Dim result As New TaxResult("DEMO-001", 45000D, 2500D, 3000D, 39500D, 5900D)

        ' Assert
        Assert.AreEqual("DEMO-001", result.TaxpayerId)
        Assert.AreEqual(45000D, result.AnnualIncome)
        Assert.AreEqual(2500D, result.Deductions)
        Assert.AreEqual(3000D, result.BasicDeduction)
        Assert.AreEqual(39500D, result.TaxableIncome)
        Assert.AreEqual(5900D, result.CalculatedTax)
    End Sub

    <TestMethod>
    <DataRow(Nothing)>
    <DataRow("")>
    <DataRow("   ")>
    Public Sub Constructor_WhenTaxpayerIdIsEmpty_ThrowsArgumentException(taxpayerId As String)
        ' Act and Assert
        Assert.Throws(Of ArgumentException)(
            Function() New TaxResult(taxpayerId, 0D, 0D, 0D, 0D, 0D))
    End Sub

    <TestMethod>
    Public Sub TaxCase_WhenCreated_HasEmptyTaxpayerIdInsteadOfNothing()
        ' Act
        Dim taxCase As New TaxCase()

        ' Assert
        Assert.AreEqual(String.Empty, taxCase.TaxpayerId)
    End Sub

End Class
