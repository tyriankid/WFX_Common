<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DistributorRegion.aspx.cs" Inherits="Hidistro.UI.Web.Admin.DistributorRegion" %>

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
              <li><asp:LinkButton ID="btnSave" CssClass="Savebutton" runat="server" Text="保存" IsShow="true"  /></li>
              <li><asp:LinkButton ID="btnClearRegionBindInfo" CssClass="Sbutton" runat="server" Text="清空已绑定区域" IsShow="true"  /></li>
            </ul>
          </div>
          <div class="pageNumber">
            <div class="pagination"><UI:Pager runat="server" ShowTotalPages="false" ID="pager" /> </div>
          </div>
        </div>

    <UI:Grid ID="grdStreetsInfo" runat="server" AutoGenerateColumns="False"  ShowHeader="true" DataKeyNames="StreetId" GridLines="None"
                   HeaderStyle-CssClass="table_title"  SortOrderBy="RegionCode" SortOrder="ASC" Width="100%">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" ItemStyle-CssClass="td_txt_cenetr">
                            <ItemTemplate>
                                <input name="CheckBoxGroup" type="checkbox" value='<%# Eval("StreetId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
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
       

        function doSearch() {
            $("#ctl00_contentHolder_hidRegionCode").val($("#regionSelectorValue").val());
            return true;
        }


    </script>
</asp:Content>