window.onload = function () {
    FastClick.attach(document.body);
    getCategorids();
    GetShoppingCartCanOrder();
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
    $("#skuSelectorDiv").on("click", "span[role='add']", function () { changeOrderQuantity("add",this) });
    $("#skuSelectorDiv").on("click", "span[role='minus']", function () { changeOrderQuantity("minus",this) });
    //点击购物车弹出购物车框
    $("[role='clickshopCart']").click(function () {
        toggleShoppingCart();
    });
    //点击地址弹出地址选择页面
    $("[ role='addressChose']").click(function () {
        location.href = "/Vshop/ShippingAddresses.aspx?type=choseAdd";
    });

    //点击门店按钮切换门店事件
    $("#main").on("click", "[role='storeSelect'] li", function () {
        location.href = "picorder.aspx?poi_id=" + $(this).attr("poi_id");
    });

    //点击按钮开始订单支付流程
    $("[role='goOrder']").click(function () {
        //测试用(无条件下单)
        //$(this).removeClass("unOrder"); $("[role='roadPrice']").html(0);

        if ($(this).hasClass("unOrder")) { //如果当前最低消费不够,弹出提示
            alert($("[role='freeRoadPrice']").html())
            return false;
        }
        if ($("[role='roadPrice']").html() > 0)//如果当前最低消费不够免配送费,也弹出提示
        {
            if (!confirm("还差" + (orderFreeRoadPrice - parseInt($("[role='cartTotal']").html())) + "元就可以免配送费了哦！确定现在就下单吗？")) return false;
        }

        $(this).addClass("unOrder");
        //批量提交购物车
        var quantities = "", skuids = "", categoryids = "";
        for (var i = 0; i < cartinfo.items.length; i++) {
            quantities +=cartinfo.items[i].quantity+",";
            skuids += cartinfo.items[i].skuid+",";
            categoryids += cartinfo.items[i].category+",";
        }
        quantities = quantities.substr(0,quantities.length-1);
        skuids = skuids.substr(0, skuids.length - 1);
        categoryids = categoryids.substr(0, categoryids.length - 1);
        
            $.ajax({
                url: "/API/VshopProcess.ashx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { action: "AddToCartByMultSkus", quantity: quantities, productSkuId: skuids, categoryid: categoryids },
                async: false,
                success: function (resultData) {
                    if (resultData.Status == "OK") {
                        //将运费,门店id等参数存入localStorage,供订单提交页面使用
                        localStorage.setItem("proLa_roadPrice", $("[role='roadPrice']").html());
                        localStorage.setItem("proLa_storeId", $("#hidUserStoreId").val());
                        location.href = "submmitorderPro.aspx";
                    } else if (resultData.Status == "0") {
                        alert("此商品已经不存在(可能被删除或被下架)，暂时不能购买"); buyNum.val(0);
                        return false;
                    }
                    else if (resultData.Status == "1") {
                        alert("商品库存不足 " + parseInt($("#skuSelectorDiv").attr("stock")) + " 件，请修改购买数量!"); buyNum.val(0);
                        return false;
                    }
                    else {
                        if (resultData.Status == "2") {
                            alert("请先登录系统");
                            location.href = "/Vshop/MemberCenter.aspx?userstatus=0&productid=" + $("#vProductDetails_litproductid").val();
                        }
                        else if (resultData.Status == "-1") {
                            alert("请先登录系统");
                            location.href = "/Vshop/MemberCenter.aspx?userstatus=0&productid=" + $("#vProductDetails_litproductid").val();
                        }
                        else {
                            alert(resultData.Status + '66');
                        }
                    }
                }
            });
        

    });

})

function init() {
    $.post("/api/VshopProcess.ashx?action=ClearShoppingCart");
    localStorage.clear();
}

