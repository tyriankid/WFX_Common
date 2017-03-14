
<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="../DistributorsTree.cs" Inherits="Hidistro.UI.Web.Admin.DistributorsTree" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="stylesheet" href="/admin/css/css.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="../zTree/zTreeStyle.css" type="text/css"/>
	<script type="text/javascript" src="../zTree/jquery-1.4.4.min.js"></script>
	<script type="text/javascript" src="../zTree/jquery.ztree.core-3.5.js"></script>
	<script type="text/javascript" src="../zTree/jquery.ztree.excheck-3.5.js"></script>
    <script type="text/javascript" src="/Utility/windows.js"></script>
    <link rel="stylesheet" href="/admin/css/windows.css" type="text/css" media="screen" />
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
        $.ajax({
            type: "POST",
            contentType: "application/json", //WebService 会返回Json类型
            url: "DistributorsTree.aspx/GetZnodes", //调用WebService的地址和方法名称组合 ---- WsURL/方法名
            data: "{TopUserId:'" + $("#hidTopUserId").attr("value") + "'}",         //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到      
            dataType: 'json',
            async: false,
            success: function (result) {     //回调函数，result，返回值
                var json = JSON.parse('[' + result.d + ']');
                zNodes = json;
            }
        });

        $.fn.zTree.init($("#treeDemo"), setting, zNodes);
    });




    function GetQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
	</script>
</head>
    <body >
       <form id="form1" runat="server">
            <div class="areacolumn clearfix">
                  <div class="columnright">
                      <div class="title">
                        <em><img src="../images/04.gif" width="32" height="32" /></em>
                        <h1>从属关系图</h1>
                        <span>上下级的从属关系图展示</span>
                      </div>
                  <div>
                    <ul id="treeDemo" class="ztree" style="width:260px; overflow:auto;"></ul>
                  </div>
                  <asp:HiddenField ID="hidTopUserId" runat="server" value="0"/>
                    </div>
                  <ul class="btn Pa_110">
                        
                        
                    </ul>
            </div>
    </form>
    </body>
</html>