<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductSettings.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.ProductSettings" Title="无标题页" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title ">
                <em>
                    <img src="../images/03.gif" width="32" height="32" /></em>
                <h1>
                    商品权限配置</h1>
                <span>管理员可为商品设置特殊的权限。</span>
            </div>
            <div class="formitem validator2">
                <ul>
                    <li><span class="formitemtitle Pw_110">商品评论权限：</span>
                        <input id="radioreviewson" type="radio" name="radioreviews" runat="server" value="1" />开启
                        <input id="radioreviewsoff" type="radio" name="radioreviews" runat="server" value="2" />关闭（管理员可在商品评论页面为商品添加评论)
                    </li>
                </ul>
                <ul class="btn Pa_100 clearfix">
                    <asp:Button ID="btnSave" runat="server"  OnClick="btnSave_Click"
                        Text="保存" CssClass="submit_DAqueding float" />
                </ul>
            </div>
        </div>
    </div>
    <script type="text/javascript">


    </script>
</asp:Content>