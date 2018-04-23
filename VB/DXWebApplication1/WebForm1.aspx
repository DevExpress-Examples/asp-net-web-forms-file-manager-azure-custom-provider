<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="WebForm1.aspx.vb" Inherits="DXWebApplication1.WebForm1" %>

<%@ Register Assembly="DevExpress.Web.v16.2, Version=16.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<dx:ASPxFileManager ID="ASPxFileManager1" runat="server" ClientInstanceName="fileManager"
				CustomFileSystemProviderTypeName="DXWebApplication1.AzureFileStorageProvider" ProviderType="Custom">
				<Settings ThumbnailFolder="~/Thumb" EnableMultiSelect="true" />
				<SettingsFileList ShowFolders="True" ShowParentFolder="True">
				</SettingsFileList>
				<SettingsEditing AllowCopy="True" AllowCreate="True" AllowDelete="True" AllowDownload="True" AllowMove="True" AllowRename="True" />
				<SettingsFolders EnableCallBacks="True" />
			</dx:ASPxFileManager>
		</div>
	</form>
</body>
</html>