Imports System.Globalization
Imports JalaTax.Core.Localization

<TestClass>
Public Class LanguagesTests

    <TestMethod>
    Public Sub DefaultLanguage_IsSupported()
        ' Act and Assert
        Assert.IsTrue(Languages.IsSupported(Languages.DefaultLanguage))
    End Sub

    <TestMethod>
    <DataRow("fi", "fi")>
    <DataRow("FI", "fi")>
    <DataRow(" en ", "en")>
    Public Sub Use_WhenLanguageIsSupported_SwitchesCurrentLanguage(languageCode As String, expected As String)
        Using New LanguageScope(Languages.English)
            ' Act
            Languages.Use(languageCode)

            ' Assert
            Assert.AreEqual(expected, Languages.Current)
        End Using
    End Sub

    <TestMethod>
    <DataRow("sv")>
    <DataRow("")>
    <DataRow(Nothing)>
    Public Sub Use_WhenLanguageIsNotSupported_ThrowsArgumentException(languageCode As String)
        ' Act and Assert
        Assert.ThrowsExactly(Of ArgumentException)(Sub() Languages.Use(languageCode))
    End Sub

    <TestMethod>
    Public Sub Use_WhenCalled_DoesNotChangeRegionalFormatCulture()
        ' Arrange
        Dim regionalCulture = CultureInfo.CurrentCulture

        Using New LanguageScope(Languages.Finnish)
            ' Assert: input parsing keeps following the user's regional settings.
            Assert.AreEqual(regionalCulture, CultureInfo.CurrentCulture)
        End Using
    End Sub

    <TestMethod>
    Public Sub DisplayName_ReturnsTheLanguagesOwnName()
        ' Act and Assert
        Assert.AreEqual("Suomi", Languages.DisplayName(Languages.Finnish))
        Assert.AreEqual("English", Languages.DisplayName(Languages.English))
    End Sub

End Class
