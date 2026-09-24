Imports System.Reflection

<TestClass>
Public Class ArchitectureTests

    <TestMethod>
    Public Sub CoreAssembly_WhenLoaded_DoesNotReferenceConsoleApplication()
        ' Arrange
        Dim coreAssembly = Assembly.Load("JalaTax.Core")

        ' Act
        Dim referencedNames = coreAssembly.GetReferencedAssemblies().Select(Function(reference) reference.Name)

        ' Assert
        CollectionAssert.DoesNotContain(referencedNames.ToList(), "JalaTax")
    End Sub

End Class
