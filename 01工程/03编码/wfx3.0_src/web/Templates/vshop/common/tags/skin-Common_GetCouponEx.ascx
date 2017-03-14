<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<a  style="cursor:pointer;" onclick="getCoupons(<%#Eval("ActivityType") %>,<%#Eval("ActivityId") %>)">
    <ul class="coupon-list1">
        <li>
            <img src="/Templates/vshop/common/images/renbg.png" alt="Alternate Text" width="100%" />
            <p>有效期：  <%#Eval("StartTime","{0:yyyy-MM-dd}") %> - <%#Eval("EndTime","{0:yyyy-MM-dd}") %></p>
            <%--<div class="coupon-left">
                <h1>
                    <b>11</b>
                    <span>满22即可使用 </span>
                    <i>元优惠券</i>

                </h1>
                <p></p>
            </div>
            <%--<div class="coupon-right">
                <h2><%#Eval("ActivityName") %></h2>
                <em>红包中奖的金额能够直接领取到个人微信钱包</em>
                <i class="red">
                    <%#((TimeSpan)(Convert.ToDateTime((Eval("EndTime")))-DateTime.Now)).Days==0?((TimeSpan)(Convert.ToDateTime((Eval("EndTime")))-DateTime.Now)).Hours+"小时":((TimeSpan)(Convert.ToDateTime((Eval("EndTime")))-DateTime.Now)).Days+"天"
                    %>后过期</i>
            </div>--%>
            <%--<div class="coupon-bottom">
                
                <img src="/Templates/vshop/common/images/gif-bottom.png" width="100%" />
            </div>--%>
        </li>
    </ul>
    </a>
    



<script>
    function getCoupons(activityType, activityid) {
        switch (activityType)
        {
            case 9:
                location.href = "/vshop/BigWheelEx.aspx?activityid=" + activityid;
                break;
        }
    }
</script>