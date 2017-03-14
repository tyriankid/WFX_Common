<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well member-orders-nav">
<div class="nav-title clearfix">
	<div class="nav-title-left">
	<p class="name-hr"><span class="text-right fr-left">收货人：<b><%# Eval("Shipto") %></b></span><span class="text-right fr-right">微信昵称：<b><%# Eval("UserName") %></b></span></p>	
    <p><span class="text-right">收货电话：</span><span><%# Eval("CellPhone") %></span></p>
    <p><span class="text-right">收货地址：</span><span><%# Eval("Address") %></span></p>	
    <p><span class="text-right">订单价格：</span><span><%# Eval("OrderTotal","{0:F2}")%></span>元</p>	
    <p><span class="text-right">订单状态：</span><span class="text-danger" style="padding-right:100px;"><Hi:OrderStatusLabel ID="OrderStatusLabel1" OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server" /></span></p>	
    <p><span class="text-right">上级负责人：</span><span><%# Eval("ParentName") %></span></p>

   </div>
	<div class="nav-title-middle">
		<p class="text-right"><span><asp:Literal runat="server" ID="litSendOrderGoods" Visible="false"></asp:Literal></span> </p>

		<!--<p><span class="text-right">优惠：</span><span><Hi:FormatedMoneyLabel ID="lbladjustsum" runat="server"></Hi:FormatedMoneyLabel> 元</span></p>-->
	</div>
</div>


<hr style="margin:0 -10px 0 -10px;">




    
 
</div>
<script type="text/javascript">
    function SendOrderGoods(OrderId, ShippingModeId)
    {
        myConfirm('询问', '确认发货吗？', '确认发货', function () {
            $.ajax({
                url: "/API/VshopProcess.ashx",
                type: 'post',
                dataType: 'json',
                timeout: 10000,
                data: {
                    action: "SendOrderGoods",
                    orderId: OrderId,
                    shippingModeId: ShippingModeId
                },
                success: function (resultData) {
                    if (resultData.success) {
                        alert('发货成功！');
                        location.href = location.href;
                    }
                    else {
                        alert(resultData.msg);
                    }
                }
            });
        });
    }
</script>