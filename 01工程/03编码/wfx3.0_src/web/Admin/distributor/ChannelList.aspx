<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeFile="ChannelList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.distributor.ChannelList" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/04.gif" width="32" height="32" /></em>
            <h1>渠道商的管理</h1>
            <span>渠道商的查看,增加,删除,编辑等操作</span>
        </div>
        <div class="btn">
	    <a href="ChannelGrade.aspx" class="submit_jia">添加渠道商</a>
    </div>
        <!--搜索-->
        <!--数据列表区域-->
        <div class="datalist">
            <table>
                <thead>
                    <tr class="table_title">
                        <td>
                            渠道商名称
                        </td>
                        <td>                   
                            备注  
                         </td>                    
                        <td>
                            操作
                        </td>
                    </tr>
                </thead>
                <asp:Repeater ID="rptChannellist" runat="server" OnItemCommand="rptChannelList_ItemCommand" EnableViewState="true">
                    <ItemTemplate>
                        <tbody>
                            <tr>                              
                                <td><%#Eval("ChannelName")%>&nbsp;                                                     
                                <td><%#Eval("Remark")%>&nbsp;</td>
                                <td width="188" class="td_txt_cenetr">
                                    <span class="submit_bianji"><a style="cursor:pointer" href="ChannelGrade.aspx?id=<%# Eval("Id")%>">
                                        编辑</a></span>                                        
                                        <span class="submit_bianji"><asp:LinkButton ID="lbtnDel" runat="server" CommandArgument='<%#Eval("Id") %>' DeleteMsg='<%# "确定要删除渠道商【"+Eval("ChannelName")+"】吗?"%>' IsShow="true" CommandName="del">删除</asp:LinkButton> </span>                              
                            </tr>
                        </tbody>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="blank12 clearfix">
            </div>
        </div>
        <!--数据列表底部功能区域-->
        <div class="bottomPageNumber clearfix">
            <div class="pageNumber">
                <div class="pagination" style="width: auto">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
