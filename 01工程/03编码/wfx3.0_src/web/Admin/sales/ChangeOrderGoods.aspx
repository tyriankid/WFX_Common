<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ChangeOrderGoods.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ChangeOrderGoods" %>
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
    <div class="datalist" style="padding10px">
    <!--退单详情-->
    <div style="background: #ECECEC;padding:5px 10px;color: #797979;">
        <p id="orderId">订单号：<asp:Literal runat="server" ID="litOrderId"></asp:Literal></p>
        <p id="orderTime">下单时间：<asp:Literal runat="server" ID="litOrderTime"></asp:Literal></p>
        <p id="orderTotalPrice">金额： <d id="orderTotal" style="color:#f00;"></d>  可退额： <d id="refundTotal" style="color:#f00;"></d> </p>     
    </div>
    <!--商品列表-->
    <table id="list" width="100%" style="border:0;" onclick="javascript:changeorder()" >
        <asp:Repeater ID="rptOrderGoods" runat="server">
            <ItemTemplate>
                <tr>
                    <td name="sku" style="display:none">
                        <asp:literal runat="server" ID="litSkuId" Text='<%#Eval("skuId") %>'></asp:literal>
                    </td>
                    <td width="50">
                        <asp:Image ID="imgUrl" runat="server" ImageUrl='<%#Eval("ThumbnailsUrl") %>' />
                    </td>
                    <td width="200">
                        <asp:literal runat="server" ID="litProductName" Text='<%#Eval("ItemDescription") %>'></asp:literal>
                    </td>
                    <td name="rowAmount" style="text-align:center">
                        <asp:literal runat="server" ID="litProductAmount" Text='<%#Eval("ItemAdjustedPrice") %>'></asp:literal>
                    </td>
                    <td name="nub" width="50">
                        <asp:TextBox ID="quantity" runat="server" onkeyup="this.value=this.value.replace(/\D/g,'')" style="width:50px;text-align:center" onafterpaste="this.value=this.value.replace(/\D/g,'')" Text='<%# Eval("Quantity") %>'></asp:TextBox>
                        <input type="hidden" role="quan" name="Quantity" value='<%# Eval("Quantity") %>' />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
        <input type="hidden" runat="server" clientidmode="Static" id="stock" >
        <input type="hidden" runat="server" clientidmode="Static" id="number">
    <!--退单操作-->
    <div style="position: fixed;padding:10px;width: 90%;bottom: 0;border-top: 1px solid #DCDCDC;">
        <!--订单备注(退单理由,金额)-->
        <asp:TextBox Width="100%" ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static"  placeholder="订单备注"></asp:TextBox>
        <div style="position:relative;border-top: 1px solid #DCDCDC;margin-top:10px">
            <p style="color: #666;line-height: 30px;font-family: 'Microsoft Yahei';">退单金额：<d id="refundAmount" style="display:block;font-size:36px;padding-left:20px">￥0.00</d></p>
            <asp:Button id="btnSubmit" style="position: absolute;width: 270px;height: 70px;top: 10px;right: 0;background: #1E97D5;color: #fff;font-size: 36px;cursor:pointer" runat="server" Text="退单" OnClientClick="return checkInput()" OnClick="btnSubmit_Click"/>
        </div>
    </div>
    </div>  
</div>
<script type="text/javascript">
    function checkInput() {
        return confirm("确定要退单吗?此操作不可逆");
    }
    var getOrderTotal = function () {
        var totalAmount = 0;
        $("td[name=rowAmount]").each(function (e) {
            totalAmount += parseFloat($(this).html()) * parseInt($("input[type=text]").eq(e).val());
        });
        $("#orderTotal").html(totalAmount.toFixed(2));
        $("#refundTotal").html(totalAmount.toFixed(2));
    };

        var getRefundAmount = function () {
            //计算退掉的金额
            var refundAmountBefore = parseFloat($("#orderTotal").html()); //parseFloat($("#refundAmount").html()) + (($(this).next().val() - $(this).val()) * parseFloat($(this).closest("td").prev().html()));
            getOrderTotal();
            var refundAmount = refundAmountBefore - parseFloat($("#orderTotal").html());

            $("#refundAmount").html((parseFloat($("#refundAmount").html().replace("￥", "")) + parseFloat(refundAmount)).toFixed(2));
            $("#txtRemark").html("退单金额：" + $("#refundAmount").html() + "元");
        }

        $(function () {
            getOrderTotal();//初始化
            $("input[type=text]").change(function () {
                if ($(this).val() == "") $(this).val(0);
                //判断是否在增加Quantity
                if (parseInt($(this).val()) > parseInt($(this).next().val())) {
                    alert("请勿增加商品!");
                    $(this).val($(this).next().val());
                }
                getRefundAmount();          
            });        
        });     
</script>
</asp:Content>

