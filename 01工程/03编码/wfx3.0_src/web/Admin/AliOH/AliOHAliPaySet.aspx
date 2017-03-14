<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.AliOH.AliOHAliPaySet" CodeBehind="AliOHAliPaySet.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
	  <div class="title  m_none td_bottom"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1>支付宝收款信息设置</h1>
         <span>请设置好您的支付宝信息。 还没开通支付宝？ <a target="_blank" href="https://b.alipay.com/order/productDetail.htm?productId=2014110308142133">立即免费申请开通支付宝接口</a></span>
      </div>
	  <div class="datafrom">
	    <div class="formitem validator1">
            <ul id="ulwapAlipay">
            <li><h2 class="colorE">支付宝手机网页支付</h2></li>
            <li class="clearfix"><span class="formitemtitle Pw_198">是否开启：</span>
                <abbr class="formselect">
            <Hi:YesNoRadioButtonList ID="radEnableWapAliPay" runat="server" RepeatLayout="Flow" />
          </abbr>
            </li>
            <li><span class="formitemtitle Pw_198">商家号：</span>
              <asp:TextBox ID="txtPartner" CssClass="forminput formwidth" runat="server" />
            </li>
            <li><span class="formitemtitle Pw_198">商家密钥：</span>
              <asp:TextBox ID="txtKey" CssClass="forminput formwidth" runat="server" />
            </li> 
            <li><span class="formitemtitle Pw_198">收款账户：</span>
              <asp:TextBox ID="txtAccount" CssClass="forminput formwidth" runat="server" />
            </li> 
	     </ul>
	      <ul class="btntf Pa_198 clear">
	        <asp:Button runat="server" ID="btnAdd" Text="保 存" onclick="btnOK_Click" CssClass="submit_DAqueding inbnt" />
          </ul>
        </div>
      </div>
</div>	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $("#ulwapAlipay").show();
      
    });


</script>
</asp:Content>

