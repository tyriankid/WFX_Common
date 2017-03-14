<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageSign.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.ManageSign" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript" src="/Utility/swfupload/swfobject.js"></script>
    <script type="text/javascript">
        function copySuccess() {
            alert("该活动链接地址已经复制，你可以使用Ctrl+V 粘贴！");
        }
        var myHerf = window.location.host;
        var myproto = window.location.protocol;
        function bindFlashCopyButton(value, containerID) {
            var flashvars = {
                content: encodeURIComponent(myproto + "//" + myHerf + applicationPath + value),
                uri: '/Utility/swfupload/flash_copy_btn.png'
            };
            var params = {
                wmode: "transparent",
                allowScriptAccess: "always"
            };
            swfobject.embedSWF("/Utility/swfupload/clipboard.swf", containerID, "23", "12", "9.0.0", null, flashvars, params);
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <style>
        #on-off{
            display:inline-block;
            position:relative;
            width:100px;
            height:26px;
            line-height:26px;
            border-radius:5px;
            border:1px solid #428BCA;
            overflow:hidden;
        }
        #on-off ul{
            position:absolute;
            left:0;
            width:150px;
            height:inherit;
            overflow:hidden;
            cursor:pointer;
        }
        #on-off ul li{
            float:left;
            width:50px;
            height:inherit;
            text-align:center;
            clear: none;
            margin: 0;
            font-size: 14px;
            font-family: "Segoe UI", "Lucida Grande", Helvetica, Arial, "Microsoft YaHei", FreeSans, Arimo, "Droid Sans", "wenquanyi micro hei", "Hiragino Sans GB", "Hiragino Sans GB W3", "FontAwesome", sans-serif;
        }
        #on-off ul li:first-child{
            color: #fff;
            background: #428bca;
        }
        #on-off ul li:last-child{
            color: #000;
            background: #dddddd;
        }
    </style>
  <div class="dataarea mainwidth databody">
    <div class="title"> 
     <em><img src="../images/sign.png" width="32" height="32" /></em>
     <h1> 签到送积分活动设置 </h1>
     <span>您可以在此设置签到送积分的规则。</span>
    </div>
<div class="areacolumn clearfix" style="overflow:visible">
    <div class="columnright" style="overflow:visible">
        <div class="formitem validator4">
            <div style="margin-bottom:10px;">
                <span class="formitemtitle Pw_110">开启签到功能：</span>
                <div id="on-off">
                    <ul>
                        <li>ON</li>
                        <li></li>
                        <li>OFF</li>
                    </ul>
                </div>
            </div>
            
            <ul role="roleArea">

            </ul>

            <ul class="btn Pa_100 clearfix">
                <a href="javascript:addRole('','',false)" class="submit_DAqueding" style="display:inline-block;text-decoration:none;">增加一条</a>

                <asp:Button ID="btnSave" runat="server" OnClientClick="return dd();" Text="保 存" CssClass="submit_DAqueding" style="display:inline-block;" />
                <a href="javascript:reset()" class="submit_DAqueding" style="display:inline-block;text-decoration:none;">重置</a>
                <a href="javascript:void(0)" class="submit_DAqueding" style="display:inline-block;text-decoration:none;" id="btnCopy">复制链接</a>
            </ul>
         </div>
    </div>
</div>
</div>



<input type="hidden" runat="server" clientidmode="Static"  id="hidRoleInfo" />
<input type="hidden" runat="server" clientidmode="Static"  id="hidState" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script>
        $(function () {
            var a = 1;

            if ($("#hidState").val() == 0) {
                $("#on-off ul").animate({ left: "-50px" });
                $("#on-off").css("border", "1px solid #cccccc");
                a = 2;
            } else {
                $("#on-off ul").animate({ left: "0" });
                $("#on-off").css("border", "1px solid #428BCA");
                a = 1;
            }
            //on-off
            $("#on-off ul").click(function () {
                if (!$(this).is(":animated") && a==1) {
                    $(this).animate({ left: "-50px" });
                    $("#on-off").css("border", "1px solid #cccccc");
                    $("#hidState").val("0");
                    a = 2;
                }
                if (!$(this).is(":animated") && a==2) {
                    $(this).animate({ left: "0" });
                    $("#on-off").css("border", "1px solid #428BCA");
                    $("#hidState").val("1");
                    a = 1;
                }
            });

            //初始化页面
            var info = $("#hidRoleInfo").val();
            if (info.length > 0) {//根据隐藏域内的值加载页面
                var listInfo = info.split(';');
                var dayInfo = listInfo[0].split(',');
                var pointInfo = listInfo[1].split(',');
                for (var i = 0; i < dayInfo.length; i++) {
                    addRole(dayInfo[i], pointInfo[i],true);
                }
            }
            else {
                addRole('','',false);
            }
            //绑定复制链接按钮
            bindFlashCopyButton("/Vshop/Sign.aspx", 'btnCopy');
        });
        var c = 0;
        function addRole(day,point,readonly) {
            c++;
            var strReadonly = readonly ? "readonly='true'" : "";
            var roleHtml = "<li><span class='formitemtitle Pw_110' >每连续签到：</span><input class='forminput' type='text' id='day_" + c + "' role='PerContinuitySignCounts' value='" + day + "'  " + strReadonly + " /> 天</li>";
            roleHtml += "<li><span class='formitemtitle Pw_110'>送积分：</span><input type='text' class='forminput' id='point_" + c + "' role='PerSignPoints' value='" + point + "'   /> 个</li>";
            $("[role='roleArea']").append(roleHtml);
            initValid(new InputValidator('day_' + c, 1, 3, false, '[1-9]+\\d*', '请输入有效的天数'));
            initValid(new InputValidator('point_' + c, 1, 4, false, '[1-9]+\\d*', '请输入有效的积分'));
        }

        function reset() {
            $("[role='roleArea']").html('');
            addRole('', '', false);
        }



        function dd() {
            if (!PageIsValid()) return false;

            var flag = true;
            var $hidRoleInfo = $("#hidRoleInfo");
            
            var days = "";
            var points = "";
            $("[role='PerContinuitySignCounts']").each(function () {
                days = days + $(this).val() + ",";
            });
            
            $("[role='PerSignPoints']").each(function () {
                points = points + $(this).val() + ",";
            });
            
            days = days.substr(0, days.length - 1);
            points = points.substr(0, points.length - 1);
            
            //签到天数重复判断
            var dayList = days.split(',');
            for (var i = 0; i < dayList.length; i++) {
                for (var j = 0; j < dayList.length; j++) {
                    if (i!=j && dayList[i] == dayList[j]) {
                        alert("天数不能重复!");
                        flag = false;
                        return false;
                    }
                }
            }
            $("#hidRoleInfo").val(days + ";" + points);
            return flag;
        }

    </script>


</asp:Content>
