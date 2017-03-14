<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"  CodeBehind="Salestimesegment.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Salestimesegment"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<script src="../js/highcharts.js"></script>

	<div class="dataarea mainwidth databody">
			  <div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>时间段销售统计</h1>
  <span>查询一段时间内订单内的销售价格(注：统计的商品不包括成功退款订单中的商品)。</span>
</div>


	  <div class="datalist">


        <div class="clearfix search_titxt2">
            <div class="searcharea clearfix ">
			    <ul class="a_none_left">
		 
                <li><span>时间段：</span><span><UI:WebCalendar ID="calendarStart" runat="server" class="forminput"/></span><span class="Pg_1010"></span></li>
				    <li><span>请选择门店：</span><asp:DropDownList ID="DDLservice" runat="server"></asp:DropDownList></li>
                    <li><span>商品品类：</span><asp:DropDownList ID="DDLcategories" runat="server"></asp:DropDownList></li>
                    <li><span>商品单品：</span><asp:DropDownList ID="DDLproduct" runat="server"></asp:DropDownList></li>
                    <li><asp:Button ID="btnQuery" runat="server" Text="查询" class="searchbutton" /></li>
			    </ul>
	        </div>
	    </div>

	  </div>

	  <div class="page">


	  <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
                  <%--  <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />--%>
                </div>

			</div>
		</div>
      </div>

</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

    <div id="container" style="width:1383px;height:300px; margin-left: auto; margin-right: auto; margin-bottom: 0; padding-right: 10px;"></div>

<input type="hidden" runat="server" clientidmode="static" id="hidTablesY" />
<input type="hidden" runat="server" clientidmode="static" id="hidTablesX" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript">
        $(function () {
            var salePriceList = $("#hidTablesY").val().split(',');
            for (var i = 0; i < salePriceList.length; i++) {
                salePriceList[i] = parseInt(salePriceList[i]);
            }

            loadTables(salePriceList);
        })

        function loadTables(salePrices) {
            $('#container').highcharts({
                title: {//走势图标题
                    text: '门店销售额走势图',
                        x: 20,
                        //style:{display:"none"}//可隐藏
                    },
                    subtitle: {//走势图来源
                        text: 'Source: www.xwcms.net',
                        x: 20,
                        style: { display: "none" }//可隐藏
                    },
                    xAxis: {//X轴分类
                        title: {
                            text: '<span style="color:red;">    时间(小时)    </span>',//Y轴表示的文本
                            //style:{display:"none"}//可隐藏
                        },
                        categories: [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23]

                    },
                    yAxis: {//Y轴会根据series的data数值自动分格并划分上下限
                        title: {
                            text: '<span style="color:red;">    销售额(元)    </span>',//Y轴表示的文本
                            //style:{display:"none"}//可隐藏
                        }
                    },
                    tooltip: {
                        //valueSuffix: ''//数据的后辍
                 
                    },
                    legend: {//线条所表示的品种分类
                        enabled: 0,//0为隐藏1为显示
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'middle',
                        borderWidth: 0
                    },
                    credits: {//制作人；可作为本站水印
                        text: "pop.gdsswf.com",
                        href: "#",
                        position: { x: -250, y: -180 },
                        style: { "z-index": "999" }
                    },
                    series: [//可以为多个品种
                        { name: '销售额', data: salePrices },
                        
                   
                    ] 
            });
        }
        
    </script>
</asp:Content>
