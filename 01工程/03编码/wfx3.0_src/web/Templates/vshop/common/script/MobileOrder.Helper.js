$(document).ready(function () {
    $("#buyButton").bind("click", function () { BuyProduct(); }); //绑定购买事件
    //加号点击事件
    $("body").on("click", ".shopcart-add", function () {
        //首次点击时,将减号和输入框显示出来
        $(this).parent().find("[class!=shopcart-add]").show();

        var productId = $(this).attr("id").substring($(this).attr("id").indexOf('_') + 1);
        var skuId = $("#buyNum_" + productId).next().val();
        var num = parseInt($("#buyNum_" + productId).val()) + 1;
        //给quantity做加减法
        if (num == 1) {
            BuyProductToCart(num, skuId, $("#vProductSearchBuy_litCategoryId").val(), $("#buyNum_" + productId),true);
        }
        else {
            chageCartProductQuantity(skuId, num, $("#buyNum_" + productId),true);
        }
        //飞入效果
        var offset = $(".btn-car").offset();
        var addcar = $(this);
        var img = $("#img_"+productId).attr('src');
        var flyer = $('<img class="u-flyer" src="' + img + '">');
        var w = $(".btn-car").width();
        var h = $(".btn-car").height();
        flyer.fly({
            start: {
                left: event.pageX, //开始位置（必填）#fly元素会被设置成position: fixed 
                top: event.pageY //开始位置（必填） 
            },
            end: {
                left: offset.left + 50, //结束位置（必填） 
                top: offset.top + 80, //结束位置（必填） 
                width: 0, //结束时宽度 
                height: 0 //结束时高度 
            },
            onEnd: function () {
                flyer.remove();//删掉飞行的图片对象
                getTotalPrice();//更新总价和总数量
                //购物车的撞击动画效果
                $(".btn-car").animate({
                    opacity: '0.5',
                },100);
                $(".btn-car").animate({
                    opacity: '1.0',
                },100);
            }
        });
    });
    //减号点击事件
    $("body").on("click", ".shopcart-minus", function () {
        var productId = $(this).attr("id").substring($(this).attr("id").indexOf('_') + 1);
        var skuId = $("#buyNum_" + productId).next().val();
        var num = parseInt($("#buyNum_" + productId).val()) - 1;
        if (num > 0) {
            chageCartProductQuantity(skuId, num, $("#buyNum_" + productId));
        }
        else if (num == 0) {
            deleteCartProduct(skuId, $("#buyNum_" + productId));//删除购物车的商品信息
            $(this).parent().find("[class!=shopcart-add]").hide();//将减号和输入框隐藏掉
        }
    });
    //规格按钮点击事件
    $("body").on("click", "[type=skusSelect]", function () {
        getSkuSelector($(this));
    });
    //规格div关闭事件,将临时变量里的html还原到存好的父元素内
    $("body").on("click", "#closeSkus", function () {
        $("#bg").css("display", "none");
        $("#skuSelectorDiv").slideUp("fast", function () {
            $("#skuSelectorDiv").html("");
            if (cloneHtml != null) {
                divParent.append(cloneHtml);
                cloneHtml = null;
                divParent = null;
            }
        });
    });
    //点击背景等同于点击关闭
    $("body").on("click", "#bg", function () {
        $("#closeSkus").trigger("click");
    });
    //找到所有多规格的产品,并进行动态隐藏加减号,展示可选规格按钮
    $(".goods-num[skucounts!=0]").each(function () {
        $(this).find("[type!=skusSelect]").hide();
        $(this).append("<div productId='" + $(this).attr("productId") + "' class='shopcart-skus' type='skusSelect'>可选规格</div>")
    });
    //找到所有单规格的产品,并对quantity=0的商品进行动态隐藏减号和输入框
    $(".goods-num[skucounts=0]").each(function () {
        if ($(this).find("input[type=tel]").attr("value")==0)
            $(this).find("[class!=shopcart-add]").hide();
    });
    //规格选择触发事件
    $("body").on("click", ".SKUValueClass", function () { SelectSkus(this); });
    //初始化总价和购物车数量
    getTotalPrice();
});


