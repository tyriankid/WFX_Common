<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_Submit.aspx.cs" Inherits="Hidistro.UI.Web.Pay.wx_Submit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
<!--<script type="text/javascript">
    document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
        WeixinJSBridge.invoke('getBrandWCPayRequest', <%= pay_json %>, function(res){
            if(res.err_msg == "get_brand_wcpay_request:ok" ) {
                alert("订单支付成功!点击确认进入我的订单中心");
                location.href = "/vshop/MemberCenter.aspx?status=3";
            }
            else
            {

                alert("支付取消或者失败");
                location.href = "/vshop/MemberCenter.aspx?status=1";
            }
        });
    });
</script>-->
<script type="text/javascript">

    //调用微信JS api 支付
    function jsApiCall()
    {
        WeixinJSBridge.invoke(
        'getBrandWCPayRequest',
        <%=pay_json%>,//josn串
                    function (res)
                    {
                        WeixinJSBridge.log(res.err_msg);
                        //alert(res.err_code + res.err_desc + res.err_msg);
                        if(res.err_msg == "get_brand_wcpay_request:ok" ) {

                            alert("订单支付成功!点击确认进入我的订单中心");
                            location.href = "/vshop/MemberCenter.aspx?status=3&orderid=<%=orderId%>&uid=<%=uid%>";
                        }
                        else
                        {
                            alert("支付取消或者失败");
                            location.href = "/vshop/MemberCenter.aspx?status=1";
                        }
                    }
              );
    }

    function sendPaySuccessMsg(){

        var ajaxUrl = "http://" + window.location.host + "/api/ProductsHandler.ashx?action=sendPaySuccessMsg&orderid=<%=orderId%>&uid=<%=uid%>";
        $.ajax({
            type: 'get', dataType: 'json', timeout: 10000,
            url: ajaxUrl,
            success: function (e) {
                alert(e.result);
            }
        });

        $.ajax({
            url: "/API/ProductsHandler.ashx?action=sendPaySuccessMsg&orderid=<%=orderId%>&uid=<%=uid%>",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "sendPaySuccessMsg", orderid: "<%=orderId%>", uid: "<%=uid%>" },
            success: function (resultData) {
                if (resultData.Status == "OK") {
                    alert(1);
                }
                else {
                    alert(2);
                }
            },
            error: function () {
            }
        });
    }

    function callpay()
    {
        if (typeof WeixinJSBridge == "undefined")
        {
            if (document.addEventListener)
            {
                document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
            }
            else if (document.attachEvent)
            {
                document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
            }
        }
        else
        {
            jsApiCall();
        }
    }
    callpay();
     </script>
</body>
</html>
