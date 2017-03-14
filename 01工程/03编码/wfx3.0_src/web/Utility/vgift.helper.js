$(document).ready(function () {


    //$("#buyButton").bind("click", function () { BuyProduct() }); //立即购买  //因为是服务端控件,在此处绑定事件无效,已经改为在VGiftDetails.cs后台绑定.
    $("#spAdd").bind("click", function () { $("#buyNum").val(parseInt($("#buyNum").val()) + 1)});
    $("#spSub").bind("click", function () { var num = parseInt($("#buyNum").val()) - 1; if (num > 0) $("#buyNum").val(parseInt($("#buyNum").val()) - 1) });
    $("#spcloces").bind("click", function () { $("#divshow").hide() });
});

function disableShoppingBtn(disabled) {//禁用(启用)购买和加入购物车按钮

    var btns = $('button[type=shoppingBtn]');
    if (disabled)
        btns.addClass('disabled');
    else
        btns.removeClass('disabled');
}

function SelectSkus(clt) {
    //禁用购买和加入购物车按钮
    disableShoppingBtn(true);

    // 保存当前选择的规格
    var AttributeId = $(clt).attr("AttributeId");
    var ValueId = $(clt).attr("ValueId");
    $("#skuContent_" + AttributeId).val(AttributeId + ":" + ValueId);
    // 重置样式
    ResetSkuRowClass("skuRow_" + AttributeId, "skuValueId_" + AttributeId + "_" + ValueId);
    // 如果全选，则重置SKU
    var allSelected = IsallSelected();
    var selectedOptions = "";
    if (allSelected) {
        $.each($("input[type='hidden'][name='skuCountname']"), function () {
            selectedOptions += $(this).attr("value") + ",";
        });
        selectedOptions = selectedOptions.substring(0, selectedOptions.length - 1);
        $.ajax({
            url: "/API/VshopProcess.ashx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "GetSkuByOptions", productId: $("#hiddenProductId").val(), options: selectedOptions },
            success: function (resultData) {
                if (resultData.Status == "OK") {
                    ResetCurrentSku(resultData.SkuId, resultData.SKU, resultData.Weight, resultData.Stock, resultData.SalePrice);
                }
                else {
                    ResetCurrentSku("", "", "", "", "0"); //带服务端返回的结果，函数里可以根据这个结果来显示不同的信息
                }
                disableShoppingBtn(false); //启用购买和加入购物车按钮
            },
            error: function () {
                disableShoppingBtn(false); //启用购买和加入购物车按钮
            }
        });
    }
}

// 是否所有规格都已选
function IsallSelected() {
    var allSelected = true;
    $.each($("input[type='hidden'][name='skuCountname']"), function () {
        if ($(this).val().length == 0) {
            allSelected = false;
        }
    });
    return allSelected;
}

// 重置规格值的样式
function ResetSkuRowClass(skuRowId, skuSelectId) {
    var pvid = skuSelectId.split("_");

    $.each($("#" + skuRowId + " div"), function () {
        $(this).removeClass('active');
    });

    $("#" + skuSelectId).addClass('active');
}

// 重置SKU

function ResetCurrentSku(skuId, sku, weight, stock, salePrice) {
    $("#hiddenSkuId").val(skuId);
    $(".spSalaPrice").html(salePrice);
    if (!isNaN(parseInt(stock))) {
        $("#spStock").html(stock);
    }
    else {
        $("#spStock").html("0");
        alert_h("该规格的产品没有库存，请选择其它的规格！");
    }
}

// 购买按钮单击事件
function BuyProduct() {
    
    if (!ValidateBuyAmount()) {
        return false;
    }
    if (!IsallSelected()) {
        debugger;
        alert_h("请选择规格");
        return false;
    }

    var quantity = parseInt($("#buyNum").val());
    var stock = parseInt($("#spStock").html());
    if (isNaN(stock) || stock == 0) {
        alert_h("该规格的产品没有库存，请选择其它的规格！");
        return false;
    }
    if (quantity > stock) {
        alert_h("商品库存不足 " + quantity + " 件，请修改购买数量!");
        return false;
    }
    //location.href = "/Vshop/UserLogin.aspx?userstatus=1&buyAmount=" + $("#buyNum").val() + "&productSku=" + $("#hiddenSkuId").val() + "&from=signBuy";
    //location.href = "/Vshop/SubmmitOrder.aspx?buyAmount=" + $("#buyNum").val() + "&isGift=1&from=signBuy";
}

// 验证数量输入
function ValidateBuyAmount() {
    var buyNum = $("#buyNum");
    if ($(buyNum).val().length == 0) {
        alert_h("请先填写购买数量!");
        return false;
    }
    if ($(buyNum).val() == "0" || $(buyNum).val().length > 5) {

        alert_h("填写的购买数量必须大于0小于99999!");
        var str = $(buyNum).val();
        $(buyNum).val(str.substring(0, 5));
        return;
    }
    var amountReg = /^[1-9]d*|0$/;
    if (!amountReg.test($(buyNum).val())) {
        alert_h("请填写正确的购买数量!");
        return false;
    }

    return true;
}

function AddProductToCart() {
    if (!ValidateBuyAmount()) {
        return false;
    }

    if (!IsallSelected()) {
        alert_h("请选择规格");
        return false;
    }
    var quantity = parseInt($("#buyNum").val());
    var stock = parseInt($("#spStock").html());
    if (isNaN(stock) || stock == 0) {
        alert_h("该规格的产品没有库存，请选择其它的规格！");
        return false;
    }
    if (quantity > stock) {
        alert_h("商品库存不足 " + quantity + " 件，请修改购买数量!");
        return false;
    }
    
    BuyProductToCart();//添加到购物车
}


