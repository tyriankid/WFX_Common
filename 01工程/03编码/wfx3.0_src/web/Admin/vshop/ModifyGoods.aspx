<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ModifyGoods.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.ModifyGoods" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <script type="text/javascript">
        function btnAddButton() {
            var len = $("#tableInfo tr").length - 1;
            if (len == 7) {
                alert('商品增加最高限制7行');
                return;
            }
            var td1 = "<td><center><input role='laiyuan' id='" + guid() + "' type='text' /></center></td>";
            var td2 = "<td><input role='code' type='text' /></td>";
            var td3 = "<td class='td_txt_cenetr'><a href='#1' onclick='$(this).parent().parent().remove()'>删 除</a></td>";
            $("#tableInfo").append("<tr role='add'>" + td1 + td2 + td3 + "</tr>");
        }
        //Button输出获取表中input的值
        function saveTextIntoHiddenFeild() {
            var line = "";
            var source = "";
            var code = "";
            var bool = true;
            $("[role='add']").each(function () {
                //拼接guid
                line += $(this).find("[role='laiyuan']").attr("id") + ",";
                //拼接来源
                if (source != $(this).find("[role='laiyuan']").val()) {
                    line += $(this).find("[role='laiyuan']").val() + ",";
                } else {
                    bool = false;
                }
                //拼接code 
                if (code != $(this).find("[role='code']").val()) {
                    line += $(this).find("[role='code']").val() + ";";
                } else {
                    bool = false;
                }
                code = $(this).find("[role='code']").val();
                source = $(this).find("[role='laiyuan']").val();
            });
            line = line.substring(0, line.length - 1);
            if (bool == false) {
                alert("商品来源或者商品编码重复");
                return false;
            }
            //隐藏域获取值
            $("#info").val(line);
        }

        //载入
        function loadMatchInfoList() {
            var mainIdList = $("#mainId").val().split(';');
            var cIdList = $("#cId").val().split(';');
            var cSourceList = $("#cSource").val().split(';');
            var cCodeList = $("#cCode").val().split(';');
            if (mainIdList.leng == 1)
                //return;
                //先清除,在根据列表数量增加
                $("#tableInfo").html("");
            for (var i = 0; i < mainIdList.length; i++) {
                btnAddButton();
            }
            //循环赋初始值
            $("[role='add']").each(function (e) {
                $(this).find("[role='laiyuan']").attr("id", mainIdList[e]);
                $(this).find("[role='laiyuan']").val(cSourceList[e]);
                $(this).find("[role='code']").val(cCodeList[e]);
            });
        } 
        function guid() {
            function S4() {
                return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
            }
            return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
        }
        //页面载入时事件
        $(function () {
            loadMatchInfoList();
        });
    </script>
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/03.gif" width="32" height="32" /></em>
            <h1><strong class="colorG">
                <asp:Literal ID="productName" runat="server" /></strong>商品编码编辑</h1>
            <span>商品编码进行管理，您可以增加或者修改商品详细信息。</span>
        </div>
        <!--搜索-->
        <!--数据列表区域-->
        <div class="datalist">
            <div class="clearfix search_titxt2">
                <div class="searcharea clearfix br_search">
                    <ul class="btn Pa_160">
                        <input type="button" onclick="btnAddButton()" class="submit_DAqueding" value="增 加" />
                    </ul>
                    <table id="tableInfo">
                        <thead>
                            <tr class="table_title">
                                <td>商品来源
                                </td>
                                <td>商品编码
                                </td>
                                <td>操作</td>
                            </tr>
                        </thead>
                    </table>
                    <div class="Pg_15 Pg_010" style="text-align: center;">
                        <asp:Button runat="server" ID="btnAddButton" Text="保存" OnClientClick="return saveTextIntoHiddenFeild()" CssClass="submit_DAqueding" OnClick="btnAddButton_Click" />
                    </div>
                </div>
            </div>
        </div>
        <!--用户操作的隐藏域-->
        <input type="hidden" id="info" runat="server" clientidmode="Static" />
        <!--载入时的隐藏域-->
        <input type="hidden" runat="server" clientidmode="Static" id="mainId" />
        <input type="hidden" runat="server" clientidmode="Static" id="cId" />
        <input type="hidden" runat="server" clientidmode="Static" id="cSource" />
        <input type="hidden" runat="server" clientidmode="Static" id="cCode" />
</asp:Content>
