
$(function () {
    //刷卡支付
    $("body").on("keypress", "#txtPayCode", function () {
        var payCode = $("#txtPayCode").val();
        if (payCode.length == 18 && $("#hidOrderId").val()) {//输入完18位code点击回车时(模拟扫码枪)触发扫码事件
            var data = {};
            data.orderId = $("#hidOrderId").val();
            data.tPrice = getX($("#priceTotal").html().replace("￥", ""), 100);//该接口使用的单位是分,所以要乘以100
            data.PayCode = payCode;
            $.post("/api/VshopProcess.ashx?action=GoMicroPay", data, function (json) {
                if (json.success === true) {//收货流程成功,订单完结
                    $("#MicroPayDiv").html(json.result);//将返回的结果用html元素存在div内
                    if ($("#MicroPayDiv").find("v[role=result_code]").html() === "SUCCESS")//取支付是否成功的值,为SUCCESS时支付成功
                    {
                        FinishOrderAndPrint($("#MicroPayDiv").find("v[role=out_trade_no]").html(), true);//将orderId传递,完成结算订单事件
                    }
                    else {
                        alert("支付失败！原因是" + json.result);
                        MicroPayClear();
                    }
                }
            });
        }
    });

    //点击商品的规格按钮时触发事件
    $("body").on("click", ".SKUValueClass", function () { SelectSkus(this); });

    //规格选择窗关闭事件
    $("body").on("click", "#closeSkus", function () {
        $("#bg").css("display", "none");
        $("#skuSelectorDiv").html("");
    });

    //点击背景等同于点击关闭规格选择窗
    $("body").on("click", "#bg", function () {
        $("#closeSkus").trigger("click");
    });
    //结算按钮事件
    $("body").on("click", "#buyButton", function () { BuyProduct(); });
    //加号点击事件
    $("body").on("click", ".shopcart-add", function () {
        var productId = $(this).attr("id").substring($(this).attr("id").indexOf('_') + 1);
        var skuId = $(this).prev().val();
        var num = parseInt($(this).prev().prev().val()) + 1;
        //给quantity做加减法
        //$("#buyNum_" + productId).val(num);
        if (num == 1) {
            BuyProductToCart(num, skuId, $("#PCProductSearchBuy_litCategoryId").val(), $(this).prev().prev());
        }
        else {
            chageCartProductQuantity(skuId, num, $(this).prev().prev());
        }
    });
    //减号点击事件
    $("body").on("click", ".shopcart-minus", function () {
        var productId = $(this).attr("id").substring($(this).attr("id").indexOf('_') + 1);
        var skuId = $(this).next().next().val();
        var num = parseInt($(this).next().val()) - 1;
        if (num > 0) {
            chageCartProductQuantity(skuId, num, $(this).next());
        }
        else if (num == 0) {
            $(this).parents("li").eq(0).remove();
            deleteCartProduct(skuId, $(this).next());
        }
    });
    //套餐优惠按钮点击事件
    $("body").on("click", "#btnDiscount", function () {
        //常亮样式
        $(this).addClass("btnActive");
        //显示遮罩层
        $("#bg").css({ display: "block", height: $(document).height() });
        $("#divDiscount").show();
        $('#txtDiscount').focus();
    });
    //提交订单
    $("body").on("click", "#btnSubmmit", function () {
        //显示遮罩层
        $("#bg").css({ display: "block", height: $(document).height() });
        $("#divCash").show();
        $('#txtCashInput').focus();
    });

    getTotalPrice();


    //按钮的点击效果样式
    $("#btnClear").bind("mouseup", function () {
        $(this).removeClass("btnActive");
    });
    $("#btnClear").bind("mousedown", function () {
        $(this).addClass("btnActive");
    });
    $("#btnGiveBuy").bind("mouseup", function () {
        $(this).removeClass("btnActive");
    });
    $("#btnGiveBuy").bind("mousedown", function () {
        $(this).addClass("btnActive");
    });

    $("#btnHalf").bind("mouseup", function () {
        $(this).removeClass("btnActive");
    });
    $("#btnHalf").bind("mousedown", function () {
        $(this).addClass("btnActive");
    });

});

