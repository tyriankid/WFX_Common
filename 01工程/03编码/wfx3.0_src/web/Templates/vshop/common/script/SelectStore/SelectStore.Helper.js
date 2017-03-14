var lat, lont;
$(function () {
    eventBind();
    /*
    var data = {};
    data.userLatitude = "29.581231"; // 纬度，浮点数，范围为90 ~ -90
    data.userLontitude = "113.215465"; // 经度，浮点数，范围为180 ~ -180。
    lat = "29.581231";
    lont = "113.215465";
    data.range =1;//配送范围
    $.post("/api/StoreHandler.ashx?action=getPoiList", data, function (json) {
        if (json.success === true) {
            if (json.isUserInRange === "True") {//匹配到了门店就跳转到点餐页面
                                var ids = json.distributorId.split(',');
                                var li = $("#streetsDiv ul li");
                                li.html(json.distributorId);

                                $(".location").find("img").attr("src", "/Templates/vshop/common/images/jdfw1.jpg").end().find("span").html("定位到当前位置");
                $("#locationDiv").hide();
                $("#streetsDiv").show(); 
            }
            else if (json.isUserInRange === "False") {//配送范围外
                
                //停止转动,显示强烈要求开店div
                $(".location").find("img").attr("src", "/Templates/vshop/common/images/jdfw1.jpg").end().find("span").html("定位到当前位置");
                $("#notInRangeDiv").show();
            }
        }
        else {
            alert("出错了:" + json.errMsg)
        }
    });
    return false;
    */
    wx.ready(function () {
        //获取用户坐标信息
        wx.getLocation({
            type: 'wgs84', // 默认为wgs84的gps坐标，如果要返回直接给openLocation用的火星坐标，可传入'gcj02'
            success: function (res) {
                var speed = res.speed; // 速度，以米/每秒计
                var accuracy = res.accuracy; // 位置精度
                //获取所有门店的信息
                var data = {};
                data.userLatitude = res.latitude; // 纬度，浮点数，范围为90 ~ -90
                data.userLontitude = res.longitude; // 经度，浮点数，范围为180 ~ -180。
                lat = res.latitude;
                lont = res.longitude;
                data.range = 1.00;//配送范围    

                $.post("/api/StoreHandler.ashx?action=getPoiList", data, function (json) {
                    
                    if (json.success === true) {
                        if (json.isUserInRange === "True") {//将匹配门店展示出来
                                var ids = json.distributorId.split(',');
                                var li = $("#streetsDiv ul li");
                                li.html(json.distributorId);

                                $(".location").find("img").attr("src", "/Templates/vshop/common/images/images/jdfw1.gif").end().find("span").html("定位到当前位置");
                                $("#locationDiv").hide();
                                $("#streetsDiv").show();
                        }
                        else if (json.isUserInRange === "False") {//配送范围外
                            //停止转动,显示强烈要求开店div
                            $(".location").find("img").attr("src", "/Templates/vshop/common/images/jdfw1.gif").end().find("span").html("定位到当前位置");
                            $("#notInRangeDiv").show();
                        }
                    }
                    else {
                        alert("出错了:" + json.errMsg)
                    }
                });
            },
            cancel: function (res) {
                //停止转动,显示强烈要求开店div
                $(".location").find("img").attr("src", "/Templates/vshop/common/images/jdfw1.gif").end().find("span").html("定位到当前位置");
                $("#notInRangeDiv").show();
            }
        });
    });



    /*
    var data = {};
    data.access_token = $("#hidAccess_token").val();
    var buffer = {};
    buffer.begin = 0;
    buffer.limit = 10;
    data.buffer = buffer;
    alert(data.access_token);
    alert(data.buffer.limit);
    $.post("https://api.weixin.qq.com/cgi-bin/poi/getpoilist?access_token=" + data, function (json) {
        alert(json.errmsg);
        if (json.errmsg === "ok") {
            alert(json.errmsg);
        }
        else {
        }
    });
    */

    //var Data = {};
    //Data.access_token = $("#hidAccess_token").val();
    //alert(Data.access_token);
    //var buffer = {};
    //buffer.begin = 0;
    //buffer.limit = 10;
    //Data.buffer = buffer;
    //$.ajax({
    //    type: "POST",   //访问WebService使用Post方式请求
    //    contentType: "application/x-www-form-urlencoded;charset=utf-8", //WebService 会返回Json类型
    //    url: "https://api.weixin.qq.com/cgi-bin/poi/getpoilist?access_token=" + $("#hidAccess_token").val(), //调用WebService的地址和方法名称组合 ---- WsURL/方法名
    //    data: "{\"begin\":0,\"limit\":10}",         //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到
    //    dataType: 'json',
    //    success: function (result) {     //回调函数，result，返回值
    //        alert(result.errmsg);
    //    },
    //    error: function (result) {
    //        alert(result);
    //    }

    //});


});

function eventBind() {


        //打烊
        var a = new Date();
        var b = new Date();
        var c = new Date();
        a.setHours(8, 30, 00);
        b.setHours(18, 00, 00);
        if (!(c > a && b > c)) {
            $("body").css("background-color", "#000")
            $("body").html('<img src="/Templates/vshop/common/images/closed.jpg" width="100%">');
        }

    //三作咖啡打烊设置 

        if ($("#isClose").val() == "True") {
            $("body").css("background-color", "#000")
            $("body").html('<img src="/Templates/vshop/common/images/closed.jpg" width="100%">');
        }

    $("#txtSearch").focus(function () {
        $("#locationDiv").fadeOut(500);
        $("#streetsDiv").fadeIn(500);
    }).blur(function () {
        $("#locationDiv").fadeIn(500);
        $("#streetsDiv").fadeOut(500);
    });

    $("#main").on("click","[role='btnStreet']",function () {
        if ($(this).attr("distributorId") == "0") {
            alert("该写字楼正在紧急装修中!尽请期待!");
            return;
        }
        if ($("#isSanZuo").val() == "1") {
            //将选中的街道对应的门店分销商id存入cookie中.
            localStorage.setItem("selectStoreId", $(this).attr("distributorId"));
            //将选中的街道名存入cookie中
            localStorage.setItem("selectStoreName", $(this).html());
            location.href = "index.aspx?selectStoreId=" + $(this).attr("distributorId");
        }
            //pro辣特殊跳转,值为2
        else if ($("#isSanZuo").val() == "2") {
            var map = new BMap.Map("allmap");
            var pointA = new BMap.Point($(this).attr("latitude"), $(this).attr("longitude"));  // 创建点坐标A--大渡口区
            var pointB = new BMap.Point(lat, lont);  // 创建点坐标B--江北区
            var distance = (map.getDistance(pointA, pointB)).toFixed(2);
            alert('距离是：' +distance + ' 米。');  //获取两点距离,保留小数点后两位
            //pro辣选中的街道对应的门店分销商id存入cookie中.
            localStorage.setItem("proSelectStoreId", $(this).attr("distributorId"));
            //将用户和门店的距离存入cookid中,以便于点餐页面计算配送费等信息
            localStorage.setItem("memberDistance", distance);
        }
        else {
            location.href = "QuickOrder.aspx?distributorId=" + $(this).attr("distributorId");
        }
    });


    $("#jumpToRequireStore").click(function () {
        location.href = "RequireStore.aspx?lat=" + lat + "&lont=" + lont;
    });

    $(".fa-crosshairs,[role='lableReload']").click(function () {
        location.reload();
    });    
}