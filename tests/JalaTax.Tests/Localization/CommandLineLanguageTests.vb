Imports JalaTax.Core.Localization

<TestClass>
Public Class CommandLineLanguageTests

    <TestMethod>
    Public Sub FromCommandLine_WhenOptionIsAbsent_ReturnsFinnishDefault()
        ' Arrange
        Dim args = New String() {"cases.json"}
        Dim otherArguments As IReadOnlyList(Of String) = Nothing

        ' Act
        Dim language = Languages.FromCommandLine(args, otherArguments)

        ' Assert
        Assert.AreEqual("fi", language)
        CollectionAssert.AreEqual(New List(Of String) From {"cases.json"}, otherArguments.ToList())
    End Sub

    <TestMethod>
    Public Sub FromCommandLine_WhenOptionIsGiven_ReturnsLanguageAndRemovesOption()
        ' Arrange
        Dim args = New String() {"--lang", "EN", "cases.json"}
        Dim otherArguments As IReadOnlyList(Of String) = Nothing

        ' Act
        Dim language = Languages.FromCommandLine(args, otherArguments)

        ' Assert
        Assert.AreEqual("en", language)
        CollectionAssert.AreEqual(New List(Of String) From {"cases.json"}, otherArguments.ToList())
    End Sub

    <TestMethod>
    <DataRow("--lang")>
    <DataRow("--lang|sv")>
    Public Sub FromCommandLine_WhenValueIsMissingOrUnsupported_ThrowsArgumentException(arguments As String)
        ' Arrange
        Dim otherArguments As IReadOnlyList(Of String) = Nothing

        ' Act and Assert
        Assert.ThrowsExactly(Of ArgumentException)(Function() Languages.FromCommandLine(arguments.Split("|"c), otherArguments))
    End Sub

End Class
