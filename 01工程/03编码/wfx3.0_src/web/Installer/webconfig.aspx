<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webconfig.aspx.cs" Inherits="Installer_CustomWebCofing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="加密配置文件" />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="解密配置文件" />
    
    </div>
    </form>
</body>
</html>
