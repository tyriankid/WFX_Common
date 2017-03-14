<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddDeliveryScop.aspx.cs" Inherits="Hidistro.UI.Web.Admin.shippingStreets" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
    <div class="title">
		<em><img src="../images/03.gif" width="32" height="32" /></em>
		  
        <h1><strong>配送区域设置</strong></h1>
        <span>对区级地区增加街道配送范围</span>
	</div>




    <div class="datalist">
        <!--搜索区-->
		<div class="searcharea clearfix br_search">
			<ul>
                <li>
                    <Hi:RegionSelector ID="dropRegion" runat="server" IsShift="false" ProvinceWidth="180" CityWidth="150" CountyWidth="150" />
                </li>
                <li>街道信息：</li>
                <li>
                    <asp:TextBox ID="txtStreetName" runat="server" CssClass="forminput"  />
                </li>
                <li><asp:Button ID="btnSearchButton" runat="server" class="searchbutton" onclientclick="return doSearch()" Text="搜索" /></li>
            </ul>
        </div>

        <div class="functionHandleArea clearfix">
          <div class="pageHandleArea">
            <ul>
              <li><input type="button" id="btnAdd" onclick="doAdd()" class="submit_jia" value="添加"/></li>
            </ul>
          </div>
          <div class="pageNumber">
            <div class="pagination"><UI:Pager runat="server" ShowTotalPages="false" ID="pager" /> </div>
          </div>
        </div>

    <UI:Grid ID="grdStreetsInfo" runat="server" AutoGenerateColumns="False"  ShowHeader="true" DataKeyNames="StreetId" GridLines="None"
                   HeaderStyle-CssClass="table_title"  SortOrderBy="RegionCode" SortOrder="ASC" Width="100%">
                    <Columns>
                       
                        <asp:TemplateField HeaderText="街道名" SortExpression="StreetName" ItemStyle-Width="280px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblStreetName" runat="server" Text='<%# Bind("StreetName") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="省市区" SortExpression="RegionName" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblRegionName" runat="server" Text='<%# Bind("RegionName") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="绑定店铺名" SortExpression="StoreName" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblStoreName" runat="server" Text='<%#string.IsNullOrEmpty(Eval("StoreName").ToString())?"还未绑定":Eval("StoreName") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="绑定用户名" SortExpression="UserName" ItemStyle-Width="120px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblUserName" runat="server" Text='<%#string.IsNullOrEmpty(Eval("UserName").ToString())?"还未绑定":Eval("UserName") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_left td_right_fff">
                         <ItemStyle CssClass="spanD spanN" />
                             <ItemTemplate>
                                 <span class="submit_bianji"><a href='javascript:0' onclick="doEdit('<%# Eval("StreetId") %>')">编辑</a></span>
		                         <span class="submit_shanchu"><Hi:ImageLinkButton  runat="server" ID="Delete" Text="删除" CommandName="Delete" IsShow="true" /></span>
                             </ItemTemplate>
                         </asp:TemplateField>  
                    </Columns>
       </UI:Grid> 
        <div class="blank5 clearfix"></div>
    </div>
    <div class="bottomPageNumber clearfix">
      <div class="pageNumber"> 
      <div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
        </div>
      </div>
    </div>
</div>
<input type="hidden" runat="server" id="hidRegionCode"/>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script language="javascript" type="text/javascript">
        function doAdd() {
            // var RegionId = $("").val();
            var RegionId = $("#regionSelectorValue").val();
            var RegionName = $("#regionSelectorName").val();
            var streetName = $("#ctl00_contentHolder_txtStreetName").val();
            if (RegionId == "" || RegionId == undefined ) {
                alert("请选择一个区域");
                return false;
            }
            if (streetName == "" || streetName == undefined ) {
                alert("请选填写街道信息");
                return false;
            }
            
            else {
                $.ajax({
                    type: "POST",   //访问WebService使用Post方式请求
                    contentType: "application/json", //WebService 会返回Json类型
                    url: "shippingStreets.aspx/AddStreetInfo", //调用WebService的地址和方法名称组合 ---- WsURL/方法名
                    data: "{regionCode:'" + RegionId + "',streetName:'" + streetName + "'}",         //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到      
                    dataType: 'json',
                    success: function (result) {     //回调函数，result，返回值
                        if (result.d == "success") {
                            location.reload();
                        }
                    }
                });

            }

        }

        function doSearch() {
            $("#ctl00_contentHolder_hidRegionCode").val($("#regionSelectorValue").val());
            return true;
        }

        function doEdit(streetId) {
            //alert(streetId);
        }
    </script>
</asp:Content>