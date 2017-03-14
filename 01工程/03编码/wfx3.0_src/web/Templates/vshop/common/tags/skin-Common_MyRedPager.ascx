<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<!--
<div class="well red-bg">
	<ul class="red-content">
		<li style="width: 25%; border-right: 1px dashed #d7d7d7;">
			<div class="money">￥<span><%# Eval("DiscountValue", "{0:F0}")%></span></div>
			<div class="money-info">满 <%# Eval("Amount", "{0:F2}")%>可用</div>
		</li>
		<li style="width: 75%">
			<div class="shop-info">
				<h1><%#Eval("Name") %></h1>
				<span>有效期至 <%#Eval("ClosingTime","{0:yyyy-MM-dd HH:mm:ss}") %></span>
				<em>该券可用于任意商品的抵扣</em>
			</div>
		</li>
	</ul>
</div>
-->
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
                <em><%#Eval("ShowCode").ToString() %></em>
                <i class="red">
                    <%#((TimeSpan)(Convert.ToDateTime((Eval("ClosingTime")))-DateTime.Now)).Days==0?((TimeSpan)(Convert.ToDateTime((Eval("ClosingTime")))-DateTime.Now)).Hours+"小时":((TimeSpan)(Convert.ToDateTime((Eval("ClosingTime")))-DateTime.Now)).Days+"天"
                    %>后过期</i>
            </div>
        </li>
    </ul>