//通过点击右侧商品列表,动态显示左侧的order列表
function showLeftOrderList(e) {
    var html = "";
    var rightid = $(e).next().find("input").last().attr("id");//获取skuid
    var skuCounts = $(e).next().attr("skuCounts");//获取规格的数量
    var len = $("#orderList ul li #" + rightid + "").length;

    if (skuCounts > 0) {//多规格的处理
        getSkuSelector($(e).next());
        return;
    }
    if (len == 0) {
        $(e).next().find("input[type=tel]").attr("value", 1);
        $("#orderList ul").append($(e).next().html());
        BuyProductToCart(1, $(e).next().find("input[type=hidden]").val(), $("#PCProductSearchBuy_litCategoryId").val());
    }
}

//遍历orderList的所有元素,获取商品价格并计算放入总价栏
function getTotalPrice() {
    var tPrice = 0.00;
    $("#orderList ul li").each(function () {
        tPrice = getSum(tPrice, getX($(this).find("b").html(), $(this).find("input[type=tel]").attr("value"), $(this).find("#giveNum").attr("value"), $(this).find("#halfNum").attr("value")));
    });
    //计算优惠券优惠
    if ($("#cutPrice").length > 0) {
        tPrice = getMin(tPrice, $("#cutPrice").attr("cutPrice"));
    }
    //计算直接优惠
    if (parseFloat($("#txtDiscount").val()) > 0) {
        tPrice = getMin(tPrice, $("#txtDiscount").val());
    }
    $("#priceTotal").html("￥" + tPrice);

    return tPrice;
}
function getSum(num1, num2) { return roundFun((parseFloat(num1) + parseFloat(num2)), 2); }
function getMin(num1, num2) { return roundFun((parseFloat(num1) - parseFloat(num2)), 2); }
//num3:买一送一数量,num4:半价数量
function getX(num1, num2, num3,num4) {
    if (num3 == null || num3 == "") {
        num3 = "0";
    }
    if (num4 == null || num4 == "") {
        num4 = "0";
    }
    return roundFun((parseFloat(num1) * (parseFloat(num2) - parseFloat(num3))) - ((parseFloat(num1) / 2 * num4)), 2);
}


