
<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="../RoleMenuPermissions.cs" Inherits="Hidistro.UI.Web.Admin.RoleMenuPermissions" %>

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
		<!--
    var setting = {
        check: {
            enable: true
        },
        data: {
            simpleData: {
                enable: true
            }
        }
    };

    var zNodes;
    $.ajax({
        type: "POST",
        contentType: "application/json", //WebService 会返回Json类型
        url: "RoleMenuPermissions.aspx/GetZnodes", //调用WebService的地址和方法名称组合 ---- WsURL/方法名
        async: false,
        success: function (result) {     //回调函数，result，返回值
            var json = JSON.parse('[' + result.d + ']');
            zNodes = json;
        }
    });
   
    $(document).ready(function () {
        $.fn.zTree.init($("#treeDemo"), setting, zNodes);
    });

    function checkSelect() {
        var selectIDS = "";
        var selectNameS = "";
        var selectLinkS = "";
        var selectDataIds = "";
        var zTree = $.fn.zTree.getZTreeObj("treeDemo");
        var nodes = zTree.getCheckedNodes(true);
        for (var i = 0; i < nodes.length; i++) {
            selectDataIds += nodes[i].DataId + ",";
            selectIDS += nodes[i].id + ",";
            selectNameS += nodes[i].name + ",";
            selectLinkS += nodes[i].Link + ",";
        }
        //$("#hiSelectIDS").val(selectIDS);
        //$("#hiSelectNameS").val(selectNameS);
        //$("#hiSelectLinkS").val(selectLinkS);

        var ids = selectDataIds.substring(0, selectDataIds.length - 1).split(",");
        var layout = "";
        
        for (var i = 0; i < ids.length; i++) {
            layout +=ids[i] + ",";
        }
        layout = layout.substring(0, layout.length - 1);
        
        $("#hiSelectIDS").val(layout);//将选中项的guid传到隐藏域内
        return true;
    }
    //页面载入时,同时载入选中项
    function loadSelect() {
        $.ajax({
            type: "POST",
            contentType: "application/json", //WebService 会返回Json类型
            url: "RoleMenuPermissions.aspx/GetSelect", //调用WebService的地址和方法名称组合 ---- WsURL/方法名
            async: false, //同步调用(如果是异步,则会慢一拍)
            success: function (result) {     //回调函数，result，返回值
                if (result.d != "false") {
                    var ids = result.d.split(",");
                    for (var i = 0; i < ids.length; i++) {
                        var zTree = $.fn.zTree.getZTreeObj("treeDemo");
                        var node = zTree.getNodeByParam("id", ids[i]);
                        if (node != null) zTree.checkNode(node, true, false);
                    }
                }
            }
        });
    }
	</script>
</head>
    <body onload="loadSelect()">
       <form id="form1" runat="server">
            <div class="areacolumn clearfix">
                  <div class="columnright">
                      <div class="title">
                        <em><img src="../images/04.gif" width="32" height="32" /></em>
                        <h1>后台模块配置</h1>
                        <span>后台权限菜单，配置管理员登录后台后可见的菜单</span>
                      </div>
                  <div>
                    <ul id="treeDemo" class="ztree" style="width:260px; overflow:auto;"></ul>
                  </div>
                    </div>
                  <ul class="btn Pa_110">
                        <asp:Button ID="btnSave" runat="server" Text="保 存" OnClientClick="return checkSelect()" CssClass="submit_DAqueding" OnClick="btnSave_Click" />
                        <asp:HiddenField ID="hiSelectIDS" runat="server"  Value=""/>
                        <asp:HiddenField ID="hiSelectNameS" runat="server"  Value=""/>
                        <asp:HiddenField ID="hiSelectLinkS" runat="server"  Value=""/>
                    </ul>
            </div>
    </form>
    </body>
</html>