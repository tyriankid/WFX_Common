<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BuyToHalf.aspx.cs" Inherits="Admin_promotion_BuyToHalf" MasterPageFile="~/Admin/Admin.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server"> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
      <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
        <h1>第二杯半价配置</h1>
        <span>第二杯半价活动启停配置</span>
      </div>
      <div class="datafrom">
          <div class="formitem validator1">
              <ul>
                  <li><span class="formitemtitle Pw_198"><em >*</em>第二杯半价状态：</span>
                      <asp:RadioButton ID="rbOn" runat="server" GroupName="Give" Text="开启" />
                      <asp:RadioButton ID="rbOff" runat="server" GroupName="Give" Checked="true" Text="停止" />
                      <p id="txtProductPointSetTip" runat="server">选择按钮控制开启或停止</p>
                  </li>
              </ul>
              <ul class="btntf Pa_198">
                    <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="submit_DAqueding inbnt" />
                    <%--OnClientClick="return PageIsValid();" --%>
	          </ul>
          </div>
      </div>
</div>  
<script>
    //function InitValidators() {
    //    initValid(new InputValidator('ctl00_contentHolder_txtProductPointSet', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '几元一积分不能为空,必须在0.1-10000000之间'))
    //    appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtProductPointSet', 0.1, 10000000, '几元一积分必须在0.1-10000000之间'));
    //}

    //$(document).ready(function () { InitValidators(); });
</script>
</asp:Content>

