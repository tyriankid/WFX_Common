<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<a  style="cursor:pointer;" onclick="getCoupons(<%#Eval("CouponId") %>)">
    <ul class="coupon-list">
        <li>
            <div class="coupon-left">
                <h1>
                    <b><%# Eval("DiscountValue", "{0:F0}")%></b>
                    <span>满<%# Eval("Amount", "{0:F2}")%>即可使用 </span>
                    <i>元优惠券</i>

                </h1>
                <p><%#Eval("StartTime","{0:yyyy-MM-dd}") %> - <%#Eval("ClosingTime","{0:yyyy-MM-dd}") %></p>
            </div>
            <div class="coupon-right">
                <h2><%#Eval("Name") %></h2>
                <em><%# Eval("txtUseRange")%></em>
                <i class="red">
                    <%#((TimeSpan)(Convert.ToDateTime((Eval("ClosingTime")))-DateTime.Now)).Days==0?((TimeSpan)(Convert.ToDateTime((Eval("ClosingTime")))-DateTime.Now)).Hours+"小时":((TimeSpan)(Convert.ToDateTime((Eval("ClosingTime")))-DateTime.Now)).Days+"天"
                    %>后过期</i>
            </div>
        </li>
    </ul>
    </a>
    



<script>
    function getCoupons(couponid) {
        var data = {};
        data.couponid = couponid;
        $.post("/api/VshopProcess.ashx?action=getCoupon", data, function (json) {
            if (json.success === true) {
                alert_h("领取成功!");
            }
            else {
                alert_h("请勿重复领取!");
            }
        });



    }
</script>