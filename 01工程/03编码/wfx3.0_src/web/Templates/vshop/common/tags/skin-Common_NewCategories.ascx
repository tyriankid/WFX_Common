<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li id='<%#Eval("CategoryId") %>'>
    <a href='/Vshop/ProductSearch.aspx?CategoryId=<%#Eval("CategoryId") %>'><%# Eval("Name") %></a>
</li>
