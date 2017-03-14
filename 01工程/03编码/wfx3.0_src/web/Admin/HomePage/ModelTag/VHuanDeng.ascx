<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BaseModel.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.DaoHang" %>
<div id='module<%=PageSN %>' i='<%=PageSN %>' name='VHuanDeng' class='small hdp' style="top:<%=tl[0] %>;left:<%=tl[1] %>;">
      <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
      <ul class='slides'>
          <%for(int i=0;i<licons.Count();i++){ %>
          <%string[] liPros = licons[i].Split('♦');%>
          <li id="nav_li<%=i+1 %>" n="<%=i+1 %>">
            <a href='<%=liPros[1] %>'>
                <img src='<%=liPros[0] %>'>
            </a>
        </li>
          <%} %>
      </ul>
</div>