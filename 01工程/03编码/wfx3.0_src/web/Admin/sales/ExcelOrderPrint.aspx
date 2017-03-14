<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExcelOrderPrint.aspx.cs" MasterPageFile="~/Admin/Admin.Master" Inherits="Admin_sales_ExcelOrderPrint" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <div class="td_txt_left">
                <em>
                    <img src="../images/01.gif" width="32" height="32" /></em>
            </div>
            <h1 class="td_txt_left">Excel订单同步</h1>
            <div class="td_txt_left">
                <span>导入Excel订单表格(点击上传文件后选择订单导入【白色表示通过，暗色表示未通过，红色表示问题字符】验证通过后即可同步数据。)</span>
            </div>
        </div>
    </div>
    <!--选项卡-->
    <div class="dataarea mainwidth">
        <div class="functionHandleArea clearfix m_none">
            <!--结束-->

            <div class="batchHandleArea">
                <ul>
                    <li class="batchHandleButton" style="width: 100%; margin-top: 10px;">
                        <span style="position: relative; line-height: 25px; overflow: hidden;" id="uploadArea" >
                            <asp:FileUpload ID="fileUpload" runat="server" Style="position: absolute; left: 0; top: 0; opacity: 0; filter: alpha(opacity=0); -webkit-opacity: 0; z-index: 1;" ClientIDMode="Static" />
                            点击这里上传文件</span>
                        <span class="sendproducts" id="unpackArea">
                            <asp:Button runat="server" ID="btnUnPack" Text="订单导入" Style="display: block; background: transparent; line-height: 25px; border: none; outline: none; color: #fff;" ClientIDMode="Static"></asp:Button></span>
                        <span class="sendproducts" id="syncArea">
                            <asp:Button runat="server" ID="btDS" Text="数据同步" Style="display: block; background: transparent; line-height: 25px; border: none; outline: none; color: #fff;" CausesValidation="False" UseSubmitBehavior="False"></asp:Button></span>
                        <span class="sendproducts" style="position:absolute; right:10%;" >
                            <asp:Button runat="server" ID="btnExcelDownLoad" Text="模板下载" Style="display: block; background: transparent; line-height: 25px; border: none; outline: none; color: #fff;" ToolTip="下载Excel样本表格" OnClick="btnExcelDownLoad_Click"></asp:Button></span>
                    </li>
                    <li>
                        <br /><span>渠道商选择:</span><span><asp:DropDownList ID="ddlChannelList" runat="server"></asp:DropDownList></span>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <asp:Repeater ID="repeateExcel" runat="server">
        <HeaderTemplate>
            <table width="0" border="0" cellspacing="0">
                <tr class="table_title">
                    <td style="width:2%;" class="td_right">排序
                    </td>
                    <td style="width:4%;" class="td_right">原始订单编号
                    </td>
                    <td style="width:4%;" class="td_right">商品编号
                    </td>
                    <td style="width:3%;" class="td_right">商品数量
                    </td>
                    <td style="width:4%;" class="td_right ">成本价
                    </td>
                    <td style="width:4%;" class="td_right">商品单价
                    </td>
                    <td style="width:5%;" class="td_right">商品描述
                    </td>
                    <%--<td style="width:5%;" class="td_right ">商品重量
                    </td>--%>
                    
                    <td style="width:5%;" class="td_right">订单产生时间
                    </td>
                    <td style="width:5%;" class="td_right">付款时间
                    </td>
                    <td style="width:5%;" class="td_right">发货时间
                    </td>
                    <td style="width:5%;" class="td_right">收货时间
                    </td>
                    <td style="width:5%;" class="td_right">收货人姓名
                    </td>
                    <td style="width:5%;" class="td_right">收货人手机
                    </td>
                    <td style="width:5%;" class="td_right">备注
                    </td>
                    <td style="width:5%;" class="td_right">物流公司
                    <td style="width:5%;" class="td_right ">送货地区
                    </td>
                    <td style="width:5%;" class="td_right ">详细地址
                    </td>
                    <td style="width:9%;" class="td_right ">错误列表
                    </td>
                </tr>
        </HeaderTemplate>

        <ItemTemplate>
            <tr class="td_bg noboer_tr">
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <%# Container.ItemIndex + 1%> 
                </td>
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("原始订单编号")>-1?"color:red": "" %>"><%#Eval("原始订单编号") %></span>
                </td>
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("商品编号")>-1?"color:red": "" %>"><%#Eval("商品编号") %></span>
                </td>
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("商品数量")>-1?"color:red": "" %>"><%#Eval("商品数量") %></span>
                </td>
               
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("成本价")>-1?"color:red": "" %>"><%#Eval("成本价") %></span>
                </td>
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("商品单价")>-1?"color:red": "" %>"><%#Eval("商品单价") %></span>
                </td>
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("商品描述")>-1?"color:red": "" %>"><%#Eval("商品描述") %></span>
                </td>
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("订单产生时间")>-1?"color:red": "" %>"><%#Eval("订单产生时间") %></span>
                </td>
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("付款时间")>-1?"color:red": "" %>"><%#Eval("付款时间") %></span>
                </td>
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("发货时间")>-1?"color:red": "" %>"><%#Eval("发货时间") %></span>
                </td>
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("收货时间")>-1?"color:red": "" %>"><%#Eval("收货时间") %></span>
                </td>
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("收货人姓名")>-1?"color:red": "" %>"><%#Eval("收货人姓名") %></span>
                </td>
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("收货人手机")>-1?"color:red": "" %>"><%#Eval("收货人手机") %></span>
                </td>
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("备注")>-1?"color:red": "" %>"><%#Eval("备注") %></span>
                </td>
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("物流公司")>-1?"color:red": "" %>"><%#Eval("物流公司") %></span>
                </td>
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("送货地区")>-1?"color:red": "" %>"><%#Eval("送货地区") %></span>
                </td>

                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span style="<%#Eval("errorFields").ToString().IndexOf("详细地址")>-1?"color:red": "" %>"><%#Eval("详细地址") %></span>
                </td>
                <td style='background-color: <%#Eval("errorInfo").ToString().Count()>0?"#e1d7d7":" "%>;' class="td_txt_cenetr">
                    <span><%# Eval("errorInfo").ToString().Length>30?Eval("errorInfo").ToString().Substring(0,30)+"...":Eval("errorInfo").ToString()%></span>
                </td>
            </tr>
        </ItemTemplate>

        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>

    <script type="text/javascript">
        $(function () {
            var $btnUpload = $("#uploadArea");
            var $btnUnpack = $("#unpackArea");
            var $btnSync = $("#syncArea");
            if ($("table").find("tr").length < 2) {
                if ($btnUpload.val() == "") {
                    $btnSync.hide();
                }

                $("#fileUpload")[0].onchange = function () {
                    if ($(this).val()!= "") {
                        $btnUpload.show();
                        $btnUnpack.show();
                        
                    }
                };
            }
            else {
                $btnSync.show();
            }

        });

    </script>
</asp:Content>

