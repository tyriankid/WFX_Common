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
            <h1>����Ż�ȯ�</h1>
            <span>�����Ż�ȯ���Ϣ����Ŀ�ʼ���ںͽ����������Żݾ���ͬ</span>
          </div>
          <asp:HiddenField ID="txtID" runat="server" />

      <div class="formitem validator2">
        <ul>
            <li> <span class="formitemtitle Pw_100" style="width:150px;"><em >*</em>�Ż�ȯ����ƣ�</span>
          <asp:TextBox ID="txtCouponActName" runat="server" CssClass="forminput"></asp:TextBox>
            <p id="ctl00_contentHolder_txtCouponActNameTip">�Ż�ȯ����Ʋ���Ϊ�գ���1��60���ַ�֮��</p>
          </li>
          <li> <span class="formitemtitle Pw_100" style="width:150px;"><em >*</em>ѡ���Ż�ȯ��</span>
              <asp:DropDownList ID="ddlCoupons" runat="server"></asp:DropDownList>
          </li>
          <li>
             <span class="formitemtitle Pw_100" style="width:150px;"><em >*</em>��ȡҳ�汳����</span>
              <div class="uploadimages">
            <Hi:UpImg runat="server" ID="upBgImg" IsNeedThumbnail="false"  UploadType="SharpPic"/>
                  </div>
          </li>
          <li><span class="formitemtitle Pw_100" style="width:150px;"><em >*</em>��Աÿ�췢��������</span>
            <asp:TextBox ID="txtColValue" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="ctl00_contentHolder_txtColValueTip">��Աÿ�췢������ֻ������ֵ��0-10000000���Ҳ��ܳ���2λС����0��ʾ������</p>
          </li>
               
      </ul>
      <ul class="btn Pa_100 clear">
      <asp:Button ID="btnAddCoupons" runat="server" Text="����" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding"  />
      </ul>
      </div>

      </div>
  </div>







</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

     <script type="text/javascript" language="javascript">
function InitValidators()
{
    initValid(new InputValidator('ctl00_contentHolder_txtCouponActName', 1, 60, false, null, '�Ż�ȯ������Ʋ���Ϊ�գ���1��60���ַ�֮��'));
    initValid(new InputValidator('ctl00_contentHolder_txtColValue', 0, 10, false, '-?[0-9]\\d*', '��Աÿ�췢������ֻ�������֣�0��ʾ������'))
    appendValid(new NumberRangeValidator('ctl00_contentHolder_txtColValue', 0, 1000, '��Աÿ�췢������ֻ�������֣���С��1000��0��ʾ������'));
}
$(document).ready(function(){ InitValidators(); });
</script>

</asp:Content>

