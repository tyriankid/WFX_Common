<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<!--订单循环主体-->
        <div class="well member-orders-nav">
        <!--订单部分信息(头部)-->
            <div class="nav-title clearfix">
                <div class="nav-title-left">
                    <span><%# Eval("OrderDate","{0:d}")%></span>
                    <em class="text-right">订单编号：<%#Eval("OrderId") %></em>
                </div>
            </div>
        <!--订单部分信息(头部)end-->
<div class="member-orders-content  member-ordersli-content">
  <!-- 商品循环区-->
        <asp:Repeater ID="rporderitems" runat="server" DataSource='<%# Eval("OrderItems") %>'>
        <ItemTemplate>
             <div class="member-orders-item">
                  <Hi:ListImage style="border-width: 0px;" ID="ListImage1" runat="server" DataField="ThumbnailsUrl" />
                  <div class="info">
                      <div class="name bcolor">
                                <%# Eval("ItemDescription")%>
                      </div>
                        <div class="specification">
                              <input type="hidden" value="<%# Eval("SkuContent")%>" />
                        </div>
                      <p class="yj">佣金：<b><%# decimal.Parse(Eval("ItemsCommission", "{0:F2}"))-decimal.Parse(Eval("ItemAdjustedCommssion","{0:F2}"))%>元</b><del><%# Eval("ItemsCommission","{0:F2}")%>元</del></p>
                      <p class="num">
                          数量：<em><%# Eval("Quantity")%></em>
                      </p>
                      <div class="member-right-box">
                          <%# Eval("OrderItemsStatus").ToString().Equals("1") ? "<a class=\"gaijia\" onclick=\"UpdatePrice(" + Eval("ItemsCommission", "{0:F2}") + ",'" + Eval("OrderId") + "','"+Eval("SkuId")+"')\">改价</a>" : ""%>
                      </div>
                  </div>
              </div>
        </ItemTemplate>
        </asp:Repeater>
   <!--商品循环区end-->

    <!--礼品循环区-->
        <asp:Repeater ID="rptordergifts" runat="server" DataSource='<%# Eval("OrderGifts") %>'>
        <ItemTemplate>
             <div class="member-orders-item">
                 <div class="icon-lipin">礼</div>
                  <Hi:ListImage style="border-width: 0px;" ID="ListImage1" runat="server" DataField="ThumbnailsUrl" />
                    <div class="info">
                        <a href="<%# Globals.ApplicationPath + "/Vshop/MemberOrderDetails.aspx?OrderId=" + Eval("OrderId") %>">
                            <div class="name bcolor"><%# Eval("GiftName")%></div>
                        </a>
                        <p class="text-muted update-price"><em>数量:<%# Eval("Quantity")%></em></p>
                    </div>
              </div>
        </ItemTemplate>
        </asp:Repeater>
        
    <!--礼品循环区end-->
</div>
    <!-- 订单信息区(下方)-->
            <div class="link-box" >
                <span style="font-size: 12px; padding-right:10px">用户昵称：<%# Eval("UserName")%></span><em class="red"><Hi:OrderStatusLabel ID="OrderStatusLabel2" OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server" /></em> <asp:Literal runat="server" ID="litSendOrderGoods" Visible="false"></asp:Literal>
              
            </div>
            <div class="money-box"><span class="money-chj">成交金额：<b>￥<%#Eval("OrderTotal","{0:F2}") %></b>元</span><span class="money-shouyi"><%#Eval("RedPagerId").ToInt()>0?"<l style='color:green'>门店服务费</l>":"订单总收益" %>：<b>￥<asp:Literal ID="litCommission" runat="server"></asp:Literal></b>元</span></div>
    <!-- 订单信息区(下方)end-->
             
             <!--<p><span class="text-right">优惠：</span><span><Hi:FormatedMoneyLabel ID="lbladjustsum" runat="server"></Hi:FormatedMoneyLabel> 元</span></p>-->
</div>
<!--订单循环主体-->


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