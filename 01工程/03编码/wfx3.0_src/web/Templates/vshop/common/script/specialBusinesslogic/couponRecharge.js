

function loadCouponRecharge() {
    if ($("#couponRecharge").val() == "0") return;
    //默认选中并隐藏
    $(".selectShipToDate").hide();//配送时间
    $(".selectShippingType").hide();//配送方式
    $("#selectShippingType").val($(".selectShippingType").children("ul").find("li").eq(0).find("a").attr("name"));//选择第一项配送方式
    $(".coupon").hide();//充值时不允许使用优惠券
    var objPrice = $(".goods-list-p").children().find(".price");
    objPrice.html(objPrice.html().replace(/¥[\w\.]+</, "¥" + $(".specification").find(".badge-h").html() + "<"));
    $(".last").hide();
}