<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="Hidistro.UI.Web.Admin.HomePage.HomePage" %>
<!doctype html>
<html lang="en">
<head>
	<meta http-equiv="Content-Type" content="text/html;charset=utf-8"/>
	<title>Document</title>
	<meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1"/>
	
	<link rel="stylesheet" type="text/css" href="css/style.css"/>
	<link rel="stylesheet" type="text/css" href="css/jquery.minicolors.css"/>
	
	<script src="js/jquery-2.1.4.min.js" type="text/javascript" charset="utf-8"></script>
	
	<!--------------拖拽效果---------------拖拽效果-------------拖拽效果-------------->
	<link rel="stylesheet" type="text/css" href="css/jquery-ui.min.css"/>
	<script src="js/jquery-ui.min.js" type="text/javascript" charset="utf-8"></script>
	<!--<script src="js/jquery.gridly.js" type="text/javascript" charset="utf-8"></script>-->
	<!--<link rel="stylesheet" type="text/css" href="css/jquery.gridly.css"/>-->
	<!----------------颜色选择器-------------颜色选择器-----------------颜色选择器------------->
	<script src="js/jquery.minicolors.min.js" type="text/javascript" charset="utf-8"></script>
	<!---------幻灯片-----------------幻灯片-----------------幻灯片----------------------->
	<script src="js/jquery.slides.min.js" type="text/javascript" charset="utf-8"></script>
	<!----------------------------百度UEditor富文本编辑器-------------------------------------->
	<script src="../../UEditor/ueditor.config.js" type="text/javascript" charset="utf-8"></script>
	<script src="../../UEditor/ueditor.all.min.js" type="text/javascript" charset="utf-8"></script>
    <script src="js/databind.js?d=4" type="text/javascript" charset="utf-8"></script>	
    <script src="js/Save.js?i=2"  type="text/javascript" charset="utf-8"></script>
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
	        //$(".slides").slidesjs({
	        //    width: 750,
	        //    height: 400,
	        //    //start: 3,
	        //    navigation: { active: false, effect: "slide" },
	        //    pagination: { active: true, effect: "fade" },
	        //    effect: {
	        //        slide: { speed: 1000 },
	        //        fade: { speed: 1000, crossfade: true }
	        //    },
	        //    play: {
	        //        active: false,
	        //        effect: "slide",
	        //        interval: 3000,
	        //        auto: true,
	        //        swap: false,
	        //        pauseOnHover: true,
	        //        restartDelay: 2000
	        //    }
	        //});

	        //jqueryUI放置
	        $(".gridly").sortable().selectable();

	        //		    //模块拖动
	        //		    $('.gridly').gridly({
	        //			    base: 50, // px 
	        //			    gutter:5, // px 沟槽
	        //			    columns: 6
	        //			});
	        $("#btncl").click(function () {
	            closechsImg();
	        });

	    });
	</script>
    
</head>
<body>
    <form id="form1" runat="server">
        
    <div class="fixed-box">
	<div class="draw">
		<div class="draw_type">双击添加模块</div>
		<ul>
			<!--<li class="signboard"><a href="javascript:;">店招</a></li>-->
			<li class="navigation"><a href="javascript:;">导航</a></li>
			<li class="text"><a href="javascript:;">文本</a></li>
			<li class="notice"><a href="javascript:;">滚动<br />公告</a></li>
			<li class="picture"><a href="javascript:;">图片</a></li>
			<li class="slide"><a href="javascript:;">幻灯片</a></li>
			<li class="search"><a href="javascript:;">搜索栏</a></li>
			<!--<li class="goods"><a href="javascript:;">商品</a></li>-->
			<li class="list"><a href="javascript:;">商品<br />列表</a></li>
            <li class="space"><a href="javascript:;">辅助<br />空白</a></li>
			<!--<li class="video"><a href="javascript:;">视频</a></li>-->
		</ul>
        <asp:Button ID="btnSave" CssClass="btn1" runat="server" Text="保存" OnClientClick="return save()" OnClick="btnSave_Click" />
        <%--<asp:Button ID="btnSaveAndView" CssClass="btn2" runat="server" Text="保存并预览" />--%>
        <asp:Button ID="btnReset" CssClass="btn3" runat="server" OnClientClick="return confirm('确定还原到初始模板吗？')" Text="还原到初始模板" />
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
	<div class="attribute_frame"></div>	
	<div class="subBtn">
        
	</div>
        <script src="js/jquery.draw.js?d=3" type="text/javascript" charset="utf-8"></script>
        </form>
</body>
</html>
