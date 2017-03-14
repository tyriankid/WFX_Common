
<%@ Page Language="C#"  MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="../RoleAreaPermissions.cs" Inherits="Hidistro.UI.Web.Admin.RoleAreaPermissions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function (e) {
            var RegionScop = $("#txtRegionScop").val();
            var RegionScopName = $("#txtRegionScopName").val();
            if (RegionScop != "" && RegionScopName != "") {
                var RegionScopArr = RegionScop.split(',');
                var RegionScopNameArr = RegionScopName.split(',');
                for (var i = 0; i < RegionScopArr.length; i++) {
                    var RegionId = RegionScopArr[i];
                    var RegionName = RegionScopNameArr[i];
                    //区域信息html
                    var scopHTML = "<tr id=\"row_{0}\"><td>{1}</td><td><span class=\"submit_bianji\"><a href=\"javascript:void(0)\" class=\"editregion\" id=\"EditRegion_{0}\">编辑</a></span><span class=\"submit_shanchu\"><a href=\"javascript:void(0)\" class=\"delregion\" id=\"DelRegion_{0}\">删除</a></span></td></tr>".format(RegionId, RegionName);
                    $(scopHTML).insertAfter($("#scoplist tr:last"));
                }
            }

            $(".editregion").live("click", function () {
                var regionId = $(this).attr("id").split("_")[1];
                EditRegionId = regionId;
                DialogFrame("store/AddDeliveryScop.aspx?regionId=" + regionId, "编辑配送范围", 550, 300, function () {
                });
            });
            $(".delregion").live("click", function () {
                var regionId = $(this).attr("id").split("_")[1];
                var RegionScop = $("#txtRegionScop").val();
                var RegionScopArr = RegionScop.split(',');
                var RegionScopName = $("#txtRegionScopName").val();
                var RegionScopNameArr = RegionScopName.split(',');
                RegionScop = "";
                RegionScopName = "";
                for (var i = 0; i < RegionScopArr.length; i++) {
                    if (RegionScopArr[i] != regionId) {
                        RegionScop = RegionScop + (RegionScop == "" ? "" : ",") + RegionScopArr[i];
                        RegionScopName = RegionScopName + (RegionScopName == "" ? "" : ",") + RegionScopNameArr[i];
                    }
                }
                $("#txtRegionScop").val(RegionScop);
                $("#txtRegionScopName").val(RegionScopName);
                $(this).parent().parent().parent().remove();
            });
        });
        String.prototype.format = function () {
            var args = arguments;
            return this.replace(/\{(\d+)\}/g, function (s, i) {
                return args[i];
            });
        }
        var dailogID = null;
        var EditRegionId = "";//当前编辑的区域ID
        //保存发货区域信息
        function SaveDeliveryScop(RegionId, RegionName) {
            RegionId = RegionId.split(',')[0];
            //区域信息html
            var scopHTML = "<tr id=\"row_{0}\"><td>{1}</td><td><span class=\"submit_bianji\"><a href=\"javascript:void(0)\" class=\"editregion\" id=\"EditRegion_{0}\">编辑</a></span><span class=\"submit_shanchu\"><a href=\"javascript:void(0)\" class=\"delregion\" id=\"DelRegion_{0}\">删除</a></span></td></tr>".format(RegionId, RegionName);
            //操作对象行
            var row = $("#row_" + RegionId);
            //如果当前编辑的区域ID不为空则将编辑的区域信息替换为新的区域信息
            if (EditRegionId != "") {

                var EditRow = $("#row_" + EditRegionId);//  编辑区域行
                if (EditRow.length > 0 && RegionId != EditRegionId) {//如果编辑区域行存在，且当当前编辑区域ID与返回的区域ID不相同,则替换为新的区域信息
                    if ($("#row_" + RegionId).length > 0) return true;//如果已存在该区域行则返回false
                    $(scopHTML).insertAfter($(EditRow));//插入新的区域信息
                    EditRow.remove();//移除原来的区域信息
                    var RegionScop = $("#txtRegionScop").val();
                    var RegionScopName = $("#txtRegionScopName").val();
                    var RegionScopArr = RegionScop.split(',');
                    var RegionScopNameArr = RegionScopName.split(',');
                    console.log(RegionScopArr.length + "-" + RegionScop);
                    RegionScop = "";//更新区域信息列表
                    RegionScopName = "";
                    for (var i = 0; i < RegionScopArr.length; i++) {
                        if (RegionScopArr[i] != EditRegionId) {
                            RegionScop = RegionScop + (RegionScop == "" ? "" : ",") + RegionScopArr[i];
                            RegionScopName = RegionScopName + (RegionScopName == "" ? "" : ",") + RegionScopNameArr[i];
                        }
                        else {
                            RegionScop = RegionScop + (RegionScop == "" ? "" : ",") + RegionId;
                            RegionScopName = RegionScopName + (RegionScopName == "" ? "" : ",") + RegionName;
                        }
                    }
                    EditRegionId = "";
                    $("#txtRegionScop").val(RegionScop);
                    $("#txtRegionScopName").val(RegionScopName);
                    //console.log(RegionScop);
                }
            }
            else {//如果是新的区域ID则插入新行
                if (row.length > 0) return true;//如果已存在该行则返回false

                $(scopHTML).insertAfter($("#scoplist tr:last"));
                var RegionScop = $("#txtRegionScop").val();
                var RegionScopName = $("#txtRegionScopName").val();
                if (RegionScop != undefined && RegionScop != "") {
                    RegionScop = RegionScop + ",";
                    RegionScopName = RegionScopName + ",";
                }
                $("#txtRegionScop").val(RegionScop + RegionId.split(',')[0]);
                $("#txtRegionScopName").val(RegionScopName + RegionName.split(',')[0]);
                //console.log(RegionScop + "new");

            }

        }


        function doSubmit() {
            // 1.先执行jquery客户端验证检查其他表单项
            if (!PageIsValid())
                return false;

            if ($("#txtRegionScop").val() == "" || $("#txtRegionScop").val() == undefined) {
                alert("请至少添加一个范围。");
                return false;
            }

            return true;
        }
        $(document).ready(function (e) {
            $("#addDeliveryScop").click(function (e) {
                DialogFrame("store/AddDeliveryScop.aspx", "添加配送范围", 550, 300, function () {

                });
            });
            InitValidators();
        });
        function InitValidators() {
            initValid(new InputValidator('ctl00_contentHolder_txtStoreUserName', 2, 20, false, null, '用户名长度不能超过2-20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtStoresName', 2, 50, false, null, '店铺名称长度必须为2-50个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtAddress', 2, 50, false, null, '街道地址不能为空长度必须为2-50个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtContactMan', 2, 10, false, null, '联系人不能为空，长度必须为2-20个字符'));
            initValid(new InputValidator('ctl00_contentHolder_txtTel', 7, 20, false, null, '联系电话不能为空，请输入合法的电话或者手机号码'));
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title  m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>设置角色所属地区</h1>
        </div>
        <input type="hidden" id="txtRegionId" value="" />
        <div class="datafrom">
            <div class="formitem validator1">
                <ul>
                        <input style="display: none" /><!-- for disable autocomplete on chrome -->
                        <asp:Label ID="labStoreUserName" runat="server"></asp:Label>
                       <%-- <Hi:TrimTextBox runat="server" CssClass="forminput" autocomplete="off" ID="txtStoreUserName" />--%>
                        <p id="ctl00_contentHolder_txtStoreUserNameTip">
                            
                        </p>
                    </li>
                    
                    <li>
                        <asp:HiddenField ID="txtRegionScop" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="txtRegionScopName" runat="server" ClientIDMode="Static" />
                        <a id="addDeliveryScop" class="submit_jia" href="javascript:void(0)" style="color: #fff;">添加用户所属范围</a>
                    </li>
                    <li>
                        <div class="datalist clearfix" style="width: 500px;">
                            <table cellpadding="0" cellspacing="0" style="width: 100%; border-collapse: collapse;" id="scoplist">
                                <tr class="table_title">
                                    <th class="td_right td_left" scope="col" width="66%">所属范围</th>
                                    <th class="td_right td_left" scope="col">操作</th>
                                </tr>

                            </table>
                        </div>
                    </li>

                </ul>
                <ul class="btntf  clear">
                    <asp:Button runat="server" ID="btnAdd" Text="保 存" OnClientClick="return doSubmit();"
                        CssClass="submit_DAqueding inbnt" />
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