//改变购物车中商品的数量
function chageCartProductQuantity(SkuId, Quantity, obj) {
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "ChageQuantity", skuId: SkuId, quantity: Quantity },
        success: function (resultData) {
            if (resultData.Status != "OK") {
                alert("最多只可购买" + resultData.Status + "件");
            }
            else {
                obj.val(Quantity);
                getTotalPrice();
            }
        }
    });
}
//添加商品至购物车
function BuyProductToCart(Quantity, ProductSkuId, Categoryid, obj) {
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "AddToCartBySkus", quantity: Quantity, productSkuId: ProductSkuId, categoryid: Categoryid },
        async: false,
        success: function (resultData) {
            if (resultData.Status == "OK") {
                getTotalPrice();
                if (obj != null)
                    obj.val(Quantity);
            } else if (resultData.Status == "0") {
                // 商品已经下架

                alert_h("此商品已经不存在(可能被删除或被下架)，暂时不能购买");
            }
            else if (resultData.Status == "1") {
                // 商品库存不足

                alert_h("商品库存不足 " + parseInt($("#buyNum").val()) + " 件，请修改购买数量!");
            }
            else {
                //alert(resultData.Status);
                if (resultData.Status == "2") {
                    //location.href = "/Vshop/UserLogin.aspx?userstatus=0&productid=" + $("#vProductDetails_litproductid").val();
                    alert_h("请先登录系统", function () {
                        location.href = "/Vshop/MemberCenter.aspx?userstatus=0&productid=" + $("#vProductDetails_litproductid").val();
                    });
                }
                else if (resultData.Status == "-1") {
                    // 抛出异常消息
                    //alert("Ee");
                    // alert_h(resultData.Status + '66');
                    //location.href = "/Vshop/MemberCenter.aspx?userstatus=0&productid=" + $("#vProductDetails_litproductid").val();
                    alert_h("请先登录系统", function () {
                        location.href = "/Vshop/MemberCenter.aspx?userstatus=0&productid=" + $("#vProductDetails_litproductid").val();
                    });
                }
                else {
                    alert_h(resultData.Status + '66');
                }
            }
        }
    });
}
//删除购物车里商品
function deleteCartProduct(SkuId, obj) {
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "DeleteCartProduct", skuId: SkuId },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                obj.val("0");
                getTotalPrice();
            }
        }
    });
}
//动态获取选择规格控件,并弹出层
function getSkuSelector(e) {
    var data = {};
    data.ProductId = e.attr("productId");
    $.post("/api/VshopProcess.ashx?action=GetSKUSelector", data, function (json) {
        if (json.success === true) {
            var $box = $('#skuSelectorDiv');
            //如果成功,显示选择规格的div,并给div赋值上该商品的id
            $box.attr("productId", e.attr("productId"));
            $box.html(json.selector);
            $box.prepend("<a href='#' id='closeSkus'>x</a>");
            //显示遮罩层
            $("#bg").css({ display: "block", height: $(document).height() });
        }
        else {
            alert("出错了!");
        }
    });
}
//点击商品规格事件
function SelectSkus(clt) {
    // 保存当前选择的规格
    var AttributeId = $(clt).attr("AttributeId");
    var ValueId = $(clt).attr("ValueId");
    var ProductId = $("#skuSelectorDiv").attr("productId");
    $("#skuContent_" + AttributeId).val(AttributeId + ":" + ValueId);
    // 重置样式
    ResetSkuRowClass("skuRow_" + AttributeId, "skuValueId_" + AttributeId + "_" + ValueId);
    // 如果全选，则重置SKU
    var allSelected = IsallSelected();
    var selectedOptions = "";
    var skuContents = "";
    if (allSelected) {
        $.each($("input[type='hidden'][name='skuCountname']"), function () {
            selectedOptions += $(this).attr("value") + ",";
        });
        $.each($(".SKUValueClass.active"), function () {
            skuContents += $(this).html() + ",";
        });
        selectedOptions = selectedOptions.substring(0, selectedOptions.length - 1);
        skuContents = skuContents.substring(0, skuContents.length - 1);
        $.ajax({
            url: "/API/VshopProcess.ashx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "GetSkuByOptions", productId: ProductId, options: selectedOptions },
            success: function (resultData) {
                if (resultData.Status == "OK") {
                    ResetCurrentSku(resultData.SkuId, resultData.SKU, resultData.Weight, resultData.Stock, resultData.SalePrice, skuContents);
                }
                else {
                    ResetCurrentSku("", "", "", "", "0"); //带服务端返回的结果，函数里可以根据这个结果来显示不同的信息
                }
                //关闭遮罩层
                $("#bg").css("display", "none");
            },
            error: function () {
            }
        });
    }
}
//将选择好的规格传递到左边orderlist处
function ResetCurrentSku(skuId, sku, weight, stock, salePrice, skuName) {
    if (stock == 0)//如果改规格没有库存,return
    {
        alert("该规格没有库存，请重新选择！");
        $("#skuSelectorDiv").html("");
        return false;
    }
    var len = $("#orderList ul li #skuid_" + skuId + "").length;//判断该skuid在orderlist是否已经存在
    if (len > 0) {//如果存在,重新选择
        $("#skuSelectorDiv").html("");
        return false;
    }
    var ProductId = $("#skuSelectorDiv").attr("productId");
    //-------先将右侧的skuid重置--------
    //获取装载skuId的input
    var $skuInput = $("ul .goods-num[productId=" + ProductId + "]").find("input[type=hidden]");
    $skuInput.attr("value", skuId);//重置input的value
    $skuInput.attr("id", "skuid_" + skuId);//重置input的id

    if (len == 0)//如果不存在就添加
    {
        var pSkuName = $skuInput.parent().parent().find("d");//skuName
        var pPrice = $skuInput.parent().parent().find("b");//价格
        $skuInput.prev().attr("value", 1);//数量input设为1
        pSkuName.html(skuName);
        pPrice.html(parseFloat(salePrice));
        $("#orderList ul").append($skuInput.parents("ul .goods-num").html());//将orderlist增加选择框
        $("#skuSelectorDiv").html("");//初始化规格选择框
        BuyProductToCart(1, skuId, $("#PCProductSearchBuy_litCategoryId").val());//加入购物车
    }
}

