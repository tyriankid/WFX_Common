<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WenBen.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.WenBen" %>
<div id='module<%=PageSN %>' i='<%=PageSN %>' name='VWenBen' class='small wb' style="<%--height:<%=dt.Rows[0]["PMHeight"] %>;--%>top:<%=tl[0] %>;left:<%=tl[1] %>;">
      <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
      <div class='wb_txt ueditor_M'><%=dt.Rows[0]["PMContents"]  %></div>
</div>