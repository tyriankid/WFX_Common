<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetOrderAlert.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SetOrderAlert" %>
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
        <h1>其他设置</h1>
        <span>商城首页公告配置和商城最低消费设置</span>
      </div>
      <div class="datafrom">
          <div class="formitem validator1">
           <ul>
            <li><span class="formitemtitle Pw_198"><em >*</em>商城公告：</span>
              <asp:TextBox ID="txtAlert" runat="server" CssClass="forminput" />
              <p id="txtAlertTip" runat="server">显示在商城首页的滚动条内容</p>
            </li>
            <li><span class="formitemtitle Pw_198"><em >*</em>起送价、配送费：</span>
                <input onkeyup="value=value.replace(/\D/g,'')" type="button" id="addSubject" value="增加一个区间" onclick="addSub()"/>
                <p id="txtKeywordTip">设置配送距离和对应的起送价、配送费,可增加任意区间</p>
            </li>
            <li id="subArea" style="display:none">
                <table id="subTable">

                </table>
            </li>

              <ul class="btntf Pa_198">
                    <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="submit_DAqueding inbnt" OnClientClick ="return saveSubs()"  />
	          </ul>
          </div>
      </div>
</div>  

    <input type="hidden" runat="server" id="hidSubInfo" clientidmode="Static" />

    <script src="/Utility/jquery-1.6.4.min.js"></script>
    <script>

        function addSub() {
            var sbjCount = $("[role='sub']").length + 1;

            var inputHtml = "<tr role='sub'>" +
                                                 "<td>起送距离：" + "<input onkeyup=\"value=value.replace(/\\D/g,'')\" type='text'  role='start_" + sbjCount + "'  style='width:100px;height:25px'/>公里 至</td>" +
                                                 "<td><input onkeyup=\"value=value.replace(/\\D/g,'')\" type='text'  role='end_" + sbjCount + "'  style='width:100px;height:25px'/>公里 ，</td>" +
                                                 "<td>起送价：<input onkeyup=\"value=value.replace(/\\D/g,'')\" type='text'  role='minPrice_" + sbjCount + "'  style='width:100px;height:25px'/>元 ，</td>" +
                                                 "<td>配送费：<input onkeyup=\"value=value.replace(/\\D/g,'')\" type='text'  role='roldPrice_" + sbjCount + "'  style='width:100px;height:25px'/>元 </td>" +
                                                 "<td>满：<input onkeyup=\"value=value.replace(/\\D/g,'')\" type='text'  role='freePrice_" + sbjCount + "'  style='width:100px;height:25px'/>元免配送费 </td>" +
                                        "</tr>";
            $("#subTable").append(inputHtml);
        }

        function initSbjInfo() {
            $("#subArea").show();
            //根据当前题目和答案的隐藏域凑行html
            var sbjList = $("#hidSubInfo").val().split(';');//题目列
            var inputHtml = "";
            for (var i = 1; i <= sbjList.length; i++) {
                var valueList = sbjList[i - 1].split(',');
                 inputHtml += "<tr role='sub'>" +
                         "<td>起送距离：" + "<input onkeyup=\"value=value.replace(/\\D/g,'')\" type='text'  role='start_" + i + "' value='" + valueList[0] + "' style='width:100px;height:25px'/>公里 至</td>" +
                         "<td><input onkeyup=\"value=value.replace(/\\D/g,'')\" type='text'  role='end_" + i + "' value='" + valueList[1] + "' style='width:100px;height:25px'/>公里 ，</td>" +
                         "<td>起送价：<input onkeyup=\"value=value.replace(/\\D/g,'')\" type='text'  role='minPrice_" + i + "' value='" + valueList[2] + "'  style='width:100px;height:25px'/>元 ，</td>" +
                         "<td>配送费：<input onkeyup=\"value=value.replace(/\\D/g,'')\" type='text'  role='roldPrice_" + i + "'  value='" + valueList[3] + "' style='width:100px;height:25px'/>元 </td>" +
                         "<td>满：<input onkeyup=\"value=value.replace(/\\D/g,'')\" type='text'  role='freePrice_" + i + "'  value='" + valueList[4] + "' style='width:100px;height:25px'/>元免配送费 </td>" +
                "</tr>";
            }
            $("#subTable").append(inputHtml);
        }

        function saveSubs() {
            var subInfo = "";
            var flag = true;
            if ($("tr[role='sub']").length <= 0) {
                alert("您还未增加至少一个区间!"); flag = false; return false;
            }
            $("tr[role='sub']").each(function () {
                $(this).find("input[type=text]").each(function () {
                    if ($(this).val() == "") {
                        alert("请将内容填写完整!"); flag = false; return false;
                    }
                    subInfo += $(this).val() + ",";
                });
                subInfo = subInfo.substr(0, subInfo.length - 1);
                subInfo += ";";
            });
            subInfo = subInfo.substr(0, subInfo.length - 1);
            $("#hidSubInfo").val(subInfo);
            return flag;
        }

        $(document).ready(function () {
            initSbjInfo();
        });
</script>
</asp:Content>

