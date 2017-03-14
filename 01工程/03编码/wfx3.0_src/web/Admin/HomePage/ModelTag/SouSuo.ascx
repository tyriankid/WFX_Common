<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WenBen.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.SouSuo" %>
<div id='module<%=PageSN %>' i='<%=PageSN %>' name='SouSuo' class='small ss'>
    <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
					<div class="search_div"><input class='search' id="txtKeywords" type='text' placeholder='商品搜索：请输入商品关键字' />
					<button type='submit' class='searchBtn' onclick="search(this)"></button></div>
</div>