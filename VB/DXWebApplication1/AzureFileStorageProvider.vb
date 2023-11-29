Imports DevExpress.Web
Imports Microsoft.WindowsAzure.Storage
Imports Microsoft.WindowsAzure.Storage.Auth
Imports Microsoft.WindowsAzure.Storage.File
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq

Namespace DXWebApplication1

    Public Class AzureFileStorageProvider
        Inherits FileSystemProviderBase

        Const accountName As String = "YourAccountName"

        Const keyValue As String = "YourKeyValue"

        Const shareName As String = "YourShareName"

        Private cred As StorageCredentials = New StorageCredentials(accountName, keyValue)

        Private account As CloudStorageAccount

        Private client As CloudFileClient

        Private fileShare As CloudFileShare

        Private rootDirectory As CloudFileDirectory

        Private rootFolderDisplayNameField As String = "root"

        Public Overrides ReadOnly Property RootFolderDisplayName As String
            Get
                Return rootFolderDisplayNameField
            End Get
        End Property

        Public Sub New(ByVal rootFolder As String)
            MyBase.New(rootFolder)
            account = New CloudStorageAccount(cred, True)
            client = account.CreateCloudFileClient()
            fileShare = client.GetShareReference(shareName)
            rootDirectory = fileShare.GetRootDirectoryReference()
        End Sub

        Public Overrides Function GetFiles(ByVal folder As FileManagerFolder) As IEnumerable(Of FileManagerFile)
            Dim prefix As String = folder.FullName
            Return GetDirectory(prefix).ListFilesAndDirectories().OfType(Of CloudFile)().[Select](Function(cf) New FileManagerFile(Me, folder, cf.Name)).ToList()
        End Function

        Public Overrides Function GetFolders(ByVal parentFolder As FileManagerFolder) As IEnumerable(Of FileManagerFolder)
            Dim prefix As String = parentFolder.FullName
            Return GetDirectory(prefix).ListFilesAndDirectories().OfType(Of CloudFileDirectory)().[Select](Function(cfd) New FileManagerFolder(Me, parentFolder, cfd.Name)).ToList()
        End Function

        Public Overrides Function Exists(ByVal file As FileManagerFile) As Boolean
            Return GetFile(file).Exists()
        End Function

        Public Overrides Function Exists(ByVal folder As FileManagerFolder) As Boolean
            Return GetDirectory(folder.FullName).Exists()
        End Function

        Public Overrides Function ReadFile(ByVal file As FileManagerFile) As Stream
            Dim fileToRead = GetFile(file)
            Return fileToRead.OpenRead()
        End Function

        Public Overrides Sub CopyFile(ByVal file As FileManagerFile, ByVal newParentFolder As FileManagerFolder)
            Dim sourceFile = GetFile(file)
            GetFile(newParentFolder.FullName, file.Name).StartCopy(sourceFile)
        End Sub

        Public Overrides Sub CopyFolder(ByVal folder As FileManagerFolder, ByVal newParentFolder As FileManagerFolder)
            Dim sourceFolder = GetDirectory(folder.FullName)
            CopyFolderInternal(GetDirectory(folder.FullName), GetDirectory(newParentFolder.FullName), folder.Name)
        End Sub

        Public Overrides Sub CreateFolder(ByVal parent As FileManagerFolder, ByVal name As String)
            GetDirectory(parent.FullName).GetDirectoryReference(name).CreateIfNotExists()
        End Sub

        Public Overrides Sub DeleteFile(ByVal file As FileManagerFile)
            GetFile(file).DeleteIfExists()
        End Sub

        Public Overrides Sub DeleteFolder(ByVal folder As FileManagerFolder)
            DeleteFolderInternal(GetDirectory(folder.FullName))
        End Sub

        Public Overrides Function GetLastWriteTime(ByVal file As FileManagerFile) As Date
            Dim shareFile = GetFile(file)
            shareFile.FetchAttributes()
            Dim properties = shareFile.Properties
            Return If(properties.LastModified Is Nothing, MyBase.GetLastWriteTime(file), Convert.ToDateTime(properties.LastModified.Value.DateTime))
        End Function

        Public Overrides Function GetLastWriteTime(ByVal folder As FileManagerFolder) As Date
            Dim directory = GetDirectory(folder.FullName)
            directory.FetchAttributes()
            Dim properties = directory.Properties
            Return If(properties.LastModified Is Nothing, MyBase.GetLastWriteTime(folder), Convert.ToDateTime(properties.LastModified.Value.DateTime))
        End Function

        Public Overrides Function GetLength(ByVal file As FileManagerFile) As Long
            Dim shareFile = GetFile(file)
            shareFile.FetchAttributes()
            Dim properties = shareFile.Properties
            Return If(properties.Length = -1, MyBase.GetLength(file), properties.Length)
        End Function

        Public Overrides Sub MoveFile(ByVal file As FileManagerFile, ByVal newParentFolder As FileManagerFolder)
            GetFile(newParentFolder.FullName, file.Name).StartCopy(GetFile(file))
            GetFile(file).DeleteIfExists()
        End Sub

        Public Overrides Sub MoveFolder(ByVal folder As FileManagerFolder, ByVal newParentFolder As FileManagerFolder)
            CopyFolder(folder, newParentFolder)
            DeleteFolder(folder)
        End Sub

        Public Overrides Sub RenameFile(ByVal file As FileManagerFile, ByVal name As String)
            GetFile(file.Folder.FullName, name).StartCopy(GetFile(file))
            GetFile(file).DeleteIfExists()
        End Sub

        Public Overrides Sub RenameFolder(ByVal folder As FileManagerFolder, ByVal name As String)
            CopyFolderInternal(folder, name)
            DeleteFolder(folder)
        End Sub

        Public Overrides Sub UploadFile(ByVal folder As FileManagerFolder, ByVal fileName As String, ByVal content As Stream)
            GetFile(folder.FullName, fileName).UploadFromStream(content)
        End Sub

        Private Function GetDirectory(ByVal prefix As String) As CloudFileDirectory
            Return If(Equals(prefix, ""), rootDirectory, rootDirectory.GetDirectoryReference(prefix))
        End Function

        Private Function GetFile(ByVal file As FileManagerFile) As CloudFile
            Return GetDirectory(file.Folder.FullName).GetFileReference(file.Name)
        End Function

        Private Function GetFile(ByVal folderName As String, ByVal fileName As String) As CloudFile
            Return GetDirectory(folderName).GetFileReference(fileName)
        End Function

        Private Sub CopyFolderInternal(ByVal sourceFolder As CloudFileDirectory, ByVal targetFolder As CloudFileDirectory, ByVal directoryName As String)
            targetFolder.GetDirectoryReference(directoryName).CreateIfNotExists()
            Dim newFolder = targetFolder.GetDirectoryReference(directoryName)
            Dim items = sourceFolder.ListFilesAndDirectories()
            For Each item As IListFileItem In items
                If item.GetType() Is GetType(CloudFile) Then
                    Dim file = CType(item, CloudFile)
                    newFolder.GetFileReference(file.Name).StartCopy(file)
                Else
                    Dim directory = CType(item, CloudFileDirectory)
                    CopyFolderInternal(directory, newFolder, directory.Name)
                End If
            Next
        End Sub

        Private Sub CopyFolderInternal(ByVal sourceFolder As FileManagerFolder, ByVal newName As String)
            CopyFolderInternal(GetDirectory(sourceFolder.FullName), GetDirectory(sourceFolder.Parent.FullName), newName)
        End Sub

        Private Sub DeleteFolderInternal(ByVal sourceFolder As CloudFileDirectory)
            Dim items = sourceFolder.ListFilesAndDirectories().ToList()
            If items.Count = 0 Then
                sourceFolder.DeleteIfExists()
                Return
            End If

            For Each item As IListFileItem In items
                If item.GetType() Is GetType(CloudFile) Then
                    Dim file = CType(item, CloudFile)
                    file.DeleteIfExists()
                Else
                    Dim directory = CType(item, CloudFileDirectory)
                    DeleteFolderInternal(directory)
                End If
            Next

            sourceFolder.DeleteIfExists()
        End Sub
    End Class
End Namespace
