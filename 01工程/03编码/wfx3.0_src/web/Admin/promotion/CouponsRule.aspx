<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CouponsRule.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CouponsRule" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
  <div class="blank12 clearfix"></div>
  <div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/06.gif" width="32" height="32" /></em>
      <h1>优惠券管理 </h1>
     <span>优惠劵默认是所有商品都可以使用，选中的分类不可以使用该优惠劵</span></div>
    <!-- 添加按钮-->
    <div class="btn">
        <asp:Button ID="btn_Save" runat="server" Text="保存"  CssClass="submit_bnt1" />
    </div>
    <!--结束-->
    <!--数据列表区域-->
    <div class="datalist">   
        <asp:HiddenField ID="txtCouponsId" runat="server" />
                    <asp:CheckBoxList ID="chk_cate" runat="server" RepeatColumns="6" RepeatDirection="Horizontal"></asp:CheckBoxList>
    </div>

      
                              
      
    <!--数据列表底部功能区域-->
  </div>
</asp:Content>