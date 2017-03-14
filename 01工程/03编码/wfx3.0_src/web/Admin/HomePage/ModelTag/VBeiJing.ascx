<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BaseModel.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.WenBen" %>
<div id='module<%=PageSN %>' i='<%=PageSN %>' name='VBeiJing' class='small bg'>
      <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
      <img src="<%=dt.Rows[0]["PMContents"] %>"/>
</div>