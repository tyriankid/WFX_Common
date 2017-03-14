<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="addWKM.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.addWKM" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
    <div class="areacolumn clearfix">
        <div class="columnright">
        <div class="title">
        <em><img src="../images/06.gif" width="32" height="32" /></em>
        <h1>添加微信答题活动</h1>
        <span>填写题目数量和题目内容与选项详细信息</span>
        </div>  
        <div class="formitem validator2">
        <ul>
            <li><span class="formitemtitle Pw_128"><em >*</em>活动描述：</span>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="forminput"></asp:TextBox>
            </li>
            <li><span class="formitemtitle Pw_128"><em >*</em>用户分享标题：</span>
                <asp:TextBox ID="txtShareTitle" runat="server" CssClass="forminput"></asp:TextBox>
            </li>
            <li><span class="formitemtitle Pw_128"><em >*</em>用户分享描述：</span>
                <asp:TextBox ID="txtShareDescription" runat="server" CssClass="forminput"></asp:TextBox>
            </li>
			<li> <span class="formitemtitle Pw_128"><em >*</em>开始日期：</span>
                <ui:webcalendar runat="server" CssClass="forminput" ID="calendarStartDate" />
            </li>
            <li> <span class="formitemtitle Pw_128"><em >*</em>结束日期：</span>
                <ui:webcalendar runat="server" CssClass="forminput" ID="calendarEndDate" />
            </li>
            <li><span class="formitemtitle Pw_128"><em >*</em>增加问题与答案：</span>
                <input type="button" id="addSubject" value="增加一个"/>
                <p id="txtKeywordTip"> 一个题目配有四个答案框，答案可不填满四个。</p>
            </li>
            
            <li id="subjectArea" style="display:none">
                <table id="subjectTable">

                </table>
            </li>

        </ul>
        <ul class="btn Pa_100 clearfix">
            <asp:Button ID="btnAddWKM" runat="server" OnClientClick="return PageIsValid();"
                Text="保 存" CssClass="submit_DAqueding" onclick="btnAddWKM_Click"/>
        </ul>
        </div>
    </div>
    </div>

    <input type="hidden" runat="server" id="subjectInfo" /><!--问题描述信息-->
    <input type="hidden" runat="server" id="optionInfo" /><!--答案信息-->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#addSubject").click(function () { addSubject() })
        });

        function addSubject() {
            $("#subjectArea").show();
            //根据当前题目的数量拼凑行html
            var sbjCount = $("tr[name=subject]").length + 1;

            var inputHtml = "<tr name='subject'>";
            inputHtml += "<td>题目" + sbjCount + "：<input type='text' id='sbj_" + sbjCount + "'   style='width:200px;height:25px'/></td>";
            for (var i = 1; i <= 4; i++) {
                inputHtml += "<td>" + i + "：<input type='text' id='option_" + sbjCount + "_" + i + "'  style='width:150px;height:25px'/></td>";
            }
            inputHtml += "<td><input type='file' id='img_" + sbjCount + "' name='img_" + sbjCount + "' /></td>";

            inputHtml += "</tr>";
            $("#subjectTable").append(inputHtml);
        }

        function PageIsValid() {
            var flag = true;
            if ($("#txtDescription").val() == null || $("#txtDescription").val() == "") {
                alert("请填写活动描述");
                return false;
            }
            if ($("#txtShareTitle").val() == null || $("#txtShareTitle").val() == "") {
                alert("请填写用户分享标题");
                return false;
            }
            if ($("#txtShareDescription").val() == null || $("#txtShareDescription").val() == "") {
                alert("请填写用户分享描述");
                return false;
            }
            if ($("#calendarStartDate").val() == null || $("#calendarStartDate").val() == "") {
                alert("请选择开始日期");
                return false;
            }
            if ($("#calendarEndDate").val() == null || $("#calendarEndDate").val() == "") {
                alert("请选择结束日期");
                return false;
            }
            if ($("tr[name=subject]").length == 0) {
                alert("请添加题目");
                return false;
            }

            //将题目的数量与答案拼接成字符串放在隐藏域内
            var sbjContent = "";
            var optionContent = "";
            $("tr[name=subject]").each(function (i) {
                $(this).find("input[type=text]").each(function (e) {
                    if (e == 0)//题目内容
                    {
                        if ($(this).val() == "" || $(this).val() == null)//验证问题描述是否填写完毕
                        {
                            alert("请将问题内容填写完整");
                            flag = false;
                            return false;
                        }
                        sbjContent += $(this).val() + ";";
                    }
                    else//答案内容
                    {
                        if ($(this).val() == "" && e <= 2)//验证是否至少有两个答案填写完毕
                        {
                            alert("请至少填写两个答案!");
                            flag = false;
                            return false;
                        }
                        if ($(this).val() != "" && $(this).val() != null)
                            optionContent += $(this).val() + "/";
                    }
                });
                optionContent = optionContent.substr(0, optionContent.length - 1);
                optionContent += ";";
            });
            sbjContent = sbjContent.substr(0, sbjContent.length - 1);
            optionContent = optionContent.substr(0, optionContent.length - 1);
            $("#subjectInfo").val(sbjContent);
            $("#optionInfo").val(optionContent);
            return flag;
        }
    </script>
</asp:Content>
