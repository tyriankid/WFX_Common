window.onload = function () {
    FastClick.attach(document.body);
    getCategorids();
    init();
    //getProducts();
}

$(function () {
    setFromMember();//获取并设置我的推荐人
    //localStorage.clear();
    //弹出规格选择框
    $("#main").on("click", "[role='productItem'],[salestatus=1]", function () {
        getSkuSelector($(this), $("#skuSelectorDiv"));
    });
    //规格选择触发事件
    $("#skuSelectorDiv").on("click", "div[role='skuBtn']", function () {
        SelectSkus($(this), $("p[role='productItem'][productId=" + $("#skuSelectorDiv").attr("productId") + "]"));
    });
    //绑定商品数量增加和减少事件
    $("#skuSelectorDiv").on("click", "span[role='add']", function () { changeOrderQuantity("add") });
    $("#skuSelectorDiv").on("click", "span[role='minus']", function () { changeOrderQuantity("minus") });
    //点击购物车弹出购物车框
    $("[role='clickshopCart']").click(function () {
        toggleShoppingCart();
    });
    //点击按钮开始订单支付流程
    $("[role='goOrder']").click(function () {
        if ($(this).hasClass("unOrder")) return false;
        //将运费,门店id等参数存入localStorage,供订单提交页面使用
        localStorage.setItem("proLa_roadPrice", $("[role='roadPrice']").html());
        localStorage.setItem("proLa_storeId", $("#hidUserStoreId").val());
        location.href = "submmitorder.aspx";
    });

    



})

function init() {
    
    $.post("/api/VshopProcess.ashx?action=ClearShoppingCart", function (json) {
        if (json.success === true) {
            getShoppingCartNumAsyc();
            GetShoppingCartTotal();
        }
    });
    localStorage.clear();
}

function setFromMember() {
    var fromMemberUserId = $("#hidFromUserId").val();
    var ajaxUrl = "http://" + window.location.host + "/api/StoreHandler.ashx?action=setFromMember&frommemberid=" + fromMemberUserId;
    $.ajax({
        type: 'get', dataType: 'json', timeout: 10000,
        url: ajaxUrl,
        success: function (e) {
            if (e.success == "1") {
                //alert("ok");
            }
        }
    });
}

var currentCategoryId = 0;
function getProducts(categoryId, P) {
    currentCategoryId = categoryId;
    var data = {
        categoryid : categoryId,
        p : P
    }
    var ajaxUrl = "http://" + window.location.host + "/api/StoreHandler.ashx?action=getProductsByCategoryId&categoryid=" + categoryId;
    $.ajax({
        type: 'get', dataType: 'json', timeout: 10000,
        url: ajaxUrl,
        success: function (e) {
            if (e.state === 0) {
                var productsLi = "";
                for (var i = 0; i < e.data.dtproduct.length; i++) {
                    productsLi += "<li ><div class='imgbox'><img onclick='getImgGroup(" + i + "," + categoryId + ")' src='http://" + window.location.host + e.data.dtproduct[i].ImageUrl1 + "' /></div><div class='productInfo'>";
                    //productsLi += "<p>参考价<span>￥<i>" + e.data.dtproduct[i].saleprice + "</i></span></p>";
                    if (e.data.dtproduct[i].salestatus == "1") {
                        productsLi += "<p role='productItem' salestatus='" + e.data.dtproduct[i].salestatus + "' CategoryId='" + e.data.dtproduct[i].categoryid + "' ProductId='" + e.data.dtproduct[i].productid + "' productname='" + e.data.dtproduct[i].productname + "' BuyPrice='" + e.data.dtproduct[i].saleprice + "' HasSku='" + e.data.dtproduct[i].hasSKU + "'  Quantity = '0' Stock='" + e.data.dtproduct[i].stock + "' SkuId='" + e.data.dtproduct[i].skuid + "'>" + e.data.dtproduct[i].productname + "<span class='standard'>" + e.data.dtproduct[i].saleprice + "</span></p></div></li>";
                    }
                    else if (e.data.dtproduct[i].salestatus == "4") {
                        productsLi += "<p>" + e.data.dtproduct[i].productname  + "<span class='standard'>售罄</span></p></div></li>";
                    }
                    
                } 
                $("#goodsList ul").append(productsLi);
            }
            else if(e.state === 1){
                //没有商品

            }
        }
    });
}

