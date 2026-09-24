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

        ''' <summary>
        ''' Reads the "--lang fi|en" option shared by the desktop app and the console runner.
        ''' Returns the default language when the option is absent.
        ''' </summary>
        ''' <param name="otherArguments">The arguments that remain after removing the option.</param>
        ''' <exception cref="ArgumentException">The option has no value or names an unsupported language.</exception>
        Public Function FromCommandLine(args As IEnumerable(Of String), ByRef otherArguments As IReadOnlyList(Of String)) As String
            ArgumentNullException.ThrowIfNull(args)

            Dim remaining As New List(Of String)()
            Dim language = DefaultLanguage
            Dim argumentList = args.ToList()

            Dim index = 0
            While index < argumentList.Count
                If String.Equals(argumentList(index), "--lang", StringComparison.OrdinalIgnoreCase) Then
                    If index + 1 >= argumentList.Count Then
                        Throw New ArgumentException(CoreText.Get("Language_MissingValue", String.Join(", ", Supported)))
                    End If

                    language = argumentList(index + 1)
                    If Not IsSupported(language) Then
                        Throw New ArgumentException(CoreText.Get("Language_Unsupported", language, String.Join(", ", Supported)))
                    End If

                    index += 2
                Else
                    remaining.Add(argumentList(index))
                    index += 1
                End If
            End While

            otherArguments = remaining.AsReadOnly()
            Return Normalize(language)
        End Function

        Private Function Normalize(languageCode As String) As String
            Return If(languageCode, String.Empty).Trim().ToLowerInvariant()
        End Function

    End Module

End Namespace
