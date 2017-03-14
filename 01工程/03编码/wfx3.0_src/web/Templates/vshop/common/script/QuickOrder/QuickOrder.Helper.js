window.addEventListener('load', function () {
    FastClick.attach(document.body);
}, false);
$(document).ready(function () {
    /******全局变量*********/
    var $productList = $("#mainProductList");

    /*********************************控件绑定事件start*****************************/

    //点击商品分类,异步加载该分类下的所有商品
    $("div[role=categoryBanner]").on("click", function () {
        var currentCategoryId = $(this).attr("categoryId");
        getProductsAsyc(currentCategoryId, $productList);
    });
    //点击商品,弹出规格选择器
    $("#mainProductList").on("click", "div[role=productItem]", function () {
        getSkuSelector($(this), $("#skuSelectorDiv"));
    });
    //规格选择触发事件
    $("#skuSelectorDiv").on("click", "div[role='skuBtn']", function () {
        SelectSkus($(this), $("div[role='productItem'][productId=" + $("#skuSelectorDiv").attr("productId") + "]"));
    });
    //绑定商品数量增加和减少事件
    $("#skuSelectorDiv").on("click", "span[role='add']", function () { changeOrderQuantity("add") });
    $("#skuSelectorDiv").on("click", "span[role='minus']", function () { changeOrderQuantity("minus") });
    //添加至购物车事件
    $("#skuSelectorDiv").on("click", "[role='addToCart']", function () {
        var quantity = parseInt($("[role='orderQuantity']").val());
        var skuId = $("#skuSelectorDiv").attr("skuid");
        var categoryId = $("#litCategoryId").val();
        BuyProductToCart(quantity, skuId, categoryId);
    });
    //直接购买事件
    $("#skuSelectorDiv").on("click", "[role='buy']", function () {
        var quantity = parseInt($("[role='orderQuantity']").val());
        var skuId = $("#skuSelectorDiv").attr("skuid");
        BuyProduct(quantity, skuId);
    });

    $("#main").on('scroll', function () {
        checkPosition($(window).height() - $(".banner").height() - 100);
    });

    $("#category div").on("click", function () {
        $(this).addClass("selected").siblings().removeClass("selected");
    });

    $("#btnShoppingcart").click(function () {
        location.href = "ShoppingCart.aspx?distributorId=" + GetQueryString("distributorId");
    });

    /*********************************控件绑定事件End*****************************/
    onInIt();
    /*
    *页面初始化
    */
    function onInIt() {
        //类型banner绑定滑动事件
        //var swiper = new Swiper('#category', {
        //    slidesPerView: 3,
        //    paginationClickable: true
        //});

        //$(".swiper-slide:eq(0)").addClass("selected");
        //var checkPosition = function checkPosition(pos) {
        //    if ($(window).scrollTop() > pos) {
        //        $('#category').addClass("category_fix");
        //    } else {
        //        $('#category').removeClass("category_fix");
        //    }
        //}

        //checkPosition($(window).height() - $(".banner").height() - 100);
        $("div[role=categoryBanner]").eq(0).click();
        //异步加载默认分类的商品
        //getProductsAsyc($("#litCategoryId").val(), $productList);

        $("#category div:eq(0)").addClass("selected");
        //异步加载购物车中数量
        getShoppingCartNumAsyc();
    }

    function getShoppingCartNumAsyc() { 
        $.post("/api/ProductsHandler.ashx?action=GetShoppingCartGoodNum", function (json) {
            if (json.success === true) {
                $("[role='cartQuantity']").html(json.num);
                if ($("#skuSelectorDiv").html() == "" && $("[role='cartQuantity']").html() !== "0") {
                    $(".shop_cart").animate({ bottom: "0" });
                    $("body").css("padding-bottom", "3rem");
                }
            }
            else {
                alert('出错了');
            }
        });
    }

    /*
    *加载商品列表
    *categoryId:分类ID
    *$container:存放商品的容器
    */
    function getProductsAsyc(categoryId, $container) {
        var data = {};
        data.CategoryId = categoryId;
        $("#litCategoryId").val(categoryId);

        //给当前分类的div加载商品列表html
        var $currentProductItem = $(".loadimg[categoryId='" + categoryId + "']");
        if ($currentProductItem.length > 0) {
            //隐藏其余分类下的商品
            $("[role='categoryProducts'][CategoryId='" + categoryId + "']").siblings().hide();
            $("[role='categoryProducts'][CategoryId='" + categoryId + "']").fadeIn(500);
            $("[role='categoryBannerImg']").attr("src", $(".banner").attr("bannerUrl_" + categoryId + ""));
        }
        else {
            //隐藏所有分类下的商品
            $container.children().hide();
            $container.append("<img class='loadimg' categoryId='" + categoryId + "' src='/Templates/vshop/common/images/5-121204193R5-50.gif' />")
            $.post("/api/ProductsHandler.ashx?action=getProductsAsyc", data, function (json) {
                if (json.success === true) {
                    //将商品列表放在容器内
                    $container.append(json.backHtml);
                    //将分类图展示
                    //$(".banner").append("<img role='categoryBannerImg' categoryId='" + categoryId + "' src='" + json.categoryBannerImgUrl + "' />");
                    $(".banner").attr("bannerUrl_" + categoryId + "", json.categoryBannerImgUrl);
                    $("[role='categoryBannerImg']").attr("src", $(".banner").attr("bannerUrl_" + categoryId + ""));
                    //展示loading图
                    $(".loadimg[categoryId='" + categoryId + "']").hide();
                    var swiper = new Swiper('.productImg', {
                        paginationClickable: true,
                    });
                }
                else {
                    alert('出错了');
                }
            });
        }
    }

    /*加载规格控件
    *$skuSelectorDiv : 规格选择器div
    *$productDiv : 商品div,div的属性包含了商品的大部分属性:控件id="product_[商品id]" ProductId,ProductName,BuyPrice,SkuCounts,Quantity(默认为0),Stock,SkuId
    */
    function getSkuSelector($productDiv, $skuSelectorDiv) {
        //弹出规格窗之前,现将该商品的默认规格和价格等属性复制给规格选择器 stock skuid
        $skuSelectorDiv.attr("stock",$productDiv.attr("stock"));
        $skuSelectorDiv.attr("skuid", $productDiv.attr("skuid"));

        var data = {};
        data.ProductId = $productDiv.attr("ProductId");
        $.post("/api/ProductsHandler.ashx?action=GetQuickOrderSKUSelector", data, function (json) {
            if (json.success === true) {
                //如果成功,显示选择规格的div,并给div赋值上该商品的id
                $skuSelectorDiv.attr("productId", $productDiv.attr("productId"));
                $skuSelectorDiv.html(json.selector);
                $skuSelectorDiv.css("bottom", -$skuSelectorDiv.height());
                $skuSelectorDiv.animate({ bottom: "0" },"fast");
                $("#mask_layer").show();
                if ($skuSelectorDiv.height() > $(window).height()) {
                    $skuSelectorDiv.css({ "height": $(window).height(), "overflow-y": "scroll" });
                }
                $(".spec-kind div").on("click", function () {
                    $(this).addClass("selected").siblings().removeClass("selected");
                });
                $("#mask_layer,.close").on("click", function () {
                    $skuSelectorDiv.height("auto");
                    $skuSelectorDiv.html('');
                    $("#mask_layer").hide();
                });
                //$box.prepend("<a href='#' id='closeSkus'>x</a>");
                ////显示遮罩层
                //$("#bg").css({ display: "block", height: $(document).height() });

            }
            else {
                alert("出错了!");
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
                        ResetCurrentSku(resultData.SkuId, resultData.SKU, resultData.Weight, resultData.Stock, resultData.SalePrice,$productDiv);
                    }
                    else {
                        ResetCurrentSku("", "", "", "", "0",$productDiv); //带服务端返回的结果，函数里可以根据这个结果来显示不同的信息
                    }
                },
                error: function () {
                }
            });
        }



    }

    //加减乘法
    var getMinus = function getSum(num1, num2) { return parseFloat(num1) - parseFloat(num2); }
    var getSum = function getSum(num1, num2) { return parseFloat(num1) + parseFloat(num2); }
    var getX = function getX(num1, num2) { return parseFloat(num1) * parseFloat(num2); }

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
        var oq = $orderQuantity.val();
        switch (type) {
            case "add":
                $orderQuantity.val(getSum(oq, isStockEnough(oq)));
                break;
            case "minus":
                if (parseInt($orderQuantity.val())>0)
                    $orderQuantity.val(getMinus(oq, 1));
                break;
        }

    }

    /*添加商品至购物车
    * Quantity : 商品数量 ; ProductSkuId:商品SkuId ; CategoryId:商品分类Id
    */
    function BuyProductToCart(Quantity, ProductSkuId, Categoryid) {
        // 验证数量输入
        var ValidateBuyAmount = function ValidateBuyAmount() {
            var buyNum = $("[role='orderQuantity']");
            if (parseInt(buyNum.val()) == 0) {
                alert("请先填写购买数量!");
                return false;
            }
            var amountReg = /^[1-9]d*|0$/;
            if (!amountReg.test($(buyNum).val())) {
                alert("请填写正确的购买数量!");
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
            return false;
        }
        if (!IsallSelected()) {
            location.href = "#skuArea";
            alert("请选择规格");
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
                    $("body").css("padding-bottom", "3rem");
                    $("#skuSelectorDiv").height("auto");
                    $("#skuSelectorDiv").html('');
                    $("#mask_layer").hide();
                    getShoppingCartNumAsyc();
                } else if (resultData.Status == "0") {
                    // 商品已经下架
                    alert("此商品已经不存在(可能被删除或被下架)，暂时不能购买");
                    return false;
                }
                else if (resultData.Status == "1") {
                    // 商品库存不足
                    alert("商品库存不足 " + parseInt($("#skuSelectorDiv").attr("stock")) + " 件，请修改购买数量!");
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

    /*购买按钮单击事件
    * Quantity : 商品数量 ; ProductSkuId:商品SkuId 
    */
    function BuyProduct(Quantity, ProductSkuId) {
        var url = document.location.href.toLowerCase();
        var type = "signBuy";
        var idName = "signBuyId";

        // 验证数量输入
        var ValidateBuyAmount = function ValidateBuyAmount() {
            var buyNum = $("[role='orderQuantity']");
            if (parseInt(buyNum.val()) == 0) {
                alert("请先填写购买数量!");
                return false;
            }
            var amountReg = /^[1-9]d*|0$/;
            if (!amountReg.test($(buyNum).val())) {
                alert("请填写正确的购买数量!");
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
            return false;
        }
        if (!IsallSelected()) {
            location.href = "#skuArea";
            alert("请选择规格");
            return false;
        }

        var stock = parseInt($("#skuSelectorDiv").attr("stock"));
        if (isNaN(stock) || stock == 0) {
            alert("该规格的产品没有库存，请选择其它的规格！");
            return false;
        }
        if (Quantity > stock) {
            alert("商品库存不足 " + quantity + " 件，请修改购买数量!");
            return false;
        }

        //增加门店匹配的参数:distributorId
        //location.href = "/Vshop/UserLogin.aspx?userstatus=1&buyAmount=" + $("#buyNum").val() + "&productSku=" + $("#hiddenSkuId").val() + "&from=signBuy";
        location.href = "/Vshop/SubmmitOrder.aspx?buyAmount=" + Quantity + "&productSku=" + ProductSkuId + "&from=" + type + "&" + idName + "=" + $('#litGroupbuyId').val() + "&distributorId=" + GetQueryString("distributorId");
    }

    function GetQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
});