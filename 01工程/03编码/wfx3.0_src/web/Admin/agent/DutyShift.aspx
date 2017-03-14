<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeFile="DutyShift.aspx.cs" Inherits="Hidistro.UI.Web.Admin.agent.DutyShift" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<style>
.areacolumn{
    border:1px solid #D8D8D8;
}
.areacolumn tr td{
    background:#fff;
    text-align:center;
}
.areacolumn tr:first-child td{
    background:#EBEBEB;
}
.areacolumn tr td input{
    width: 80px;
    height: 36px;
    background-color: #1195DB;
    color: #fff;
    border-radius: 5px;
    margin: 10px;
}
</style>
    <div class="dataarea mainwidth databody">
    <div class="title">
		  <em><img src="../images/daka.png" width="32" height="32" /></em>
          <h1><strong>开班/关班</strong></h1>
          <span>上下班注意打卡</span>
		</div>
<table class="areacolumn clearfix2" width="100%" cellpadding="5" cellspacing="5" border="1">
    <tr>
        <td>用户名</td>
        <td>当前时间</td>
        <td>时间段</td>
        <td>总时长</td>
        <td>收入总额</td>
        <td>订单统计</td>
        <td>销售</td>
    </tr>
    <tr>
        <asp:Literal ID="litUserInfo" runat="server"></asp:Literal>
        <asp:Literal ID="litTimeInfo" runat="server"></asp:Literal>
        <asp:Literal ID="litOrderInfo" runat="server"></asp:Literal>
        <asp:Literal ID="litSaleInfo" runat="server"></asp:Literal>
    </tr>

    <tr>
        <td colspan="7"><asp:Button runat="server" ID="btnDutyOn" Text="打卡" />
        <asp:Button runat="server" ID="btnDutyOff" Text="关班" OnClientClick="return goDutyOff()" /></td> 
    </tr>
</table>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script>
        $(function () {
            if ($("#DutyHours").html() != undefined)
                $("#ctl00_contentHolder_btnDutyOn").hide();
            else
                $("#ctl00_contentHolder_btnDutyOff").hide();
        });
        function goDutyOff() {
            return confirm("确定要关班吗？");
        }
        
    </script>
</asp:Content>

