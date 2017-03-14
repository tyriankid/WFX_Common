<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BaseModel.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.ShangPin" %>
<div id='module<%=PageSN %>' i='<%=PageSN %>' name='ShangPin' class='small sp'>
    <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
	<ul class='<%=StyleName %>'>
        <asp:Repeater ID="rptGoods" runat="server">
            <ItemTemplate>
                <li>
                    <a href="<%# "ProductDetails.aspx?ProductId=" + Eval("ProductId") %>">
                        <img src="<%#Eval("ImageUrl1") %>">
                        <p><%#Eval("ProductName") %></p>
                        <p><span>￥<%# Eval("SalePrice", "{0:F2}") %></span><span>原价：￥<%# Eval("MarketPrice", "{0:F2}") %></span></p>
                    </a>
                </li>
            </ItemTemplate>
        </asp:Repeater>
	</ul>				
</div>