﻿<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditManager.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditManager" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1 class="title_line">编辑管理员信息</h1>
          </div>
          <div class="formtab Pg_45">
            <ul>
                <li class="visited">基本信息</li>                                      
               <li><a href='<%="EditManagerPassword.aspx?userId="+Page.Request.QueryString["userId"] %>'>修改密码</a></li>
            </ul>
          </div>
      <div class="formitem validator2">
        <ul>
          <li> <span class="formitemtitle Pw_100">用户名：</span>
          <strong class="colorG"><asp:Literal ID="lblLoginNameValue" runat="server" /></strong></li>
          <li> <span class="formitemtitle Pw_100"><em >*</em>所属部门：</span><abbr class="formselect">
           <Hi:RoleDropDownList ID="dropRole" runat="server" AllowNull="false" />
          </abbr></li>
          <li> <span class="formitemtitle Pw_100"><em >*</em>邮件地址：</span>
           <asp:TextBox ID="txtprivateEmail" CssClass="forminput" runat="server"></asp:TextBox>
           <p id="ctl00_contentHolder_txtprivateEmailTip">请输入正确电子邮件，长度在1-256个字符以内</p>
          </li>
            <li> <span class="formitemtitle Pw_100">后台昵称：</span>
           <asp:TextBox ID="txtAgentName" CssClass="forminput" MaxLength="20" runat="server"></asp:TextBox>
           <p>后台昵称用于代理商订单相关信息的显示，如：武汉代理商</p>
          </li>

          <li> <span class="formitemtitle Pw_100">激活状态：</span>
           <asp:DropDownList ID="DDLState" runat="server">
               <asp:ListItem Value="1">未激活</asp:ListItem>
               <asp:ListItem Value="0">已激活</asp:ListItem>
              </asp:DropDownList>
           <p>激活后台代理商账号</p>
          </li>

          <li> <span class="formitemtitle Pw_100">注册日期：</span><Hi:FormatedTimeLabel ID="lblRegsTimeValue" runat="server" /> </li>
      </ul>
      <ul class="btn Pa_100">
        <asp:Button ID="btnEditProfile" runat="server" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding" Text="保 存" />
        </ul>
      </div>

      </div>
  </div>
<div class="databottom">
  <div class="databottom_bg"></div>
</div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" Runat="Server">
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtprivateEmail', 1, 256, false, '[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\.[\\w-]+)+', '请输入正确电子邮件，长度在1-256个字符以内'));
        }
        
        function link()
        {
            window.location.href='Managers.aspx';
        }
			        
        $(document).ready(function() { InitValidators(); });
    </script>
</asp:Content>