//重置规格值的样式
function ResetSkuRowClass(skuRowId, skuSelectId) {
    var pvid = skuSelectId.split("_");
    $.each($("#" + skuRowId + " div"), function () {
        $(this).removeClass('active');
    });
    $("#" + skuSelectId).addClass('active');
}

//是否所有规格都已选
function IsallSelected() {
    var allSelected = true;
    $.each($("input[type='hidden'][name='skuCountname']"), function () {
        if ($(this).val().length == 0) {
            allSelected = false;
        }
    });
    return allSelected;
}

var eggHalfPrice = "off";//挝蛋蛋捆绑销售半价,默认为关闭
//提交
function submmitorder() {

    var totalPrice = 0.00;
    $("#orderList ul li").each(function () {
        totalPrice = getSum(totalPrice, getX($(this).find("b").html(), $(this).find("input[type=tel]").attr("value")));
    });
    var cutPrice = parseFloat($("#cutPrice").attr("cutPrice"));
    var reachPrice = parseFloat($("#cutPrice").attr("reachPrice"));
    //是否至少选择一个商品
    if (totalPrice == 0.00) {
        alert("请选好商品后再提交！");
        return false;
    }
    //总价是否大于优惠券减少价格的验证
    if (totalPrice <= cutPrice) {
        alert("优惠价格不能大于商品价格！");
        return false;
    }
    //优惠券是否达到使用要求验证
    if (totalPrice < reachPrice) {
        alert("还差" + getMin(reachPrice, totalPrice) + "元才能使用该优惠券！");
        return false;
    }
    //优惠金额不能大于等于订单金额
    if (getTotalPrice() <= 0) {
        $("#txtDiscount").val('');
        getTotalPrice();
        alert("优惠的金额不能大于或等于订单金额!");
        return false;
    }

    var PaymentType = 99;//线下付款
    var ShippingType = 5;//自提
    var ShipTo = 1972;//深圳
    $(this).attr("disabled", "disabled");
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: {
            action: "Submmitorder", shipDistributorId: $('#distributorSelect').val(), shippingType: ShippingType, paymentType: PaymentType, couponCode: $("#txtCouponCode").val(), redpagerid: $("#selectRedPager").val(), shippingId: ShipTo,
            productSku: getParam("productSku"), buyAmount: getParam("buyAmount"), from: getParam("from"), shiptoDate: $("#selectShipToDate").val(), groupbuyId: $('#groupbuyHiddenBox').val(), countdownId: $('#countdownHiddenBox').val(), cutdownId: $('#cutdownHiddenBox').val(), remark: $('#remark').val(), pcDiscountAmount: $("#txtDiscount").val()
        },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                localStorage.clear();

                //调用微信的刷卡支付接口,根据传过来的参数判断是否支付成功,如果成功,继续下面的完成订单以及打印流程
                if (MicroPayMode === "on")//先判断刷卡支付功能是否开启,若开启,弹出扫码框,之后流程在扫码事件里完成
                {
                    $("#bg").css({ display: "block", height: $(document).height() });//显示遮罩层
                    $("#MicroPay").show();
                    $("#txtPayCode").focus();
                    $("#hidOrderId").val(resultData.OrderId);
                    return false;
                }
                else {//若刷卡支付功能未开启,直接走完流程
                    //生成订单后直接确认收货完结订单
                    FinishOrderAndPrint(resultData.OrderId, false);
                }
            }
            else if (resultData.ErrorMsg) {
                alert_h(resultData.ErrorMsg);
                $("#aSubmmitorder").removeAttr("disabled");
            }

        }
    });
}
//结束订单并打印
function FinishOrderAndPrint(OrderId, isMicroPay) {
    var data = {};
    data.orderId = OrderId;
    data.isMicroPay = isMicroPay;
    $.post("/api/VshopProcess.ashx?action=FinishOrder", data, function (json) {
        if (json.success === true) {//收货流程成功,订单完结
            //清除用于防止页面刷新的优惠券信息
            sessionStorage.clear();
            //打印
            $.post("/api/VshopProcess.ashx?action=PrintOrderInfo", data, function (json) {
                if (json.success === true) {
                    //$("#printDiv").show();
                    $("#printDiv").html(json.inHtml);
                    batchPrintData();
                    batchPrintData();
                    batchPrintData();
                    if (getParam("type") == "activity") {
                       batchPrintData();
                    }
                    MicroPayClear();
                }
            });
        }
        else {
            alert(json.msg);
        }
    });
}