function setFromMember() {
    var fromMemberUserId = $("#hidFromUserId").val();
    var ajaxUrl = "http://" + window.location.host + "/api/StoreHandler.ashx?action=setFromMember&frommemberid=" + fromMemberUserId;
    /*
    $.ajax({
        type: 'get', dataType: 'json', timeout: 10000,
        url: ajaxUrl,
        success: function (e) {
            if (e.success == "1") {
                //alert("ok");
            }
        }
    });
    */
}

var currentCategoryId = 0;
function getProducts(categoryId, P) {
    currentCategoryId = categoryId;
    var data = {
        categoryid : categoryId,
        p : P
    }
    var ajaxUrl = "http://" + window.location.host + "/api/StoreHandler.ashx?action=getProductsByCategoryId&categoryid=" + categoryId + "&storeid=" + $("#hidUserStoreId").val();
    $.ajax({
        type: 'get', dataType: 'json', timeout: 10000,
        url: ajaxUrl,
        success: function (e) {
            if (e.state === 0) {
                var productsLi = "";
                var orderedProductids = [];
                var orderedProductQuantity = [];
                for (var o = 0; o < cartinfo.items.length; o++) {
                    orderedProductids.push(parseInt(cartinfo.items[o].productid));
                    orderedProductQuantity.push(parseInt(cartinfo.items[o].quantity));
                }


                for (var i = 0; i < e.data.dtproduct.length; i++) {
                    
                    productsLi += "<li ><div class='imgbox'><img onclick='getImgGroup(" + i + "," + categoryId + ")' src='http://" + window.location.host + e.data.dtproduct[i].ImageUrl1 + "' /></div><div class='productInfo'>";
                    var count=0;
                    for (var p = 0; p < orderedProductids.length; p++) {
                        if(orderedProductids[p] == e.data.dtproduct[i].productid){
                            count+=orderedProductQuantity[p];
                        }
                    }
                    var strOrderedQuantity = "";
                    if (count > 0) { strOrderedQuantity = " (已点" + count + "份)" }
                    if (e.data.dtproduct[i].salestatus == "1") {
                        productsLi += "<p role='productItem' salestatus='" + e.data.dtproduct[i].salestatus + "' CategoryId='" + e.data.dtproduct[i].categoryid + "' ProductId='" + e.data.dtproduct[i].productid + "' productname='" + e.data.dtproduct[i].productname + "' BuyPrice='" + e.data.dtproduct[i].saleprice + "' HasSku='" + e.data.dtproduct[i].hasSKU + "'  Quantity = '0' Stock='" + e.data.dtproduct[i].stock + "' SkuId='" + e.data.dtproduct[i].skuid + "'>" + e.data.dtproduct[i].productname + strOrderedQuantity+"<span class='standard'>" + e.data.dtproduct[i].saleprice +"</span></p></div></li>";
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
    if ($skuSelectorDiv.hasClass("add")) return;

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
            //loadSelectInfo();
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
    setTimeout(function () { $skuSelectorDiv.height("auto"); $skuSelectorDiv.hide().removeClass("add") }, 900);
    $(".mask").hide();
    isfly = false;
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
                    ResetCurrentSku(resultData.SkuId, resultData.SKU, resultData.Weight, resultData.Stock, parseInt( resultData.SalePrice), $productDiv);
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
var cartinfo = {}; cartinfo.items = [];
function changeOrderQuantity(type, self) {

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
    
    //获取当前商品的属性

    var $skuSelector = $("#skuSelectorDiv");
    var $cartOrder = $(self).closest("[role='cartOrder']");

    var skuid, quantity, currentSkuid, productName, productPrice, productId, stock, skuName;

    if ($skuSelector.html().length > 10) //如果是规格选择框触发的此方法
    {
        skuid = $skuSelector.attr("skuid");
        //var $orderQuantity = $("[role='orderQuantity']");
        quantity = 0;
        productName = $("[role='productName']").html();
        productPrice = $("[role='skuPrice']").html().replace('￥', '');
        productId = $skuSelector.attr("productid");
        stock = $skuSelector.attr("stock");
        skuName = ""
        $("[role='skuBtn'].selected").each(function () {
            skuName += $(this).html() + ",";
        });
        skuName = skuName.substr(0, skuName.length - 1);
    }
    else //如果是购物车预览页面触发的此方法
    {
        skuid = $cartOrder.attr("skuid");
        quantity = 0;
        productName = $cartOrder.attr("productname"); 
        productPrice = $cartOrder.find("[role='cartOrderPrice']").html(); 
        productId = $cartOrder.attr("productid");
        stock = $cartOrder.attr("stock");
        skuName = $cartOrder.attr("productskuname");

    }




    var isExist = false;
    for (var i = 0; i < cartinfo.items.length; i++) {
        if (skuid == cartinfo.items[i].skuid) {//遇到重复的商品
            isExist = true;
            quantity = parseInt( cartinfo.items[i].quantity);
            if (type == "add") {
                cartinfo.items[i].quantity = quantity + 1;
                break;
            }
            else if (quantity > 1) {
                cartinfo.items[i].quantity = quantity - 1;
                break;
            }
            else if (quantity == 1) {
                cartinfo.items.splice(i, 1); break;
            }
        }
    }

    
    if (type == "add" &&  isExist == false) {
        //将商品插入items内
        cartinfo.items.push({ "quantity": quantity + 1, "skuid": skuid, "category": currentCategoryId, "name": productName, "price": productPrice, "skuname": skuName, "productid": productId, "stock": stock });
        
    }
    
    var orderedProductids = [];
    var orderedProductQuantity = [];
    for (var o = 0; o < cartinfo.items.length; o++) {
        orderedProductids.push(parseInt(cartinfo.items[o].productid));
        orderedProductQuantity.push(parseInt(cartinfo.items[o].quantity));
    }

    var count = 0;
    for (var p = 0; p < orderedProductids.length; p++) {
        if (orderedProductids[p] == productId) {
            count += orderedProductQuantity[p];
        }
    }
    var strOrderedQuantity = "";
    if (count > 0) { strOrderedQuantity = " (已点" + count + "份)" }
    $("[role='productItem'][productid='" + productId + "']").html(productName + strOrderedQuantity +"<span class='standard'>" + productPrice +"</span>");

    //同步总价,总数量,购物车预览页面
    var totalPrice = 0;
    var totalNum = 0;
    var li = "";
    for (var o = 0; o < cartinfo.items.length; o++) {
        totalPrice += parseFloat(cartinfo.items[o].price) * parseInt(cartinfo.items[o].quantity);
        totalNum += parseInt(cartinfo.items[o].quantity);
        li += '<li role="cartOrder" categoryid="' + cartinfo.items[o].category + '" skuid="' + cartinfo.items[o].skuid + '" productid="' + cartinfo.items[o].productid + '" stock="' + cartinfo.items[o].stock + '" productname="' + cartinfo.items[o].name + '"   productskuname="' + cartinfo.items[o].skuname + '"> <div class="leftbar">' + '<p role="cartProductName">' + cartinfo.items[o].name + " " + cartinfo.items[o].skuname + '</p>' //+ '<p onclick="deleteCartProduct(\'' + skuid + '\')">删除</p>'
        + '</div><div class="rightbar">'
            + '<span class="privilege"><img src="/templates/vshop/common/images/ad/privilege.png" /></span>'
            + '<span role="cartOrderPrice">' + cartinfo.items[o].price + '</span>'
            + '<span class="opration" onclick="changeOrderQuantity(\'minus\',this)"><img src="/templates/vshop/common/images/ad/minus.png" /></span>'
            + '<span class="goodsNum" role="cartOrderQuantity">' + cartinfo.items[o].quantity + '</span>'
            + '<span class="opration" onclick="changeOrderQuantity(\'add\',this)"><img src="/templates/vshop/common/images/ad/plus.png" /></span>'
        + '</div></li>'
    }
    $("[role='shoppingCartDiv']").find("ul").html('').append(li);//购物车预览
    $("[role='cartTotal']").html(totalPrice); //商品总价
    $("[role='marketPriceToral']").html(totalPrice); //商品总价
    $("[role='cartNum']").html(totalNum); //商品数量

    //判断是否能够点单
    GetShoppingCartCanOrder();

    flyHide();


    //改变quantity

    /*
    var oq = $orderQuantity.val();
    switch (type) {
        case "add":
            $orderQuantity.val(getSum(oq, isStockEnough(oq)));

            isfly = true;

            
            if ($orderQuantity.val() == "1")//第一次加
            {
                orderToCart(1, currentSkuid);
                //BuyProductToCart(1,currentSkuid , currentCategoryId);
            }
            else {//之后就是更改数量
                orderToCart($orderQuantity.val(), currentSkuid);
                //chageCartProductQuantity(currentSkuid, $orderQuantity.val(), $orderQuantity);
            }
           
           
            break;
        case "minus":
            //先做减法,再改值
            if (parseInt($orderQuantity.val()) > 0) {
                $orderQuantity.val(getMinus(oq, 1));
            }
            if(parseInt($orderQuantity.val()) >0){
                //大于零是更改
                orderToCart($orderQuantity.val(), currentSkuid);
                //chageCartProductQuantity(currentSkuid, $orderQuantity.val(), $orderQuantity);
            }
            else {//等于零是删除
                orderToCart($orderQuantity.val(), currentSkuid);
                //deleteCartProduct(currentSkuid, $orderQuantity);
            }
            break;
    }

    //setSelectInfo("selector");
    */
}








function toggleShoppingCart() {
    $("[role='shoppingCartDiv']").toggle(200);
}

//加减乘法
var getMinus = function getSum(num1, num2) { return parseFloat(num1) - parseFloat(num2); }
var getSum = function getSum(num1, num2) { return parseFloat(num1) + parseFloat(num2); }
var getX = function getX(num1, num2) { return parseFloat(num1) * parseFloat(num2); }

var isfly = false;


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
function GetShoppingCartCanOrder() {

    var totalPrice = $("[role='cartTotal']").html();
    var flag = false;
    var isOutOfRange = false;

    //如果点餐金额超过了最低消费金额,或者是加单状态,显示订单提交按钮
    var roadPriceInfo = $("#hidMinPrice").val();
    var subList = roadPriceInfo.split(';');//获取区间list
    //向后取整.0.1公里等于1公里,2.3公里等于3公里
    var memberDistance = Math.ceil($("#hidUserDistance").val()); //parseInt(localStorage.getItem("memberDistance"));//获取用户当前距离店面的距离
    for(var i =0;i<subList.length;i++){
        var valueList = subList[i].split(',');
        var dStart = valueList[0], dEnd = valueList[1], minPrice = valueList[2], roadPrice = valueList[3], freeRoadPrice = valueList[4]; 
        //根据距离匹配值
        if (memberDistance == 0) memberDistance = 1;
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
    if ((parseFloat(totalPrice) >= orderMinPrice && flag) || getCookie("proLa_AddOrder").length > 10) {
        if ((parseFloat(totalPrice) < orderFreeRoadPrice))//满多少免配送费
        {
            $("[role='freeRoadPrice']").html("满" + orderFreeRoadPrice + "元免配送费！");
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
    //当前门店
    $("[role='storeSelect']").find("li").eq(0).before("<li>(当前门店：" +$("#hidUserStoreName").val()+")</li>");
    document.title ="Pro辣 - "+ $("#hidUserStoreName").val();
    //如果在配送距离之外,给出提示
    if (isOutOfRange) {
        $("[role='freeRoadPrice']").html("您在配送范围之外!");
    }


}

function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}


