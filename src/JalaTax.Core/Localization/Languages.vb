Imports System.Globalization

Namespace Localization

    ''' <summary>
    ''' Supported languages and switching between them. Finnish is the application default.
    ''' The language decides both the texts and the number format (see <see cref="DisplayFormat"/>).
    ''' </summary>
    Public Module Languages

        Public Const Finnish As String = "fi"
        Public Const English As String = "en"
        Public Const DefaultLanguage As String = Finnish

        Private ReadOnly FinnishCulture As CultureInfo = CultureInfo.GetCultureInfo("fi-FI")
        Private ReadOnly EnglishCulture As CultureInfo = CultureInfo.GetCultureInfo("en-US")

        Public ReadOnly Property Supported As IReadOnlyList(Of String) = {Finnish, English}

        ''' <summary>The language of the current UI culture: "fi" for Finnish, otherwise "en".</summary>
        Public ReadOnly Property Current As String
            Get
                Return If(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName = Finnish, Finnish, English)
            End Get
        End Property

        Public Function IsSupported(languageCode As String) As Boolean
            Return Supported.Contains(Normalize(languageCode))
        End Function

        ''' <summary>Switches JalaTax texts and number formatting for the current thread.</summary>
        ''' <exception cref="ArgumentException">The language is not supported.</exception>
        Public Sub Use(languageCode As String)
            If Not IsSupported(languageCode) Then
                Throw New ArgumentException($"Unsupported language '{languageCode}'. Use one of: {String.Join(", ", Supported)}.", NameOf(languageCode))
            End If

            CultureInfo.CurrentUICulture = If(Normalize(languageCode) = Finnish, FinnishCulture, EnglishCulture)
        End Sub

        ''' <summary>The language's own name, for example "Suomi" or "English".</summary>
        Public Function DisplayName(languageCode As String) As String
            Return If(Normalize(languageCode) = Finnish, "Suomi", "English")
        End Function

        ''' <summary>Culture for formatting numbers in the current language.</summary>
        Friend Function NumberCulture() As CultureInfo
            Return If(Current = Finnish, FinnishCulture, CultureInfo.InvariantCulture)
        End Function

        Private Function Normalize(languageCode As String) As String
            Return If(languageCode, String.Empty).Trim().ToLowerInvariant()
        End Function

    End Module

End Namespace