//选择规格方法
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
    if (allSelected) {
        $.each($("input[type='hidden'][name='skuCountname']"), function () {
            selectedOptions += $(this).attr("value") + ",";
        });
        selectedOptions = selectedOptions.substring(0, selectedOptions.length - 1);
        $.ajax({
            url: "/API/VshopProcess.ashx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "GetSkuByOptions", productId: ProductId, options: selectedOptions },
            success: function (resultData) {
                if (resultData.Status == "OK") {
                    ResetCurrentSku(resultData.SkuId, resultData.SKU, resultData.Weight, resultData.Stock, resultData.SalePrice);
                }
                else {
                    ResetCurrentSku("", "", "", "", "0"); //带服务端返回的结果，函数里可以根据这个结果来显示不同的信息
                }
            },
            error: function () {
            }
        });
    }
}

// 重置SKU,并出现+ -按钮和加钱
var divParent = null;
var cloneHtml = null;
var skuSelectMode ="off";//规格选择模式默认为关闭,用于避免获取父元素时,获取到了新生成的加减框的父元素
function ResetCurrentSku(skuId, sku, weight, stock, salePrice) {
    var currentProductId = $("#skuSelectorDiv").attr("productId");
    var currentProductSkuidArea = $("input[productid=" + currentProductId + "]");//首先获取该商品的skuId input控件对象
    var currentProductDivArea = currentProductSkuidArea.parents("div").eq(0);//获取包含这些元素的div对象
    var currentProductPriceArea = $("#buyPrice_"+currentProductId);//获取该商品的展示区域对象

    currentProductSkuidArea.val(skuId);//重置该商品的skuId
    currentProductPriceArea.html(salePrice);//重置该商品的价格

    if (!isNaN(parseInt(stock))) {//判断库存
        currentProductSkuidArea.attr("stock", stock);
    }
    else {
        currentProductSkuidArea.attr("stock", 0);
        alert("该规格的产品没有库存，请选择其它的规格！");
        return false;
    }
    
    //当选择规格模式开启后,存第一次父元素,避免多规格的重复选择后造成父元素获取错误的情况.
    if (skuSelectMode == "on") {
        cloneHtml = currentProductDivArea.outerHTML();//将当前的html存在临时变量内,
        divParent = currentProductDivArea.parent();//获取当前的html的父元素,结合上面的临时变量用于还原
        //向上再移动80px给出+ -号的空间
        var hei = parseInt($("#skuSelectorDiv").css("height")) + 80;
        $("#skuSelectorDiv").css({ height: hei });
        $("#skuSelectorDiv").slideDown();
    }
    currentProductDivArea.remove();//将之前的+ -号区域清除,避免id冲突
    $("#skuSelectorDiv").find("b[type=showPrice]").remove();//如果有商品价格,先清除,下面再显示
    $("#skuSelectorDiv").append("<b type='showPrice'>￥" + salePrice + "</b>");//显示该商品的价格
    $("#skuSelectorDiv").append(currentProductDivArea.outerHTML());//将+ -号区域放置在当前div内
    $("#skuSelectorDiv").find(":hidden").show();//将减号和数字框显示
    $("#skuSelectorDiv").find("[type=skusSelect]").remove();


    //获取并重置该商品在购物车中已存在的数量
    var currentProductNumArea = $("#buyNum_" + currentProductId);//获取该商品选择数量的input控件
    var data = {};
    data.SkuId = skuId;
    $.post("/api/VshopProcess.ashx?action=GetNumberInShoppingcart", data, function (json) {
        if (json.success === true) {
            currentProductNumArea.val(json.number);
        }
    });
    skuSelectMode = "off";

}

