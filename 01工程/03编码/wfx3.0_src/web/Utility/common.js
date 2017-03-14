function getParam(paramName) {
    paramValue = "";
    isFound = false;
    if (this.location.search.indexOf("?") == 0 && this.location.search.indexOf("=") > 1) {
        arrSource = unescape(this.location.search).substring(1, this.location.search.length).split("&");
        i = 0;
        while (i < arrSource.length && !isFound) {
            if (arrSource[i].indexOf("=") > 0) {
                if (arrSource[i].split("=")[0].toLowerCase() == paramName.toLowerCase()) {
                    paramValue = arrSource[i].split("=")[1];
                    isFound = true;
                }
            }
            i++;
        }
    }
    return paramValue;
}
var num = 0;
$(function () {
    var data = {};
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post',
        dataType: 'json',
        timeout: 10000,
        data: {
            action: "orderRemind"
        },
        success: function (resultData) {
            if (resultData.EnableOrderRemind == "True") {
                window.setInterval("show();",60000);
            }
            else {

            }
        }
    });
    var param = window.location.search;
    if (param.indexOf("pageindex=") >= 0) {
        $("html,body").animate({ scrollTop: $(".pagination").offset().top },1000);
    }
});

function show() {
    var data = {};
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post',
        dataType: 'json',
        timeout: 10000,
        data: {
            action: "NoYesOrderRemind"
        },
        success: function (resultData) {
            if (resultData.success == "True") {
                showDiv();
            }
            else {

            }
        }
    });
}

function showDiv() {
    var divhtml = "<a href='MemberOrders.aspx?status=1' class='showDiv' style='position:fixed;display:none;bottom:45px;z-index:999;max-width: 650px;width:100%;text-align: center;background: rgba(255,0,0,0.8);color: #fff;padding: 5px 0;'>您还有订单尚未付款，请及时付款哦</a>";
    $("footer").after(divhtml);
    $(".showDiv").slideToggle(1000);
    window.setTimeout(function () {
        $(".showDiv").slideToggle(1000);
    }, 8000);
}

function html_encode(str) {
    var s = "";
    if (str.length == 0) return "";
    s = str.replace(/</g, "&lt;");
    s = s.replace(/>/g, "&gt;");
    s = s.replace(/ /g, "&nbsp;");
    s = s.replace(/\'/g, "&#39;");
    s = s.replace(/\"/g, "&quot;");
    s = s.replace(/\n/g, "<br>");
    return s;
}
function encode_html(str) {
    var s = "";
    if (str.length == 0) return "";
    s = str.replace(/&lt;/g, "<");
    s = s.replace(/&gt;/g, ">");
    s = s.replace(/&nbsp;/g, " ");
    s = s.replace(/&#39;/g, "\'");
    s = s.replace(/&quot;/g, '\"');
    s = s.replace(/<br>/g, "\n");
    return s;
}
$(function () {
    $("input,textarea").each(function () {
        $(this).blur(function () {
            var str = $(this).val();
            $(this).val(html_encode(str));
        }).focus(function () {
            var str = $(this).val();
            $(this).val(encode_html(str));
        });
    });
})
//$(function () {
//    $("input,textarea").each(function () {
//        $(this).change(function () {
//            var str = $(this).val();
//            if (str.toLowerCase().indexOf("<script>") >= 0) {
//                alert_h("内容包含非法字符串，请重新填写");
//                $(this).val("内容包含非法字符串，请重新填写");
//            } else {
//                return true;
//            }
//        });
//    });
//});