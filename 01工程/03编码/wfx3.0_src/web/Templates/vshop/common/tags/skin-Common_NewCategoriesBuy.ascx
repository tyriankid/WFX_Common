<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<li id='<%#Eval("CategoryId") %>' onclick="location.href='/Vshop/ProductSearchBuy.aspx?CategoryId=<%#Eval("CategoryId") %>'">
    <%# Eval("Name") %>
</li>
