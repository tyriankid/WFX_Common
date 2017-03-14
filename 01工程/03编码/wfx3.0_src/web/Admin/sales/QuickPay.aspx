<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="QuickPay.aspx.cs" Inherits="Hidistro.UI.Web.Admin.QuickPay" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server"> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
      <div class="title"> <em><img src="../images/22.jpg" width="32" height="32" /></em>
        <h1>快速收银</h1>
        <span>扫描SN码即可快速完成收银流程</span>
      </div>
      <div class="datafrom">
          <div class="formitem validator1">
              <ul>
            <li ><span class="formitemtitle Pw_198"><em >*</em>在输入框内扫描SN码：</span>
              <input type="text" ID="snCode"  style="height:30px" />
            </li>
            
              </ul>
              <ul class="btntf Pa_198">
                    <asp:Button ID="btnOK" runat="server" style="display:none" Text="提 交" OnClientClick="return false" CssClass="submit_DAqueding inbnt"  /> 
	          </ul>
              
          </div>
      </div>
    <asp:Button runat="server" ID="btnSuccess" Text="触发后台方法,绑定列表数据" onclick="btnSuccess_Click" style="display:none" />
        
    <asp:Button runat="server" ID="btnSubmit" Text="提    交"  OnClick="btnSubmit_Click" OnClientClick="return check()" CssClass="submit_DAqueding inbnt" />
    <!--数据列表区域-->
        <div class="datalist">
            <asp:DataList ID="dlstOrders" runat="server" DataKeyField="OrderId" Width="100%">
                <HeaderTemplate>
                    <table width="0" border="0" cellspacing="0">
                        <tr class="table_title">
                            <td width="20%" class="td_right td_left">
                                昵称
                            </td>
                            <td width="20%" class="td_right td_left">
                                收货人
                            </td>
                            <td width="110px" class="td_right td_left">
                                支付方式
                            </td>
                            <td width="14%" class="td_right td_left">
                                订单实收款(元)
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="td_bg noboer_tr">
                        <td>
                           订单编号：<%#Eval("OrderId") %><asp:Literal
                                ID="group" runat="server" Text='<%#  Eval("GroupBuyId")!=DBNull.Value&& Convert.ToInt32(Eval("GroupBuyId"))>0?"(团)":"" %>' />
                        </td>
                        <td>
                            提交时间：<Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("OrderDate") %>' ShopTime="true"
                                runat="server"></Hi:FormatedTimeLabel>
                        </td>
                        <td class="td_txt_cenetr" style="color:#666;">
                            <%# Eval("IsPrinted")!=DBNull.Value&&(bool)Eval("IsPrinted")?"已打印":"未打印" %>
                        </td>
                        <td style="color:#666;">
                        <%#Eval("ReferralUserId").ToString()=="" ? "订单来源：主站" : Eval("ReferralUserId").ToString()=="0" ? "订单来源：主站" : "订单来源：" + Eval("StoreName")%>
                        </td>
                    </tr>
                    <tr class="td_bg">
                        <td>
                            &nbsp; <a href="<%# Eval("UserId").ToString()!="1100"?"javascript:DialogFrame('"+Globals.GetAdminAbsolutePath(string.Format("/member/MemberDetails.aspx?userId={0}",Eval("UserId")))+"','查看会员',null,null)":"javascript:void(0)" %>">
                                <%#Eval("UserName")%></a>
                            <Hi:WangWangConversations runat="server" ID="WangWangConversations" WangWangAccounts='<%#Eval("Wangwang") %>' />
                        </td>
                        <td>
                            <%#Eval("ShipTo") %>&nbsp;
                        </td>
                        <td style="color: #0B5BA5" class="td_txt_cenetr">
                            <%#Eval("PaymentType") %>
                        </td>
                        <td style="font-weight: bold; font-family: Arial;">
                            <Hi:FormatedMoneyLabel ID="lblOrderTotal" CssClass="money" Money='<%#Eval("OrderTotal") %>' runat="server" />
                            <span class="Name">
                               <%--修改价格--%> 
                                <asp:HyperLink ID="lkbtnEditPrice" CssClass="edit_bi" runat="server" NavigateUrl='<%# "EditOrder.aspx?OrderId="+ Eval("OrderId") %>'   Text="修改" Visible="false"> </asp:HyperLink></span>
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


</div>  
<script type="text/javascript">


    $(function () {
        $("#snCode").focus();
        $("#snCode").keypress(
            function () {
                if (event.keyCode == 13) 
                    regInput();
            });
    });


    function regInput() {
        if ($("#snCode").val().trim().length == 15) {
            var orderId = $("#snCode").val();
            $.ajax({
                type: "POST",   //访问WebService使用Post方式请求
                contentType: "application/json", //WebService 会返回Json类型
                url: "QuickPay.aspx/AjaxServiceTest", //调用WebService的地址和方法名称组合 ---- WsURL/方法名
                data: "{str:'"+orderId+"'}",         //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到       
                dataType: 'json',
                success: function (result) {     //回调函数，result，返回值
                    //alert(result.d);
                    if (result.d == "success") {
                        $("#ctl00_contentHolder_btnSuccess").click();
                        //document.getElementById("ctl00_contentHolder_btnSuccess").click;
                    }
                    else if (result.d == "fail") {
                        alert("该订单不存在或状态不正确!");
                        $("#snCode").val("");
                        $("#snCode").focus();
                    }
                }
            });
        }
        else {
            return false;
        }

    }

    function check() {
        
        if ($("#ctl00_contentHolder_dlstOrders")[0]== null) {
            
            alert("请先扫码!");
            return false;
        }
        if(!confirm("确认订单信息无误吗?"))
        return false;
    }


</script>
</asp:Content>

