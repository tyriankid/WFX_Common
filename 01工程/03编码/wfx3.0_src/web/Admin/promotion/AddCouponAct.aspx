<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddCoupon.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddCouponAct" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="areacolumn clearfix">

      <div class="columnright">
          <div class="title">
            <em><img src="../images/06.gif" width="32" height="32" /></em>
            <h1>添加优惠券活动</h1>
            <span>创建优惠券活动信息，活动的开始日期和结束日期与优惠卷相同</span>
          </div>
          <asp:HiddenField ID="txtID" runat="server" />

      <div class="formitem validator2">
        <ul>
            <li> <span class="formitemtitle Pw_100" style="width:150px;"><em >*</em>优惠券活动名称：</span>
          <asp:TextBox ID="txtCouponActName" runat="server" CssClass="forminput"></asp:TextBox>
            <p id="ctl00_contentHolder_txtCouponActNameTip">优惠券活动名称不能为空，在1至60个字符之间</p>
          </li>
          <li> <span class="formitemtitle Pw_100" style="width:150px;"><em >*</em>选择优惠券：</span>
              <asp:DropDownList ID="ddlCoupons" runat="server"></asp:DropDownList>
          </li>
          <li>
             <span class="formitemtitle Pw_100" style="width:150px;"><em >*</em>领取页面背景：</span>
              <div class="uploadimages">
            <Hi:UpImg runat="server" ID="upBgImg" IsNeedThumbnail="false"  UploadType="SharpPic"/>
                  </div>
          </li>
          <li><span class="formitemtitle Pw_100" style="width:150px;"><em >*</em>会员每天发放数量：</span>
            <asp:TextBox ID="txtColValue" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="ctl00_contentHolder_txtColValueTip">会员每天发放数量只能是数值，0-10000000，且不能超过2位小数，0表示无限制</p>
          </li>
               
      </ul>
      <ul class="btn Pa_100 clear">
      <asp:Button ID="btnAddCoupons" runat="server" Text="保存" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding"  />
      </ul>
      </div>

      </div>
  </div>







</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

     <script type="text/javascript" language="javascript">
function InitValidators()
{
    initValid(new InputValidator('ctl00_contentHolder_txtCouponActName', 1, 60, false, null, '优惠券活动的名称不能为空，在1至60个字符之间'));
    initValid(new InputValidator('ctl00_contentHolder_txtColValue', 0, 10, false, '-?[0-9]\\d*', '会员每天发放数量只能是数字，0表示不限制'))
    appendValid(new NumberRangeValidator('ctl00_contentHolder_txtColValue', 0, 1000, '会员每天发放数量只能是数字，需小于1000，0表示不限制'));
}
$(document).ready(function(){ InitValidators(); });
</script>

</asp:Content>

