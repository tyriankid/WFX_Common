<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="VoteResultInfo.aspx.cs"  Inherits="Hidistro.UI.Web.Admin.VoteResultInfo" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">    
  <div class="dataarea mainwidth databody">
    <div class="title"><em><img src="../images/01.gif" width="32" height="32" /></em>
      <h1>微投票管理 </h1>
     <span>您可以发起一个投票，投票完成后可点击展开查看投票结果。</span></div>
    <!-- 结果列表，程序构建-->
    <div class="datalist">
     	<asp:Literal runat="server" ID="litResultInfoTable"></asp:Literal>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
