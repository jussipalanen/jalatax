Imports System.Text.RegularExpressions
Imports JalaTax.Core

<TestClass>
Public Class ApplicationInfoTests

    <TestMethod>
    Public Sub Version_WhenRead_IsMajorMinorPatchWithoutCommitSuffix()
        ' Act
        Dim version = ApplicationInfo.Version

        ' Assert
        Assert.IsTrue(Regex.IsMatch(version, "^\d+\.\d+\.\d+$"), $"Unexpected version format: {version}")
    End Sub

    <TestMethod>
    Public Sub Version_WhenRead_MatchesTheCoreAssemblyVersion()
        ' Arrange
        Dim assemblyVersion = GetType(ApplicationInfo).Assembly.GetName().Version

        ' Act
        Dim version = ApplicationInfo.Version

        ' Assert
        Assert.AreEqual($"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}", version)
    End Sub

    <TestMethod>
    Public Sub Name_WhenRead_IsJalaTax()
        ' Act and Assert
        Assert.AreEqual("JalaTax", ApplicationInfo.Name)
    End Sub

End Class
