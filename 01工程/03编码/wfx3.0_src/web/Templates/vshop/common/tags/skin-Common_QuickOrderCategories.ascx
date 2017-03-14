<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<div role="categoryBanner" categoryId='<%#Eval("CategoryId") %>'><%# Eval("Name") %></div>
