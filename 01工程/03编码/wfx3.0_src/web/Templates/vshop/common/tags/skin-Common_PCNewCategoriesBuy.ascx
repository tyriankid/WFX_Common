<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<li id='<%#Eval("CategoryId") %>' onclick="location.href='/Vshop/PCProductSearchBuy.aspx?CategoryId=<%#Eval("CategoryId") %>&type=<%#Eval("PType") %>'">
    <%# Eval("Name") %>
</li>
