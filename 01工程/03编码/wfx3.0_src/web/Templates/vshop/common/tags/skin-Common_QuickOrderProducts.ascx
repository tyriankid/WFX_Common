<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%--<li >
    <a class="left-imgbox" href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>">
        <img id='img_<%# Eval("ProductId") %>' src="<%#Eval("ThumbnailUrl310").ToString().Length>5?Eval("ThumbnailUrl310").ToString():"/utility/pics/none.gif" %>" />
        <div class="info">
            <span class="title"><%# Eval("ProductName") %></span>
            <span class="selled">月销售：<%# Eval("SaleCounts") %>件</span>
            <p class="price"><b id='buyPrice_<%# Eval("ProductId") %>'><%# Eval("SalePrice","{0:F0}") %></b>元</p>
        </div>
    </a>
    <div class="goods-num" skuCounts="<%# Eval("skuCounts") %>" productId='<%# Eval("ProductId") %>'>
        <div id='spSub_<%# Eval("ProductId") %>' class="shopcart-minus">-</div>
            <input type="tel" onFocus="this.blur()" id='buyNum_<%# Eval("ProductId") %>' class="form-control" value="<%# Eval("Quantity") %>" />
            <input type="hidden" stock="<%# Eval("stock") %>" productId="<%# Eval("ProductId") %>" id='skuid_<%# Eval("SkuId") %>' value="<%# Eval("SkuId") %>"/>
        <div id='spAdd_<%# Eval("ProductId") %>' class="shopcart-add">+</div>
    </div>
</li>--%>


<div id="product_<%# Eval("ProductId") %>" ProductId='<%#Eval("ProductId") %>' ProductName='<%# Eval("ProductName") %>' BuyPrice='<%# Eval("SalePrice") %>' SkuCounts='<%# Eval("skuCounts") %>' Quantity='<%# Eval("Quantity") %>' Stock='<%# Eval("stock") %>' SkuId='<%# Eval("SkuId") %>'>
    <div id="slides">
        <input type="hidden" runat='server' id='hidProductId' value='<%#Eval("ProductId") %>' />
        <div class="swiper-container productImg">
            <div class="swiper-wrapper">
                <hi:vshoptemplatedrepeater id="rptProductImages" templatefile="/Tags/skin-Common_QuickOrderSlide.ascx" runat="server" />
            </div>
        </div>
    </div>
    <div class="info">
        <span class="title"><%# Eval("ProductName") %></span>
        <p class="price">￥<%# Eval("SalePrice","{0:F0}") %><i class="fa fa-plus-circle"></i></p>
    </div>
</div>