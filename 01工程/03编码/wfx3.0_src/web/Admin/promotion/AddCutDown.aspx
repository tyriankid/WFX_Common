<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddCutDown.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddCutDown" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/06.gif" width="32" height="32" /></em>
            <h1>添加砍价活动</h1>
            <span>填写砍价活动详细信息</span>
          </div>          
          
      <div class="formitem validator5" style="padding-left:15px;">      
		<ul class="kuang_ul">
				<table border="0" cellspacing="5" cellpadding="0" style="width:775px;" class=float">
                  <tr>
                    <td><span class="formitemtitle Pw_100">商品名称：</span></td>
                    <td><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></td>
                    <td><abbr class="formselect">
						 <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择商品分类--" />
					     </abbr></td>
                    <td ><span class="formitemtitle Pw_100" style="white-space:nowrap;">商家编码：</span></td>
                    <td><asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="forminput" /></td>
                    <td><input type="button" id="btnSearch" value="查询" onclick="ResetGroupBuyProducts()" class="searchbutton"/></td>
                  </tr>
                </table>
		 </ul>
        <ul>
        <li></li>
        <li><span class="formitemtitle Pw_128">砍价商品：</span>
			<abbr class="formselect">
						<Hi:GroupBuyProductDropDownList ID="dropCutDownProduct" runat="server" />
					</abbr>
					<p id="P1">当此砍价活动有会员已订购时，商品不能再进行编辑</p>
			</li>
            <li id="li_price"><span class="formitemtitle Pw_128">一口价：</span>
			<abbr class="formselect"><asp:Label ID="lblPrice" runat="server"></asp:Label></abbr>
					<p id="P4"></p>
			</li>
			 <li> <span class="formitemtitle Pw_128"><em >*</em>开始日期：</span>
          <UI:WebCalendar runat="server" CssClass="forminput" ID="calendarStartDate" /><abbr class="formselect"><Hi:HourDropDownList ID="drophours" runat="server" style=" margin-left:5px;"/></abbr>
          <p id="P3">当达到开始日期时，活动会自动变为正在参与活动状态。</p>
          </li>
          <li> <span class="formitemtitle Pw_128"><em >*</em>结束日期：</span>
          <UI:WebCalendar runat="server" CssClass="forminput" ID="calendarEndDate" /><abbr class="formselect"><Hi:HourDropDownList ID="HourDropDownList1" runat="server" style=" margin-left:5px;"/></abbr>
          <p id="P2">当达到结束日期时，活动会自动变为结束状态。</p>
          </li>
          <li><span class="formitemtitle Pw_128"><em >*</em>限购总数量：</span>
            <asp:TextBox ID="TxtCount" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="ctl00_contentHolder_txtMaxCountTip">此次活动可购买的商品总数量,不能为空,订购达到此上限时，活动会自动变为结束状态。</p>
          </li>
          <li><span class="formitemtitle Pw_128"><em >*</em>最大被砍价数：</span>
            <asp:TextBox ID="TxtMaxCount" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="P6">此次活动可被砍价的总数量,不能为空,砍价达到此上限时，无法继续被砍价。</p>
          </li>
          <li><span class="formitemtitle Pw_128"><em >*</em>每次砍价金额：</span>
            <asp:TextBox ID="TxtPerCutPrice" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="P5">此次活动每次砍价减少的金钱数量</p>
          </li>
          <li><span class="formitemtitle Pw_128"><em >*</em>起始金额：</span>
            <asp:TextBox ID="TxtCurrentPrice" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="P8">此次活动商品的起始价格,不能为空。</p>
          </li>
          <li><span class="formitemtitle Pw_128"><em >*</em>最低价格：</span>
            <asp:TextBox ID="TxtMinPrice" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="P7">此次活动商品最低的价格,不能为空,砍价超过此底线时，无法继续被砍价。</p>
          </li>


          
         <li>
	       <span class="formitemtitle Pw_128">活动说明：</span>
		     <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Columns="50" Rows="5" CssClass="forminput"></asp:TextBox>
	            </li>
	          <li></li>        
      </ul>
      <ul class="btn Pa_100 clear">
      <li  class="li_pa_left">
         <asp:Button ID="btnAddCutDown" runat="server" Text="添  加" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding"  />
         </li>
        </ul>
      </div>

      
  </div>
  </div>

 <script type="text/javascript" src="groupbuy.helper.js"></script>
  <script type="text/javascript" language="javascript">
      function InitValidators() {

          initValid(new InputValidator('ctl00_contentHolder_txtMaxCount', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，最大砍价次数只能输入整数型数值'))
          appendValid(new NumberRangeValidator('ctl00_contentHolder_txtMaxCount', 1, 9999999, '输入的数值超出了系统表示范围'));

          initValid(new InputValidator('ctl00_contentHolder_txtCount', 1, 10, false, '-?[0-9]\\d*', '数据类型错误，商品最大购买数只能输入整数型数值'))
          appendValid(new NumberRangeValidator('ctl00_contentHolder_txtCount', 1, 9999999, '输入的数值超出了系统表示范围'));

          initValid(new InputValidator('ctl00_contentHolder_TxtPerCutPrice', 1, 10, false, '([0-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，每次砍价金额只能输入数值'))
          appendValid(new MoneyRangeValidator('ctl00_contentHolder_TxtPerCutPrice', 0.01, 9999999, '输入的数值超出了系统表示范围'));

          initValid(new InputValidator('ctl00_contentHolder_TxtMinPrice', 1, 10, false, '([0-9]\\d*(\\.\\d{1,2})?)', '数据类型错误，最低价格只能输入数值'))
          appendValid(new MoneyRangeValidator('ctl00_contentHolder_TxtMinPrice', 0.01, 9999999, '输入的数值超出了系统表示范围'));
      }
      
      $(function () {
          //InitValidators();
          $("#li_price").hide();
          
          $("#ctl00_contentHolder_dropCutDownProduct").change(function () {
              var pId = $(this).val();
              if (pId == "") {
                  $("#li_price").hide();
              }
              else {
                  $.ajax({
                      url: "EditCutDown.aspx",
                      data:
                      {
                          isCallback: "true",
                          productId: pId
                      },
                      type: 'GET', dataType: 'json', timeout: 10000,
                      async: false,
                      success: function (resultData) {
                          if (resultData.Status == "OK") {
                              var price = resultData.Price;
                              $("#ctl00_contentHolder_lblPrice").html(price);
                              $("#li_price").show();
                          }
                      }
                  });
              }
          });
      });
  </script> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    
</asp:Content>
