<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Login" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <Hi:HeadContainer ID="HeadContainer1" runat="server" />
    <Hi:PageTitle ID="PageTitle1" runat="server" />
    <link href="css/login.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Utility/jquery-1.6.4.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login_box">
    <div class="login-logo"><img src="images/login_logo.png" /></div>
    <ul class="login-formbox clear">
        <li>
	    <dl class="clear">
        <dt>账号：</dt>
        <dd><span class="user_bg"><asp:TextBox ID="txtAdminName" CssClass="user" runat="server"></asp:TextBox></span></dd>


        </dl>
        </li>

        <li>
	        <dl class="clear">
            <dt>密码：</dt>
            <dd><span class="pwd_bg"><asp:TextBox ID="txtAdminPassWord" CssClass="pwd" runat="server" TextMode="Password" /></span></dd>
            </dl>
        </li>

	    <%--<dl class="clear">--%>
            <li runat="server" id="imgCode" visible="true" class="clear">
                <dl>
        <dt>验证码：</dt>
        <dd><span class="yanzh_bg"><asp:TextBox ID="txtCode" runat="server" class="yanzh" MaxLength="4"></asp:TextBox></span><span class="yanzh_img">
            <img id="img_txtCode" src="" alt="" class="img_txtCode" />     <img id="imgVerifyCode" src="../VerifyCodeImage.aspx" alt="" style="border-style: none" class="img_txtCode_img" /></span>
        </dd>
        <%--</dl>--%>
                    </dl>
        </li>
        <li id="regAgent" style="display:none;text-align: center;padding-right: 50px;padding-top: 10px;">
            <a href="RegAgent.aspx" style="color: #B5B5B5;">注册后台代理商</a>
        </li>
        <div class="tips">
            <Hi:SmallStatusMessage ID="lblStatus" runat="server" Visible="False" Width="260px" />
        </div>
        <div class="tijiao_bg"> <asp:Button ID="btnAdminLogin" runat="server" Text="" CssClass="tijiao" /></div>
    </ul>
    <p>Copyright © 众赞移动云商 All Rights Reserved.</p>
    <input type="hidden" id="specialHideShow" runat="server" clientidmode="Static" />
    </div>
    <%--<asp:Panel ID="Panel1" runat="server" DefaultButton="btnAdminLogin">
             <div class="login">
	        <p><asp:TextBox ID="txtAdminName" CssClass="input_txt" runat="server"></asp:TextBox></p>
	        <p><asp:TextBox ID="txtAdminPassWord" CssClass="input_txt" runat="server" TextMode="Password" /></p>
	        <p><span id="divYZM"><asp:TextBox ID="txtCode" runat="server" CssClass="yzm" Size="9" MaxLength="4"></asp:TextBox></span>
            <span id="divYZMTP"><img id="img_txtCode" src="" alt="" /><img id="imgVerifyCode" src='<%= Globals.ApplicationPath + "/VerifyCodeImage.aspx" %>'
                                        style="border-style: none" onclick="javascript:refreshCode();" /></span>
            </p>
	        <p><div id="divOK" ><asp:Button ID="btnAdminLogin" runat="server" Text="" CssClass="login_btn" /></div> 		<div id="divTS"><Hi:SmallStatusMessage ID="lblStatus" runat="server" Visible="False" Width="260px" /></div>
                <p>
                </p>
                 </p>
        </div>
    </asp:Panel>--%>
    </form>

    <script language="javascript" type="text/javascript">
        function refreshCode() {
            var img = document.getElementById("imgVerifyCode");
            if (img != null) {
                var currentDate = new Date();
                img.src = '<%= Globals.ApplicationPath + "/VerifyCodeImage.aspx?t=" %>' + currentDate.getTime();
            }
        }
        $(document).ready(function () {
            $("#img_txtCode").hide();
            $("#txtCode").keyup(function () {
                var value = $(this).val();
                var temp;
                if (value.length < 4) {
                    $("#img_txtCode").hide();
                    temp = "";
                }
                else if (value.length == 4) {
                    if (temp != value) {
                        $("#img_txtCode").show();
                        $.ajax({
                            url: "Login.aspx",
                            type: 'post', dataType: 'json', timeout: 10000,
                            data: {
                                isCallback: "true",
                                code: $("#txtCode").val()
                            },
                            async: false,
                            success: function (resultData) {
                                var flag = resultData.flag;
                                if (flag == "1") {
                                    $("#img_txtCode").attr("src", "images/true.png");
                                }
                                else if (flag == "0") {
                                    $("#img_txtCode").attr("src", "images/false.png");
                                }
                            }
                        });
                    }
                    temp = value;
                }
            });

            //针对不同的用户进行不同的功能区域隐藏显示
            var customName = "";
            if ($("#specialHideShow").val()) {
                customName = $("#specialHideShow").val();
                switch (customName) {
                    case "jxjj"://玖信健佳:打开后台代理商注册按钮
                        $("#regAgent").show();
                        break;
                }
            }

        });
    </script>

</body>
</html>
