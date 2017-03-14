<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RegAgent.aspx.cs" Inherits="Hidistro.UI.Web.Admin.RegAgent" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" >
    <style>
         #mainhtml { overflow: visible; }
        .areacolumn .columnright .formitem li, .databody .datafrom .formitem li, .Pop_up .mianform li, .dataarea .areaform li {
        overflow:visible;
        }
    </style>
<div class="areacolumn clearfix" style="overflow:visible">
      <div class="columnright" style="overflow:visible">
          <div class="title">
             <em><img src="images/04.gif" width="32" height="32" /></em>
            <h1>注册代理商</h1>
            <span>注册后台代理商账号，以便完成后台采购流程</span>
          </div>
          <asp:Panel ID="PanelID" runat="server">
                <div class="formitem validator4">
                <ul>
                  <li> <span class="formitemtitle Pw_110"><em >*</em>用户名：</span>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="forminput" />            
                    <p id="ctl00_contentHolder_txtUserNameTip">用户名不能为空，必须以汉字或是字母开头,且在3-20个字符之间</p>
                  </li>
                   <li> <span class="formitemtitle Pw_110"><em >*</em>密码：</span>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="forminput" />
                    <p id="ctl00_contentHolder_txtPasswordTip">密码长度在6-20个字符之间</p>
                  </li>
                   <li> <span class="formitemtitle Pw_110"><em >*</em>确认密码：</span>
                    <asp:TextBox ID="txtPasswordagain" runat="server" TextMode="Password" CssClass="forminput" />
                    <p id="ctl00_contentHolder_txtPasswordagainTip">请重复一次上面输入的登录密码</p>
                  </li>
                   <li> <span class="formitemtitle Pw_110"><em >*</em>电子邮件地址：</span>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="forminput" />
                    <p id="ctl00_contentHolder_txtEmailTip">请输入有效的电子邮件地址，电子邮件地址的长度在256个字符以内</p>
                  </li>
                  <li> <span class="formitemtitle Pw_110"><em >*</em>所属部门：</span><abbr class="formselect">
                    <Hi:RoleDropDownList ID="dropRole" runat="server" AllowNull="false" />
                  </abbr></li>
                  <li><span class="formitemtitle Pw_110"><em >*</em>后台昵称：</span><asp:TextBox ID="txtAgentName" runat="server" MaxLength="20" CssClass="forminput" />            
                    <p id="ctl00_contentHolder_txtAgentNameTip">后台昵称不能为空，必须以汉字或是字母开头,且在3-20个字符之间</p>
                  </li>
                  
                  <li><span class="formitemtitle Pw_110"><em >*</em>收货手机号：</span>
                      <asp:TextBox ID="txtCellPhone" runat="server" CssClass="forminput" />
                      <p id="ctl00_contentHolder_txtCellPhoneTip">请输入有效的手机号</p>
                  </li>

                  <li><span class="formitemtitle Pw_110"><em >*</em>收货人姓名：</span>
                      <asp:TextBox ID="txtShipName" runat="server" CssClass="forminput" />
                      <p id="ctl00_contentHolder_txtShipNameTip">收货人真实姓名</p>
                  </li>

                  <li><span class="formitemtitle Pw_110"><em >*</em>收货地址：</span>
                      <Hi:RegionSelector ID="dropRegion" runat="server" IsShift="false" ProvinceWidth="180" CityWidth="150" CountyWidth="150" />
                  </li>
                  <li style="overflow:hidden">
                      <span class="formitemtitle Pw_110"><span style="width:10px;height:10px;display:block"></span></span>
                      <asp:TextBox ID="txtAddress" runat="server" MaxLength="200" CssClass="forminput" Height="46px" Width="350px" TextMode="MultiLine"  />
                      <p id="ctl00_contentHolder_txtAddressTip">真实的收货地址</p>
                  </li>


              </ul>
              <ul class="btn Pa_110">
                <asp:Button ID="btnCreate" runat="server" OnClientClick="return valiRegion();" Text="添 加"  CssClass="submit_DAqueding" style="float:left;"/>
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
        initValid(new InputValidator('ctl00_contentHolder_txtUserName', 3, 20, false, '[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*', '用户名不能为空，必须以汉字或是字母开头,且在3-20个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtPassword', 6, 20, false, null, '密码长度在6-20个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtPasswordagain', 6, 20, false, null, '请重复一次上面输入的登录密码'))
        appendValid(new CompareValidator('ctl00_contentHolder_txtPasswordagain', 'ctl00_contentHolder_txtPassword', '重复密码错误'));
        initValid(new InputValidator('ctl00_contentHolder_txtEmail', 1, 256, false, '[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\.[\\w-]+)+', '请输入有效的电子邮件地址，电子邮件地址的长度在256个字符以内'))
        initValid(new SelectValidator('ctl00_contentHolder_dropRole', false, '选择管理员要加入的部门'))
        initValid(new InputValidator('ctl00_contentHolder_txtAgentName', 3, 20, false, '[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*', '后台昵称不能为空，必须以汉字或是字母开头,且在3-20个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtCellPhone', 1, 11, false, '^[1][3-8]+\\d{9}', '请输入有效的手机号'))
        initValid(new InputValidator('ctl00_contentHolder_txtShipName', 2, 8, false, null, '姓名在2-4个字符之间'))
        initValid(new InputValidator('ctl00_contentHolder_txtAddress', 1, 1024, false, null, '请输入有效的收货地址'))
        //initValid(new InputValidator('regionSelectorValue', 1, 5, false, null, '请选择有效的省市区'))
    }
    function valiRegion() {
        var flag = PageIsValid();
        if ($("#regionSelectorValue").val().length > 0 && $("#regionSelectorValue").val().length < 5) {
            flag = true;
        }
        else {
            flag = false;
            alert("请选择有效的省市区");
        }
        return flag;
    }
    $(document).ready(function () { InitValidators(); });
</script>
</asp:Content>