//打印
function batchPrintData() {
    var LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));

    try {
        LODOP.PRINT_INIT("打印订单");
        LODOP.SET_PRINT_PAGESIZE(3, 710, 0, "");
        LODOP.ADD_PRINT_HTM(0, 0, 710, $("#printDiv").height() - 300, $("#printDiv").html());
        LODOP.PRINT();
    } catch (e) {
        alert("请先安装打印控件！");
        return false;
    }
    location.reload();
}

//优惠券代码填完后的检查事件
$(function () {
    //页面刷新时,如果优惠券信息存在,也要将优惠券信息载入
    if (sessionStorage.getItem("cutPrice") != null) {
        //页面展示
        $("#enterCouponCode").append("<span id='cutPrice' cutPrice='" + parseFloat(sessionStorage.getItem("cutPrice")) + "' reachPrice='" + parseFloat(sessionStorage.getItem("reachPrice")) + "'> - ￥" + parseFloat(sessionStorage.getItem("cutPrice")) + "</span>")
        //控件填值
        $('#txtCouponCode').val(sessionStorage.getItem("showCode"));
    }
    $('#accept').click(function () {
        //验证输入规则
        var showCode = $('#txtCouponCode').val();
        if (showCode.length != 10) {
            alert("错误的优惠券码!");
            $("#enterCouponCode").removeClass("btnActive");//去掉按钮激活样式
            $("#bg").css("display", "none");//关闭遮罩层
            $('#txtCouponCode').val("");
            return false;
        }
        var data = {};
        data.showCode = showCode;
        $.post("/api/VshopProcess.ashx?action=CheckCouponUseable", data, function (json) {
            if (json.success === true) {
                if (json.msg == "ok") {
                    //总价后面增加html显示优惠的价格
                    $("#enterCouponCode").append("<span id='cutPrice' cutPrice='" + parseFloat(json.cutPrice) + "' reachPrice='" + parseFloat(json.reachPrice) + "'> - ￥" + parseFloat(json.cutPrice) + "</span>")
                    //并且不能第二次输入
                    $("#enterCouponCode").click(function () { return false; });
                    //隐藏输入框
                    $("#divActivity").hide();
                    $("#enterCouponCode").removeClass("btnActive");//去掉按钮激活样式
                    $("#bg").css("display", "none");//关闭遮罩层
                    //重新计算总价
                    getTotalPrice();
                    //将优惠券信息存入localStorage
                    sessionStorage.setItem("cutPrice", json.cutPrice);
                    sessionStorage.setItem("reachPrice", json.reachPrice);
                    sessionStorage.setItem("showCode", showCode);
                }
                else {
                    alert(json.msg);
                    $("#enterCouponCode").removeClass("btnActive");//去掉按钮激活样式
                    $("#bg").css("display", "none");//关闭遮罩层
                    $('#txtCouponCode').val("");
                }
            }
        });

    });
});

