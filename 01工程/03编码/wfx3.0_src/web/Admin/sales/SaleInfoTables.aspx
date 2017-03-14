<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.SaleInfoTables" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
    <div class="datalist">
        <!--搜索框-->
        <div class="clearfix search_titxt2">
            <div class="searcharea clearfix ">
                <ul class="a_none_left">
                    <li><span>时间段：</span></li>
                    <li><span><UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" /></span></li>
                    <li><span class="Pg_1010">至</span></li>
                    <li><span><UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" /></span></li>
                    <li><Hi:Style ID="Style1"  runat="server" Href="/admin/css/backstage.css" Media="screen" />
                        <asp:DropDownList runat="server" ID="ddlSenders" AutoPostBack="true"></asp:DropDownList>
                    </li>
                    <li><asp:Button ID="btnExport" runat="server" class="submit_queding" Text="导出" /></li>
                    <li><asp:Button ID="btnSerch"  runat="server" class="submit_queding" Text="搜索" /></li>
                    <li><asp:Button ID="btnProductTotalExport" runat="server" class="submit_queding" Text="商品销售数量导出" /></li>
                </ul>
            </div>
        </div>
        <div class="blank12 clearfix"></div>

        <!--表格主体-->
        <div class="backstage-box clearfix">
            <div class="backstage clearfix">
                <asp:Literal runat="server" ID="litTables" />
            </div>
        </div>
    </div>
</div>
  <script type="text/javascript">
        $(function () {
            if ($("#calendarStartDate").val() == "" || $("#calendarEndDate").val() == "")
            {
                
            }
        });
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

