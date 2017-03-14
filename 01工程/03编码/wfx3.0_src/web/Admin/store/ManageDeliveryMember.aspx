<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageDeliveryMember.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageDeliveryMember" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
             <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1>管理配送员</h1>
          </div>
          <asp:Panel ID="PanelID" runat="server">
                <div class="formitem validator4">
                <ul>
                  <li> <span class="formitemtitle Pw_110"><em >*</em>姓名：</span>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="forminput" />            
                    <p id="ctl00_contentHolder_txtUserNameTip">用户名不能为空，必须为汉字，长度在4-8个字符之间</p>
                  </li>
                   <li> <span class="formitemtitle Pw_110"><em >*</em>联系电话：</span>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="forminput" />
                    <p id="ctl00_contentHolder_txtPhoneTip">配送员的联系方式</p>
                  </li>
                   <li><span class="formitemtitle Pw_110"><em >*</em>性别：</span>
                    <asp:DropDownList runat="server" ID="DDLSex">
                        <asp:ListItem Value="0">男</asp:ListItem>
                        <asp:ListItem Value="1">女</asp:ListItem>
                       </asp:DropDownList>
                  </li>
                   <li><span class="formitemtitle Pw_110"><em >*</em>状态：</span>
                    <asp:DropDownList runat="server" ID="DDLState">
                        <asp:ListItem Value="0">正常</asp:ListItem>
                        <asp:ListItem Value="1">离职</asp:ListItem>
                       </asp:DropDownList>
                  </li>
                   <li> <span class="formitemtitle Pw_110"><em >*</em>所属门店：</span>
                    <asp:DropDownList runat="server" ID="DDLStore"></asp:DropDownList>
                     <p id="ctl00_contentHolder_DDLStoreTip">选择当前配送员所属的门店</p>
                  </li>
              </ul>
              <ul class="btn Pa_110">
                <asp:Button ID="btnCreate" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding" style="float:left;"/>
                </ul>
              </div>
        </asp:Panel>
    </div>
</div>

<div class="title"><span><asp:Literal ID="litTitle" runat="server" Visible="false"></asp:Literal></span></div>

<div class="databottom">
  <div class="databottom_bg"></div>
</div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtUserName', 4, 8, false, '[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*', '用户名不能为空，必须为汉字，长度在4-8个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtPhone', 11, 11, false, '^1(3[0-9]|4[57]|5[0-35-9]|8[0-9]|70)\\d{8}$', '请输入正确的手机号码'))
        initValid(new SelectValidator('ctl00_contentHolder_DDLStore', 1, 50, false, '请选择所属门店'))
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>

