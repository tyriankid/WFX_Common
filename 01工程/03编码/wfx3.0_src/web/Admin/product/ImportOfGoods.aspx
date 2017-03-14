<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Admin/Admin.Master" 
    CodeFile="ImportOfGoods.aspx.cs" Inherits="Admin_product_ImportOfGoods" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>
                商品编码管理</h1>
            <span>店铺中所有的商品，您可以对商品进行搜索，也能对商品编码进行编辑的操作</span>
        </div>
        <div class="datalist">
            <!--搜索-->                     
            <div class="clearfix search_titxt2">
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li><span>商品信息：</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></span></li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--请选择商品分类--"
                                Width="150" />
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--请选择品牌--"
                                Width="153" />
                        </abbr>
                    </li>
                    <li style="display: none;">
                        <abbr class="formselect">
                            <Hi:ProductTagsDropDownList runat="server" ID="dropTagList" NullToDisplay="--请选择标签--"
                                Width="153" />
                        </abbr>
                    </li>
                    <li>
                        <abbr class="formselect">
                            <Hi:ProductTypeDownList ID="dropType" runat="server" NullToDisplay="--请选择类型--" Width="153" />
                        </abbr>
                    </li>
                </ul>
            </div>
            <div class="searcharea clearfix" style="padding: 3px 0px 10px 0px;">
                <ul>                  
                    <li><span>添加时间：</span></li>
                    <li>
                        <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                        <span class="Pg_1010">至</span>
                        <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                    </li>
                    <li>
                        <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" /></li>
                </ul>
            </div>
            </div>
            <!--结束-->
            <div class="functionHandleArea clearfix">
                <!--分页功能-->
                <div class="pageHandleArea">
                    <ul>
                        <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize"/>
                        </li>
                    </ul>
                </div>
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
                    </div>
                </div>
                <!--结束-->
                <div class="blank8 clearfix">
                </div>
            </div>
            <!--数据列表区域-->
            <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                GridLines="None" DataKeyNames="ProductId" SortOrder="Desc" AutoGenerateColumns="false"
                HeaderStyle-CssClass="table_title" CssClass="goods-list">
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <Columns>
                    <asp:BoundField HeaderText="排序" DataField="DisplaySequence" ItemStyle-Width="35px"
                       ItemStyle-CssClass="td_txt_cenetr" />
                    <asp:TemplateField ItemStyle-Width="30%" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <div style="float: left; margin-right: 10px;">
                                <a href='<%#"../../Vshop/ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                    <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40" />
                                </a>
                            </div>
                            <div style="float: left;">
                                <span class="Name"><a href='<%#"../../Vshop/ProductDetails.aspx?productId="+Eval("ProductId")%>'target="_blank"><%# Eval("ProductName") %></a></span>                              
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField ItemStyle-Width="25%" HeaderText="商品来源" >
                        <ItemTemplate>
                                <asp:Literal ID="litMarketPrice" runat="server" Text='<%#Eval("CommoditySource")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField ItemStyle-Width="15%" HeaderText="商品渠道商" >
                        <ItemTemplate>
                                <asp:Literal ID="litMarketPrice" runat="server" Text='<%#Eval("Distributor")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField><asp:TemplateField ItemStyle-Width="15%" HeaderText="商品价格" >
                        <ItemTemplate>
                                <asp:Literal ID="litMarketPrice" runat="server" Text='<%#Eval("Price")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="商品编码" ItemStyle-Width="25%" ItemStyle-CssClass="td_txt_cenetr" >
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="litSaleStatus" runat="server" Text='<%#Eval("CommodityCode")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="操作" HeaderStyle-Width="20" ItemStyle-CssClass="td_txt_cenetr" >
                        <ItemTemplate>
                            <span class="submit_bianji"><a href="javascript:void(0);" onclick="javascript:EditProducts('<%#Eval("ProductId")%>')">设置商品编码</a></span>
                            <span class="submit_shanchu">   
                            <%--<span class="submit_bianji"><a href="javascript:void(0);" onclick="javascript:EditProduct('<%#Eval("ProductId")%>')">设置渠道商</a></span>
                            <span class="submit_shanchu"> --%>                           
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </UI:Grid>
            <div class="blank12 clearfix">
            </div>
        </div>
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>
        </div>
    </div>

<script type="text/javascript">
    function EditProducts(ProductId) {       
        DialogFrame("vshop/ModifyGoods.aspx?ProductId=" + ProductId, "商品编码编辑");
    } 
    //function EditProduct(ProductId){
    //    DialogFrame("vshop/ProductDistributor.aspx?ProductId=" + ProductId,"商品渠道商编辑")
    //}
</script>   
</asp:Content>