function getCategorids() {
    var ajaxUrl = "http://" + window.location.host + "/api/StoreHandler.ashx?action=getCategories";
    $.ajax({
        type: 'get', dataType: 'json', timeout: 10000,
        url: ajaxUrl,
        success: function (e) {
            if (e.state === 0) {
                //拼接分类列表
                var categoryLi = "";
                for (var i = 0; i < e.data.length; i++) {
                    var cls = "";
                    if (i == 0) {
                        cls = "activeLi";
                        //$(".title").text(e.data[i].Name);
                        getProducts(e.data[i].CategoryId);
                    }
                    categoryLi += "<li class='" + cls + "' categoryid='" + e.data[i].CategoryId + "' onclick='getProductsList(this," + e.data[i].CategoryId + ",\"" + e.data[i].Name + "\")'><a>" + e.data[i].Name + "</a></li>";
                }
                $("#categoryList").append(categoryLi);
            }
            else {
                alert("error");
            }
        }
    });
}

//选择分类
function getProductsList(target, categoryId, name) {
    //$(".title").text(name);
    $(target).addClass("activeLi").siblings().removeClass("activeLi");
    $("#goodsList ul").html(" ");
    getProducts(categoryId);
}

/*加载规格控件
    *$skuSelectorDiv : 规格选择器div
    *$productDiv : 商品div,div的属性包含了商品的大部分属性:控件id="product_[商品id]" ProductId,ProductName,BuyPrice,SkuCounts,Quantity(默认为0),Stock,SkuId
    */
function getSkuSelector($productDiv, $skuSelectorDiv) {
    //弹出规格窗之前,现将该商品的默认规格和价格等属性复制给规格选择器 stock skuid
    $skuSelectorDiv.attr("stock", $productDiv.attr("stock"));
    $skuSelectorDiv.attr("skuid", $productDiv.attr("skuid"));
    $skuSelectorDiv.attr("categoryid", $productDiv.attr("categoryid"));

    var data = {};
    data.ProductId = $productDiv.attr("ProductId");
    $.post("/api/ProductsHandler.ashx?action=GetQuickOrderSKUSelectorWithoutBtn", data, function (json) {
        if (json.success === true) {
            //如果成功,显示选择规格的div,并给div赋值上该商品的id
            $skuSelectorDiv.attr("productId", $productDiv.attr("productId"));
            $skuSelectorDiv.html(json.selector); $skuSelectorDiv.show();
            $skuSelectorDiv.css("bottom", -$skuSelectorDiv.height());
            $skuSelectorDiv.animate({ bottom: "0" }, "fast");
            $(".mask").show();
            if ($skuSelectorDiv.height() > $(window).height()) {
                $skuSelectorDiv.css({ "height": $(window).height(), "overflow-y": "scroll" });
            }
            $(".spec-kind div").on("click", function () {
                $(this).addClass("selected").siblings().removeClass("selected");
            });
            $(".mask").on("click", function () {

                quickHide();

            });
            //从cookie中读取之前选过的规格等信息
            loadSelectInfo();
        }
        else {
            alert("出错了!");
        }
    });
}

function quickHide() {
    var $skuSelectorDiv = $("#skuSelectorDiv");
    $skuSelectorDiv.height("auto");
    $skuSelectorDiv.html('');
    $(".mask").hide();
}

function flyHide() {
    var $skuSelectorDiv = $("#skuSelectorDiv");
    $skuSelectorDiv.css("width", $skuSelectorDiv.css("width"));
    $skuSelectorDiv.css("height", $skuSelectorDiv.css("height"));
    $skuSelectorDiv.html('1');
    $skuSelectorDiv.addClass("add");
    setTimeout(function () { $skuSelectorDiv.height("auto"); $skuSelectorDiv.hide().removeClass("add") }, 850);
    $(".mask").hide();
    isfly = false;
}