function roundFun(numberRound, roundDigit)   //四舍五入，保留位数为roundDigit     
{
    if (numberRound >= 0) {
        var tempNumber = parseInt((numberRound * Math.pow(10, roundDigit) + 0.5)) / Math.pow(10, roundDigit);
        return tempNumber;
    }
    else {
        numberRound1 = -numberRound
        var tempNumber = parseInt((numberRound1 * Math.pow(10, roundDigit) + 0.5)) / Math.pow(10, roundDigit);
        return -tempNumber;
    }
}

//优惠券虚拟小键盘
$('#txtCouponCode')
 .keyboard({
     layout: 'custom',
     display: {
         'a': '使用',
         'c': '取消',
         'bksp': '退格',
         'clear': '清空'
     },
     customLayout: {
         'default': [
          '{bksp} 7 8 9',
          '{clear} 4 5 6',
          '{c} 1 2 3',
          '{a} 0 {left} {right} '
         ]
     },
     position: {
         of: null, // null (attach to input/textarea) or a jQuery object (attach elsewhere)
         at2: 'center bottom+20', // used when "usePreview" is false (centers keyboard at the bottom of the input/textarea)
         collision: 'fit fit'
     },
     accepted: function (event, keyboard, el) {
         //alert('The content "' + el.value + '" was accepted!');
         $('#accept').click();
         $("#divActivity").hide();
     },
     canceled: function (event, keyboard, el) {//取消显示回调函数
         $("#divActivity").hide();
         $("#enterCouponCode").removeClass("btnActive");//去掉按钮激活样式
         $("#bg").css("display", "none");//关闭遮罩层
     },
     usePreview: false, // no preveiw
     maxLength: 10,
     restrictInput: true, // Prevent keys not in the displayed keyboard from being typed in
     useCombos: false // don't want A+E to become a ligature
 });

//手动优惠虚拟小键盘
$('#txtDiscount')
 .keyboard({
     layout: 'custom',
     display: {
         'a': '确认',
         'c': '取消',
         'bksp': '退格',
         'clear': '清空'
     },
     customLayout: {
         'default': [
          '{dec} 7 8 9',
          '{clear} 4 5 6',
          '{c} 1 2 3',
          '{a} 0 {left} {right} '
         ]
     },
     position: {
         of: null, // null (attach to input/textarea) or a jQuery object (attach elsewhere)
         at2: 'center bottom+20', // used when "usePreview" is false (centers keyboard at the bottom of the input/textarea)
         collision: 'fit fit'
     },
     accepted: function (event, keyboard, el) {
         $("#divDiscount").hide();
         $("#btnDiscount").removeClass("btnActive");//去掉按钮激活样式
         $("#bg").css("display", "none");//关闭遮罩层
         getTotalPrice();
     },
     canceled: function (event, keyboard, el) {//取消显示回调函数
         $("#divDiscount").hide();
         $("#btnDiscount").removeClass("btnActive");//去掉按钮激活样式
         $("#bg").css("display", "none");//关闭遮罩层
     },
     usePreview: false, // no preveiw
     maxLength: 10,
     restrictInput: true, // Prevent keys not in the displayed keyboard from being typed in
     useCombos: false // don't want A+E to become a ligature
 });


//结算时输入现金虚拟小键盘
$('#txtCashInput')
 .keyboard({
     layout: 'custom',
     display: {
         'a': '确认',
         'c': '取消',
         'bksp': '退格',
         'clear': '清空'
     },
     customLayout: {
         'default': [
          '{dec} 7 8 9',
          '{clear} 4 5 6',
          '{c} 1 2 3',
          '{a} 0 {left} {right} '
         ]
     },
     position: {
         of: null, // null (attach to input/textarea) or a jQuery object (attach elsewhere)
         at2: 'center bottom+20', // used when "usePreview" is false (centers keyboard at the bottom of the input/textarea)
         collision: 'fit fit'
     },
     accepted: function (event, keyboard, el) {
         var result = getMin($('#txtCashInput').val(), getTotalPrice());
         alert("找零：" + result);
         submmitorder();
     },
     canceled: function (event, keyboard, el) {//取消显示回调函数
         $("#divCash").hide();
         $("#bg").css("display", "none");//关闭遮罩层
         $('#txtCashInput').val('');
     },
     usePreview: false, // no preveiw
     maxLength: 10,
     restrictInput: true, // Prevent keys not in the displayed keyboard from being typed in
     useCombos: false // don't want A+E to become a ligature
 });

