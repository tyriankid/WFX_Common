<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li id='<%#Eval("TypeId") %>'>
    <a href='/Vshop/ProductSearch.aspx?TypeId=<%#Eval("TypeId") %>'><%# Eval("TypeName") %></a>
</li>
