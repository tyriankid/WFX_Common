<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShiJian.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.ShiJian" %>
<div id='module<%=PageSN %>' i='<%=PageSN %>' PMID="<%=PMID %>" name='ShiJian' bt="<%=Bt %>" class='small'>
    <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
    <div class='vote'>
        <div class='wz'>
            <h3><%=TitleZ %></h3><h4><%=TitleC %></h4>
            <input value='' readonly='readonly' name='appDate' id='appDate<%=PageSN %>' type='text'>
        </div>
    </div>
</div>