Imports System.IO

''' <summary>
''' Locates the repository for tests that check source files (resources, changelog, build settings).
''' </summary>
Friend Module RepositoryPaths

    ''' <summary>The folder that contains JalaTax.sln.</summary>
    Friend Function Root() As String
        Dim directory = New DirectoryInfo(AppContext.BaseDirectory)
        While directory IsNot Nothing AndAlso Not File.Exists(Path.Combine(directory.FullName, "JalaTax.sln"))
            directory = directory.Parent
        End While

        Assert.IsNotNull(directory, "Repository root (JalaTax.sln) not found.")
        Return directory.FullName
    End Function

End Module
