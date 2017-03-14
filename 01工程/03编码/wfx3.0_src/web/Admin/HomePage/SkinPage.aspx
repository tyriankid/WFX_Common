<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SkinPage.aspx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.SkinPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html;charset=utf-8"/>
	<title>Document</title>
	<meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1"/>
	
	<link rel="stylesheet" type="text/css" href="css/V_style.css"/>
	<link rel="stylesheet" type="text/css" href="css/jquery.minicolors.css"/>
	
	<script src="js/jquery-2.1.4.min.js" type="text/javascript" charset="utf-8"></script>
	<!--------------拖拽效果---------------拖拽效果-------------拖拽效果-------------->
	<link rel="stylesheet" type="text/css" href="css/jquery-ui.min.css"/>
	<script src="js/jquery-ui.min.js" type="text/javascript" charset="utf-8"></script>
	<!----------------颜色选择器-------------颜色选择器-----------------颜色选择器------------->
	<script src="js/jquery.minicolors.min.js" type="text/javascript" charset="utf-8"></script>
	<!---------幻灯片-----------------幻灯片-----------------幻灯片----------------------->
	<script src="js/jquery.slides.min.js" type="text/javascript" charset="utf-8"></script>
	<!----------------------------百度UEditor富文本编辑器-------------------------------------->
	<script src="../../UEditor/ueditor.config.js" type="text/javascript" charset="utf-8"></script>
	<script src="../../UEditor/ueditor.all.min.js" type="text/javascript" charset="utf-8"></script>
    <script src="js/V_databind.js?d=2" type="text/javascript" charset="utf-8"></script>	
    <script src="js/VSave.js?i=2"  type="text/javascript" charset="utf-8"></script>
	<style type="text/css">
		.slides{
			position: relative;
		}
		.slides li{
			position: absolute;
		}
		.slides li:first-child{
			position: relative;
		}
	</style>
	<script>
	    $(function () {
	        $("#btncl").click(function () {
	            closechsImg();
	        });
	        $("#Ul_Skin li").hover(function () {
	            $(this).find(".delete").show();
	        }, function () {
	            $(this).find(".delete").hide();
	        });
	       
	    });
	    function checkName() {
	        if ($("#txtName").val().trim() == "") {
	            $("#msg").html("二级页面名称不能为空").dialog({
	                show: {
	                    effect: "slide",
	                    duration: 1000
	                },
	                hide: {
	                    effect: "slide",
	                    duration: 1000
	                }
	            });
	            return false;
	        }
	        if ($("#txtName").val().trim().length > 6) {
	            $("#msg").html("二级页面名称长度不能大于6个字符").dialog({
	                show: {
	                    effect: "slide",
	                    duration: 1000
	                },
	                hide: {
	                    effect: "slide",
	                    duration: 1000
	                }
	            });
	            return false;
	        }
	        return true;
	    }
	</script>
    
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="txtSkinName" runat="server" />
        <asp:HiddenField ID="txtSkinID" runat="server" />

        <div style="margin:10px">

        <div class="subBtn">
		<div class="subBtn_type">新增二级页</div>
		<ul id="Ul_Skin">
			<asp:Repeater ID="rptSkin" runat="server">
                <ItemTemplate>
                    <li>
                         <asp:LinkButton ID="btnSkin" CommandName="skin" CommandArgument='<%#Eval("SkinID") %>' runat="server"><%#Eval("Name") %></asp:LinkButton> <asp:Button ID="delete" OnClientClick="return confirm('确定要删除吗？');" CommandName="del" CommandArgument='<%#Eval("SkinID") %>' CssClass="delete" runat="server"  Text="X" />
                    </li>
                </ItemTemplate>
            </asp:Repeater>
		</ul>
            
            <asp:TextBox ID="txtName" CssClass="txtName" runat="server" style="cursor: auto;"></asp:TextBox>
            <asp:Button ID="btnAdd" CssClass="btnAdd" runat="server" OnClientClick ="return checkName()" Text="确认添加" />
        
	</div>

	<div class="draw ">
		<div class="draw_type">双击添加模块</div>
		<ul>
			<!--<li class="signboard"><a href="javascript:;">店招</a></li>-->
			<li class="navigation"><a href="javascript:;">导航</a></li>
			<li class="text"><a href="javascript:;">文本</a></li>
			<li class="notice"><a href="javascript:;">滚动<br />公告</a></li>
			<li class="picture"><a href="javascript:;">图片</a></li>
			<li class="slide"><a href="javascript:;">幻灯片</a></li>
            <li class="background"><a href="javascript:;">背景</a></li>
		</ul>
        
        <asp:Button ID="btnSave" CssClass="btn1" runat="server" Text="保存" OnClientClick="return save()" OnClick="btnSave_Click" />
        <a id="btnyl" class="btn2" href="javascript:window.open('/VShop/WgwIndex.aspx');" >预览</a>
	</div>

            </div>

    <div id="msg" title="友情提示"></div>
	<div class="phone">
        <asp:HiddenField ID="txtRes" runat="server" />
		<div class="phone-top"><img src="img/phone-top.png"/></div>
		<!--<div class="gridly"></div>-->
        <asp:Panel ID="panelHomePage" CssClass="gridly" runat="server"></asp:Panel>
		<div class="phone-bottom"><img src="img/phone-bottom.png"/></div>
	</div>
     <!--选择图片-->
    <div class="layoutImg">
        <a  href="javascript:" onclick="setIframeUrl('/Admin/HomePage/ImageData.aspx')">选择图片</a><a href="javascript:" onclick="setIframeUrl('/Admin/store/ImageFtp.aspx')">上传图片</a><a type="button" id="btncl" class="allSelect"/>X</a>
        <div>
            <iframe src="ImageData.aspx"></iframe>
        </div>
    </div>
        <!--选择url-->
    <div class="layoutUrl">
    <span>选择链接</span><a type="button" onclick="clo()" class="allSelect"/>X</a>
    <div style="border-top:1px dashed #efefef">
        <iframe src=""></iframe>
    </div>
    </div>
	<div class="attribute_frame"></div>	
    <script src="js/V_jquery.draw.js" type="text/javascript" charset="utf-8"></script>
        </form>
</body>
</html>
