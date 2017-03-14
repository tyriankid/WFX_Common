<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeFile="Closed.aspx.cs" Inherits="Hidistro.UI.Web.Admin.agent.Closed" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<style>
.areacolumn tr td{
    background:#fff;
    text-align:center;
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
          <h1><strong>打烊</strong></h1>
          <span>三作咖啡打烊</span>
		</div>
            <li>
            <span class="formitemtitle Pw_110">三作咖啡打烊设置：</span>
            <input id="radioOpenStoreInfoSanzuoOn" type="radio" name="OpenStoreInfo" runat="server" value="1"/>开启
            <input id="radioOpenStoreInfoSanzuoOff" type="radio" name="OpenStoreInfo" runat="server" value="2"/>关闭（默认为关闭，开启后，可以对三作咖啡店铺进行打烊的操作）
            </li>
        <div class="Pg_15 Pg_010" style="text-align: center;">
        <asp:Button runat="server" ID="btnDutyOff" Text="保存"  CssClass="submit_DAqueding" OnClientClick="return goDutyOff()" />
         </div> 
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script> 
        
        function goDutyOff() {
            return confirm("确定要修改吗？");
        }

    </script>
</asp:Content>

