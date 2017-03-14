<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div id="cartProducts" class="well shopcart">
<asp:Literal ID="litpromotion" runat="server"></asp:Literal> 

<asp:Repeater ID="rptCartProduct" runat="server" DataSource='<%# Eval("LineItems") %>' >
    <ItemTemplate>
    <hr style="margin:0 0px 0 0px;">
        <div class="goods-box goods-box-shopcart">
            <a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?productId=" + Eval("ProductId")%>">        
                <Hi:ListImage runat="server" DataField="ThumbnailUrl100" />
                <div class="info">
                     <div class="name bcolor">
                        <%# Eval("Name")%></div>                               
                    <div class="specification">
                        <input id="skucontent" type="hidden" value="<%# Eval("SkuContent")%>" />
                    </div>
                    <div class="price text-danger">
                        ¥<%# Eval("AdjustedPrice", "{0:F2}")%>
                    </div>
                    <div class="goods-num">
		                <div name="spSub" class="shopcart-minus">
		                    - 
		                </div>
		                <input type="tel" class="ui_textinput" name="buyNum" value='<%# Eval("Quantity")%>'
		                    skuid='<%# Eval("SkuId")%>' />
		                <div name="spAdd" class="shopcart-add">
		                    + 
		                </div>
		            </div>
                </div>
                </a>
            <a class="link-del" href="javascript:void(0)" name="iDelete" skuid='<%# Eval("SkuId")%>'><span></span></a>
            <!--<div class="goods-num">
                <div name="spSub" class="shopcart-minus">
                    - 
                </div>
                <input type="tel" class="ui_textinput" name="buyNum" value='<%# Eval("Quantity")%>'
                    skuid='<%# Eval("SkuId")%>' />
                <div name="spAdd" class="shopcart-add">
                    + 
                </div>
            </div>-->
        </div>
    </ItemTemplate>
    
</asp:Repeater>
<div><asp:Literal ID="litline" runat="server"></asp:Literal> </div>

</div>
