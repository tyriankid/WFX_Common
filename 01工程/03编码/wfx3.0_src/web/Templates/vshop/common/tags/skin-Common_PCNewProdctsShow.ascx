<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li role="productBox">
    <a class="left-imgbox" href="javascript:void(0)" onclick="showLeftOrderList(this) ">
    <img src="<%#Eval("ThumbnailUrl310").ToString().Length>5?Eval("ThumbnailUrl310").ToString():"/utility/pics/none.gif" %>" />
        <span><%# Eval("ProductName") %></span>
        <b>￥<%# Eval("SalePrice","{0:F2}") %></b>
    </a>

    <ul class="goods-num" productId='<%# Eval("ProductId") %>' skuCounts="<%# Eval("skuCounts") %>" style="display:none">
        <li>
            <span><%# Eval("ProductName") %></span>
            <div class="guige"><d type="skuName"></d><b style="display:none"><%# Eval("SalePrice","{0:F2}") %></b></div>
            <span>
                <a id='spSub_<%# Eval("ProductId") %>' class="shopcart-minus">-</a>
                <input type="tel" id='buyNum_<%# Eval("ProductId") %>' class="form-control" value="0" disabled="disabled" />
                <input type="hidden" id='skuid_<%# Eval("SkuId") %>' value="<%# Eval("SkuId") %>"/>
                <a id='spAdd_<%# Eval("ProductId") %>' class="shopcart-add">+</a>
            </span>
        </li>
    </ul>
</li>
