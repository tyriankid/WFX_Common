<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetOrderOption.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SetOrderOption" %>
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
      <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
        <h1>订单设置</h1>
        <span>对订单管理配置</span>
      </div>
      <div class="datafrom">
          <div class="formitem validator1">
           <ul>
            <li style="display:none;"><span class="formitemtitle Pw_198"><em >*</em>显示几天内订单数：</span>
              <asp:TextBox ID="txtShowDays" runat="server" CssClass="forminput" />
              <p id="txtShowDaysTip" runat="server">前台发货查询中显示最近几天内的订单项</p>
            </li>
            <!--特殊订单运费规则start-->
            <li><span class="formitemtitle Pw_198">特殊订单运费规则：</span>
                <asp:RadioButtonList ID="rdoSpecialOrderAmount" runat="server" RepeatLayout="Flow" ClientIDMode="Static">
                    <asp:ListItem Value="default">默认</asp:ListItem>
                    <asp:ListItem Value="freeShipping">满额免运费</asp:ListItem>
                    <asp:ListItem Value="addShipping">未满额加运费</asp:ListItem>
                </asp:RadioButtonList>
            </li>
            <li id="freeShippingArea"><span class="formitemtitle Pw_198" id="specialLabel1">满额免运费：</span>
              <asp:TextBox ID="txtSpecialValue1" runat="server" CssClass="forminput" />
              <p id="L1" runat="server">满足消费金额后订单自动包邮,设置为0为不包邮</p>
            </li>
            <li id="addShippingArea"><span class="formitemtitle Pw_198" id="specialLabel2">满额免运费：</span>
              <asp:TextBox ID="txtSpecialValue2" runat="server" CssClass="forminput" />
              <p id="L2" runat="server">满足消费金额后订单自动包邮,设置为0为不包邮</p>
            </li>
            <!--特殊订单运费规则end-->

            <li><span class="formitemtitle Pw_198"><em >*</em>过期几天自动关闭订单：</span>
              <asp:TextBox ID="txtCloseOrderDays" runat="server" CssClass="forminput" />
              <p id="txtCloseOrderDaysTip" runat="server">下单后过期几天系统自动关闭未付款订单</p>
            </li>
             <li><span class="formitemtitle Pw_198"><em >*</em>发货几天自动完成订单：</span>
              <asp:TextBox ID="txtFinishOrderDays" runat="server" CssClass="forminput" />
              <p id="txtFinishOrderDaysTip" runat="server">发货几天后，系统自动把订单改成已完成状态</p>
            </li>            
            <li  style="display:none;"><span class="formitemtitle Pw_198"><em >*</em>订单发票税率：</span>
              <asp:TextBox ID="txtTaxRate" runat="server" CssClass="forminput" />%
              <p id="txtTaxRateTip" runat="server">发票收税比率，0表示顾客将不承担订单发票税金</p>
            </li>
                  <li  style="display:block;" class="clearfix"><span class="formitemtitle Pw_198"><em >*</em>是否开启未付款提醒：</span>
             
            <Hi:YesNoRadioButtonList ID="radEnableOrderRemind" runat="server" RepeatLayout="Flow" />
          
              <p id="P1" runat="server"></p>
            </li>
                  <li id="t" style="display:block;"><span class="formitemtitle Pw_198"><em >*</em>提醒时间设置：</span>
              <asp:TextBox ID="txtOrderRemindTime" runat="server" CssClass="forminput" Text="5" />
              <p id="txtOrderRemindTimeTip" runat="server">时间以分钟单位，多个时间提醒请用‘,’隔开,例：5,60。这个设置是在查询到用户5分钟和60分钟没付款后做出提醒</p>
            </li>
              </ul>
              <div class="clear"></div>
              <ul class="btntf Pa_198">
                    <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="submit_DAqueding inbnt" OnClientClick="return PageIsValid();"  />
	          </ul>
          </div>
      </div>
