Imports System.Text.RegularExpressions
Imports JalaTax.Core
Imports JalaTax.Core.Localization

<TestClass>
Public Class TimeFormatTests

    Private Shared ReadOnly SampleTime As New DateTimeOffset(2026, 1, 15, 10, 30, 5, TimeSpan.Zero)

    <TestMethod>
    Public Sub Time_WhenLanguageIsEnglish_UsesColons()
        Using New LanguageScope(Languages.English)
            ' Act and Assert
            Assert.IsTrue(Regex.IsMatch(DisplayFormat.Time(SampleTime), "^\d{2}:\d{2}:05$"))
        End Using
    End Sub

    <TestMethod>
    Public Sub Time_WhenLanguageIsFinnish_UsesDots()
        Using New LanguageScope(Languages.Finnish)
            ' Act and Assert
            Assert.IsTrue(Regex.IsMatch(DisplayFormat.Time(SampleTime), "^\d{2}\.\d{2}\.05$"))
        End Using
    End Sub

End Class