//买一送一配置
function updateBuyToGive() {
    
    if (localStorage.getItem("buyHalf")) {
        alert("已经半价咯！");
        return;
    }
    localStorage.setItem("buyGive", 1);
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "BuyToGive" },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                if (resultData.SkuId != "") {
                    //giveSkuIds += resultData.SkuId + ",";
                    if (resultData.Price != "") {
                        //var strgivePrice = sessionStorage.getItem("givePrice");
                        //var givePrice = 0;
                        //if(strgivePrice !=null && strgivePrice !=""){
                        //    givePrice = parseFloat(strgivePrice);
                        //}
                        //var price = parseFloat(resultData.Price);
                        //givePrice += price;
                        //sessionStorage.setItem("givePrice", givePrice);
                    }

                }
                alert("操作成功！");
                
                location.reload();
                //getTotalPrice();
                //obj.val("0");
            }
            else {
                alert("不能再送了");
            }
        }
    });

}
//第二杯半价配置
function updateBuyToHalf() {
    
    if (localStorage.getItem("buyGive")) {
        alert("已经买一送一咯。");
        return;
    }
    localStorage.setItem("buyHalf", 1);
        
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "BuyToHalf" },
        success: function (resultData) {
            if (resultData.Status == "OK") {
                if (resultData.SkuId != "") {
                    if (resultData.Price != "") {
                    }
                }
                alert("操作成功！");
                
                location.reload();

            }
            else {
                alert("已经半价咯");
            }
        }
    });

}

//优惠券按钮点击:显示优惠券输入框
function showCoupon() {
    //常亮样式
    var currentBtn = $("#enterCouponCode");
    currentBtn.addClass("btnActive");
    //显示遮罩层
    $("#bg").css({ display: "block", height: $(document).height() });
    $("#divActivity").show();
    $('#txtCouponCode').focus();

}
//买一送一按钮点击
function BuyGive() {
    //$("#btnHalf").setAttribute("disabled", "disabled");

    var currentBtn = $("#btnGiveBuy");
    var num = 0;
    $("#orderList ul li").each(function () {
        num += parseInt($(this).find("input[type=tel]").attr("value"));
    });
    if (num > 1) {
        updateBuyToGive();
    }
}
//第二杯半价按钮点击
function BuyHalf() {

    var currentBtn = $("#btnHalf");
    var num = 0;
    $("#orderList ul li").each(function () {
        num += parseInt($(this).find("input[type=tel]").attr("value"));
    });
    if (num > 1) {
        updateBuyToHalf();
    }
}

//清除按钮点击:清除购物车和缓存信息
function Clear() {
    var currentBtn = $("#btnClear");
    localStorage.clear();
    sessionStorage.clear();
    $.post("/api/VshopProcess.ashx?action=ClearShoppingCart");
    location.reload();
}

//微信支付按钮点击:开启关闭微信刷卡支付
var MicroPayMode = "off";//微信支付功能默认为关闭
function OpenMicroPay() {
    var currentBtn = $("#btnMicroPay");
    currentBtn.toggleClass("btnActive");
    //开关微信支付功能
    if (currentBtn.is(".btnActive")) {
        MicroPayMode = "on";
    }
    else {
        MicroPayMode = "off";
    }
}

//清空刷卡支付各个信息
function MicroPayClear() {
    location.reload();
    return;
    $("#txtPayCode").val("");//清空输入框
    $("#MicroPay").hide();
    $("#hidOrderId").val("");//清空orderId框
    $("#MicroPayDiv").html("");//清空返回值的html框
    $("#bg").css("display", "none");//关闭遮罩层
    alert(MicroPayMode);
}