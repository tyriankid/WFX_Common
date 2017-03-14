<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WenBen.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.KongBai" %>
<div id='module<%=PageSN %>' i='<%=PageSN %>' name='KongBai' class='kb'>
    <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
    <div class="wb_txt" style="height:<%=height%>;"></div>
</div>