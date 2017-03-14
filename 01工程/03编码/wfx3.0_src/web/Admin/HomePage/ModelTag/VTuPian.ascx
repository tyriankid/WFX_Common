<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WenBen.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.TuPian" %>
<div id='module<%=PageSN %>' i='<%=PageSN %>' name='VTuPian' class='small tp' style="top:<%=tl[0] %>;left:<%=tl[1] %>;">
    <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
    <ul class='<%=dt.Rows[0]["PMStyle"] %>'>
        <%for(int i=0;i<licons.Count();i++){ %>
          <%string[] liPros = licons[i].Split('♦');%>
        <li id="nav_li<%=i+1 %>" n="<%=i+1 %>">
            <a href='<%=liPros[2] %>'>
                <img src='<%=liPros[0] %>'>
                <span style='display: <%=dis%>;'><%=liPros[1] %></span>
            </a>
        </li>
        <%} %>
    </ul>
</div>