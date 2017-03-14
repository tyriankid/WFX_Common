<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="StoreOnShelves.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.StoreOnShelves"  %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	 <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1 class="title_line">批量上架门店商品</h1>
	    <span class="font">选择总店的商品进行上架，如要修改商品内容，请先上架后再批量编辑！</span>
     </div>


    <div class="Pg_15 Pg_010" style="text-align:center;">
        <a onclick="selectAll()"  CssClass="searchbutton">全选</a>
        <asp:Button ID="btnSaveInfo" runat="server" OnClientClick="return PageIsValid();" Text="保存设置"  CssClass="submit_DAqueding"/>

    </div>

    
     <div class="datalist">
         <asp:Repeater ID="SelectedProducts" runat="server">
             <HeaderTemplate>
                <table border="1" width="100%">
                    <tr>
                    <th style="display:none">id</th>
                    <th>操作</th>
                    <th>商品图片</th>
                    <th>商品名称</th>
                    <th>商家编码</th>
                    <th>市场价</th>
                    </tr>
            </HeaderTemplate>
             <ItemTemplate>
                <tr>
                <td style="display:none"><%#Eval("productid")%></td>
                <td><input type="checkbox" role="chk"  runat="server" id="chkboxProductid" value='<%#Eval("productid")%>'/></td>
                <td><img src="<%#Eval("ThumbnailUrl40")%>" /></td>
                <td><%#Eval("productname")%></td>
                <td><%#Eval("productcode")%></td>
                <td><%#Eval("marketprice")%></td>
                </tr>
            </ItemTemplate>


             <FooterTemplate>
            </table>
            </FooterTemplate>
         </asp:Repeater>
    </div>     

     
</div>
    
<script>
    function CloseWindow() {
        var win = art.dialog.open.origin; //来源页面
        // 如果父页面重载或者关闭其子对话框全部会关闭
        win.location.reload();
    }

    function selectAll() {
        $("[role='chk']").attr("checked", "true");
    }

    //return false;
</script>
</asp:Content>
