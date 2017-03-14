<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WenBen.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.WenBen" %>
<div id='module<%=PageSN %>' i='<%=PageSN %>' name='WenBen' class='small wb' <%-- style='height:<%=dt.Rows[0]["PMHeight"] %>;'--%>>
      <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
      <div class='wb_txt' <%--style='height:<%=dt.Rows[0]["PMHeight"] %>;'--%>><%=dt.Rows[0]["PMContents"]  %></div>
</div>