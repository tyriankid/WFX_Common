<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"  CodeBehind="fansTable.aspx.cs" Inherits="Hidistro.UI.Web.Admin.fansTable"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<style type="text/css">
*{
	padding: 0;
	margin: 0;
	-webkit-box-sizing: border-box;
	-moz-box-sizing: border-box;
	-ms-box-sizing: border-box;
	-o-box-sizing: border-box;
	box-sizing: border-box;
}
body{
	font: 12px "Microsoft Yahei";
}
ul,li{
	list-style: none;
}
.content{
	padding: 20px;
}
.content ul{
	border:1px solid #e7e7eb;
}
.content ul li{
	font-size: 14px;
	line-height: 36px;
	border-top:1px solid #e7e7eb;
	overflow: hidden;
}
.content ul li.listHeader{
	font-size: 16px;
	border-top: 0;
}
.content ul li.listHeader span i{
	display: inline-block;
	width: 10px;
	height: 12px;
	margin-left: 5px;
	background-image: url(../images/sprite.png);
	background-position: 0 0;
	background-repeat: no-repeat;
}
.content ul li.listHeader span i.active{
	background-position: -12px 0;
}
.content ul li.listHeader span i.activeUp{
	background-position: -24px 0;
}
.content ul li.listHeader span i.activeDown{
	background-position: -36px 0;
}
.content ul li span{
	float: left;
	width: 20%;
	text-align: center;
    cursor:pointer;
}
</style>
	<div class="dataarea mainwidth databody">
			  <div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>门店粉丝统计</h1>
  <span>根据时间段统计各个门店的粉丝数量，用户总数和用户增长数分别根据不同方法和时间点来统计，可能出现不匹配。</span>
</div>

	    <!--数据列表区域-->
	  <div class="datalist">
      		<!--搜索-->
		<!--结束-->
            <div class="clearfix search_titxt2">
      <div class="searcharea clearfix ">
			<ul class="a_none_left">
		 
          <li><span>时间段：</span><span><UI:WebCalendar ID="calendarStart" runat="server" class="forminput"/></span><span class="Pg_1010">至</span><span><UI:WebCalendar ID="calendarEnd" runat="server" class="forminput" IsStartTime="false" /></span></li>
				<li><span>请选择门店：</span><asp:DropDownList ID="DDLservice" runat="server"></asp:DropDownList></li>
                <li><asp:Button ID="btnQuery" runat="server" Text="查询" class="searchbutton" /></li>
			</ul>
	  </div>
	  </div>
      <div class="blank12 clearfix"></div>
     
      <div class="blank12 clearfix"></div>
     
	  </div>



</div>


	<div class="content">
		<ul>
			<li class="listHeader">
				<span>时间<i  ptype="date"></i></span>
				<span>新关注人数<i ptype="newSub"></i></span>
				<span>取消关注人数<i ptype="unSub"></i></span>
				<span>净增关注人数<i ptype="realSub"></i></span>
				<span>累计关注人数<i ptype="totalSub"></i></span>
			</li>
		</ul>
	</div>

	<div class="databottom"></div>
<input type="hidden" runat="server" clientidmode="static" id="hidTables" />


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        $(function () {
            var numList = $("#hidTables").val().split(';');
            var tableHtml = "";
            for (var i = 0; i < numList.length; i++) {
                var nums = numList[i].split(',');
                var timestamp2 = Date.parse(new Date(nums[0]));
                timestamp2 = timestamp2 / 1000;
                tableHtml += "<li role='line' date='" + timestamp2 + "'  newSub='" + nums[1] + "'  unSub='" + nums[2] + "'  realSub='" + nums[3] + "' totalSub='" + nums[4] + "'>" +
                    '<span >' + nums[0] + '</span>' +
                    '<span >' + nums[1] + '</span>' +
                    '<span >' + nums[2] + '</span>' +
                    '<span >' + nums[3] + '</span>' +
                    '<span >' + nums[4] + '</span>' +
                    '</li>';
            }
            $(".listHeader").after(tableHtml);


            $(".listHeader i").addClass("active");
            $(".listHeader span").click(function () {
                var a = $(this).find('i');
                if (a.hasClass("activeUp")) {
                    a.removeClass("activeUp");
                    a.addClass("activeDown");
                    paixu(a.attr("ptype"), 1);
                }
                else if (a.hasClass("activeDown")) {
                    a.removeClass("activeDown");
                    a.addClass("activeUp");
                    paixu(a.attr("ptype"), 0);
                }
                else {
                    $(".listHeader i").removeClass(); $(".listHeader i").addClass("active");
                    a.addClass("activeUp");
                    paixu(a.attr("ptype"), 0);
                }
            });


            function paixu(type, sort) {
                var arr = $("[role='line']").get();
                arr.sort(function (a, b) {
                    var ai = parseFloat($(a).attr(type), 10);
                    var bi = parseFloat($(b).attr(type), 10);
                    if (sort == 0) {
                        return ai - bi;
                    } else {
                        return bi - ai;
                    }
                });
                $("[role='line']").remove();
                $(".listHeader").after(arr);
            }
        })
    </script>
</asp:Content>