/*添加商品至购物车
* Quantity : 商品数量 ; ProductSkuId:商品SkuId ; CategoryId:商品分类Id
*/
function BuyProductToCart(Quantity, ProductSkuId, Categoryid) {
    // 验证数量输入
    var buyNum = $("[role='orderQuantity']");
    var ValidateBuyAmount = function ValidateBuyAmount() {
        
        if (parseInt(buyNum.val()) == 0) {
            alert("请先填写购买数量!"); buyNum.val(0);
            return false;
        }
        var amountReg = /^[1-9]d*|0$/;
        if (!amountReg.test($(buyNum).val())) {
            alert("请填写正确的购买数量!"); buyNum.val(0)
            return false;
        }
        return true;
    }
    // 是否所有规格都已选
    var IsallSelected = function IsallSelected() {
        var allSelected = true;
        $.each($("input[type='hidden'][name='skuCountname']"), function () {
            if ($(this).val().length == 0) {
                allSelected = false;
            }
        });
        return allSelected;
    }


    if (!ValidateBuyAmount()) {
        return false; buyNum.val(0);
    }
    if (!IsallSelected()) {
        location.href = "#skuArea";
        alert("请选择规格"); buyNum.val(0);
        return false;
    }
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "AddToCartBySkus", quantity: Quantity, productSkuId: ProductSkuId, categoryid: Categoryid },
        async: false,
        success: function (resultData) {
            if (resultData.Status == "OK") {
                ///////////////////////////////////////////////////
                /*
                $("body").css("padding-bottom", "3rem");
                $("#skuSelectorDiv").height("auto");
                $("#skuSelectorDiv").html('');
                $(".mask").hide();
                */
                
                getShoppingCartNumAsyc();
            } else if (resultData.Status == "0") {
                // 商品已经下架
                alert("此商品已经不存在(可能被删除或被下架)，暂时不能购买"); buyNum.val(0);
                return false;
            }
            else if (resultData.Status == "1") {
                // 商品库存不足
                alert("商品库存不足 " + parseInt($("#skuSelectorDiv").attr("stock")) + " 件，请修改购买数量!"); buyNum.val(0);
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

/*点选规格值后赋值
    * $clt:规格按钮
    * $productDiv:该规格所属的商品div
    */
function SelectSkus($clt, $productDiv) {
    //根据选择的规格动态给当前skuSelectDiv的属性赋值
    var ResetCurrentSku = function ResetCurrentSku(skuId, sku, weight, stock, salePrice, $productDiv) {
        var currentProductId = $productDiv.attr("productId");
        var currentProductSkuidArea = $("#skuSelectorDiv");//获取该商品的skuId
        var currentProductPriceArea = $("p[role='skuPrice']");//获取该商品的展示区域对象
        //刷新productDiv的skuid
        $productDiv.attr("skuid", skuId);
        currentProductSkuidArea.attr("skuId", skuId);//重置该商品的skuId
        currentProductPriceArea.html(salePrice);//重置该商品的价格
        currentProductSkuidArea.attr("stock", stock);//设置该商品的stock属性

        if (!isNaN(parseInt(stock))) {//判断库存
            currentProductSkuidArea.attr("stock", stock);
        }
        else {
            currentProductSkuidArea.attr("stock", 0);
            alert("该规格的产品没有库存，请选择其它的规格！");
            return false;
        }

    }
    //重置规格值的样式
    var ResetSkuRowClass = function ResetSkuRowClass(skuRowId, skuSelectId) {
        var pvid = skuSelectId.split("_");
        $.each($("#" + skuRowId + " div"), function () {
            $(this).removeClass('active');
        });
        $("#" + skuSelectId).addClass('active');
    }
    //是否所有规格都已选
    var IsallSelected = function IsallSelected() {
        var allSelected = true;
        $.each($("input[type='hidden'][name='skuCountname']"), function () {
            if ($(this).val().length == 0) {
                allSelected = false;
            }
        });
        return allSelected;
    }


    // 保存当前选择的规格
    var AttributeId = $clt.attr("AttributeId");
    var ValueId = $clt.attr("ValueId");
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
                    ResetCurrentSku(resultData.SkuId, resultData.SKU, resultData.Weight, resultData.Stock, resultData.SalePrice, $productDiv);
                }
                else {
                    ResetCurrentSku("", "", "", "", "0", $productDiv); //带服务端返回的结果，函数里可以根据这个结果来显示不同的信息
                }
            },
            error: function () {
            }
        });
    }

}

/*修改商品点餐数量
    * type: 若为add则是加,若为minus则是减
    */
