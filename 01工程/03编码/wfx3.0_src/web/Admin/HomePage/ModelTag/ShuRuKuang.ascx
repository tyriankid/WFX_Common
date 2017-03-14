<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShuRuKuang.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.ShuRuKuang" %>

<div id='module<%=PageSN %>' i='<%=PageSN %>' PMID="<%=PMID %>" name='ShuRuKuang' bt="<%=Bt %>" class='small'>
    <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
    <div class='vote'>
        <div class='wz'>
            <h3><%=TitleZ %></h3><h4><%=TitleC %></h4>
            <%=ControlHtml %>
        </div>
    </div>
</div>