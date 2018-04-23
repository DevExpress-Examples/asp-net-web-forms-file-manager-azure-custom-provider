using DevExpress.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DXWebApplication1 {
	public class AzureFileStorageProvider : FileSystemProviderBase {
		const string accountName = "YourAccountName";
		const string keyValue = "YourKeyValue";
		const string shareName = "YourShareName";
		StorageCredentials cred = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName, keyValue);
		CloudStorageAccount account;
		CloudFileClient client;
		CloudFileShare fileShare;
		CloudFileDirectory rootDirectory;
		string rootFolderDisplayName = "root";
		public override string RootFolderDisplayName { get { return rootFolderDisplayName; } }
		public AzureFileStorageProvider(string rootFolder) : base(rootFolder) {
			account = new CloudStorageAccount(cred, true);
			client = account.CreateCloudFileClient();
			fileShare = client.GetShareReference(shareName);
			rootDirectory = fileShare.GetRootDirectoryReference();
		}
		public override IEnumerable<FileManagerFile> GetFiles(FileManagerFolder folder) {
			String prefix = folder.FullName;

			return GetDirectory(prefix)
			 .ListFilesAndDirectories()
			 .OfType<CloudFile>()
			 .Select(cf => new FileManagerFile(this, folder, cf.Name))
			 .ToList();
		}
		public override IEnumerable<FileManagerFolder> GetFolders(FileManagerFolder parentFolder) {
			String prefix = parentFolder.FullName;

			return GetDirectory(prefix)
				.ListFilesAndDirectories()
				.OfType<CloudFileDirectory>()
				.Select(cfd => new FileManagerFolder(this, parentFolder, cfd.Name))
				.ToList();
		}
		public override bool Exists(FileManagerFile file) {
			return GetFile(file).Exists();
		}
		public override bool Exists(FileManagerFolder folder) {
			return GetDirectory(folder.FullName).Exists();
		}
		public override System.IO.Stream ReadFile(FileManagerFile file) {
			var fileToRead = GetFile(file);
			return fileToRead.OpenRead();
		}
		public override void CopyFile(FileManagerFile file, FileManagerFolder newParentFolder) {
			var sourceFile = GetFile(file);
			GetFile(newParentFolder.FullName, file.Name).StartCopy(sourceFile);
		}
		public override void CopyFolder(FileManagerFolder folder, FileManagerFolder newParentFolder) {
			var sourceFolder = GetDirectory(folder.FullName);
			CopyFolderInternal(GetDirectory(folder.FullName), GetDirectory(newParentFolder.FullName), folder.Name);
		}
		public override void CreateFolder(FileManagerFolder parent, string name) {
			GetDirectory(parent.FullName).GetDirectoryReference(name).CreateIfNotExists();
		}
		public override void DeleteFile(FileManagerFile file) {
			GetFile(file).DeleteIfExists();
		}
		public override void DeleteFolder(FileManagerFolder folder) {
			DeleteFolderInternal(GetDirectory(folder.FullName));
		}
		public override DateTime GetLastWriteTime(FileManagerFile file) {
			var shareFile = GetFile(file);
			shareFile.FetchAttributes();
			var properties = shareFile.Properties;
			return (properties.LastModified == null) ? base.GetLastWriteTime(file) : Convert.ToDateTime(properties.LastModified.Value.DateTime);
		}
		public override DateTime GetLastWriteTime(FileManagerFolder folder) {
			var directory = GetDirectory(folder.FullName);
			directory.FetchAttributes();
			var properties = directory.Properties;
			return (properties.LastModified == null) ? base.GetLastWriteTime(folder) : Convert.ToDateTime(properties.LastModified.Value.DateTime);
		}
		public override long GetLength(FileManagerFile file) {
			var shareFile = GetFile(file);
			shareFile.FetchAttributes();
			var properties = shareFile.Properties;
			return (properties.Length == -1) ? base.GetLength(file) : properties.Length;
		}
		public override void MoveFile(FileManagerFile file, FileManagerFolder newParentFolder) {
			GetFile(newParentFolder.FullName, file.Name).StartCopy(GetFile(file));
			GetFile(file).DeleteIfExists();
		}
		public override void MoveFolder(FileManagerFolder folder, FileManagerFolder newParentFolder) {
			CopyFolder(folder, newParentFolder);
			DeleteFolder(folder);
		}
		public override void RenameFile(FileManagerFile file, string name) {
			GetFile(file.Folder.FullName, name).StartCopy(GetFile(file));
			GetFile(file).DeleteIfExists();
		}
		public override void RenameFolder(FileManagerFolder folder, string name) {
			CopyFolderInternal(folder, name);
			DeleteFolder(folder);
		}
		public override void UploadFile(FileManagerFolder folder, string fileName, Stream content) {
			GetFile(folder.FullName, fileName).UploadFromStream(content);
		}
		CloudFileDirectory GetDirectory(string prefix) {
			return (prefix == "") ? rootDirectory : rootDirectory.GetDirectoryReference(prefix);
		}
		CloudFile GetFile(FileManagerFile file) {
			return GetDirectory(file.Folder.FullName).GetFileReference(file.Name);
		}
		CloudFile GetFile(string folderName, string fileName) {
			return GetDirectory(folderName).GetFileReference(fileName);
		}
		void CopyFolderInternal(CloudFileDirectory sourceFolder, CloudFileDirectory targetFolder, string directoryName) {
			targetFolder.GetDirectoryReference(directoryName).CreateIfNotExists();
			var newFolder = targetFolder.GetDirectoryReference(directoryName);
			var items = sourceFolder.ListFilesAndDirectories();
			foreach(IListFileItem item in items) {
				if(item.GetType() == typeof(CloudFile)) {
					var file = (CloudFile)item;
					newFolder.GetFileReference(file.Name).StartCopy(file);
				}
				else {
					var directory = (CloudFileDirectory)item;
					CopyFolderInternal(directory, newFolder, directory.Name);
				}
			}
		}
		void CopyFolderInternal(FileManagerFolder sourceFolder, string newName) {
			CopyFolderInternal(GetDirectory(sourceFolder.FullName), GetDirectory(sourceFolder.Parent.FullName), newName);
		}
		void DeleteFolderInternal(CloudFileDirectory sourceFolder) {
			var items = sourceFolder.ListFilesAndDirectories().ToList();
			if(items.Count == 0) {
				sourceFolder.DeleteIfExists();
				return;
			}
			foreach(IListFileItem item in items) {
				if(item.GetType() == typeof(CloudFile)) {
					var file = (CloudFile)item;
					file.DeleteIfExists();
				}
				else {
					var directory = (CloudFileDirectory)item;
					DeleteFolderInternal(directory);
				}
			}
			sourceFolder.DeleteIfExists();
		}
	}
}