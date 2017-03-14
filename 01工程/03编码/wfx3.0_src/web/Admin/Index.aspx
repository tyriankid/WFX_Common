<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Index" CodeBehind="Index.aspx.cs"%>


<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />

<script src="../Utility/jquery-1.6.4.min.js"></script>
<script src="../Utility/jquery.cookie.js"></script>
<script src="../Utility/jquery.artDialog.js"></script>
<script src="../Utility/iframeTools.js"></script>
<script src="js/Frame.js"></script>
<link rel="stylesheet" type="text/css" href="css/frame.css">
<link rel="stylesheet" type="text/css" href="../Utility/skins/black.css">
<title>无标题文档</title>
    <script>
        //IFrame弹出
        function DialogFrame(url, title_tip, w_width, h_height) {
            var tmpwidth = 900;
            var tmpheight = 520;
            if (w_width != null) {
                tmpwidth = w_width;
            }
            if (h_height != null) {
                tmpheight = h_height;
            }
            if (tmpwidth != 0) {
                if (tmpheight != 0) {
                    art.dialog.open(url, { title: title_tip, width: tmpwidth, height: tmpheight }, true);
                } else {
                    art.dialog.open(url, { title: title_tip, width: tmpwidth }, true);
                }
            } else {
                if (tmpheight != 0) {
                    art.dialog.open(url, { title: title_tip, height: tmpheight }, true);
                } else {
                    art.dialog.open(url, { title: title_tip }, true);
                }
            }

        }

    </script>
</head>
<body>

<div class="hishop_head">
	<div class="hishop_logo"><img src="images/logogo.png" class="pngFix" /></div>
    <div id="menu_arrow" class="close_arrow" onclick="javascript:ExpendMenuLeft()" style="display:none" title="点击展开或关闭左侧菜单"></div>
    <!-- 导航栏 -->
	<div class="hishop_banner">
    	<div class="hishop_con2"> 
        
            <!-- 权限内的导航链接 -->
            <div class="hishop_menu">
            <asp:Literal runat="server" ID="litMenuBanner"></asp:Literal>
			</div>
            <!--权限内的导航链接end--> 

			<div class="hishop_banneritem">
            
            <!--<img src="images/ren.png" /><font color="#FBD711">admin</font></a>-->　
            <!---->
					<!--<a target="_blank" href="../Vshop/Default.aspx"><img src="images/font.png" />浏览前台</a>-->
                    <!--<span><img src="images/font.png" /></span>-->

                    
                    <asp:Literal runat="server" ID="litManagerName"></asp:Literal><!-- 当前用户名 -->
                    <a href="LoginExit.aspx"><strong>[安全退出]</strong></a>
                    <a target="_blank" href="../Vshop/Default.aspx">浏览前台</a>
                    <!--<span><img src="images/split.jpg" /></span>-->
					<a href="Default.aspx">桌面</a>
                    <!--<span><img src="images/split.jpg" /></span><img src="images/flag.png" />-->
					<!--<a href="javascript:DialogFrame('help/vshopindex.html','新手向导',750,450)">新手向导</a>-->
                    <!--<span><img src="images/split.jpg" /></span>-->
                    <!--<a target="_blank" href="">123</a><img src="images/about.png" />-->
					<a href="javascript:DialogFrame('help/about.html','关于众赞移动云商平台',550,345)">关于</a>
			</div>
			
        </div>
	</div>
</div>

<div class="hishop_content">
	<div class="hishop_menu_scroll">
		<div id="menu_left">

		</div>
	</div>
	<div class="hishop_content_r">
		<iframe id="frammain" name="frammain" class="framecontent" runat="server" src="Default.aspx" frameborder="no" border="0" scrolling="auto"></iframe>
	</div>
</div>

</body>
</html>
