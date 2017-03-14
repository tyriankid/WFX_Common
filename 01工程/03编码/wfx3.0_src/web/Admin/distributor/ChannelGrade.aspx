<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="ChannelGrade.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ChannelGrade" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1>渠道商</h1>
            <span>渠道商信息的显示.增加,修改操作</span>
          </div>
      <div class="formitem validator4 clearfix">
        <ul>
          <li> <span class="formitemtitle Pw_110">渠道商名称</span><asp:TextBox ID="txtName" runat="server" CssClass="forminput"></asp:TextBox><p id="ctl00_contentHolder_txtNameTip"></p></li>         
          <li style="margin-bottom:10px;"> <span class="formitemtitle Pw_110">备注</span><asp:TextBox ID="txtRemark" runat="server" CssClass="forminput" TextMode="MultiLine" Width="350" Height="90"></asp:TextBox>
          </li>
      </ul>
      <ul class="btn Pa_198">
        <asp:Button ID="btnEditUser" runat="server" Text="确 定" OnClientClick="return CheckForm()"  CssClass="submit_DAqueding" />
        </ul>
      </div>

      </div>
  </div>

</asp:Content>