function changeOrderQuantity(type) {

    //对库存数量做判断
    var isStockEnough = function isStockEnough(orderQuantity) {
        orderQuantity = parseInt(orderQuantity);
        orderStock = parseInt($("#skuSelectorDiv").attr("stock"));
        if (orderQuantity < orderStock)
            return 1;
        else
            alert("库存不足!");
        return 0;
    }

    //改变quantity
    var $orderQuantity = $("[role='orderQuantity']");
    var currentSkuid = $("#skuSelectorDiv").attr("skuid");
    var oq = $orderQuantity.val();
    switch (type) {
        case "add":

            

            $orderQuantity.val(getSum(oq, isStockEnough(oq)));

            isfly = true;
            if ($orderQuantity.val() == "1")//第一次加
            {
                BuyProductToCart(1,currentSkuid , currentCategoryId);
            }
            else {//之后就是更改数量
                chageCartProductQuantity(currentSkuid, $orderQuantity.val(), $orderQuantity);
            }
           

            break;
        case "minus":
            if (parseInt($orderQuantity.val()) > 0) {
                $orderQuantity.val(getMinus(oq, 1));
            }
            if(parseInt($orderQuantity.val()) >0){
                //大于零是更改
                chageCartProductQuantity(currentSkuid, $orderQuantity.val(), $orderQuantity);
            }
            else {//等于零是删除
                deleteCartProduct(currentSkuid, $orderQuantity);
            }
            break;
    }

    setSelectInfo("selector");
}

function changeOrderQuantityCart(type, skuid) {
    var $cartProductLi = $("[role='shoppingCartDiv']").find("li[skuid='"+skuid+"']");
    //对库存数量做判断
    var isStockEnough = function isStockEnough(orderQuantity) {
        orderQuantity = parseInt(orderQuantity);
        
        orderStock = parseInt($cartProductLi.attr("stock"));
        if (orderQuantity < orderStock)
            return 1;
        else
            alert("库存不足!");
        return 0;
    }

    //改变quantity
    var $orderQuantity = $cartProductLi.find("[role='cartOrderQuantity']");
    var currentSkuid = skuid;
    var productCategoryId = $cartProductLi.attr("categoryid");
    var oq = $orderQuantity.html();
    switch (type) {
        case "add":
            $orderQuantity.html(getSum(oq, isStockEnough(oq)));
            if ($orderQuantity.html() == "1")//第一次加
            {
                BuyProductToCart(1, currentSkuid, productCategoryId);
            }
            else {//之后就是更改数量
                chageCartProductQuantity(currentSkuid, $orderQuantity.html(), $orderQuantity);
            }
            break;
        case "minus":
            if (parseInt($orderQuantity.html()) > 0) {
                $orderQuantity.html(getMinus(oq, 1));
            }
            if (parseInt($orderQuantity.html()) > 0) {
                //大于零是更改
                chageCartProductQuantity(currentSkuid, $orderQuantity.html(), $orderQuantity);
            }
            else {//等于零是删除
                deleteCartProduct(currentSkuid, $orderQuantity);
            }
            break;
    }

    setSelectInfo("cart", $cartProductLi);
}

//将选好的规格,数量等信息存入cookie
//type为规格选择框和购物车预览框,
function setSelectInfo(type, obj) {
    var $selector, productid, price, quantity, productname, skuid, stock, categoryid;
    switch (type) {
        case "selector":
            
            $selector = $("#skuSelectorDiv"); productid = $selector.attr("productid"); price = $selector.find("[role='skuPrice']").html(); 
            quantity = $selector.find("[role='orderQuantity']").val(); productname = $selector.find("[role='productName']").html();
            skuid = $selector.attr("skuid"); stock = $selector.attr("stock"); categoryid = $selector.attr("categoryid");
            break;
        case "cart":
            $selector = obj; productid = obj.attr("productid"); price = obj.find("[role='cartOrderPrice']").html().replace("￥", "");
            quantity = obj.find("[role='cartOrderQuantity']").html(); productname = obj.find("[role='cartProductName']").html();
            skuid = obj.attr("skuid"); stock = obj.attr("stock"); categoryid = obj.attr("categoryid");
            break;
    }


    var selectedSkus = "";
    var selectedSkuNames = ""
    if (type == "selector") {
        $selector.find(".selected,active").each(function () {
            selectedSkus += $(this).attr("id") + ",";
            selectedSkuNames += $(this).html() + ",";
        });
        selectedSkus = selectedSkus.substr(0, selectedSkus.length - 1);
        selectedSkuNames = selectedSkuNames.substr(0, selectedSkuNames.length - 1);
    }


    localStorage.setItem("product_" + skuid + "_price", price);
    if (type == "selector") {
        localStorage.setItem("product_" + skuid + "_skus", selectedSkus);
        localStorage.setItem("product_" + skuid + "_skunames", selectedSkuNames);
    }
    localStorage.setItem("product_" + skuid + "_quantity", quantity);
    localStorage.setItem("product_" + skuid + "_name", productname);
    localStorage.setItem("product_" + skuid + "_skuid", skuid);
    localStorage.setItem("product_" + skuid + "_stock", stock);
    localStorage.setItem("product_" + skuid + "_categoryid", categoryid);
    //顺便将操作过的数量大于零的商品放入cookie内,便于左下角弹出购物车预览时取值
    var idList = localStorage.getItem("productIdList");
    if (idList == null) {
        localStorage.setItem("productIdList", skuid);
    } else if (("," + idList + ",").indexOf("," + skuid + ",") < 0) {
        localStorage.setItem("productIdList", idList + "," + skuid);
    }


    //如果商品数量为零,清除所有相关的cookie
    if (quantity == "0" && obj!=null) {
        obj.remove();//去掉购物车当前商品列
        //获取productIdlist,删除掉quantity为0的productid
        var idListArray = localStorage.getItem("productIdList").split(",");
        if (idListArray.indexOf(skuid) >= 0) {
            idListArray.splice(idListArray.indexOf(skuid), 1);
            var idListNew = "";
            for (var i = 0; i < idListArray.length; i++) {
                idListNew += idListArray[i] + ",";
            }
            idListNew = idListNew.substr(0, idListNew.length - 1);
            localStorage.setItem("productIdList",idListNew);
        }
        localStorage.removeItem("product_" + skuid + "_price");
        localStorage.removeItem("product_" + skuid + "_skus");
        localStorage.removeItem("product_" + skuid + "_skunames");
        localStorage.removeItem("product_" + skuid + "_quantity");
        localStorage.removeItem("product_" + skuid + "_name");
        localStorage.removeItem("product_" + skuid + "_skuid");
        localStorage.removeItem("product_" + skuid + "_stock");
        localStorage.removeItem("product_" + skuid + "_categoryid");
        
    }

    getShoppingCartNumAsyc();
    GetShoppingCartTotal();

    
}

