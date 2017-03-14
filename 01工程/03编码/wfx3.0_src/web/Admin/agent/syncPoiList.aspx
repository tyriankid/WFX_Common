<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/Admin.Master" CodeFile="syncPoiList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.agent.syncPoiList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <style>
        #divSenderList{
            position: absolute;
            background: #fff;
            padding: 20px;
            text-align: center;
            box-shadow: 0 0 10px #CECECE;
            -moz-box-shadow: 0 0 10px #CECECE;
            -webkit-box-shadow: 0 0 10px #CECECE;
        }
        #divSenderList a{
            display: block;
            margin-top: 10px;
            text-decoration: none;
            background: #0B5BA5;
            color: #fff;
        }
    </style>
<div id="main">
<div class="dataarea mainwidth databody">
   <!--info-->
    <div class="title">
		<em><img src="../images/02.gif" width="32" height="32" /></em>
        <h1><strong>微信门店列表</strong></h1>
        <span>查看与管理公众平台内的微信门店信息 </span>
	</div>
  <div class="datalist">
    <!--搜索-->
    <div class="clearfix search_titxt2">
    <div class="searcharea clearfix br_search">
			<ul class="a_none_left">
				<li><asp:Button ID="btnSyncPoiInfos" runat="server" class="searchbutton" Text="同步门店" OnClick="btnSyncPoiInfos_Click" /></li>
			</ul>
	  </div>
	</div>
    <!--列表-->
    <asp:DataList ID="dlPoiList" runat="server" DataKeyField="poi_id" Style="width: 100%;">
         <HeaderTemplate>
          <table width="0" border="0" cellspacing="0" >
            <tr class="table_title">
              <td width="15%" class="td_right td_left">门店Id</td>
              <td width="22%" class="td_right td_left">审核状态</td>
              <td width="22%" class="td_left td_right_fff">分店名称</td>
              <td width="16%" class="td_left td_right_fff">分店地址</td>
              <td width="16%" class="td_left td_right_fff">后台账号</td>
              <td width="16%" class="td_left td_right_fff">操作</td>
            </tr>
         </HeaderTemplate>
         <ItemTemplate>
          <tr  class="td_bg">
              <td ><%#Eval("poi_id") %></td>
              <td ><%#Eval("available_state_name") %></td>
              <td ><%#Eval("branch_name") %></td>
              <td ><%#Eval("address") %></td>
              <td ><%#string.IsNullOrEmpty(Eval("sender").ToString())?"未绑定":Eval("UserName") %></td>
              <td ><a onclick="getSender('<%#Eval("poi_id") %>',<%#Eval("available_state") %>,this)" style="cursor:pointer">绑定门店</a></td>
            </tr>

         </ItemTemplate>
         <FooterTemplate>
           </table>
         </FooterTemplate>
    </asp:DataList>

   </div>
</div>
    <!--后台门店账号选择div-->
    <div id="divSenderList" style="display:none">

    </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#main").on("click", "#btnBind", function () {
                bindSender($(this).attr("poi_id"));
            });
        });

        function getSender(poi_id, available_state,e) {
            if (available_state != 3) {
                alert("还未通过审核的门店不能绑定后台账号!");
                return false;
            }
            var $senderNameTD = $(e).parent("td").prev();
            /*
            if ($senderNameTD.html() != "未绑定") {
                alert("请勿重复绑定");
                return false;
            }
            */
            $.ajax({
                type: "POST",   //访问WebService使用Post方式请求
                contentType: "application/json", //WebService 会返回Json类型
                url: "syncPoiList.aspx/getSenderDiv", //调用WebService的地址和方法名称组合 ---- WsURL/方法名
                data: "{poi_id:'" + poi_id + "'}",         //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到      
                dataType: 'json',
                success: function (result) {     //回调函数，result，返回值
                    $("#divSenderList").show();
                    var x = $(e).offset().left,
                        y = $(e).offset().top;
                    $("#divSenderList").html(result.d)
                        .css({
                            "top": y - 110,
                            "left": x - 60
                        });
                }
            });
        }

        function bindSender(poi_id) {

            $.ajax({
                type: "POST",   
                contentType: "application/json", 
                url: "syncPoiList.aspx/bindSender",
                data: "{poi_id:'" + poi_id + "',sender:'" + $("#ddlSender option:selected").val() + "'}",         //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到      
                dataType: 'json',
                success: function (result) {     //回调函数，result，返回值
                    if (result.d === "ok") {
                        location.reload();
                    }
                }
            });
        }
    </script>
</asp:Content>

