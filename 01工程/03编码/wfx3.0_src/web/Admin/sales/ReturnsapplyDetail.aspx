<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
   CodeFile="ReturnsapplyDetail.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.ReturnsapplyDetail" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="Server">
    <div class="areacolumn clearfix">
        <div class="columnright">
            <div class="title">
                <em>
                    <img src="../images/04.gif" width="32" height="32" /></em> <span>退货(款)的详细信息</span>
            </div>
            <div class="formitem clearfix">
                <ul>
                    <li><span class="formitemtitle Pw_110">订单编号：</span>
                        <asp:Literal runat="server" ID="litOrderid" /></li>
                    <li><span class="formitemtitle Pw_110">会员名：</span><asp:Literal runat="server" ID="litUserName" /></li>
                    <li><span class="formitemtitle Pw_110">退款金额(元)：</span>
                        <asp:Literal runat="server" ID="litRefundMoney" /></li>
                    <li><span class="formitemtitle Pw_110">申请时间：</span><asp:Literal runat="server" ID="litApplyForTime" /></li>
                    <li><span class="formitemtitle Pw_110">申请备注：</span><asp:Literal runat="server" ID="litComments" /></li>
                    <li><span class="formitemtitle Pw_110">处理状态：</span><asp:Literal runat="server" ID="litHandleStatus" /></li>
                    <li><span class="formitemtitle Pw_110">处理时间：</span><asp:Literal runat="server" ID="litHandleTime" /></li>
                    <li><span class="formitemtitle Pw_110">管理员备注：</span><asp:Literal runat="server" ID="litAdminRemark" />
                    </li>
                    <li><span class="formitemtitle Pw_110">收款帐号：</span><asp:Literal runat="server" ID="litAccount" />
                    </li>
                    <li><span class="formitemtitle Pw_110">商品名称：</span><asp:Literal runat="server" ID="litProductName" />
                    </li>
                    <li><span class="formitemtitle Pw_110">审核时间：</span><asp:Literal runat="server" ID="litAuditTime" />
                    </li>
                    <li><span class="formitemtitle Pw_110">退款时间：</span><asp:Literal runat="server" ID="litRefundTime" />
                    </li>
                    <li><span class="formitemtitle Pw_110">操作人员：</span><asp:Literal runat="server" ID="litOperator" />
                    </li>
                </ul>
                <ul class="btn Pa_110" style="display:none">
                    <asp:Button runat="server" ID="btnSubmit" CssClass="submit_DAqueding" Text="返回" />
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="Server">
</asp:Content>