</div>  
    <script src="/Utility/jquery-1.6.4.min.js"></script>
    <script>
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtShowDays', 1, 10, false, '-?[0-9]\\d*', '设置前台发货查询中显示最近几天内的已发货订单'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtShowDays', 0, 90, '设置前台发货查询中显示最近几天内的已发货订单'));

            initValid(new InputValidator('ctl00_contentHolder_txtCloseOrderDays', 1, 10, false, '-?[0-9]\\d*', '下单后过期几天系统自动关闭未付款订单'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtCloseOrderDays', 0, 90, '下单后过期几天系统自动关闭未付款订单'));
                        
            initValid(new InputValidator('ctl00_contentHolder_txtFinishOrderDays', 1, 10, false, '-?[0-9]\\d*', '发货几天后，系统自动把订单改成已完成状态'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtFinishOrderDays', 0, 90, '发货几天后，系统自动把订单改成已完成状态'));

            initValid(new InputValidator('ctl00_contentHolder_txtOrderRemindTime', 1, 10, false, '[1-9][0-9]*,?[0-9]*', '时间以分钟单位，多个时间提醒请用‘,’隔开'))
            appendValid(new NumberRangeValidator('ctl00_contentHolder_txtOrderRemindTime', 0, 90, '时间以分钟单位，多个时间提醒请用‘,’隔开,例：5,60。这个设置是在查询到用户5分钟和60分钟没付款后做出提醒'));
            
//            initValid(new InputValidator('ctl00_contentHolder_txtTaxRate', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '税率不能为空,必须在0-100之间'))
//            appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtTaxRate', 0, 100, '税率必须在0-100之间'));

            initValid(new InputValidator('ctl00_contentHolder_txtKey', 0, 60, true, null, '快递100所需Key在物流跟踪时会用到，长度限制在60字符以内'))
        }

        $(document).ready(function () {
            InitValidators();
            if ($("#ctl00_contentHolder_radEnableOrderRemind_1").attr("checked")=="checked") {
                $("#t").hide();
                $("#ctl00_contentHolder_txtOrderRemindTime").val("5");
            } else {
                $("#t").show();
            }
            $("#ctl00_contentHolder_radEnableOrderRemind_1").click(function () {
                $("#t").hide();
                $("#ctl00_contentHolder_txtOrderRemindTime").val("5");
            });
            $("#ctl00_contentHolder_radEnableOrderRemind_0").click(function () {
                $("#t").show();
                $("#ctl00_contentHolder_txtOrderRemindTime").focus()
            });

            //订单特殊规则载入

            var init = function (type) {
                var freeShippingArea = $("#freeShippingArea");
                var addShippingArea = $("#addShippingArea");
                freeShippingArea.show(); addShippingArea.show();
                switch (type) {
                    case "default":
                        freeShippingArea.hide();
                        addShippingArea.hide();
                        break;
                    case "freeShipping":
                        $("#specialLabel1").html("满额免运费：");
                        $("#L1").html("满足消费金额后订单自动包邮,设置为0为不包邮");
                        $("#ctl00_contentHolder_txtSpecialValue2").val("");
                        addShippingArea.hide();
                        break;
                    case "addShipping":
                        $("#specialLabel1").html("未满额：");
                        $("#ctl00_contentHolder_L1").html("设置订单的最低金额,若未满足，则按照下方的价格加运费");
                        $("#specialLabel2").html("加价额：");
                        $("#ctl00_contentHolder_L2").html("未满足订单的最低金额情况下，增加相应的费用");
                        break;
                }
            }

            var type1 = "";
            $("input[type=radio]").each(function () {
                if ($(this).attr("checked") === "checked") {
                    type1 = $(this).val();
                    return false;
                }
            });

            init(type1);

            $("input[type=radio]").click(function () {
                init($(this).val());
            });


        });
</script>
</asp:Content>

