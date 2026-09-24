Imports System.IO
Imports JalaTax.Core.Configuration

<TestClass>
Public Class TaxRuleConfigurationLoaderTests

    Private Const ValidJson As String = "{
  ""basicDeduction"": 3000,
  ""taxBrackets"": [
    { ""min"": 0, ""max"": 20000, ""rate"": 0.10 },
    { ""min"": 20000, ""max"": null, ""rate"": 0.20 }
  ]
}"

    Private ReadOnly _loader As New TaxRuleConfigurationLoader()

    <TestMethod>
    Public Sub Load_WhenExampleRulesFileIsUsed_ReturnsExpectedConfiguration()
        ' Arrange
        Dim filePath = Path.Combine(AppContext.BaseDirectory, "data", "rules.json")

        ' Act
        Dim configuration = _loader.Load(filePath)

        ' Assert
        Assert.AreEqual(3000D, configuration.BasicDeduction)
        Assert.HasCount(3, configuration.TaxBrackets)
        Assert.AreEqual(0D, configuration.TaxBrackets(0).Min)
        Assert.AreEqual(20000D, configuration.TaxBrackets(0).Max)
        Assert.AreEqual(0.1D, configuration.TaxBrackets(0).Rate)
        Assert.AreEqual(50000D, configuration.TaxBrackets(2).Min)
        Assert.IsFalse(configuration.TaxBrackets(2).Max.HasValue)
        Assert.AreEqual(0.3D, configuration.TaxBrackets(2).Rate)
    End Sub

    <TestMethod>
    Public Sub Load_WhenFileDoesNotExist_ThrowsConfigurationException()
        ' Arrange
        Dim filePath = Path.Combine(AppContext.BaseDirectory, "data", "missing-rules.json")

        ' Act
        Dim exception = Assert.ThrowsExactly(Of ConfigurationException)(Function() _loader.Load(filePath))

        ' Assert
        Assert.Contains("not found", exception.Message)
    End Sub

    <TestMethod>
    <DataRow(Nothing)>
    <DataRow("")>
    <DataRow("   ")>
    Public Sub Load_WhenPathIsEmpty_ThrowsArgumentException(filePath As String)
        ' Act and Assert
        Assert.Throws(Of ArgumentException)(Function() _loader.Load(filePath))
    End Sub

    <TestMethod>
    Public Sub Parse_WhenJsonIsValid_ReturnsConfiguration()
        ' Act
        Dim configuration = _loader.Parse(ValidJson)

        ' Assert
        Assert.AreEqual(3000D, configuration.BasicDeduction)
        Assert.HasCount(2, configuration.TaxBrackets)
        Assert.AreEqual(0.2D, configuration.TaxBrackets(1).Rate)
    End Sub

    <TestMethod>
    Public Sub Parse_WhenMaxIsOmittedForLastBracket_TreatsItAsOpenEnded()
        ' Arrange
        Dim json = "{ ""basicDeduction"": 0, ""taxBrackets"": [ { ""min"": 0, ""rate"": 0.25 } ] }"

        ' Act
        Dim configuration = _loader.Parse(json)

        ' Assert
        Assert.IsFalse(configuration.TaxBrackets(0).Max.HasValue)
    End Sub

    <TestMethod>
    <DataRow("{ ""basicDeduction"": 3000, ")>
    <DataRow("not json")>
    <DataRow("")>
    Public Sub Parse_WhenJsonIsMalformed_ThrowsConfigurationException(json As String)
        ' Act
        Dim exception = Assert.ThrowsExactly(Of ConfigurationException)(Function() _loader.Parse(json))

        ' Assert
        Assert.Contains("JSON is invalid", exception.Message)
    End Sub

    <TestMethod>
    Public Sub Parse_WhenJsonIsNullLiteral_ThrowsConfigurationException()
        ' Act
        Dim exception = Assert.ThrowsExactly(Of ConfigurationException)(Function() _loader.Parse("null"))

        ' Assert
        Assert.Contains("empty", exception.Message)
    End Sub

    <TestMethod>
    <DataRow("{ ""taxBrackets"": [ { ""min"": 0, ""rate"": 0.1 } ] }", "basicDeduction")>
    <DataRow("{ ""basicDeduction"": 3000 }", "taxBrackets")>
    <DataRow("{ ""basicDeduction"": 3000, ""taxBrackets"": [ { ""rate"": 0.1 } ] }", "min")>
    <DataRow("{ ""basicDeduction"": 3000, ""taxBrackets"": [ { ""min"": 0 } ] }", "rate")>
    Public Sub Parse_WhenRequiredValueIsMissing_ThrowsConfigurationException(json As String, missingProperty As String)
        ' Act
        Dim exception = Assert.ThrowsExactly(Of ConfigurationException)(Function() _loader.Parse(json))

        ' Assert
        Assert.Contains(missingProperty, exception.Message)
    End Sub

    <TestMethod>
    Public Sub Parse_WhenUnknownPropertyIsPresent_ThrowsConfigurationException()
        ' Arrange
        Dim json = "{ ""basicDeducton"": 3000, ""basicDeduction"": 3000, ""taxBrackets"": [ { ""min"": 0, ""rate"": 0.1 } ] }"

        ' Act
        Dim exception = Assert.ThrowsExactly(Of ConfigurationException)(Function() _loader.Parse(json))

        ' Assert
        Assert.Contains("basicDeducton", exception.Message)
    End Sub

    <TestMethod>
    Public Sub Parse_WhenValuesAreInvalid_ThrowsConfigurationExceptionListingEveryError()
        ' Arrange
        Dim json = "{
  ""basicDeduction"": -1,
  ""taxBrackets"": [
    { ""min"": 0, ""max"": 20000, ""rate"": 1.5 },
    { ""min"": 25000, ""max"": null, ""rate"": 0.2 }
  ]
}"

        ' Act
        Dim exception = Assert.ThrowsExactly(Of ConfigurationException)(Function() _loader.Parse(json))

        ' Assert
        Assert.HasCount(3, exception.Errors)
        For Each message In exception.Errors
            Assert.Contains(message, exception.Message)
        Next
    End Sub

    <TestMethod>
    Public Sub Parse_WhenJsonHasCommentsAndTrailingCommas_ReturnsConfiguration()
        ' Arrange
        Dim json = "{
  // Fictional demo values
  ""basicDeduction"": 3000,
  ""taxBrackets"": [ { ""min"": 0, ""rate"": 0.1, }, ],
}"

        ' Act
        Dim configuration = _loader.Parse(json)

        ' Assert
        Assert.HasCount(1, configuration.TaxBrackets)
    End Sub

End Class
