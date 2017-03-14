
    var isPrize = false;

    //验证是否可以抽奖，信息是否全
    function validateSpin() {
        var iexist = parseInt(getMemberInfo());
        if (iexist == 0) {
            ShowDiv('popup2');//显示信息输入弹出层
        }
        if (iexist == 1) {
            spin(realname, cellPhone);//直接抽奖
        }
    }

    //输入层提交按钮
    function Save() {
        var name = $("#name").val().trim();
        var phone = $("#phone").val().trim();
        if (name != null && name != "" && phone != null && phone != "") {
            CloseDiv('popup2');
            spin(name, phone);//开始抽奖
        }else{
            alert_h("请输入姓名及电话。");
        }
    }

    var realname = "";
    var cellPhone = "";
    //验证是否存在昵称，及电话
    function getMemberInfo() {
        var iexist = 0;
        $.ajax({
            url: "/API/VshopProcess.ashx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "GetMemberInfo" },
            async: false,
            success: function (resultData) {
                iexist = resultData.IExist;
                realname = resultData.RealName;
                cellPhone = resultData.CellPhone;
            }
        });
        return iexist;
    }
    
    //抽奖方法
    function spin(name,phone) {
        if (isPrize) {
            alert_h("正在抽奖请等待...");
            return;
        } 
        isPrize = true;
        var no = parseInt(getPrize(name.trim(), phone.trim()));
        if (no == -1) {
            alert_h("您已经达到抽奖次数上限");
        }
        else if (no == -2) {
            alert_h("您未登录或者，登录超时，请重新从微信进入！");
        }
        else if (no == -3) {
            alert_h("对不起，活动还未开始，或者已经结束！");
        }
        else if (no == -9) {
            alert_h(errorMsg);//抽奖出错
        }
        else if (no == 0) {
            updateNum();
            alert_h("很遗憾,您未中奖,谢谢参与.", function () { window.location.reload() });
        }
        else {
            updateNum();
            alert_h("恭喜您获得了 " + jeMsg, function () { window.location.reload() });
        }
    }


    function GetActivityid() {
        var activityid = window.location.search.substr(window.location.search.indexOf("=") + 1);
        if (activityid.indexOf("&") > 0)
            activityid = activityid.substr(0, activityid.indexOf("&"));
        return activityid;
    }


    var errorMsg = "";
    var jeMsg = "";
    function getPrize(name, phone) {
        var no = 0;
        jeMsg = "";
        var activityid = GetActivityid();
        $.ajax({
            url: "/API/VshopProcess.ashx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "GetPrize", "activityid": activityid, "RealName": name, "CellPhone": phone },
            async: false,
            success: function (resultData) {
                isPrize = false;
                no = resultData.No;
                jeMsg = resultData.JE;
                errorMsg = resultData.Msg;
            }
        });
        return no;
    }

    String.prototype.trim = function () {
        return this.replace(/(^\s*)|(\s*$)/g, '');
    }