<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="UnderDistributorManage.aspx.cs" Inherits="Hidistro.UI.Web.Admin.distributor.UnderDistributorManage" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">

        <link rel="stylesheet" href="/admin/css/css.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="../zTree/zTreeStyle.css" type="text/css"/>
	<script type="text/javascript" src="../zTree/jquery-1.4.4.min.js"></script>
	<script type="text/javascript" src="../zTree/jquery.ztree.core-3.5.js"></script>
	<script type="text/javascript" src="../zTree/jquery.ztree.excheck-3.5.js"></script>
    <script type="text/javascript" src="/Utility/windows.js"></script>
    <link rel="stylesheet" href="/admin/css/windows.css" type="text/css" media="screen" />
    
<%--    <link rel="stylesheet" href="treeTable/default/jquery.treeTable.css" type="text/css"/>--%>
	<script type="text/javascript" src="../treeTable/jquery.treeTable.js"></script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title">
            <em>
                <img src="../images/04.gif" width="32" height="32" /></em>
            <h1>下属信息查询</h1>
            <span>对商家的下属信息进行查询,您可以查询下属的佣金和详细信息。</span>
        </div>
        <!--搜索-->
        <!--数据列表区域-->
        <div class="datalist">
            <div class="clearfix search_titxt2">
            <div class="searcharea clearfix br_search">
                <ul>
                    <li><span>上级店铺：</span> 
                        <!--<asp:TextBox ID="txtStoreName" CssClass="forminput" runat="server" /></span>-->
                        <asp:TextBox ID="txtReferralUserId" runat="server" CssClass="forminput" autocomplete="off" />
                        <p id="P1"></p>
                    </li>
                    <li>
                        <span>时 间 段：</span>
                        <span><UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" /></span>
                        <span class="Pg_1010">至</span> 
                        <span><UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" /></span>
                    </li>
                    <li>
                        <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="搜索" />
                        <a id="btnDistributorTree"  class="searchbutton" href="javascript:void(0)" onclick="showRelationship()" style="display: none"/>查看关系图</a>
                        <asp:Button ID="btnExport" runat="server" class="submit_queding" Text="导出" />
                        <asp:HiddenField ID="hiIsAgent" Value="0" runat="server" />
                    </li>
                </ul>
            </div>
            </div>
            <div class="functionHandleArea m_none">
                   <!--分页功能-->
		  <div class="pageHandleArea" style="float:left;display:none">
		    <ul>
		      <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
	        </ul>
	          
	      </div>
               <asp:Literal runat="server" ID="litShowInfo"></asp:Literal>
            </div>
            <table id="mainTB">
                <thead>
                    <tr class="table_title" >
                        <td>店铺名</td>
                        <td>微信昵称</td>
                        <td>佣金总额</td>
                        <td>订单总额</td>
                        <td>订单成本总额</td>
                        <td>所属上级店铺</td>
                        <td>是否分销商</td>
                        <td>操作</td>
                    </tr>
                </thead>
                <asp:Repeater ID="reDistributor" runat="server" EnableViewState="true">
                    <ItemTemplate>
                        
                            <tr <%# Convert.ToInt32(Eval("HasChild"))==1?"hasChild='true'":""%> id="<%# Eval("UserId")%>" <%# Eval("ParentUserId").ToString()==Eval("UserId").ToString()?"":"pid='"+Eval("ParentUserId")+"'"%>>
                                <td><%# Eval("StoreName")%></td>
                                <td><%# Eval("UserName")%></td>
                                <td><%# Eval("CommTotal","{0:F2}")%></td>
                                <td><%# Eval("OrderTotal","{0:F2}")%></td>
                                <td><%# Eval("CostTotal","{0:F2}")%></td>
                                <td><%# Eval("referralUserId").ToString()==Eval("UserId").ToString()?"无":Eval("ParentStoreName")%></td>
                                <td><%# Convert.ToInt32(Eval("isAgent"))==1?"否":"是"%></td>
                                <td class="td_txt_cenetr">
                                 <span class="submit_bianji" ><asp:HyperLink ID="qlkbView1" runat="server" Text="详细" NavigateUrl='<%# "DistributorDetails.aspx?UserId="+Eval("UserId")+"&IsAgent="+Eval("IsAgent")%>' ></asp:HyperLink></span>
                                 <span class="submit_bianji"><asp:HyperLink ID="HyperLink1" runat="server" Text="佣金明细" NavigateUrl='<%# "CommissionsList.aspx?UserId="+Eval("UserId")%>' ></asp:HyperLink></span>
                                 <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/sales/ManageOrder.aspx?ReferralUserId={0}",Server.UrlEncode(Eval("UserId").ToString()))) %>'>所有订单</a>
                                </td>
                            </tr>
                        
                    </ItemTemplate>
                </asp:Repeater>
            </table>
         
            <div class="blank12 clearfix"></div>

            <div>
                 <ul id="treeDemo" class="ztree" style="width:260px; overflow:auto; display:none"></ul>
            </div>
            <asp:HiddenField ID="hidTopUserId" runat="server" value="0"/>


        </div>
        <!--数据列表底部功能区域-->
     
        <div class="bottomPageNumber clearfix" style="display:none">
            <div class="pageNumber">
                <div class="pagination" style="width: auto">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                </div>
            </div>
        </div>
    </div>


