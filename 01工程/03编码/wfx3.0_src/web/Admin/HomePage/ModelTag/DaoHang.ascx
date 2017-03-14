<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BaseModel.ascx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.DaoHang" %>
<div id='module<%=PageSN %>' i='<%=PageSN %>' name='DaoHang' class='small dh'>
      <div class='pitch'><p><a onclick="integrate()">编辑</a><a onclick="delcon(this)">删除</a></p></div>
      <ul class='<%=dt.Rows[0]["PMStyle"] %>'>
          <%for(int i=0;i<licons.Count();i++){ %>
          <%string[] liPros = licons[i].Split('♦');%>
          <li id='nav_li<%=i+1 %>' n='<%=i+1 %>'>
              <a href='<%=liPros[4] %>'>
                  <img src='<%=liPros[0] %>'' style='display:<%=dis%>' />
                  <span class='wb_txt' style='height: <%=dt.Rows[0]["PMHeight"]%>; line-height: <%=dt.Rows[0]["PMHeight"]%>; background-color: <%=liPros[2] %>;color:<%=liPros[3]%>;display:<%=wzdis%>'><%=liPros[1] %></span>
              </a>
          </li>
          <%} %>
      </ul>
</div>