<ul class="coupon-list1">
    <li>
        <a href="<%# Hidistro.UI.SaleSystem.CodeBehind.VGetCouponsEx.GetUrl(Eval("ID"))%>">
        <%--<asp:LinkButton ID="btntz" runat="server" CommandArgument='<%#Eval("ID")%>' CommandName="tz">--%>
            <img src="/Templates/vshop/common/images/camera.png" width="100%"/>
            <p>有效期：  <%#Eval("StartTime") %>-<%#Eval("ClosingTime") %></p>
        <%--</asp:LinkButton>--%>
            </a>
<%--<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<a href="CouponsActDetail.aspx?ID=<%#Eval("ID") %>">
<img src='<%#Eval("BgImg") %>' height="80" />
<%#Eval("ColValue2") %>

优惠卷金额：<%#Eval("DiscountValue") %></a>--%>
        </li>  
</ul>