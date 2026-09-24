Imports System.IO
Imports System.Text.RegularExpressions
Imports JalaTax.Core

''' <summary>
''' The release workflow takes the release notes from CHANGELOG.md. These tests make a version bump
''' without changelog notes fail in CI, before anything is released.
''' </summary>
<TestClass>
Public Class ChangelogTests

    Private Shared ReadOnly Changelog As String = File.ReadAllText(Path.Combine(RepositoryPaths.Root(), "CHANGELOG.md"))

    <TestMethod>
    Public Sub Changelog_HasSectionForCurrentVersion()
        ' Arrange
        Dim heading = $"## [{ApplicationInfo.Version}] - "

        ' Act
        Dim lines = Changelog.Split({vbCrLf, vbLf}, StringSplitOptions.None)

        ' Assert
        Assert.IsTrue(lines.Any(Function(line) line.StartsWith(heading, StringComparison.Ordinal)),
                      $"CHANGELOG.md needs a section '{heading}YYYY-MM-DD' for the version in Directory.Build.props.")
    End Sub

    <TestMethod>
    Public Sub Changelog_HasUnreleasedSectionFirst()
        ' Act
        Dim firstSection = Regex.Match(Changelog, "^## \[(?<name>[^\]]+)\]", RegexOptions.Multiline)

        ' Assert
        Assert.AreEqual("Unreleased", firstSection.Groups("name").Value)
    End Sub

    <TestMethod>
    Public Sub Changelog_EveryVersionHasCompareOrReleaseLink()
        ' Arrange
        Dim versions = Regex.Matches(Changelog, "^## \[(?<name>\d+\.\d+\.\d+)\]", RegexOptions.Multiline).
            Select(Function(match) match.Groups("name").Value).
            ToList()

        ' Assert
        Assert.IsNotEmpty(versions)
        For Each versionNumber In versions
            Assert.IsTrue(Regex.IsMatch(Changelog, $"^\[{Regex.Escape(versionNumber)}\]: https://", RegexOptions.Multiline),
                          $"CHANGELOG.md is missing the link definition for [{versionNumber}].")
        Next
    End Sub

End Class
