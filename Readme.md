<!-- default file list -->
*Files to look at*:

* [AzureFileStorageProvider.cs](./CS/DXWebApplication1/AzureFileStorageProvider.cs) (VB: [AzureFileStorageProvider.vb](./VB/DXWebApplication1/AzureFileStorageProvider.vb))
* [WebForm1.aspx](./CS/DXWebApplication1/WebForm1.aspx) (VB: [WebForm1.aspx](./VB/DXWebApplication1/WebForm1.aspx))
* [WebForm1.aspx.cs](./CS/DXWebApplication1/WebForm1.aspx.cs) (VB: [WebForm1.aspx.vb](./VB/DXWebApplication1/WebForm1.aspx.vb))
<!-- default file list end -->
# ASPxFileManager - How to implement a custom provider with Azure File storage/ File Service


<p>This example describes how to use our file manager control with <a href="https://blogs.msdn.microsoft.com/windowsazurestorage/2014/05/12/introducing-microsoft-azure-file-service/">the Azure File storage  / File Service</a>. The main idea is to create a <a href="https://documentation.devexpress.com/AspNet/9907/ASP-NET-WebForms-Controls/File-Management/File-Manager/Concepts/File-System-Providers/Custom-File-System-Provider">custom file system provider</a> and use <a href="https://azure.microsoft.com/en-us/downloads/">Azure SDK</a> to implement all required operations. As a starting point, it is possible to use the <a href="https://docs.microsoft.com/en-us/azure/storage/storage-dotnet-how-to-use-files">Get started with Azure File storage on Windows</a> Microsoft documentation.<br><br>To run the example, define the <strong>AccountName</strong>, <strong>KeyValue</strong>, and <strong>FileShare</strong> constants with your credentials in the <strong><em>AzureFileStorageProvider</em></strong> class.<br><br>Please note that <a href="https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blobs-introduction">the Azure Blob Storage</a> is supported in our ASPxFileManager by default. You can review <a href="https://demos.devexpress.com/ASPxFileManagerAndUploadDemos/FileManager/AzureProvider.aspx">our Demo</a>, where it is shown how to configure the file manager in this case.<br><br>See also:<br><a href="https://www.devexpress.com/Support/Center/p/E5024">ASPxFileManager - How to implement a List data bound custom file system provider</a><br><a href="https://www.devexpress.com/Support/Center/p/E2900">ASPxFileManager - How to implement a LINQ to SQL based file system provider</a></p>

<br/>


