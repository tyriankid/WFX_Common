<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AgentProducts.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.AgentProducts" %>

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
            <em>
                <img src="../images/8.png" width="32" height="32" /></em>
            <h1>代理商下单</h1>
            <span>已订货的所有商品，您可以对商品进行下单操作</span>
        </div>
        <div class="datalist">
            <!--搜索-->
            <div class="clearfix search_titxt2">
            <div class="searcharea clearfix" style="padding: 3px 0px 10px 0px;">
                <ul>
                    <li><span>提示：如果无送货地址选项请登录微信商品配置地址；如果要增加支付方式请联系管理员。</span></li>
                </ul>
            </div>
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li>
                        <span><span style="color:red">*</span>送货地址：</span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="userAddress" runat="server"></asp:DropDownList>
                        </abbr>
                    </li>
                    <li>
                        <span><span style="color:red">*</span>配送方式：</span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="userGiveMode" runat="server"></asp:DropDownList>
                        </abbr>
                    </li>
                    <li>
                        <span><span style="color:red">*</span>支付方式：</span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="userPayMode" runat="server"></asp:DropDownList>
                        </abbr>
                    </li>
                </ul>
            </div>
            <div class="searcharea clearfix" style="padding: 3px 0px 10px 0px;">
                <ul>
                    <li><span style="margin-left:5px;">订单说明：</span><span><asp:TextBox ID="txtOrderRemark" runat="server" Width="400" MaxLength="200" CssClass="forminput" /></span></li>
                    <%--<li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" /></li>--%>
                </ul>
            </div>
            </div>
               
            <!--结束-->
            <div class="functionHandleArea clearfix">
                <!--结束-->
                <div class="blank8 clearfix">
                </div>
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton"><span class="signicon"></span>
                            <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span> 
                            <span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span> 
                            <span class="delete"><Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除" IsShow="true" DeleteMsg="确定要把商品移出订货列表吗？" /></span>
                             <%--OnClientClick="doDelete();"--%>
                            <asp:HiddenField ID="hiSkuIds" runat="server" ClientIDMode="Static" />
                        </li>
                    </ul>
                </div>
                <div class="filterClass">
                    <asp:Button ID="btnBuy" runat="server" Text="生成订单" CssClass="submit_DAqueding inbnt" OnClientClick="return doSubmit();" />
                </div>
            </div>
            <!--数据列表区域-->
            <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                GridLines="None" DataKeyNames="SkuId" SortOrder="Desc" AutoGenerateColumns="false"
                HeaderStyle-CssClass="table_title" CssClass="goods-list">
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <Columns>
                    <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" ItemStyle-CssClass="td_txt_cenetr">
                        <ItemTemplate>
                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("SkuId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="排序" DataField="DisplaySequence" ItemStyle-Width="35px"
                       ItemStyle-CssClass="td_txt_cenetr" />
                    <asp:TemplateField ItemStyle-Width="46%" HeaderText="商品" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <div style="float: left; margin-right: 10px;">
                                <a href='<%#"../../Vshop/ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                    <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40" />
                                </a>
                                <asp:HiddenField ID="hiProductId" runat="server" ClientIDMode="Static" />
                            </div>
                            <div style="float: left;">
                                <span class="Name"><a href='<%#"../../Vshop/ProductDetails.aspx?productId="+Eval("ProductId")%>'
                                    target="_blank">
                                    <%# Eval("ProductName") %></a></span> <span class="colorC" style="display:block">商家编码：<%# Eval("ProductCode") %>
                                        库存：<%# Eval("Stock") %>
                                        成本：<%# Eval("CostPrice", "{0:f2}")%>
                                    </span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="22%" HeaderText="商品价格" >
                        <ItemTemplate>
                            <span class="Name">
                                单价:<asp:Literal ID="litSalePrice" runat="server" Text='<%#Eval("SalePrice", "{0:f2}")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="商品状态" ItemStyle-Width="80" ItemStyle-CssClass="td_txt_cenetr" >
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="litSaleStatus" runat="server" Text='<%#Eval("SaleStatus")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="商品销售区域" ItemStyle-Width="80" ItemStyle-CssClass="td_txt_cenetr" >
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="litRegionName" runat="server" Text='<%#Eval("ProductId")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="商品规格说明" ItemStyle-Width="80" ItemStyle-CssClass="td_txt_cenetr" >
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="litSkuId" runat="server" Text='<%#Eval("SkuId")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="购买数量" HeaderStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <%--<asp:TextBox ID="txtNum" runat="server" Text="" Width="80px" onblur='yzValue("<%# Eval("Stock") %>",this.value,this)' />--%>
                            <input name="txtNum" style="width:80px;" onblur="yzValue('<%# Eval("Stock") %>',this)"  />
                            <asp:HiddenField ID="hiValue" runat="server" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </UI:Grid>
            <div class="blank12 clearfix">
            </div>
        </div>
        <asp:Literal ID="litTitle" runat="server" Visible="false"></asp:Literal>
        <%--<div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>
        </div>--%>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript" src="producttag.helper.js"></script>
    <script type="text/javascript">
        
        function GetSkuId() {
            var v_str = "";

            $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
                v_str += $(rowItem).attr("value") + ",";
            });

            if (v_str.length == 0) {
                //alert("请选择商品");
                return "";
            }
            return v_str.substring(0, v_str.length - 1);
        }

        function CollectionProduct(url) {
            DialogFrame("product/" + url, "相关商品");
        }

        //在删除前先存储待删除的SkuId集合，CheckBox选择的行集合
        function doDelete() {
            $("#hiSkuIds").val(null);
            var skuIds = GetSkuId();
            if (skuIds.length > 0) {
                $("#hiSkuIds").val(skuIds);//存储到对象
            }
        }

        //验证是否允许提交到订货列表
        function doSubmit() {
            //var dropaddress = $("#dropaddress").find('option:selected').text();
            var dropaddress = $("#ctl00_contentHolder_userAddress").val();
            var dropgivemode = $("#ctl00_contentHolder_userGiveMode").val();
            var droppaymode = $("#ctl00_contentHolder_userPayMode").val();
            if (dropaddress == "") {
                alert("请选择送货地址");
                return false;
            }
            if (dropgivemode == "") {
                alert("请选择配送方式");
                return false;
            }
            if (droppaymode == "") {
                alert("请选择支付方式");
                return false;
            }
            return true;
        }
        
        //验证订货值是否大于库存值
        function yzValue(valuekc, input) {
            var kuNum = parseInt(valuekc);
            var dhNum = parseInt(input.value);
            if (dhNum > kuNum) {
                //验证失败
                alert("您输入的订货数量大于库存数量，请重新输入");
                input.focus();
            }
            else {
                //验证成功
                $(input).next().val(input.value);//设置在影藏域控件中
            }
        }

    </script>
</asp:Content>
