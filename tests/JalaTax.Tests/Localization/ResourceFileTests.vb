Imports System.IO
Imports System.Text.RegularExpressions

''' <summary>
''' Checks every .resx file pair in src/: each English text has a Finnish translation and the other way
''' round, and every text key used in the code exists. Reads the source files, so it also covers the
''' Windows Forms and console projects that the tests do not reference.
''' </summary>
<TestClass>
Public Class ResourceFileTests

    <TestMethod>
    Public Sub ResourceFiles_EveryEnglishTextHasFinnishTranslationAndViceVersa()
        ' Arrange
        Dim neutralFiles = FindNeutralResourceFiles()

        ' Act and Assert
        Assert.IsNotEmpty(neutralFiles)
        For Each neutralFile In neutralFiles
            Dim finnishFile = Path.ChangeExtension(neutralFile, ".fi.resx")
            Assert.IsTrue(File.Exists(finnishFile), $"Missing Finnish resource file for {neutralFile}.")

            Dim englishKeys = ReadKeys(neutralFile)
            Dim finnishKeys = ReadKeys(finnishFile)
            CollectionAssert.AreEquivalent(englishKeys, finnishKeys, $"Keys differ between {neutralFile} and its Finnish file.")
        Next
    End Sub

    <TestMethod>
    Public Sub ResourceFiles_NoTextIsEmpty()
        For Each resourceFile In Directory.GetFiles(SourceDirectory(), "*.resx", SearchOption.AllDirectories).Where(AddressOf IsSourceFile)
            For Each entry In XDocument.Load(resourceFile).Root.Elements("data")
                Assert.IsFalse(String.IsNullOrWhiteSpace(entry.Element("value")?.Value), $"Empty text '{entry.Attribute("name").Value}' in {resourceFile}.")
            Next
        Next
    End Sub

    <TestMethod>
    Public Sub SourceCode_EveryLiteralTextKeyExistsInItsProjectResources()
        ' Arrange: calls such as CoreText.Get("Audit_CaseLoaded"), UiText.Get(...), ConsoleText.Get(...).
        Dim keyPattern As New Regex("\b\w+Text\.Get\(""(?<key>[A-Za-z_]+)""")

        For Each projectDirectory In Directory.GetDirectories(SourceDirectory())
            Dim projectKeys = Directory.GetFiles(projectDirectory, "*.resx", SearchOption.AllDirectories).
                Where(AddressOf IsSourceFile).
                Where(Function(file) Not Path.GetFileName(file).Contains(".fi.", StringComparison.Ordinal)).
                SelectMany(AddressOf ReadKeys).
                ToHashSet()

            For Each codeFile In Directory.GetFiles(projectDirectory, "*.vb", SearchOption.AllDirectories).Where(AddressOf IsSourceFile)
                For Each match As Match In keyPattern.Matches(File.ReadAllText(codeFile))
                    Dim key = match.Groups("key").Value
                    Assert.Contains(key, projectKeys, $"Text key '{key}' used in {codeFile} is missing from the project's resources.")
                Next
            Next
        Next
    End Sub

    Private Shared Function FindNeutralResourceFiles() As List(Of String)
        Return Directory.GetFiles(SourceDirectory(), "*.resx", SearchOption.AllDirectories).
            Where(AddressOf IsSourceFile).
            Where(Function(file) Not Path.GetFileName(file).Contains(".fi.", StringComparison.Ordinal)).
            ToList()
    End Function

    Private Shared Function ReadKeys(resourceFile As String) As List(Of String)
        Return XDocument.Load(resourceFile).Root.Elements("data").
            Select(Function(entry) entry.Attribute("name").Value).
            ToList()
    End Function

    Private Shared Function IsSourceFile(filePath As String) As Boolean
        Dim separator = Path.DirectorySeparatorChar
        Return Not filePath.Contains($"{separator}bin{separator}", StringComparison.Ordinal) AndAlso
               Not filePath.Contains($"{separator}obj{separator}", StringComparison.Ordinal)
    End Function

    Private Shared Function SourceDirectory() As String
        Return Path.Combine(RepositoryPaths.Root(), "src")
    End Function

End Class
