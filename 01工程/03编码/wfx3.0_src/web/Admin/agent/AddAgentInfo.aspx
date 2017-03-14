<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeFile="AddAgentInfo.aspx.cs" Inherits="Hidistro.UI.Web.Admin.agent.AddAgentInfo" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix2">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1>升级会员为代理商</h1>
            <span><asp:Literal runat="server" ID="litTitle" Text=""></asp:Literal></span>
          </div>
        <div class="formitem validator4">
        <ul>
          <li> <span class="formitemtitle Pw_110"><em >*</em>代理商账号：</span>
            <asp:TextBox ID="txtUserName" runat="server" CssClass="forminput" autocomplete="off" Enabled="false" />        
            <p id="ctl00_contentHolder_txtUserNameTip">账号不能为空，必须以汉字或是字母开头,且在3-20个字符之间</p>
          </li>
           <%--<li> <span class="formitemtitle Pw_110"><em >*</em>密码：</span>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="forminput" autocomplete="off" />
            <p id="ctl00_contentHolder_txtPasswordTip">密码长度在6-20个字符之间</p>
          </li>
           <li> <span class="formitemtitle Pw_110"><em >*</em>确认密码：</span>
            <asp:TextBox ID="txtPasswordagain" runat="server" TextMode="Password" CssClass="forminput" autocomplete="off" />
            <p id="ctl00_contentHolder_txtPasswordagainTip">请重复一次上面输入的登录密码</p>
          </li>--%>
          <li> <span class="formitemtitle Pw_110"><em >*</em>代理商等级：</span><abbr class="formselect"/>
            <Hi:AgentGradeDropDownList ID="ddlAgentGrade" runat="server" AllowNull="false" Height ="30" />
          <p id="ctl00_contentHolder_ddlAgentGradeTip">请选择代理商等级</p></li>
            <li> <span class="formitemtitle Pw_110"><em >*</em>店铺名称：</span>
            <asp:TextBox ID="txtStoreName" runat="server" CssClass="forminput" autocomplete="off" />
            <p id="ctl00_contentHolder_txtStoreNameTip">店铺名称不能为空，长度在2-50个字符之间</p>
          </li>
            <li> <span class="formitemtitle Pw_110">推荐代理商：</span>
            <asp:TextBox ID="txtReferralUserId" runat="server" CssClass="forminput" autocomplete="off" />
            <p id="P1"></p>
          </li>
      </ul>
      <ul class="btn Pa_110">
        <asp:Button ID="btnCreate" runat="server" OnClientClick="return PageIsValidEx();" Text="确 定"  CssClass="submit_DAqueding" style="float:left;"/>
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
    </abbr>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<Hi:Script ID="Script1" runat="server" Src="/utility/jquery.bigautocomplete.js"></Hi:Script>
<hi:style ID="Style1" runat="server" href="/utility/jquery.bigautocomplete.css" media="screen" />
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtUserName', 3, 20, false, '[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*', '用户名不能为空，必须以汉字或是字母开头,且在3-20个字符之间'))
        //initValid(new InputValidator('ctl00_contentHolder_txtPassword', 6, 20, false, null, '密码长度在6-20个字符之间'))
        //initValid(new InputValidator('ctl00_contentHolder_txtPasswordagain', 6, 20, false, null, '请重复一次上面输入的登录密码'))
        //appendValid(new CompareValidator('ctl00_contentHolder_txtPasswordagain', 'ctl00_contentHolder_txtPassword', '重复密码错误'));
        //initValid(new SelectValidator('ctl00_contentHolder_ddlAgentGrade', false, '选择代理商所属的代理商等级'))
        initValid(new InputValidator('ctl00_contentHolder_txtStoreName', 2, 50, false, null, '店铺名称不能为空，长度在2-50个字符之间'))
    }
    $(document).ready(function () { InitValidators(); });

    // 2015-8-20 by jinhb
    function PageIsValidEx() {
        //添加确定信息 
        var name = $("#<%=txtUserName.ClientID%>").attr("value");
        var grade = $("#<%=ddlAgentGrade.ClientID%>").find("option:selected").text();
        var store = $("#<%=txtStoreName.ClientID%>").attr("value"); 
        var refeId = $("#<%=txtReferralUserId.ClientID%>").attr("value");
        var msg = "确认一下信息无误吗?\n(注意:如果推荐代理商输入错误,按照无推荐代理商处理!)\n代理商账号:" + name + "\n代理商等级:" + grade + "\n店铺名称:" + store + "\n推荐代理商:" + refeId;
        var isCon = window.confirm(msg);
        if (!isCon) { return false;}
        
        var isValid = true;
        var validateGroup = "default";// 默认分组

        if (arguments.length > 0)
            validateGroup = arguments[0];
        var ctls = $("[ValidateGroup='" + validateGroup + "']");
        ctls.each(function () {
            if ($("#" + this["id"]).get(0).validator != undefined && $("#" + this["id"]).get(0).validator != null) {
                for (var i = 0 ; i < $("#" + this["id"]).get(0).validator.length; i++) {
                    if ($("#" + this["id"]).get(0).validator[i]._IsValid == false) {
                        $("#" + this["id"]).get(0).validator[i].UpdateStatus();
                        isValid = false;
                    }
                }
            }
        }); 
        if ($("#ctl00_contentHolder_ddlAgentGrade").val() == "") {
            alert('请选择代理商所属的代理商等级！');
            return false;
        }
        
        return isValid;
    }



    // 2015-8-20 by jinhb 无刷新获取推荐ID
    $(function () {
        var searchajax = "?action=SearchKey";
        $("#ctl00_contentHolder_txtReferralUserId").bigAutocomplete({ url: searchajax, width: 443 });

        $("#ctl00_contentHolder_txtReferralUserId").keypress(function (e) {
            if (e.which == 13) {
                return false;
            }
        });
    });
</script>
</asp:Content>

