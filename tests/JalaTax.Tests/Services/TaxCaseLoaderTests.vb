Imports System.IO
Imports JalaTax.Core.Services
Imports JalaTax.Core.Validation

<TestClass>
Public Class TaxCaseLoaderTests

    <TestMethod>
    Public Sub Load_WhenExampleFileIsUsed_ReturnsAllDemoCases()
        ' Arrange
        Dim filePath = Path.Combine(AppContext.BaseDirectory, "data", "example-taxpayer.json")

        ' Act
        Dim taxCases = TaxCaseLoader.Load(filePath)

        ' Assert
        Assert.HasCount(3, taxCases)
        Assert.AreEqual("DEMO-001", taxCases(0).TaxpayerId)
        Assert.AreEqual(45000D, taxCases(0).AnnualIncome)
        Assert.AreEqual(2500D, taxCases(0).Deductions)
        Assert.IsTrue(taxCases.All(Function(taxCase) taxCase.TaxpayerId.StartsWith("DEMO-", StringComparison.Ordinal)))
    End Sub

    <TestMethod>
    Public Sub Load_WhenExampleFileIsValidated_OnlyDemo003IsInvalid()
        ' Arrange
        Dim filePath = Path.Combine(AppContext.BaseDirectory, "data", "example-taxpayer.json")
        Dim validator As New TaxCaseValidator()

        ' Act
        Dim invalidIds = TaxCaseLoader.Load(filePath).
            Where(Function(taxCase) Not validator.Validate(taxCase).IsValid).
            Select(Function(taxCase) taxCase.TaxpayerId).
            ToList()

        ' Assert
        CollectionAssert.AreEqual(New List(Of String) From {"DEMO-003"}, invalidIds)
    End Sub

    <TestMethod>
    Public Sub Load_WhenFileDoesNotExist_ThrowsInputDataException()
        ' Arrange
        Dim filePath = Path.Combine(AppContext.BaseDirectory, "data", "missing-cases.json")

        ' Act
        Dim exception = Assert.ThrowsExactly(Of InputDataException)(Function() TaxCaseLoader.Load(filePath))

        ' Assert
        Assert.Contains("not found", exception.Message)
    End Sub

    <TestMethod>
    Public Sub Parse_WhenJsonIsValid_ReturnsCases()
        ' Arrange
        Dim json = "[ { ""taxpayerId"": ""DEMO-010"", ""annualIncome"": 1000.50, ""deductions"": 0 } ]"

        ' Act
        Dim taxCases = TaxCaseLoader.Parse(json)

        ' Assert
        Assert.HasCount(1, taxCases)
        Assert.AreEqual("DEMO-010", taxCases(0).TaxpayerId)
        Assert.AreEqual(1000.5D, taxCases(0).AnnualIncome)
    End Sub

    <TestMethod>
    Public Sub Parse_WhenCaseValuesAreInvalid_StillLoadsCaseForValidation()
        ' Arrange
        Dim json = "[ { ""taxpayerId"": """", ""annualIncome"": -1, ""deductions"": -1 } ]"

        ' Act
        Dim taxCases = TaxCaseLoader.Parse(json)

        ' Assert
        Assert.HasCount(1, taxCases)
    End Sub

    <TestMethod>
    <DataRow("[ { ""taxpayerId"": ""DEMO-001"" ")>
    <DataRow("not json")>
    <DataRow("{ ""taxpayerId"": ""DEMO-001"", ""annualIncome"": 1, ""deductions"": 0 }")>
    Public Sub Parse_WhenJsonIsNotAListOfCases_ThrowsInputDataException(json As String)
        ' Act
        Dim exception = Assert.ThrowsExactly(Of InputDataException)(Function() TaxCaseLoader.Parse(json))

        ' Assert
        Assert.Contains("JSON is invalid", exception.Message)
    End Sub

    <TestMethod>
    <DataRow("null", "empty")>
    <DataRow("[]", "no tax cases")>
    <DataRow("[ null ]", "empty entries")>
    Public Sub Parse_WhenListIsMissingOrEmpty_ThrowsInputDataException(json As String, expectedText As String)
        ' Act
        Dim exception = Assert.ThrowsExactly(Of InputDataException)(Function() TaxCaseLoader.Parse(json))

        ' Assert
        Assert.Contains(expectedText, exception.Message)
    End Sub

    <TestMethod>
    <DataRow("[ { ""annualIncome"": 1, ""deductions"": 0 } ]", "taxpayerId")>
    <DataRow("[ { ""taxpayerId"": ""DEMO-001"", ""deductions"": 0 } ]", "annualIncome")>
    <DataRow("[ { ""taxpayerId"": ""DEMO-001"", ""annualIncome"": 1 } ]", "deductions")>
    Public Sub Parse_WhenRequiredValueIsMissing_ThrowsInputDataException(json As String, missingProperty As String)
        ' Act
        Dim exception = Assert.ThrowsExactly(Of InputDataException)(Function() TaxCaseLoader.Parse(json))

        ' Assert
        Assert.Contains(missingProperty, exception.Message)
    End Sub

    <TestMethod>
    Public Sub Parse_WhenUnknownPropertyIsPresent_ThrowsInputDataException()
        ' Arrange
        Dim json = "[ { ""taxpayerId"": ""DEMO-001"", ""annualIncome"": 1, ""deductions"": 0, ""socialSecurityNumber"": ""x"" } ]"

        ' Act
        Dim exception = Assert.ThrowsExactly(Of InputDataException)(Function() TaxCaseLoader.Parse(json))

        ' Assert
        Assert.Contains("socialSecurityNumber", exception.Message)
    End Sub

End Class
