<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


                <li class="goods-pro-box">
                    <div>
                        <Hi:ListImage ID="ListImage2" runat="server" DataField="ThumbnailUrl310" />
                        <div class="info">
                            <a href='<%#  "ProductDetails.aspx?ProductId=" + Eval("ProductId") %>'>
                            <div class="name">
                                <%# Eval("ProductName") %>
                            </div>
                            </a>
                            <div class="price">
                               <b> ¥<%# Eval("SalePrice", "{0:F2}") %></b><span>已售<%# Eval("SaleCounts")%>件</span>
                            </div>
                        </div>
                    <a class="link-select "> <input type="checkbox" name="DistributorCheckGroup" id='CheckGroup<%#Eval("ProductId") %>' value='<%# Eval("ProductId") %>' /></a>
                    </div>
                        
                </li>