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
            <h1>�������µ�</h1>
            <span>�Ѷ�����������Ʒ�������Զ���Ʒ�����µ�����</span>
        </div>
        <div class="datalist">
            <!--����-->
            <div class="clearfix search_titxt2">
            <div class="searcharea clearfix" style="padding: 3px 0px 10px 0px;">
                <ul>
                    <li><span>��ʾ��������ͻ���ַѡ�����¼΢����Ʒ���õ�ַ�����Ҫ����֧����ʽ����ϵ����Ա��</span></li>
                </ul>
            </div>
            <div class="searcharea clearfix" style="padding: 10px 0px 3px 0px;">
                <ul>
                    <li>
                        <span><span style="color:red">*</span>�ͻ���ַ��</span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="userAddress" runat="server"></asp:DropDownList>
                        </abbr>
                    </li>
                    <li>
                        <span><span style="color:red">*</span>���ͷ�ʽ��</span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="userGiveMode" runat="server"></asp:DropDownList>
                        </abbr>
                    </li>
                    <li>
                        <span><span style="color:red">*</span>֧����ʽ��</span>
                        <abbr class="formselect">
                            <asp:DropDownList ID="userPayMode" runat="server"></asp:DropDownList>
                        </abbr>
                    </li>
                </ul>
            </div>
            <div class="searcharea clearfix" style="padding: 3px 0px 10px 0px;">
                <ul>
                    <li><span style="margin-left:5px;">����˵����</span><span><asp:TextBox ID="txtOrderRemark" runat="server" Width="400" MaxLength="200" CssClass="forminput" /></span></li>
                    <%--<li><asp:Button ID="btnSearch" runat="server" Text="��ѯ" CssClass="searchbutton" /></li>--%>
                </ul>
            </div>
            </div>
               
            <!--����-->
            <div class="functionHandleArea clearfix">
                <!--����-->
                <div class="blank8 clearfix">
                </div>
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton"><span class="signicon"></span>
                            <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">ȫѡ</a></span> 
                            <span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">��ѡ</a></span> 
                            <span class="delete"><Hi:ImageLinkButton ID="btnDelete" runat="server" Text="ɾ��" IsShow="true" DeleteMsg="ȷ��Ҫ����Ʒ�Ƴ������б���" /></span>
                             <%--OnClientClick="doDelete();"--%>
                            <asp:HiddenField ID="hiSkuIds" runat="server" ClientIDMode="Static" />
                        </li>
                    </ul>
                </div>
                <div class="filterClass">
                    <asp:Button ID="btnBuy" runat="server" Text="���ɶ���" CssClass="submit_DAqueding inbnt" OnClientClick="return doSubmit();" />
                </div>
            </div>
            <!--�����б�����-->
            <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true"
                GridLines="None" DataKeyNames="SkuId" SortOrder="Desc" AutoGenerateColumns="false"
                HeaderStyle-CssClass="table_title" CssClass="goods-list">
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <Columns>
                    <asp:TemplateField ItemStyle-Width="30px" HeaderText="ѡ��" ItemStyle-CssClass="td_txt_cenetr">
                        <ItemTemplate>
                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("SkuId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="����" DataField="DisplaySequence" ItemStyle-Width="35px"
                       ItemStyle-CssClass="td_txt_cenetr" />
                    <asp:TemplateField ItemStyle-Width="46%" HeaderText="��Ʒ" HeaderStyle-CssClass="td_right td_left">
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
                                    <%# Eval("ProductName") %></a></span> <span class="colorC" style="display:block">�̼ұ��룺<%# Eval("ProductCode") %>
                                        ��棺<%# Eval("Stock") %>
                                        �ɱ���<%# Eval("CostPrice", "{0:f2}")%>
                                    </span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="22%" HeaderText="��Ʒ�۸�" >
                        <ItemTemplate>
                            <span class="Name">
                                ����:<asp:Literal ID="litSalePrice" runat="server" Text='<%#Eval("SalePrice", "{0:f2}")%>'></asp:Literal>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="��Ʒ״̬" ItemStyle-Width="80" ItemStyle-CssClass="td_txt_cenetr" >
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="litSaleStatus" runat="server" Text='<%#Eval("SaleStatus")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="��Ʒ��������" ItemStyle-Width="80" ItemStyle-CssClass="td_txt_cenetr" >
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="litRegionName" runat="server" Text='<%#Eval("ProductId")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="��Ʒ���˵��" ItemStyle-Width="80" ItemStyle-CssClass="td_txt_cenetr" >
                        <ItemTemplate>
                            <span>
                                <asp:Literal ID="litSkuId" runat="server" Text='<%#Eval("SkuId")%>'></asp:Literal></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="��������" HeaderStyle-Width="80px" HeaderStyle-CssClass="td_right td_left">
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
                //alert("��ѡ����Ʒ");
                return "";
            }
            return v_str.substring(0, v_str.length - 1);
        }

        function CollectionProduct(url) {
            DialogFrame("product/" + url, "�����Ʒ");
        }

        //��ɾ��ǰ�ȴ洢��ɾ����SkuId���ϣ�CheckBoxѡ����м���
        function doDelete() {
            $("#hiSkuIds").val(null);
            var skuIds = GetSkuId();
            if (skuIds.length > 0) {
                $("#hiSkuIds").val(skuIds);//�洢������
            }
        }

        //��֤�Ƿ������ύ�������б�
        function doSubmit() {
            //var dropaddress = $("#dropaddress").find('option:selected').text();
            var dropaddress = $("#ctl00_contentHolder_userAddress").val();
            var dropgivemode = $("#ctl00_contentHolder_userGiveMode").val();
            var droppaymode = $("#ctl00_contentHolder_userPayMode").val();
            if (dropaddress == "") {
                alert("��ѡ���ͻ���ַ");
                return false;
            }
            if (dropgivemode == "") {
                alert("��ѡ�����ͷ�ʽ");
                return false;
            }
            if (droppaymode == "") {
                alert("��ѡ��֧����ʽ");
                return false;
            }
            return true;
        }
        
        //��֤����ֵ�Ƿ���ڿ��ֵ
        function yzValue(valuekc, input) {
            var kuNum = parseInt(valuekc);
            var dhNum = parseInt(input.value);
            if (dhNum > kuNum) {
                //��֤ʧ��
                alert("������Ķ����������ڿ������������������");
                input.focus();
            }
            else {
                //��֤�ɹ�
                $(input).next().val(input.value);//������Ӱ����ؼ���
            }
        }

    </script>
</asp:Content>
