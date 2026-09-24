Imports JalaTax.Core.Validation

<TestClass>
Public Class ValidationResultTests

    <TestMethod>
    Public Sub IsValid_WhenNoErrorsAdded_ReturnsTrue()
        ' Arrange
        Dim result As New ValidationResult()

        ' Act
        Dim isValid = result.IsValid

        ' Assert
        Assert.IsTrue(isValid)
        Assert.IsEmpty(result.Errors)
    End Sub

    <TestMethod>
    Public Sub AddError_WhenCalled_MakesResultInvalidAndRecordsError()
        ' Arrange
        Dim result As New ValidationResult()

        ' Act
        result.AddError("AnnualIncome", "Annual income must not be negative.")

        ' Assert
        Assert.IsFalse(result.IsValid)
        Assert.HasCount(1, result.Errors)
        Assert.AreEqual("AnnualIncome", result.Errors(0).PropertyName)
        Assert.AreEqual("Annual income must not be negative.", result.Errors(0).Message)
    End Sub

    <TestMethod>
    Public Sub AddError_WhenCalledTwice_KeepsBothErrorsInOrder()
        ' Arrange
        Dim result As New ValidationResult()

        ' Act
        result.AddError("TaxpayerId", "Taxpayer ID is required.")
        result.AddError("Deductions", "Deductions must not be negative.")

        ' Assert
        Assert.HasCount(2, result.Errors)
        Assert.AreEqual("TaxpayerId", result.Errors(0).PropertyName)
        Assert.AreEqual("Deductions", result.Errors(1).PropertyName)
    End Sub

    <TestMethod>
    <DataRow(Nothing)>
    <DataRow("")>
    <DataRow("   ")>
    Public Sub AddError_WhenMessageIsEmpty_ThrowsArgumentException(message As String)
        ' Arrange
        Dim result As New ValidationResult()

        ' Act and Assert
        Assert.Throws(Of ArgumentException)(Sub() result.AddError("AnnualIncome", message))
    End Sub

End Class
