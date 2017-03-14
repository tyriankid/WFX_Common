<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductDistributor.aspx.cs" Inherits="Hidistro.UI.Web.Admin.vshop.ProductDistributor" %>

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
            var td1 = "<td><input id='" + guid() + "' type='text'/></a></td>";
            var td2 = "<td><input type='text'/></td>";
            var td3 = "<td class='td_txt_cenetr'><a href='#1' onclick='$(this).parent().parent().remove()'>删 除</a></td>";
            $("#tableInfo").append("<tr role='add'>" + td1 + td2 + td3 + "</tr>");
        }
        function test2() {
            $("#tableInfo tr").each(function (i) {
                if (i > 0) {
                    alert($("#tableInfo tr").eq(i).find("input:first").val());
                    //alert($("#tableInfo tr").eq(i).find("input:eq(0)").val());
                }
            });
        }
        //Button输出获取表中input的值
        function saveTextIntoHiddenFeild() {
            var line = "";
            $("[role='add']").each(function () {
                //拼接guid
                line += $(this).find('input').attr("id") + ",";
                //拼接分销商
                line += $(this).find("input:eq(0)").val() + ",";
                //拼接价格
                line += $(this).find("input:eq(1)").val() + ";";
            });
            line = line.substring(0, line.length - 1);
            //隐藏域获取值
            $("#into").val(line);
        }
        ////载入
        function loadMatchInfoList() {
            var mainIdList = $("#mainId").val().split(';');
            var cProductIdList = $("#cProductId").val().split(';');
            var cDistributorList = $("#cDistributor").val().split(';');
            var cPriceList = $("#cPrice").val().split(';');
            if (mainIdList.leng == 1)
                //return;
                //先清除,在根据列表数量增加
                $("#tableInfo").html("");
            for (var i = 0; i < mainIdList.length; i++) {
                btnAddButton();
            }
            $("[role='add']").each(function (e) {
                $(this).find("input").attr("id", mainIdList[e]);
                $(this).find("input:eq(0)").val(cDistributorList[e]);
                $(this).find("input:eq(1)").val(cPriceList[e]);
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
            <h1><strong class="colorG"><asp:Literal ID="productName" runat="server" /></strong>商品渠道商编辑</h1>
            <span>商品渠道商进行管理，您可以增加或者修改商品详细信息。</span>
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
                                <td>商品渠道商
                                </td>
                                <td>商品价格
                                </td>
                                <td>操作</td>
                            </tr>
                        </thead>
                    </table>
                    <div class="Pg_15 Pg_010" style="text-align:center;"><asp:Button runat="server" ID="btnAddButton" Text="保存" OnClientClick="return saveTextIntoHiddenFeild()" Class="submit_DAqueding" OnClick="btnAddButton_Click" />
                </div>
            </div>
        </div>
    </div>
    <!--用户操作的隐藏域-->
    <input type="hidden" id="into" runat="server" clientidmode="Static" />
    <!--载入时的隐藏域-->
    <input type="hidden" runat="server" clientidmode="Static" id="mainId" />
    <input type="hidden" runat="server" clientidmode="Static" id="cProductId" />
    <input type="hidden" runat="server" clientidmode="Static" id="cDistributor" />
    <input type="hidden" runat="server" clientidmode="Static" id="cPrice" />
</asp:Content>
