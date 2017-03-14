<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeFile="DutyShiftList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.agent.DutyShiftList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
    <!--info-->
    <div class="title">
		<em><img src="../images/02.gif" width="32" height="32" /></em>
        <h1><strong>交接班列表</strong></h1>
        <span>查看各个子账号的交接班信息 </span>
	</div>
  <div class="datalist">
    <!--搜索-->
    <div class="clearfix search_titxt2">
    <div class="searcharea clearfix br_search">
			<ul class="a_none_left">
                
                <li><span>选择时间段：</span><span><UI:WebCalendar runat="server" CalendarType="StartDate" CssClass="forminput" ID="calenderFromDate" /></span><span class="Pg_1010">至</span><span><UI:WebCalendar runat="server"  CalendarType="EndDate"  CssClass="forminput" ID="calenderToDate" IsStartTime="false"/></span></li>
				<li><asp:Button ID="btnQueryLogs" runat="server" class="searchbutton" Text="查询" /></li>
			</ul>
	  </div>
	</div>
    <!--列表-->
    <asp:DataList ID="dlstDutyList" runat="server" DataKeyField="ID" Style="width: 100%;">
         <HeaderTemplate>
          <table width="0" border="0" cellspacing="0" >
            <tr class="table_title">
              <td width="15%" class="td_right td_left">登录人</td>
              <td width="22%" class="td_right td_left">开班时间</td>
              <td width="22%" class="td_left td_right_fff">关班时间</td>
              <td width="16%" class="td_left td_right_fff">订单总数</td>
              <td width="16%" class="td_left td_right_fff">订单总额</td>
              <td width="16%" class="td_left td_right_fff">班时</td>
            </tr>
         </HeaderTemplate>
         <ItemTemplate>
          <tr  class="td_bg">
              <td ><%#Eval("UserName") %></td>
              <td ><%#Eval("LoginTime") %></td>
              <td ><%#Eval("LoginOutTime") %></td>
              <td ><%#Eval("OrdersCount") %></td>
              <td ><%#Eval("OrdersTotal","{0:F2}") %></td>
              <td ><%#Eval("DutyHours") %></td>
            </tr>

         </ItemTemplate>
         <FooterTemplate>
           </table>
         </FooterTemplate>
    </asp:DataList>

   </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

</asp:Content>

