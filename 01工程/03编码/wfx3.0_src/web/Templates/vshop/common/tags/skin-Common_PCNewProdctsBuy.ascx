<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li>
    <a class="left-imgbox" href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>">
    <img src="<%#Eval("ThumbnailUrl310").ToString().Length>5?Eval("ThumbnailUrl310").ToString():"/utility/pics/none.gif" %>" />
        <span><%# Eval("ProductName") %></span>
        <b>￥<%# Eval("SalePrice","{0:F2}") %></b>
    </a>

    
    <div class="goods-num">
        <div id="spSub_<%# Eval("ProductId") %>" class="shopcart-minus">-</div>
            <input type="tel" id="buyNum_<%# Eval("ProductId") %>" class="form-control" value="0" />
            <input type="hidden" id="skuid_<%# Eval("SkuId") %>" value="<%# Eval("SkuId") %>"/>
        <div id="spAdd_<%# Eval("ProductId") %>" class="shopcart-add">+</div>
    </div>
    

</li>
