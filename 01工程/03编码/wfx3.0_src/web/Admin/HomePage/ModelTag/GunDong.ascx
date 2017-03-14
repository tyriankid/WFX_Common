<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WenBen.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.GunDong" %>
<div id="module<%=PageSN %>" i="<%=PageSN %>" name="GunDong" class="small gd">
    <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
    <div id="gongao">
        <div id="scroll_div" class="scroll_div wb_txt"<%-- style="height: <%=height%>; line-height:<%=height%>;"--%>>
            <div id="scroll_begin">
                <a href='<%=cons[2] %>' style="color: <%=cons[1] %>;"><%=cons[0] %></a></div>
            <div id="scroll_end">
                 <a href='<%=cons[2] %>' style="color: <%=cons[1] %>;"><%=cons[0] %></a></div>
        </div>
    </div>
</div>