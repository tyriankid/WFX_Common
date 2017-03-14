<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<a href="<%# Globals.ApplicationPath + "/Vshop/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>">
            <div>
                <div>
                <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl220"/>
                    <div class="info">
                        <div class="name bcolor"><%# Eval("ProductName") %></div>
                        <div class="price font-s text-danger">
                            ¥<%# Eval("SalePrice", "{0:F2}") %> <del class="text-muted font-xs">¥<%# Eval("MarketPrice", "{0:F2}") %> </del>
                        </div>
                        <div class="sales text-muted font-xs">已售<b><%# Eval("SaleCounts")%></b>件</div>
                    </div>
                </div>
            </div>
        </a>