//将对应的商品选中的内容从cookie中取出并展示在页面上
function loadSelectInfo() {

    var $selector = $("#skuSelectorDiv");
    var productid = $selector.attr("productid");
    var skuid = $selector.attr("skuid");

    //首先判断该商品的cookie是否有值
    var price = localStorage.getItem("product_" + skuid + "_price");
    var quantity = localStorage.getItem("product_" + skuid + "_quantity");
    var selectedSkus = localStorage.getItem("product_" + skuid + "_skus");

    if (price === null) {
        return;
    }
    $selector.find("[role='skuPrice']").html(price);
    $selector.find("[role='orderQuantity']").val(quantity);
    //将所有保存起来的规格按钮点一遍.
    var arraySkus = selectedSkus.split(',');
    for (var i = 0; i < arraySkus.length; i++) {
        $("#" + arraySkus[i]).click();
    }

}

function syncInfoToShoppingCart() {
    $("[role='shoppingCartDiv']").find("ul").html('');
    var idList = localStorage.getItem("productIdList");
    if (idList == null) return;
    var arrayIdList = idList.split(",");

    var li = "";
    for (var i = 0; i < arrayIdList.length; i++) {
        var price = localStorage.getItem("product_" + arrayIdList[i] + "_price");
        var quantity = localStorage.getItem("product_" + arrayIdList[i] + "_quantity");
        var name = localStorage.getItem("product_" + arrayIdList[i] + "_name");
        var skuid = localStorage.getItem("product_" + arrayIdList[i] + "_skuid");
        var stock = localStorage.getItem("product_" + arrayIdList[i] + "_stock");
        var categoryid = localStorage.getItem("product_" + arrayIdList[i] + "_categoryid");
        //将规格名加上去
        var skunames = localStorage.getItem("product_" + skuid + "_skunames");
        if (skunames != null) {
            skunames = skunames.substr(0, skunames.length - 1);
        }
        else {
            return;
        }
        /*
        var skunamesArray = skunames.split(",");
        var skunameShow = "";
        if (skunamesArray.length > 0) {
            for (var o = 0; o < skunamesArray.length; o++) {
                skunameShow += skunamesArray[o] + ",";
            }
            skunameShow = skunameShow.substr(0, skunameShow.length - 1);
        }
        */

        li += '<li categoryid="' + categoryid + '" skuid="' + skuid + '" productid="' + arrayIdList[i] + '" stock="' + stock + '"> <div class="leftbar">' + '<p role="cartProductName">' + name + " " + skunames + '</p>' + '<p onclick="deleteCartProduct(\'' + skuid + '\')">删除</p>'
                + '</div><div class="rightbar">'
                    + '<span class="privilege"><img src="/templates/vshop/common/images/ad/privilege.png" /></span>'
                    + '<span role="cartOrderPrice">' + price + '</span>'
                    + '<span class="opration" onclick="changeOrderQuantityCart(\'minus\',\'' + skuid + '\')"><img src="/templates/vshop/common/images/ad/minus.png" /></span>'
                    + '<span class="goodsNum" role="cartOrderQuantity">' + quantity + '</span>'
                    + '<span class="opration" onclick="changeOrderQuantityCart(\'add\',\'' + skuid + '\')"><img src="/templates/vshop/common/images/ad/plus.png" /></span>'
                + '</div></li>'
    }
    $("[role='shoppingCartDiv']").find("ul").append(li);
    //alert(idList);
}

