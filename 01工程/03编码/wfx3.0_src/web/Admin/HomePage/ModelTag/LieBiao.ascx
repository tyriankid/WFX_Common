<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LieBiao.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.LieBiao" %>
<div id='module<%=PageSN %>' i='<%=PageSN %>' name='LieBiao' sort="<%=goodsort %>" class='small splb'>
    <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
	<ul class='<%=StyleName %>' style='background:<%=colors[0]%>;'>
        <asp:Repeater ID="rptGoods" runat="server">
            <ItemTemplate>
                <li>
                    <a href="<%# "ProductDetails.aspx?ProductId=" + Eval("ProductId") %>">
                        <img src="<%#Eval("ImageUrl1") %>">
                        <p style="color:<%=colors[1]%>;"><%#Eval("ProductName") %></p>
                        <p style="color:<%=colors[1]%>;"><span>￥<%# Eval("SalePrice", "{0:F2}") %></span><%--<span>原价：￥<%# Eval("MarketPrice", "{0:F2}") %></span>--%></p>
                    </a>
                </li>
            </ItemTemplate>
        </asp:Repeater>
	</ul>				
</div>