<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>



<!--订单循环主体-->
        <div class="well member-orders-nav">
        <!--订单部分信息-->
            <div class="nav-title clearfix">
                <div class="nav-title-left">
                    <span><%# Eval("OrderDate","{0:d}")%></span>
                    <em class="text-right">订单编号：<%#Eval("OrderId") %></em>
                </div>
            </div>
        <!--订单部分信息end-->
<div class="member-orders-content">
  <!-- 商品循环区-->
        <asp:Repeater ID="rporderitems" runat="server" DataSource='<%# Eval("OrderItems") %>'>
        <ItemTemplate> 
            <a href="<%# Globals.ApplicationPath + "/Vshop/MemberOrderDetails.aspx?OrderId=" + Eval("OrderId") %>">            
             <div class="member-orders-item">
                  <Hi:ListImage style="border-width: 0px;" ID="ListImage1" runat="server" DataField="ThumbnailsUrl" />
                  <div class="info">
                      <div class="name bcolor">                           
                                <%# Eval("ItemDescription")%>
                      </div>
                        <div class="specification">
                              <input type="hidden" value="<%# Eval("SkuContent")%>" />
                        </div>
                      <p class="yj"><b>￥<%# Eval("ItemAdjustedPrice","{0:F2}") %></b><del></del></p>
                      <p class="num">
                          数量：<em><%# Eval("Quantity") %></em>
                      </p>  
                
                  </div>
              </div>
                </a>                                             
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
                    </div>
              </div>                                
        </ItemTemplate>
        </asp:Repeater>                   
    <!--礼品循环区end-->
</div>
    <!-- 按钮动态显示区-->
            <div class="zongjia-box">
                <a href="/Vshop/saleservice.aspx" style="color:blue; float:left; padding-left:20px">售后服务</a>                              
                <span>共<%# string.IsNullOrEmpty(Eval("ProductSum").ToString())?0:Eval("ProductSum") %>件商品                
                <%# string.IsNullOrEmpty(Eval("GiftSum").ToString())?"":","+Eval("GiftSum")+"件礼物" %></span>
                <span>实付：<i class="red">￥<%# Eval("OrderTotal","{0:F2}")%>元
                <%# string.IsNullOrEmpty(Eval("PointSum").ToString())?"</i>":","+Eval("PointSum")+"点积分</i>" %></span>
            </div>
            <div class="link-box">
                <asp:Repeater ID="Repeater1" runat="server" DataSource='<%# Eval("OrderItems") %>'>
                    <ItemTemplate>
                    <!--<a  href="<%# Globals.ApplicationPath + "ProductDetails.aspx?ProductId=" + Eval("ProductId") %>"  style="color:white; float:left; padding-left:11px" class="link link-color">再次购买</a>-->
                    </ItemTemplate>
                </asp:Repeater>
                <em class="red"><Hi:OrderStatusLabel ID="OrderStatusLabel2" OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server" /></em> 
                <a href='javascript:void(0)'onclick="CloseOrder('<%#Eval("OrderId") %>')" class='link <%# ((int)Eval("OrderStatus") == 1) ? "link" : "hide"%>'>关闭订单</a>
                <a href='javascript:void(0)'onclick="AddOrder('<%#Eval("OrderId") %>')" payTime="<%#Eval("payDate") %>" role="btnAddOrder"  class='link <%# ((int)Eval("OrderStatus") == 2) ? "link" : "hide"%>'>追加订单</a>
                <a href='<%# Globals.ApplicationPath + "/Vshop/MyLogistics.aspx?OrderId=" + Eval("OrderId") %> '
                                        class='link <%# ((int)Eval("OrderStatus") == 3 || (int)Eval("OrderStatus") == 5) ? "link" : "hide"%>' style='display:<%=Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.IsSanzuo?"none":""%>'>查看物流</a>
                <a href='javascript:void(0)' onclick="FinishOrder('<%#Eval("OrderId") %>')" class='link <%# (int)Eval("OrderStatus") == 3 ? "link link-color" : "hide"%>'>
                                    确认收货</a>
                <a href='<%# Globals.ApplicationPath + "/Vshop/FinishOrder.aspx?OrderId=" + Eval("OrderId") %> '
                                         class='link <%# (int)Eval("OrderStatus") == 1&&(int)Eval("PaymentTypeId")!=0&&(string)Eval("GateWay")!="hishop.plugins.payment.bankrequest"&&(string)Eval("GateWay")!="hishop.plugins.payment.podrequest"? "link link-color" : "hide"%>'
                                        >去付款</a>
                <a href='<%# Globals.ApplicationPath + "/Vshop/FinishOrder.aspx?OrderId=" + Eval("OrderId")+"&onlyHelp=true" %> '
                 class='link <%# (int)Eval("PaymentTypeId")==99&&(int)Eval("OrderStatus")==1 ? "link link-color" : "hide"%>'
                >线下支付帮助</a>
                <%#(Eval("HasRedPage")).ToString()=="1"?"<a href='/Vshop/GetRedShare.aspx?orderid="+Eval("OrderId")+"' class='link link-color'>发钱咯</a>":"" %>
            </div>

    <!-- 按钮动态显示区end-->
             <!-- <div class="money-box"><span class="money-chj">成交金额：<b>￥30.00</b>元</span><span class="money-shouyi">订单总收益：<b>￥9.00</b>元</span></div>-->
  </div>
<!--订单循环主体-->
<script type="text/javascript">
    $(function () {
        var skuInputs = $('.specification input');
        $.each(skuInputs, function (j, input) {
            var text = '';
            var sku = $(input).val().split(';');
            var changsku = '';
            for (var i = sku.length - 2; i >= 0; i--) {
                changsku += sku[i] + ';';
            }
            $.each(changsku.split(';'), function (i, sku) {
                if ($.trim(sku))
                    text += '<span class="property">' + sku.split('：')[1] + '</span>';
            });
            $(input).parent().html(text);
        });
    });
</script>