function toggleShoppingCart() {
    //if ($("[role='cartNum']").html() == "0") return;
    syncInfoToShoppingCart();
    $("[role='shoppingCartDiv']").toggle(200);
}

//加减乘法
var getMinus = function getSum(num1, num2) { return parseFloat(num1) - parseFloat(num2); }
var getSum = function getSum(num1, num2) { return parseFloat(num1) + parseFloat(num2); }
var getX = function getX(num1, num2) { return parseFloat(num1) * parseFloat(num2); }

var isfly = false;
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
                flyHide();
                //$(".close").click();
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
                if (obj != undefined)
                    obj.val("0");
                //getTotalPrice();
            }
        }
    });
}

function getShoppingCartNumAsyc() {
    $.post("/api/ProductsHandler.ashx?action=GetShoppingCartGoodNum", function (json) {
        if (json.success === true) {
            $("[role='cartNum']").html(json.num);
            if (isfly)
                flyHide();
            else
                quickHide();
        }
        else {
            alert('出错了');
        }
    });
}

//获取cookie
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
    }
    return "";
}
var orderRoadPrice = 0;
var orderMinPrice = 0;
var orderFreeRoadPrice = 0;
function GetShoppingCartTotal() {
    $.post("/api/ProductsHandler.ashx?action=GetShoppingCartTotal", function (json) {
        if (json.success === true) {
            var flag = false; var isOutOfRange = false;
            $("[role='cartTotal']").html(json.totalprice);
            $("[role='marketPriceToral']").html(json.totalprice);
            //如果点餐金额超过了最低消费金额,或者是加单状态,显示订单提交按钮
            var roadPriceInfo = $("#hidMinPrice").val();
            var subList = roadPriceInfo.split(';');//获取区间list
            var memberDistance = Math.round($("#hidUserDistance").val()); //parseInt(localStorage.getItem("memberDistance"));//获取用户当前距离店面的距离
            for(var i =0;i<subList.length;i++){
                var valueList = subList[i].split(',');
                var dStart = valueList[0], dEnd = valueList[1], minPrice = valueList[2], roadPrice = valueList[3], freeRoadPrice = valueList[4]; 
                //根据距离匹配值
                if (memberDistance && dStart <= memberDistance && dEnd >= memberDistance)//匹配到区间,设置对应的最低消费价格和配送金额
                {
                    orderMinPrice = minPrice, orderRoadPrice = roadPrice, orderFreeRoadPrice = freeRoadPrice;
                    $("[role='roadPrice']").html(orderRoadPrice);
                    flag = true;
                    isOutOfRange = false;
                    break;
                }
                else {
                    isOutOfRange = true;
                }

                //flag = true; //测试用,永远显示点单按钮
            }
            //满足配送最低价的显示
            if ((parseFloat(json.totalprice) >= orderMinPrice && flag) || getCookie("proLa_AddOrder").length > 10) {
                if ((parseFloat(json.totalprice) < orderFreeRoadPrice))//满多少免配送费
                {
                    $("[role='freeRoadPrice']").html("满" + orderFreeRoadPrice + "元免配送费");
                }
                else {
                    $("[role='freeRoadPrice']").html(""); $("[role='roadPrice']").html(0);
                }
                
                $("[role='goOrder']").removeClass("unOrder");
            }
            else {//不满足最低配送价的显示
                $("[role='freeRoadPrice']").html("满" + orderMinPrice + "元可送餐");
                $("[role='goOrder']").addClass("unOrder");
            }

            //如果在配送距离之外,给出提示
            if (isOutOfRange) {
                $("[role='freeRoadPrice']").html("您在配送范围之外!");
            }
        }
        else {
            alert('出错了');
        }
    });
}

function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}


