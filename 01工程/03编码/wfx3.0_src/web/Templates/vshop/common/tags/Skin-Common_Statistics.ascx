<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well member-orders-nav">
<div class="nav-title clearfix">
	<div class="nav-title-left">
	<p class="name-hr"><span class="text-right fr-left"><b><%# Eval("Name") %></b></span></p>	
    <p class="p1">店铺访问量：<span style="color:red"><%# Eval("visitCount") %></span> 
        今日访问量：<span style="color:red"><%# Eval("visitCountT") %></span>	
    会员总数：<span style="color:red"><%# Eval("memberCount")%></span></p>
        	
    <p class="p1">今日新增会员：<span style="color:red"><%# Eval("memberAddCount")%></span>	
    订单总数：<span style="color:red"><%# Eval("orderCount") %></span>
    今日订单：<span style="color:red"><%# Eval("orderToday") %></span></p>

   </div>
</div>


<hr style="margin:0 -10px 0 -10px;">




    
 
</div>
