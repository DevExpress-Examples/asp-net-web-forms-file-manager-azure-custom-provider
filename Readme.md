<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128554498/24.2.1%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T532265)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->

# File Manager for ASP.NET Web Forms - How to implement a custom file system provider for Azure Storage

This example demonstrates how to implement a custom file system provider to use our [ASPxFileManager](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxFileManager) control with Microsoft Azure Storage.

> [!NOTE]
> [ASPxFileManager](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxFileManager) supports the most popular cloud services, including [Azure Blob Storage](https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blobs-introduction) out of the box. For more information, refer to the following topic: [Azure File System Provider](https://docs.devexpress.com/AspNet/17780/components/file-management/file-manager/concepts/file-system-providers/azure-file-system-provider).

## Implementation Details

Create a [custom file system provider](https://docs.devexpress.com/AspNet/9907/components/file-management/file-manager/concepts/file-system-providers/custom-file-system-provider) and use [Azure SDK](https://azure.microsoft.com/en-us/downloads/) to implement all required functionality.

To run the example, define `AccountName`, `KeyValue`, and `FileShare` constants with your credentials in the [AzureFileStorageProvider](./CS/DXWebApplication1/AzureFileStorageProvider.cs) class.


## Files to Review

* [AzureFileStorageProvider.cs](./CS/DXWebApplication1/AzureFileStorageProvider.cs) (VB: [AzureFileStorageProvider.vb](./VB/DXWebApplication1/AzureFileStorageProvider.vb))
* [WebForm1.aspx](./CS/DXWebApplication1/WebForm1.aspx) (VB: [WebForm1.aspx](./VB/DXWebApplication1/WebForm1.aspx))
* [WebForm1.aspx.cs](./CS/DXWebApplication1/WebForm1.aspx.cs) (VB: [WebForm1.aspx.vb](./VB/DXWebApplication1/WebForm1.aspx.vb))

## Documentation

* [Develop for Azure Files with .NET](https://learn.microsoft.com/en-us/azure/storage/files/storage-dotnet-how-to-use-files)

## More Examples

* [How to implement a custom file system provider for a List data source](https://github.com/DevExpress-Examples/asp-net-web-forms-file-manager-list-custom-file-system-provider)
* [How to implement a custom file system provider for LINQ to SQL data source](https://github.com/DevExpress-Examples/asp-net-web-forms-file-manager-linq-to-sql-custom-file-system-provider)
<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-web-forms-file-manager-azure-custom-provider&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-web-forms-file-manager-azure-custom-provider&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
