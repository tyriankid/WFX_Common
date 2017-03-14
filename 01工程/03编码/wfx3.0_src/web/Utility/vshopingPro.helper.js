$(document).ready(function () {
    $("input[name='inputQuantity']").bind("blur", function () { chageQuantity(this); }); //改变购买数量
    $("[name='iDelete']").bind("click", function () {//绑定删除事件
        var obj = this;
        myConfirm('询问', '确定要从购物车里删除该商品吗？', '确认删除', function () {
            deleteCartProduct(obj);
        });
    });
    $("#selectShippingType").bind("change", function () { chageShippingType() });//更改收获方式
    $("#selectCoupon").bind("change", function () { chageCoupon() });//选择优惠券
    $("#aSubmmitorder").bind("click", function () { submmitorder() });//提交订单
});

function deleteCartProduct(obj) {
    //update@20150923 by hj
    //新增一个giftId参数,并在VshopProcess增加相应的处理
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "DeleteCartProduct", skuId: $(obj).attr("skuId"), giftId: $(obj).attr("giftId") },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                location.href = "/Vshop/ShoppingCart.aspx";
            }
        }
    });
}

function chageQuantity(obj) {
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "ChageQuantity", skuId: $(obj).attr("skuId"), quantity: parseInt($(obj).val()) },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                location.href = "/Vshop/ShoppingCart.aspx";
            }
            else {
                alert_h("最多只可购买" + resultData.Status + "件", function () {
                    location.href = "/Vshop/ShoppingCart.aspx";
                });
            }
        }
    });
}






function chageShippingType() {
    var freight = 0;
    if ($("#selectShippingType").val() != "-1") {
        var selectedShippingType = $("#selectShippingType option:selected").text();
        freight = parseFloat(selectedShippingType.substring(selectedShippingType.lastIndexOf("￥") + 1));
    }

    var discountValue = 0;
    if ($("#selectCoupon").val() != undefined && $("#selectCoupon").val() != "") {
        var selectCoupon = $("#selectCoupon option:selected").text();
        discountValue = parseFloat(selectCoupon.substring(selectCoupon.lastIndexOf("￥") + 1));
    }

    var orderTotal = parseFloat($("#vSubmmitOrder_hiddenCartTotal").val()) + freight - discountValue;
    $("#strongTotal").html("¥" + orderTotal.toFixed(2));
}

function chageCoupon() {
    var freight = 0;
    if ($("#selectShippingType").val() != "-1") {
        var selectedShippingType = $("#selectShippingType option:selected").text();
        freight = parseFloat(selectedShippingType.substring(selectedShippingType.lastIndexOf("￥") + 1));
    }

    var discountValue = 0;
    if ($("#selectCoupon").val() != "") {
        var selectCoupon = $("#selectCoupon option:selected").text();
        discountValue = parseFloat(selectCoupon.substring(selectCoupon.lastIndexOf("￥") + 1));
    }

    var orderTotal = parseFloat($("#vSubmmitOrder_hiddenCartTotal").val()) + freight - discountValue;
    $("#strongTotal").html("¥" + orderTotal.toFixed(2));
}

function submmitorder() {
    try {
        if (stockError != undefined && stockError == true) {
            alert_h(stockErrorInfo);
            return false;
        }
        
    }
    catch (e) { }
        if (!$("#selectShippingType").val()) {
            alert_h("请选择配送方式");
            return false;
        }

    if (!$("#selectPaymentType").val()) {
        alert_h("请选择支付方式");
        return false;
    }

    if (!$('#selectShipToDate').val()) {
        alert_h("请选择送货上门时间");
        return false;
    }
    if ($(".last p span").eq(0).html() < 0) {
            alert_h("商品金额不能小于零")
            return false;
        }       
    // 旧版门店匹配,现在已经由前面匹配了门店,改页面只接受传参distributorId即可
    var DisId = getParam("distributorId");
    //三作咖啡特殊取值
    if (localStorage.getItem("selectStoreId") != null) {
        DisId = localStorage.getItem("selectStoreId");
    }
    //pro辣特殊取值
    var roadPrice = -1;
    if (localStorage.getItem("proLa_roadPrice") != null && localStorage.getItem("proLa_storeId") != null) {
        DisId = localStorage.getItem("proLa_storeId");
        roadPrice = parseInt(localStorage.getItem("proLa_roadPrice"));
    }


    if ($("#isStreetEnable").val() == "True" && !$('#distributorSelect').val() && DisId == "") {//如果打开了自动配送方法并且没有选择店铺的话
        alert_h("请选择街道和配送门店");
        return false;
    }

    

    $("#aSubmmitorder").attr("disabled", "disabled");
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: {
            action: "Submmitorder",proLa_RoadPrice:roadPrice, distributorId: DisId, shipDistributorId: $('#distributorSelect').val(), shippingType: $("#selectShippingType").val(), paymentType: $("#selectPaymentType").val(), couponCode: $("#selectCoupon").val(), redpagerid: $("#selectRedPager").val(), shippingId: $('#selectShipTo').val(),
            productSku: getParam("productSku"), buyAmount: getParam("buyAmount"), from: getParam("from"), shiptoDate: $("#selectShipToDate").val(), groupbuyId: $('#groupbuyHiddenBox').val(), countdownId: $('#countdownHiddenBox').val(), cutdownId: $('#cutdownHiddenBox').val(), remark: $('#remark').val() + "用餐人数" + $("#numberDine").html(),
            selectAgentId: $("#selectAgent").val()
        },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                //如果支付方式为微信支付,那么直接跳转到微信支付
                
                if ($("#selectPaymentType").val() == 88) {
                    $("#aSubmmitorder").html("支付中....");
                    location.href = "/pay/wx_Submit.aspx?orderId=" + resultData.OrderId;
                }
                else {
                    location.href = "/Vshop/FinishOrder.aspx?orderId=" + resultData.OrderId;
                }
                
            }
            else if (resultData.ErrorMsg) {
                $("#aSubmmitorder").removeAttr("disabled");
            }
        }
    });
}


