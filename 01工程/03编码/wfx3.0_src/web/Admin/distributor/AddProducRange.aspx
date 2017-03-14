<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddProducRange.aspx.cs" Inherits="Hidistro.UI.Web.Admin.distributor.AddProducRange" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
<script type="text/javascript">
    var auth = "<%=(Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value) %>";
</script>
    <script src="../js/swfupload/swfupload.js" type="text/javascript"></script>
    <script src="../js/UploadHandler.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server" ClientIDMode="Static">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>设置商品上架范围</h1>
            <span class="font">设置后，当前代理商下属的分销商只能销售当前代理商有的商品</span>
      </div>
          <div class="formitem validator2">
            <ul>
            <li><span class="formitemtitle Pw_100">商品分类：</span>
                <asp:CheckBoxList ID="cboxCategory" runat="server" RepeatDirection="Horizontal"></asp:CheckBoxList>
            </li>    
            <li><span class="formitemtitle Pw_100">商品品牌：</span>
                <asp:CheckBoxList ID="cboxBrand" runat="server" RepeatDirection="Horizontal"></asp:CheckBoxList>
            </li>   
            <li><span class="formitemtitle Pw_100">商品类型：</span>
                <asp:CheckBoxList ID="cboxType" runat="server" RepeatDirection="Horizontal"></asp:CheckBoxList>
            </li>          
            </ul>
              <ul class="btn Pa_100 clearfix">
                <asp:Button ID="btnAddBanner" runat="server"  Text="确 定"  
                      CssClass="submit_DAqueding float" onclick="btnAddBanner_Click" />
         </ul>
         <!--隐藏图片地址-->
               <input id="fmSrc" runat="server" clientidmode="Static" type="hidden" value="" />
               <input id="locationUrl" runat="server" clientidmode="Static" type="hidden" value="" />   
          </div>
  </div>    
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script src="../js/UploadBanner.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
           
        }
        );
        
    </script>

</asp:Content>

