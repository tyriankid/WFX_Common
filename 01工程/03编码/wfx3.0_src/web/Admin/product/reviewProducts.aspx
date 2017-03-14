<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="reviewProducts.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.reviewProducts" Title="无标题页" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1>子门店商品审核</h1>
        <span>子门店的所有待审核的商品，未经审核的商品无法展示，为了门店的正常营业流程，请尽快完成审核！</span>
     </div>


    <div>
        <a onclick="selectall()">全选</a> | <a onclick="unselectall()">反选</a> | <a onclick="passReview('pass')">审核通过</a> | <a onclick="passReview('refuse')">审核不通过</a>
        选择门店<asp:DropDownList ID="DDLStoreId" AutoPostBack="true" runat="server"></asp:DropDownList>
    </div>
     <div class="datalist">
	     <Hi:SelectGridSkuMemberPriceTable ID="SelectGridSkuMemberPriceTable1" runat="server" />
     </div>          

    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
    function loadSkuPrice() {
        if (!checkPrice())
            return false;

        var skuPriceXml = "<xml><skuPrices>";
        $.each($(".SkuPriceRow"), function () {
            var skuId = $(this).attr("skuId");
            var costPrice = $("#tdCostPrice_" + skuId).val();
            var salePrice = $("#tdSalePrice_" + skuId).val();
            var itemXml = String.format("<item skuId=\"{0}\" costPrice=\"{1}\" salePrice=\"{2}\">", skuId, costPrice, salePrice);
            itemXml += "<skuMemberPrices>";

            $(String.format("input[type='text'][name='tdMemberPrice_{0}']", skuId)).each(function (rowIndex, rowItem) {
                var id = $(this).attr("id");
                var gradeId = id.substring(0, id.indexOf("_"));
                var memberPrice = $(this).val();
                if (memberPrice != "")
                    itemXml += String.format("<priceItme gradeId=\"{0}\" memberPrice=\"{1}\" \/>", gradeId, memberPrice);
            });

            itemXml += "<\/skuMemberPrices>";
            itemXml += "<\/item>";
            skuPriceXml += itemXml;
        });
        skuPriceXml += "<\/skuPrices><\/xml>";
        $("#ctl00_contentHolder_txtPrices").val(skuPriceXml);
        return true;
    }

    function refuseReview() {

    }

    function passReview(type) {
        var productids = "";
        var refuseReason = "";
        $("[role='chk']:checked").each(function () {
            productids += $(this).val() + ",";
            refuseReason += $("[role='reviewReason'][skuid='" + $(this).val() + "']").val() + ",";
        });
        productids = productids.substr(0, productids.length - 1);
        refuseReason = refuseReason.substr(0, refuseReason.length - 1);
        //passReview
        $.ajax({
            type: "POST",   //访问WebService使用Post方式请求
            contentType: "application/json", //WebService 会返回Json类型
            url: "reviewProducts.aspx/passReview", //调用WebService的地址和方法名称组合 ---- WsURL/方法名
            data: "{ProductIds:'" + productids + "',RefuseReason:'" + refuseReason + "',Type:'" + type + "'}",//这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到      
            dataType: 'json',
            success: function (result) { //回调函数，result，返回值
                if (result.d == "false") {
                    alert("审核出错");
                }
                else {
                    alert("操作成功！");
                    location.href = "reviewProducts.aspx?productids=" + result.d;
                }
            }
        });

    }




    function selectall() {
        $("[role='chk']").attr("checked", "true");
    }

    function unselectall() {
        $("[role='chk']").removeAttr("checked");
    }

</script>
</asp:Content>