jQuery.fn.outerHTML = function (s) {
    return (s) ? this.before(s).remove() : jQuery("<p>").append(this.eq(0).clone()).html();
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
            //查询有多少个规格,并动态赋值高度
            var height = $box.children().find("div[id^=skuRow_]").length *90;
            $box.css({ height: height });
            $box.slideDown(200);
            //将选择规格模式打开
            skuSelectMode = "on";
        }
        else {
            alert("出错了!");
        }
    });
}

//改变购物车数量
function chageCartProductQuantity(SkuId, Quantity, obj,isAfter) {
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "ChageQuantity", skuId: SkuId, quantity: Quantity },
        success: function (resultData) {
            if (resultData.Status != "OK") {
                alert("最多只可购买" + resultData.Status + "件");
                return false;
            }
            else {
                obj.val(Quantity);
                if (!isAfter) {
                    getTotalPrice();
                }
            }
        }
    });
}
//添加商品至购物车
//isafter,是否等动画效果结束后再做变值处理
function BuyProductToCart(Quantity, ProductSkuId, Categoryid, obj,isAfter) {
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "AddToCartBySkus", quantity: Quantity, productSkuId: ProductSkuId, categoryid: Categoryid },
        async: false,
        success: function (resultData) {
            if (resultData.Status == "OK") {
                obj.val(Quantity);
                if (!isAfter) {
                    getTotalPrice();
                }
            } else if (resultData.Status == "0") {
                // 商品已经下架
                alert("此商品已经不存在(可能被删除或被下架)，暂时不能购买");
                return false;
            }
            else if (resultData.Status == "1") {
                // 商品库存不足
                alert("商品库存不足 " + parseInt($("#buyNum").val()) + " 件，请修改购买数量!");
                return false;
            }
            else {
                //alert(resultData.Status);
                if (resultData.Status == "2") {
                    //location.href = "/Vshop/UserLogin.aspx?userstatus=0&productid=" + $("#vProductDetails_litproductid").val();
                    alert("请先登录系统");
                    location.href = "/Vshop/MemberCenter.aspx?userstatus=0&productid=" + $("#vProductDetails_litproductid").val();
                    
                }
                else if (resultData.Status == "-1") {
                    // 抛出异常消息
                    //alert("Ee");
                    // alert_h(resultData.Status + '66');
                    //location.href = "/Vshop/MemberCenter.aspx?userstatus=0&productid=" + $("#vProductDetails_litproductid").val();
                    alert("请先登录系统");
                    location.href = "/Vshop/MemberCenter.aspx?userstatus=0&productid=" + $("#vProductDetails_litproductid").val();
                }
                else {
                    alert(resultData.Status + '66');
                }
            }
        }
    });
}
//从购物车删除商品
function deleteCartProduct(SkuId, obj) {
    //update@20150923 by hj
    //新增一个giftId参数,并在VshopProcess增加相应的处理
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

//获取商品总价并计算放入总价栏
function getTotalPrice() {
    var tPrice = 0.00;//总价 //$("#vProductSearchBuy_litTotalPrice").val();
    var tNum = 0; //$("#vProductSearchBuy_litTotalNum").val();//总数量;
    /*
    $("[id^='buyPrice_']").each(function () {
        var buyPrice = $(this).html().replace("￥", "");
        var buyNum = $("#buyNum_" + $(this).attr("id").substring($(this).attr("id").indexOf('_')+1)).attr("value");;
        tPrice = getSum(tPrice, getX(buyPrice, buyNum));
        tNum = getSum(tNum, buyNum);
    });
    */
    $.ajax({
        url: "/API/VshopProcess.ashx",
        async:false,
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "GetTotalNumAndPrice", },
        success: function (resultData) {
            if (resultData.success == true) {
                tPrice = resultData.tPrice;
                tNum = resultData.tNum;
            }
        }
    });
    $("#priceTotal").html("总计：￥" + tPrice);
    $("#numTotal").show();
    if (tNum > 0) $("#numTotal").html(tNum)
    else $("#numTotal").hide();
    
}
function getSum(num1, num2) { return parseFloat(num1) + parseFloat(num2); }
function getX(num1, num2) { return parseFloat(num1) * parseFloat(num2); }