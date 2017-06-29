<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SelectProduct.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SelectProduct"  %>
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

    <div style="line-height:30px;position:fixed;width:100%;text-align:center;background:#fff;top:0;"><input type="button" id="btnQd" class="allSelect" style="width:66px;height:27px;line-height:27px;background:#F55656;color:#fff;border-radius:3px" value="确认选择"/></div>
     <div class="datalist" style="margin-top: 30px;">
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
                <td><input type="radio" role="chk" name="prj"  id="chkboxProductid" value='<%#Eval("productid")%>'/></td>
                <td><img style="height:80px" productid="<%#Eval("productid")%>" src="<%#Eval("ImageUrl1")%>" /></td>
                <td><%#Eval("productname")%></td>
                <td><%#Eval("productcode")%></td>
                <td><%#Eval("marketprice","{0:F2}")%></td>
                </tr>
            </ItemTemplate>


             <FooterTemplate>
            </table>
            </FooterTemplate>
         </asp:Repeater>
    </div>     

     
</div>
    
<script>
    $("#btnQd").click(function () {
        var url = $("input[name = 'prj']:checked").parent().next().find("img").attr("src");
        var productid = $("input[name = 'prj']:checked").parent().next().find("img").attr("productid");
        var name = $("input[name = 'prj']:checked").parent().next().next().html();
        var price = "￥"+$("input[name = 'prj']:checked").parent().next().next().next().next().html();
        window.parent.setImg(url,name,price,productid);
        window.parent.closeSelectPrj();
    });


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
