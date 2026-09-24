Imports System.Globalization
Imports System.Reflection
Imports System.Resources

Namespace Localization

    ''' <summary>
    ''' Reads localized texts from one .resx resource set in the current UI language.
    ''' English is the neutral language; Finnish comes from the satellite assembly.
    ''' </summary>
    Public NotInheritable Class TextResources

        Private ReadOnly _resources As ResourceManager

        ''' <param name="baseName">Manifest name of the resources, for example "JalaTax.Core.Strings".</param>
        Public Sub New(baseName As String, resourceAssembly As Assembly)
            ArgumentException.ThrowIfNullOrWhiteSpace(baseName)
            ArgumentNullException.ThrowIfNull(resourceAssembly)
            _resources = New ResourceManager(baseName, resourceAssembly)
        End Sub

        ''' <summary>Returns the text, with {0}-style placeholders filled from <paramref name="args"/>.</summary>
        ''' <exception cref="InvalidOperationException">The text does not exist; a missing translation key is a programming error.</exception>
        Public Function [Get](name As String, ParamArray args As Object()) As String
            Dim template = _resources.GetString(name, CultureInfo.CurrentUICulture)
            If template Is Nothing Then
                Throw New InvalidOperationException($"Missing text resource '{name}'.")
            End If

            Return If(args.Length = 0, template, String.Format(CultureInfo.CurrentCulture, template, args))
        End Function

    End Class

End Namespace
