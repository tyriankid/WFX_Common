<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="XuanXiang.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.XuanXiang" %>
<div id='module<%=PageSN %>' i='<%=PageSN %>' PMID="<%=PMID %>" name='XuanXiang' bt="<%=Bt %>" class='small'>
    <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
    <div class='vote'>
        <div class='wz'><h3><%=TitleZ %></h3><h4><%=TitleC %></h4></div>
        <ul>
            <%=ControlHtml %>
        </ul>
    </div>
</div>