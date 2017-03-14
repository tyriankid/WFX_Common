<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<a href="<%# Globals.ApplicationPath + "/Vshop/CutDownDetail.aspx?cutdownId=" + Eval("cutDownId") + "&productId=" + Eval("productId") %>">
    <div class="well goods-box">
    <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl160" />
        <div class="info">
            <div class="name font-xl">
                <%# Eval("ProductName") %></div>

            <div class="price text-danger">
               ¥<%# Eval("CurrentPrice", "{0:F2}") %><del class="font-s text-muted">¥<%# Eval("FirstPrice", "{0:F2}") %></del> 
                <span class="sales font-s text-muted">已售<%# Eval("SoldCount")%>件</span></div>
        </div>
    </div>
</a>