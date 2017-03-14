<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AgentOrders.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AgentOrders" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <script type="text/javascript">
        var goUrl = "<%=Reurl %>" + "&t=" + (new Date().getTime());
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="optiongroup mainwidth">
        <ul>
            <li id="anchorsAll">
                <asp:HyperLink ID="hlinkAllOrder" runat="server"><span>���ж���</span></asp:HyperLink></li>
            <li id="anchorsWaitBuyerPay">
                <asp:HyperLink ID="hlinkNotPay" runat="server" Text=""><span>�ȴ���Ҹ���</span></asp:HyperLink></li>
            <li id="anchorsBuyerAlreadyPaid">
                <asp:HyperLink ID="hlinkYetPay" runat="server" Text=""><span>�ȴ�����</span></asp:HyperLink></li>
            <li id="anchorsSellerAlreadySent">
                <asp:HyperLink ID="hlinkSendGoods" runat="server" Text=""><span>�ѷ���</span></asp:HyperLink></li>
            <li id="anchorsFinished">
                <asp:HyperLink ID="hlinkTradeFinished" runat="server" Text=""><span>�ɹ�����</span></asp:HyperLink></li>
            <li id="anchorsClosed">
                <asp:HyperLink ID="hlinkClose" runat="server" Text=""><span>�ѹر�</span></asp:HyperLink></li>
            <li id="anchorsHistory">
                <asp:HyperLink ID="hlinkHistory" runat="server" Text=""><span>��ʷ����</span></asp:HyperLink></li>
            <li id="anchorsDelete">
                <asp:HyperLink ID="hlinkDelete" runat="server" Text=""><span>��ɾ��</span></asp:HyperLink></li>
        </ul>
    </div>
    <!--ѡ�-->
    <div class="dataarea mainwidth">
        <!--����-->
        <%--<style>
            .searcharea ul li { padding: 5px 0px; }
        </style>--%>
        <div class="searcharea clearfix br_search search_titxt">
            <ul>
                <li><span>ʱ �� �Σ�</span>
                    <span><UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" /></span>
                    <span class="Pg_1010">��</span> 
                    <span><UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" /></span>

                </li>
                <li class="huan_hang"></li>
                <li><span>��Ա����</span><span>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="forminput" />
                </span></li>
                <li><span>������ţ�</span><span>
                    <asp:TextBox ID="txtOrderId" runat="server" CssClass="forminput" /><asp:Label ID="lblStatus"
                        runat="server" Style="display: none;"></asp:Label>
                </span></li>
                <li><span>��Ʒ���ƣ�</span><span>
                    <asp:TextBox ID="txtProductName" runat="server" CssClass="forminput" />
                </span></li>
                 <li style="display:<%=ViewState["StoreFiter"].ToString()=="1"?"":"none"%>">
                <span>&nbsp;&nbsp;�������ͣ�</span> 
                    <span>
                        <asp:DropDownList ID="ddlStoreType" runat="server" Width="100">
                            <asp:ListItem Text="ȫ��" Value=""></asp:ListItem>
                            <asp:ListItem Text="΢��" Value="΢��"></asp:ListItem>
                            <asp:ListItem Text="�ŵ�" Value="�ŵ�"></asp:ListItem>
                            <asp:ListItem Text="�" Value="�"></asp:ListItem>
                        </asp:DropDownList>
                    </span>
                </li>
                <li class="huan_hang"></li>
                <li><span>�ջ��ˣ�</span><span>
                    <asp:TextBox ID="txtShopTo" runat="server" CssClass="forminput"></asp:TextBox>
                </span></li>
                <li><span>��ӡ״̬��</span><span>
                    <abbr class="formselect">
                        <asp:DropDownList runat="server" ID="ddlIsPrinted" Width="107" />
                    </abbr>
                </span></li>
                <li><span>���ͷ�ʽ��</span><span>
                    <abbr class="formselect">
                        <Hi:ShippingModeDropDownList runat="server" AllowNull="true" ID="shippingModeDropDownList"
                            Width="100" />
                    </abbr>
                </span></li>
                <li>
                    <abbr class="formselect">
                        <Hi:RegionSelector runat="server" ID="dropRegion" />
                    </abbr>
                </li>
                <li style="display:none"><span>�������ࣺ</span><span>
                    <abbr class="formselect">
                         <asp:DropDownList ID="OrderFromList" runat="server" Width="107">
                         <asp:ListItem Text="����" Value="0"></asp:ListItem>
                                                  <asp:ListItem Text="��ͨ����" Value="1"></asp:ListItem>
                                                                           <asp:ListItem Text="�Ź�����" Value="2"></asp:ListItem>


                        </asp:DropDownList>
                    </abbr>
                </span></li>
                <li>
                    <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="��ѯ" />
                    <asp:Button ID="btnExport" runat="server" class="submit_queding" Text="����" />
                </li>
            </ul>
        </div>
        <!--����-->
        <div class="functionHandleArea clearfix m_none">
            <!--��ҳ����-->
            <div class="pageHandleArea">
                <ul>
                    <li class="paginalNum"><span>ÿҳ��ʾ������</span><UI:PageSize runat="server" ID="hrefPageSize" />
                    </li>
                </ul>
            </div>
            <div class="pageNumber">
                <div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
                </div>
            </div>
            <!--����-->
            <div class="blank8 clearfix">
            </div>
        </div>
        <input type="hidden" id="hidOrderId" runat="server" />
        <!--�����б�����-->
        <div class="datalist">
            <asp:DataList ID="dlstOrders" runat="server" DataKeyField="OrderId" Width="100%">
                <HeaderTemplate>
                    <table width="0" border="0" cellspacing="0">
                        <tr class="table_title">
                            <td width="20%" class="td_right td_left">
                                �ǳ�
                            </td>
                            <td width="20%" class="td_right td_left">
                                �ջ���
                            </td>
                            <td width="20%" class="td_right td_left">
                                ������Ϣ
                            </td>
                            <td width="110px" class="td_right td_left">
                                ֧����ʽ
                            </td>
                            <td width="14%" class="td_right td_left">
                                ����ʵ�տ�(Ԫ)
                            </td>
                            <td width="170px" class="td_right td_left">
                                ����״̬
                            </td>
                            <td width="170px" class="td_left td_right_fff">
                                ����
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="td_bg noboer_tr">
                        <td>
                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("OrderId") %>' />������ţ�<%#Eval("OrderId") %><asp:Literal
                                ID="group" runat="server" Text='<%#  Eval("GroupBuyId")!=DBNull.Value&& Convert.ToInt32(Eval("GroupBuyId"))>0?"(��)":"" %>' />
                        </td>
                        <td>
                            �ύʱ�䣺<Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("OrderDate") %>' ShopTime="true"
                                runat="server"></Hi:FormatedTimeLabel>
                        </td>
                        <td><!--������Ϣ-->
                            �����ŵ꣺<%#Eval("ModeName") %></td>
                        <td class="td_txt_cenetr" style="color:#666;">
                            <%# Eval("IsPrinted")!=DBNull.Value&&(bool)Eval("IsPrinted")?"�Ѵ�ӡ":"δ��ӡ" %>
                        </td>
                        <td style="color:#666;">
                        <%#Eval("ReferralUserId").ToString()=="" ? "������Դ����վ" : Eval("ReferralUserId").ToString()=="0" ? "������Դ����վ" : "������Դ��" + Eval("StoreName")%>
                        </td>
                        <td class="td_txt_right" style="color:#666;">
                            <%# String.IsNullOrEmpty(Eval("ShipOrderNumber").ToString()) ? "" : "�������ţ�" + Eval("ShipOrderNumber")%>
                        </td>
                        <td class="td_txt_cenetr">
                            <a href="javascript:RemarkOrder('<%#Eval("OrderId") %>','<%#Eval("OrderDate") %>','<%#Eval("OrderTotal") %>','<%#Eval("ManagerMark") %>','<%#Eval("ManagerRemark") %>');">
                                <Hi:OrderRemarkImage runat="server" DataField="ManagerMark" ID="OrderRemarkImageLink" /></a>
                        </td>
                    </tr>
                    <tr class="td_bg">
                        <td>
                            &nbsp; <a href="<%# Eval("UserId").ToString()!="1100"?"javascript:DialogFrame('"+Globals.GetAdminAbsolutePath(string.Format("/member/MemberDetails.aspx?userId={0}",Eval("UserId")))+"','�鿴��Ա',null,null)":"javascript:void(0)" %>">
                                �ǳƣ�<%#Eval("UserName")%></a><Hi:WangWangConversations runat="server" ID="WangWangConversations" WangWangAccounts='<%#Eval("Wangwang") %>' />
                            &nbsp;������<%#Eval("RealName") %></td>
                        <td>
                            �ռ��ˣ�<%#Eval("ShipTo") %>&nbsp;&nbsp;�绰��<%#Eval("CellPhone") %></td>
                        <td><!--������Ϣ-->
                            ��ַ��<%#Eval("Address") %></td>
                        <td style="color: #0B5BA5" class="td_txt_cenetr">
                            <%#Eval("PaymentType") %>
                        </td>
                        <td style="font-weight: bold; font-family: Arial;">
                            <Hi:FormatedMoneyLabel ID="lblOrderTotal" CssClass="money" Money='<%#Eval("OrderTotal") %>' runat="server" />
                            <span class="Name">
                               <%--�޸ļ۸�--%> 
                                <asp:HyperLink ID="lkbtnEditPrice" CssClass="edit_bi" runat="server" NavigateUrl='<%# "EditOrder.aspx?OrderId="+ Eval("OrderId") %>'   Text="�޸�" Visible="false"> </asp:HyperLink></span>
                        </td>
                        <td style="color: #000" class="td_txt_right">
                            &nbsp;
                            <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>'
                                runat="server" />
                            <span class="Name" style="display:<%=Hidistro.ControlPanel.Config.CustomConfigHelper.Instance.AutoShipping?"none":"" %>"><a class="link_default_blue" href='<%# "OrderDetails.aspx?OrderId="+Eval("OrderId") %>'>����</a></span>
                        </td>
                        <td class="td_txt_cenetr">
                            <asp:Button ID="Button8" CommandArgument='<%# Eval("OrderId") %>'  CommandName="PAY" runat="server" Text="����" />
                            
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:DataList>
            
            <div class="blank5 clearfix">
            </div>
        </div>
        <!--�����б�ײ���������-->
        <div class="page">
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
    </div>
    <!--�رն���--->
    <div id="closeOrder" style="display: none;">
        <div class="frame-content">
            <p>
                <em>�رս���?����ȷ���Ѿ�֪ͨ���,���Ѵ��һ�����,��������رս���,�����ܵ��½��׾���</em></p>
            <p>
                <span class="frame-span frame-input110">�رոö���������:</span>
                <Hi:CloseTranReasonDropDownList runat="server" ID="ddlCloseReason" />
            </p>
        </div>
    </div>
    <!--�༭��ע--->
    <style>
        .frame-content { margin-top: -20px; }
    </style>
    <div id="RemarkOrder" style="display: none;">
        <div class="frame-content">
            <p>
                <span class="frame-span frame-input100">�����ţ�</span><span id="spanOrderId"></span></p>
            <p>
                <span class="frame-span frame-input100">�ύʱ�䣺</span><span id="lblOrderDateForRemark"></span></p>
            <p>
                <span class="frame-span frame-input100">����ʵ�տ�(Ԫ)��</span><strong class="colorA"><Hi:FormatedMoneyLabel
                    ID="lblOrderTotalForRemark" runat="server" /></strong></p>
            <span class="frame-span frame-input100">��־��<em>*</em></span><Hi:OrderRemarkImageRadioButtonList
                runat="server" ID="orderRemarkImageForRemark" />
            <p>
                <span>����¼��</span><asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server"
                    Width="300" Height="50" /></p>
        </div>
    </div>
    <div id="DownOrder" style="display: none;">
        <div class="frame-content" style="text-align: center;">
            <input type="button" id="btnorderph" onclick="javascript: Setordergoods();" class="submit_DAqueding"
                value="���������" />
            &nbsp;
            <input type="button" id="Button1" onclick="javascript: Setproductgoods();" class="submit_DAqueding"
                value="��Ʒ�����" />
            <p>
                ��������ֻ�����ȴ�����״̬�Ķ���</p>
            <p>
                �����������ϲ���ͬ����Ʒ,��Ʒ��������ϲ���ͬ����Ʒ��</p>
        </div>
    </div>
    <!--ȷ���˿�--->
    <div id="CheckRefund" style="display: none;">
        <div class="frame-content">
            <p>
                <em>ִ�б�����ǰȷ����<br />
                    1.����Ѹ�����ɣ���ȷ������ 2.ȷ����ҵ������˿ʽ��</em></p>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
                <tr>
                    <td align="right" width="30%">
                        ������:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="lblOrderId" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        �������:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="lblOrderTotal" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ����˿ʽ:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="lblRefundType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        �˿�ԭ��:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="lblRefundRemark" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ��ϵ��:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="lblContacts" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        �����ʼ�:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="lblEmail" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ��ϵ�绰:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="lblTelephone" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ��ϵ��ַ:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="lblAddress" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <p>
                <span class="frame-span frame-input100" style="margin-right: 10px;">����Ա��ע:</span>
                <span>
                    <asp:TextBox ID="txtAdminRemark" runat="server" CssClass="forminput" Width="243" /></span></p>
            <br />
            <div style="text-align: center;">
                <input type="button" id="Button2" onclick="javascript: acceptRefund();" class="submit_DAqueding"
                    value="ȷ���˿�" />
                &nbsp;
                <input type="button" id="Button3" onclick="javascript: refuseRefund();" class="submit_DAqueding"
                    value="�ܾ��˿�" />
            </div>
        </div>
    </div>
    <!--ȷ���˻�--->
    <div id="CheckReturn" style="display: none;">
        <div class="frame-content">
            <p>
                <em>ִ�б�����ǰȷ����<br />
                    1.���յ���ҼĻ������Ļ�Ʒ����ȷ������ 2.ȷ����ҵ������˿ʽ��</em></p>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
                <tr>
                    <td align="right" width="30%">
                        ������:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="return_lblOrderId" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        �������:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="return_lblOrderTotal" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ����˿ʽ:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="return_lblRefundType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        �˻�ԭ��:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="return_lblReturnRemark" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ��ϵ��:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="return_lblContacts" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        �����ʼ�:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="return_lblEmail" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ��ϵ�绰:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="return_lblTelephone" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ��ϵ��ַ:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="return_lblAddress" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        �˿���:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:TextBox ID="return_txtRefundMoney" runat="server" />
                    </td>
                </tr>
            </table>
            <p>
                <span class="frame-span frame-input100" style="margin-right: 10px;">����Ա��ע:</span>
                <span>
                    <asp:TextBox ID="return_txtAdminRemark" runat="server" CssClass="forminput" Width="243" /></span></p>
            <br />
            <div style="text-align: center;">
                <input type="button" id="Button4" onclick="javascript: acceptReturn();" class="submit_DAqueding"
                    value="ȷ���˻�" />
                &nbsp;
                <input type="button" id="Button5" onclick="javascript: refuseReturn();" class="submit_DAqueding"
                    value="�ܾ��˻�" />
            </div>
        </div>
    </div>
    <!--ȷ�ϻ���--->
    <div id="CheckReplace" style="display: none;">
        <div class="frame-content">
            <p>
                <em>ִ�б�����ǰȷ����<br />
                    1.���յ���ҼĻ������Ļ�Ʒ����ȷ������ </em>
            </p>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
                <tr>
                    <td align="right" width="30%">
                        ������:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="replace_lblOrderId" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        �������:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="replace_lblOrderTotal" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ������ע:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="replace_lblComments" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ��ϵ��:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="replace_lblContacts" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        �����ʼ�:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="replace_lblEmail" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ��ϵ�绰:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="replace_lblTelephone" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ��ϵ��ַ:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="replace_lblAddress" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ��������:
                    </td>
                    <td align="left" class="bd_td">
                        &nbsp;<asp:Label ID="replace_lblPostCode" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <p>
                <span class="frame-span frame-input100" style="margin-right: 10px;">����Ա��ע:</span>
                <span>
                    <asp:TextBox ID="replace_txtAdminRemark" runat="server" CssClass="forminput" Width="243" /></span></p>
            <br />
            <div style="text-align: center;">
                <input type="button" id="Button6" onclick="javascript: acceptReplace();" class="submit_DAqueding"
                    value="ȷ�ϻ���" />
                &nbsp;
                <input type="button" id="Button7" onclick="javascript: refuseReplace();" class="submit_DAqueding"
                    value="�ܾ�����" />
            </div>
        </div>
    </div>
    <div id="printDiv" style="display:none"></div>
    <div style="display: none">
        <input type="hidden" id="hidOrderTotal" runat="server" />
        <input type="hidden" id="hidRefundType" runat="server" />
        <input type="hidden" id="hidRefundMoney" runat="server" />
        <input type="hidden" id="hidAdminRemark" runat="server" />
        <input type="hidden" id="specialHideShow" runat="server" clientidmode="Static" />
        <asp:Button ID="btnCloseOrder" runat="server" CssClass="submit_DAqueding" Text="�رն���" />
        <asp:Button ID="btnAcceptRefund" runat="server" CssClass="submit_DAqueding" Text="ȷ���˿�" />
        <asp:Button ID="btnRefuseRefund" runat="server" CssClass="submit_DAqueding" Text="�ܾ��˿�" />
        <asp:Button ID="btnAcceptReturn" runat="server" CssClass="submit_DAqueding" Text="ȷ���˻�" />
        <asp:Button ID="btnRefuseReturn" runat="server" CssClass="submit_DAqueding" Text="�ܾ��˻�" />
        <asp:Button ID="btnAcceptReplace" runat="server" CssClass="submit_DAqueding" Text="ȷ�ϻ���" />
        <asp:Button ID="btnRefuseReplace" runat="server" CssClass="submit_DAqueding" Text="�ܾ�����" />
        <asp:Button runat="server" ID="btnRemark" Text="�༭��ע��Ϣ" CssClass="submit_DAqueding" />
        <asp:Button ID="btnOrderGoods" runat="server" CssClass="submit_DAqueding" Text="���������" />&nbsp;
        <asp:Button runat="server" ID="btnProductGoods" Text="��Ʒ�����" CssClass="submit_DAqueding" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        var formtype = "";

        function ConfirmPayOrder() {
            return confirm("����ͻ��Ѿ�ͨ������;��֧���˶������������ʹ�ô˲����޸Ķ���״̬\n\n�˲����ɹ�����Ժ󣬶����ĵ�ǰ״̬����Ϊ�Ѹ���״̬��ȷ�Ͽͻ��Ѹ��");
        }

        function ShowOrderState() {
            var status;
            if (navigator.appName.indexOf("Explorer") > -1) {

                status = document.getElementById("ctl00_contentHolder_lblStatus").innerText;

            } else {

                status = document.getElementById("ctl00_contentHolder_lblStatus").textContent;

            }
            if (status != "0") {
                //document.getElementById("anchors0").className = 'optionstar';
            }
            if (status != "99") {
                //document.getElementById("anchors99").className = 'optionend';
            }
            //alert(document.getElementById("anchors" + status));
            document.getElementById("anchors" + status).className = 'menucurrent';
        }

        $(document).ready(function () {
            ShowOrderState();
            $(".link_default_gray[name=link_default_gray]").each(function () {
                if ($(this).html().replace(/\s+/g, "") == "") {
                    $(this).hide();
                    // $(this).attr("display", "none");

                }
            });
        });


        //��ע��Ϣ
        function RemarkOrder(OrderId, OrderDate, OrderTotal, managerMark, managerRemark) {
            arrytext = null;
            formtype = "remark";
            $("#ctl00_contentHolder_lblOrderTotalForRemark").html(OrderTotal);
            $("#ctl00_contentHolder_hidOrderId").val(OrderId);
            $("#spanOrderId").html(OrderId);
            $("#lblOrderDateForRemark").html(OrderDate);




            for (var i = 0; i <= 5; i++) {
                if (document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).value == managerMark) {
                    setArryText("ctl00_contentHolder_orderRemarkImageForRemark_" + i, "true");
                    $("#ctl00_contentHolder_orderRemarkImageForRemark_" + i).attr("check", true);
                }
                else {
                    $("#ctl00_contentHolder_orderRemarkImageForRemark_" + i).attr("check", false);
                }

            }
            setArryText("ctl00_contentHolder_txtRemark", managerRemark);
            DialogShow("�޸ı�ע", 'updateremark', 'RemarkOrder', 'ctl00_contentHolder_btnRemark');
        }

        function CloseOrder(orderId) {
            arrytext = null;
            formtype = "close";
            $("#ctl00_contentHolder_hidOrderId").val(orderId);
            DialogShow("�رն���", 'closeframe', 'closeOrder', 'ctl00_contentHolder_btnCloseOrder');
        }

        function ValidationCloseReason() {
            var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
            if (reason == "��ѡ��رյ�����") {
                alert("��ѡ��رյ�����");
                return false;
            }
            setArryText("ctl00_contentHolder_ddlCloseReason", reason);
            return true;
        }

        //������ӡ������
        function printGoods() {
            var OrderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                OrderIds += $(this).val() + ",";
            });
            OrderIds = OrderIds.substr(0, OrderIds.length - 1);
            var orderList = OrderIds.split(',');
            if (OrderIds == "") {
                alert("��ѡҪ��ӡ�Ķ���");
            }
            else {
                if ($("#specialHideShow").val() == "sswk") {//ˬˬ�ηȶ����ӡ��ʽ
                    for (var i = 0; i < orderList.length; i++) {
                        $.ajax({
                            type: "POST",   //����WebServiceʹ��Post��ʽ����
                            contentType: "application/json", //WebService �᷵��Json����
                            url: "ManageOrder.aspx/PrintOrderInfo", //����WebService�ĵ�ַ�ͷ���������� ---- WsURL/������
                            data: "{OrderIds:'" + orderList[i] + "'}",         //������Ҫ���ݵĲ�������ʽΪ data: "{paraName:paraValue}",���潫�ῴ��      
                            dataType: 'json',
                            success: function (result) {     //�ص�������result������ֵ
                                $("#printDiv").html(result.d);
                                batchPrintData();
                                batchPrintData();
                            }
                        });
                    }
                } else {//Ĭ�ϴ�ӡ��ʽ
                    var url = "/Admin/sales/BatchPrintSendOrderGoods.aspx?OrderIds=" + orderIds;
                    for (var i = 0; i < 2; i++) {
                        window.open(url, "������ӡ������", "width=700, top=0, left=0, toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=n o, status=no");
                    }
                }
            }
        }

        function getNowFormatDate() {
            var date = new Date();
            var seperator1 = "-";
            var seperator2 = ":";
            var month = date.getMonth() + 1;
            var strDate = date.getDate();
            if (month >= 1 && month <= 9) {
                month = "0" + month;
            }
            if (strDate >= 0 && strDate <= 9) {
                strDate = "0" + strDate;
            }
            var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
                    + " " + date.getHours() + seperator2 + date.getMinutes()
                    + seperator2 + date.getSeconds();
            return currentdate;
        }
        //�սᵥ��ӡ
        function todayOrderPrint() {
            var d = new Date();
            var startDate = GetQueryString("StartDate") != null ? GetQueryString("StartDate").replace("+", " ") : (d.getFullYear() + "-0" + (d.getMonth() + 1) + "-0" + d.getDate() + " 0:00:00");
            var endDate = GetQueryString("EndDate") != null ? GetQueryString("EndDate").replace("+", " ") : (d.getFullYear() + "-0" + (d.getMonth() + 1) + "-0" + d.getDate() + " 23:59:59");
            /*
            if (startDate == "" || endDate == "") {
                alert("��ѡ��ʱ��Σ�"); return false;
            }
            */
            $.ajax({
                type: "POST",   //����WebServiceʹ��Post��ʽ����
                contentType: "application/json", //WebService �᷵��Json����
                url: "ManageOrder.aspx/todayOrderPrint", //����WebService�ĵ�ַ�ͷ���������� ---- WsURL/������
                data: "{startDate:'" + startDate + "',endDate:'" + endDate + "'}",         //������Ҫ���ݵĲ�������ʽΪ data: "{paraName:paraValue}",���潫�ῴ��      
                dataType: 'json',
                success: function (result) {     //�ص�������result������ֵ
                    $("#printDiv").show();
                    $("#printDiv").html(result.d);
                    //return;
                    batchPrintData();

                }
            });

        }

        //��ӡ
        function batchPrintData() {
            var LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
            try {
                LODOP.PRINT_INIT("��ӡ����");
                LODOP.SET_PRINT_PAGESIZE(3, 710, $("#printDiv").height(), "");
                LODOP.ADD_PRINT_HTM(0, 0, 710, $("#printDiv").height(), $("#printDiv").html());
                LODOP.PRINT();
            } catch (e) {
                alert("���Ȱ�װ��ӡ�ؼ���");
                return false;
            }
            location.reload();
        }

        //��������
        function batchSend() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("��ѡҪ�����Ķ���");
            }
            else {
                DialogFrame("sales/BatchSendOrderGoods.aspx?OrderIds=" + orderIds, "��������", null, null, function () { location.reload(); });
            }
        }
        function Setordergoods() {
            $("#ctl00_contentHolder_btnOrderGoods").trigger("click");
        }
        function Setproductgoods() {
            $("#ctl00_contentHolder_btnProductGoods").trigger("click");
        }
        //������ӡ��ݵ�
        function printPosts() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("��ѡҪ��ӡ�Ķ���");
            }
            else {
                var url = "sales/BatchPrintData.aspx?OrderIds=" + orderIds;
                DialogFrame(url, "������ӡ��ݵ�", null, null);
            }
        }

        //��֤
        function validatorForm() {
            switch (formtype) {
                case "remark":
                    arrytext = null;
                    $radioId = $("input[type='radio'][name='ctl00$contentHolder$orderRemarkImageForRemark']:checked")[0];
                    if ($radioId == null || $radioId == "undefined") {
                        alert('���ȱ�Ǳ�ע');
                        return false;
                    }
                    setArryText($radioId.id, "true");
                    setArryText("ctl00_contentHolder_txtRemark", $("#ctl00_contentHolder_txtRemark").val());
                    break;
                case "shipptype":
                    return ValidationShippingMode();
                    break;
                case "close":
                    return ValidationCloseReason();
                    break;
            };
            return true;
        }
        // ���������
        function downOrder() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                alert("��ѡҪ����������Ķ���");
            }
            else {
                ShowMessageDialog("����������α�", "downorder", "DownOrder");
            }
        }
        $(function () {
            $(".datalist img[src$='tui.gif']").each(function (item, i) {
                $parent_link = $(this).parent();
                $parent_link.attr("href", "javascript:DialogFrame('sales/" + $parent_link.attr("href") + "','�˿���ϸ��Ϣ',null,null);");
            });

            //
        });

        $(function () {
            //��Բ�ͬ���û����в�ͬ�Ĺ�������������ʾ
            var customName = "";
            if ($("#specialHideShow").val()) {
                customName = $("#specialHideShow").val();
                switch (customName) {
                    case "sswk":
                        //���ε�������ӡ��ݵ���ť,�����������ť
                        $(".printorder").hide();
                        $(".downproduct").hide();
                        //������ӡ��������ť������

                        break;
                }
            }
        });

        function GetQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

    </script>
</asp:Content>
