<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.StoreMessageList" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    
<div class="h20"></div>


	<div class="dataarea mainwidth">
		<!--搜索-->
        <div class="clearfix search_titxt2">
      <div class="Pa_15">
        
	    </div>
		<!--结束-->
	    <div class="searcharea clearfix br_search">
			<ul class="a_none_left">
				<li>
					留言人名称：<asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
				</li>
                <li>代理商店名：<asp:TextBox ID="txtStoreName" runat="server"></asp:TextBox></li>
				<li style="margin-top:3px;"><asp:Button ID="btnSearch" runat="server" class="searchbutton" Text="查询" /></li>
			</ul>
	  </div>
	  </div>

<div class="functionHandleArea clearfix m_none">
			<!--分页功能-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                 <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
               </div>
			</div>
			<!--结束-->

			<div class="blank8 clearfix"></div>
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)">全选</a></span>
					<span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck"　IsShow="true" runat="server" Text="删除" /></span></li>
				</ul>
			</div>
		</div>
		
		<!--数据列表区域-->
	  <div class="datalist clearfix">
	  <UI:Grid ID="grdStoreMessage" runat="server" ShowHeader="true" AutoGenerateColumns="false"  DataKeyNames="ID" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>                 
                <UI:CheckBoxColumn ReadOnly="true" HeaderStyle-CssClass="td_right td_left"/>
                <asp:TemplateField HeaderText="留言人" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                         <asp:Literal ID="lblUserName" Text='<%#Eval("UserName") %>' runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                   <asp:TemplateField HeaderText="留言时间" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                      <Hi:FormatedTimeLabel ID="lblMsgTime" Time='<%#Eval("MsgTime") %>' runat="server"></Hi:FormatedTimeLabel>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="留言内容" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                       <asp:Label ID="lblMessaegeCon" Text='<%#Eval("MessaegeCon") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass=" td_left td_right_fff">
                     <ItemTemplate>
                         <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkbtnDeleteSelect" CommandName="Delete" runat="server" IsShow="true" CssClass="SmallCommonTextButton" Text="删除" /></span>
                     </ItemTemplate>
                 </asp:TemplateField>
            </Columns>
        </UI:Grid>        
      <div class="blank5 clearfix"></div>
	  </div>
	  <!--数据列表底部功能区域-->
	  <div class="page">
	  <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
        　　　</div>

			</div>
		</div>
      </div>

	</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
      
</asp:Content>