<Hi:Script ID="Script1" runat="server" Src="/utility/jquery.bigautocomplete.js"></Hi:Script>
<hi:style ID="Style1" runat="server" href="/utility/jquery.bigautocomplete.css" media="screen" />

<script>
    $(function () {
        var searchajax = "?action=SearchKey";
        $("#ctl00_contentHolder_txtReferralUserId").bigAutocomplete({ url: searchajax, width: 443 });

        $("#ctl00_contentHolder_txtReferralUserId").keypress(function (e) {
            if (e.which == 13) {
                return false;
            }
        });
    });

</script>

<script type="text/javascript">
    var setting = {
        check: {
            enable: false
        },
        data: {
            simpleData: {
                enable: true,
                pIdKey: "pid",
                rootPId: null
            }
        },
        view: {
            showIcon: false
        }
    };
    var zNodes;


    $(document).ready(function () {
        //alert($("#ctl00_contentHolder_hidTopUserId").attr("value"));
        var topDistributorId = $("#ctl00_contentHolder_hidTopUserId").attr("value");
        if (topDistributorId == null) topDistributorId=0;
        $.ajax({
            type: "POST",
            contentType: "application/json", //WebService 会返回Json类型
            url: "UnderDistributorManage.aspx/GetZnodes", //调用WebService的地址和方法名称组合 ---- WsURL/方法名
            data: "{TopUserId:'" + topDistributorId + "'}",         //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到      
            dataType: 'json',
            async: false,
            success: function (result) {     //回调函数，result，返回值
                var json = JSON.parse('[' + result.d + ']');
                zNodes = json;
            }
        });

        $.fn.zTree.init($("#treeDemo"), setting, zNodes);
        

        //treetable
        var option = {
            theme: 'vsStyle',
            expandLevel: 1,
            beforeExpand: function ($treeTable, id) {

                var startDate = GetQueryString("StartDate") != null?GetQueryString("StartDate").replace("+", " "):"";
                var endDate = GetQueryString("EndDate") != null?GetQueryString("EndDate").replace("+", " "):"";
               
                //判断id是否已经有了孩子节点，如果有了就不再加载，这样就可以起到缓存的作用
                if ($('.' + id, $treeTable).length) { return; }
                //这里的html可以是ajax请求
                $.ajax({
                    type: "POST",   //访问WebService使用Post方式请求
                    contentType: "application/json", //WebService 会返回Json类型
                    url: "UnderDistributorManage.aspx/GetUnderDistributorTr", //调用WebService的地址和方法名称组合 ---- WsURL/方法名
                    data: "{currentDistributorId:'" + id + "',startDate:'" + startDate + "',endDate:'" + endDate + "'}",         //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到      
                    dataType: 'json',
                    success: function (result) {     //回调函数，result，返回值
                        var html = result.d;
                        $treeTable.addChilds(html);
                    },
                    error: function (err) {
                    alert('错误', '' + err + '', 'error');
                    }
                });  
            },
            onSelect: function ($treeTable, id) {
                window.console && console.log('onSelect:' + id);

            }

        };
        $('#mainTB').treeTable(option);


    });

    

    function showRelationship() {
        $("#mainTB").toggle();
        $("#treeDemo").toggle();
        if ($("#btnDistributorTree").html() == "查看关系图") {
            $("#btnDistributorTree").html("查看表格");
        }
        else {
            $("#btnDistributorTree").html("查看关系图");
        }
        /*
        $("#mainTB").treetable({
            initialState: "expanded"
        });
        $("#mainTB").find(".expanded").each(function () {
            $(this).find("td").eq(0).prepend("<a href='javascript:void(0)' onclick='disexpand(this)'>aa</a>");
        });
        */
    }

    function disexpand(e) {
        var currentId = $(e).parent("td").parent("tr").attr("data-tt-id");
        var pId = $(e).parent("td").parent("tr").attr("data-tt-parent-id");
        
        $("tr[data-tt-parent-id='" + currentId + "']").toggle(200);
        //$(e).parent("td").parent("tr").toggle();
    }

    function toToggle(e) {
        $(e).toggle();
    }

    function GetQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }

	</script>

</asp:Content>