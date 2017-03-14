




function GetActivityid() {
    var activityid = window.location.search.substr(window.location.search.indexOf("=") + 1);
    if (activityid.indexOf("&") > 0)
        activityid = activityid.substr(0, activityid.indexOf("&"));
    return activityid;
}

var errorMsg = "";
var je = -1;
function getPrize() {
    var no = 0;
    var activityid = GetActivityid();
    $.ajax({
        url: "/API/VshopProcess.ashx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "GetPrize", "activityid": activityid },
        async: true,
        success: function (resultData) {
            no = resultData.No;
            errorMsg = resultData.Msg;
            //alert(errorMsg);
            if (no>-1)
                je = resultData.JE;

            if (no == -1) {
                //alert_h("您已经达到抽奖次数上限");
                alert("您已经达到抽奖次数上限");
                isClickRed = false;
                $('.red').removeClass('tongRotate');
            }
            else if (no == -2) {
                alert("您未登录或者，登录超时，请重新从微信进入！");
                isClickRed = false;
                $('.red').removeClass('tongRotate');
            }
            else if (no == -3) {
                alert("对不起，活动还未开始，或者已经结束！", function () {
                    location.href = "/vshop/default.aspx";
                });
            }
            else if (no == -9) {
                alert("抽取微信红包失败,请稍后再试！");
                //alert(errorMsg);
                isClickRed = false;
                $('.red').removeClass('tongRotate');
            }
            else {
                setResult(je);
            }

        }
    });
    return no;
}
