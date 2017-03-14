<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeFile="EditAgentGrade.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditAgentGrade" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" Runat="Server">
<style type="text/css">.Pw_110{width:145px;padding-right: 5px;}.errorFocus{width:220px;}.forminput{width:220px;padding:4px 0px 4px 2px}.areacolumn .columnright .formitem li{margin-bottom:0;}</style>
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1><%=htmlOperatorName %>代理商等级</h1>
            <span>不同的代理商对应不同的分佣比</span>
          </div>
      <div class="formitem validator4 clearfix">
        <ul>
          <li> <span class="formitemtitle Pw_110">代理商等级名称</span><asp:TextBox ID="txtName" runat="server" CssClass="forminput"></asp:TextBox><p id="ctl00_contentHolder_txtNameTip"></p></li>
          
          <li> <span class="formitemtitle Pw_110"><em ></em>直接销售佣金上浮</span><asp:TextBox ID="txtFirstCommissionRise" runat="server" CssClass="forminput"></asp:TextBox>%<p id="ctl00_contentHolder_txtFirstCommissionRiseTip"></p>
          </li>
          <li> <span class="formitemtitle Pw_110">等级图标</span>
                 <div class="uploadimages">
                    <Hi:UpImg runat="server" ID="uploader1" IsNeedThumbnail="false" UploadType="vote"  />
                </div>
              <p id="uploader1_uploadedImageUrlTip" style="padding-left:23px;">（建议上传PNG背景透明的图片，大小50px * 50px）</p>
          </li>
          <li style="margin-bottom:10px;"> <span class="formitemtitle Pw_110">备注</span><asp:TextBox ID="txtDescription" runat="server" CssClass="forminput" TextMode="MultiLine" Width="350" Height="90"></asp:TextBox>
          </li>
      </ul>
      <ul class="btn Pa_110">
        <asp:Button ID="btnEditUser" runat="server" Text="确 定" OnClientClick="return CheckForm()"  CssClass="submit_DAqueding" />
        </ul>
      </div>

      </div>
  </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" Runat="Server">
    <script type="text/javascript" language="javascript">
        function CheckForm() {
            if (PageIsValid()) {
                var info = $("#uploader1_uploadedImageUrl").val();
                if (info.length < 10) {
                    $("#uploader1_uploadedImageUrlTip").attr("class", "msgError").html("请上传等级图标");
                    return false;
                } else {
                    $("#uploader1_uploadedImageUrlTip").attr("class", "").html("");
                }
            } else {
                return false;
            }
        }
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtName', 1, 20, false, null, '代理商等级名称在20个字符以内'));
            initValid(new InputValidator('ctl00_contentHolder_txtFirstCommissionRise', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，只能输入实数型数值'));
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtFirstCommissionRise', 0, 100, '佣金上浮必须在0-100之间'));
            //initValid(new InputValidator('uploader1_uploadedImageUrl', 1, 200, false, null, '请上传等级图标'));
        }
        $(document).ready(function() { InitValidators(); });

    </script>
    
</asp:Content>