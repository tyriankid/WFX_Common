<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="jumpPage.aspx.cs" Inherits="Hidistro.UI.Web.Admin.jumpPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
    <div class="title">
		  <em><img src="../images/01.gif" width="32" height="32" /></em>
          <h1><strong>PC端点餐系统</strong></h1>
        <asp:Button ID="btnQuickDutyOn" runat="server" Text="快速打卡" />
		</div>
    
        <div class="blank12 clearfix" style="border-bottom: 1px solid #C1C1C1;margin: 20px;"></div>
        <a id="jump" href="javascript:void(0)" onclick="jump()" style="display:block;margin:0 20px;"><img src="../images/tp.png" /></a>
        
        <input type="hidden" runat="server" id="isDutyExist" clientidmode="Static" /><!--当天是否已打卡-->
        <input type="hidden" runat="server" id="adminType" />
    </div>
    <script type="text/javascript">
        function jump() {
            var isDutyExist = $("#isDutyExist").val();
            if (isDutyExist === "0") {
                alert("您还未打卡!");
                return false;
            }
            else {
                var type = $("#ctl00_contentHolder_adminType").val();
                var pageHttp = "/Vshop/PCProductSearchBuy.aspx?type=" + type + "";
                
                if (window.location.href.toLowerCase().indexOf('gdsswf.com') > -1) {
                    pageHttp = "http://112.74.84.89/Vshop/PCProductSearchBuy.aspx?type=" + type + "&Id=<%=loginId%>";
                }
                
                window.open(pageHttp, "newwindow", "height=820, width=900, top=0, left=0,titlebar=no,toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no");

            }
         }
    </script>
</asp:Content>
