$(function(){
	$(".gridly").children("div").each(function(){
		var t=$(this).attr("i");
		switch($(this).attr("name")){
			case "DaoHang":
				dhAttrBind(t);
			break;
		    case "SouSuo":
		        ssAttrBind(t);
			break;
		    case "WenBen":
		        wbAttrBind(t);
			break;
		    case "GunDong":
		        gdAttrBind(t);
			break;
		    case "TuPian":
		        tpAttrBind(t);
			break;
		    case "HuanDeng":
		        hdpAttrBind(t);
			break;
			case "ShangPin":
			break;
		    case "LieBiao":
		        splbAttrBind(t);
		    break;
		    case "KongBai":
		        kbAttrBind(t)
		    break;
			case "DianZhao":
			break;
			case "ShiPin":
			break;
		}
		$(this).click(function () {
			integrate(this,t);
		});
	});
	choseColor();
})

//rgb(10进制)转#fff(16进制)的方法
function RGBToHex(rgb) {
if(rgb==null || rgb=="" || rgb==undefined)rgb="#ffffff";
    var regexp = /^rgb\(([0-9]{0,3})\,\s([0-9]{0,3})\,\s([0-9]{0,3})\)/g;
    var re = rgb.replace(regexp, "$1 $2 $3").split(" ");//利用正则表达式去掉多余的部分
    var hexColor = "#";
    var hex = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'];
    for (var i = 0; i < 3; i++) {
        var r = null;
        var c = re[i];
        var hexAr = [];
        while (c > 16) {
            r = c % 16;
            c = (c / 16) >> 0;
            hexAr.push(hex[r]);
        }
        hexAr.push(hex[c]);
        hexColor += hexAr.reverse().join('');
    }
    return hexColor;
}
//导航数据绑定
function dhAttrBind(t) {
    n = 1;
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
					  "<table class='nature' cellpadding='0' cellspacing='0' border='0'>"+
                      "<tr><td align='right'>显示方式：</td><td><input type='radio' name='way" + t + "' value='two' />一行两列</td><td><input type='radio' name='way" + t + "' value='three' />一行三列</td><td><input type='radio' name='way" + t + "' value='four'  />一行四列</td></tr>" +
                      "<tr><td align='right'>显示设置：</td><td><input type='radio' name='displaySet" + t + "' value='1' />图片</td><td><input type='radio' name='displaySet" + t + "' value='2' />文字</td><td><input type='radio' name='displaySet" + t + "' value='3' checked='checked' />图文</td></tr>" +
                      "<tr><td align='right'>高度：</td><td colspan='2'><div class='sliding'></div></td><td><span class='amount'></span>px</td></tr>" +
                  "</table><p class='increase' onclick='addDH(this)'>增加</p></div>";
    $(divhtml).appendTo(".attribute_frame");
    if ($("#module" + t).find("ul").is(".dh_style4")) {
        $("#addAttr" + t + " input[value='four']").attr("checked", "checked");
    } else if ($("#module" + t).find("ul").is(".dh_style3")) {
        $("#addAttr" + t + " input[value='three']").attr("checked", "checked");
    } else {
        $("#addAttr" + t + " input[value='two']").attr("checked", "checked");
    }
    if ($("#module" + t).find("ul li a img").first().css("display") == "block" && $("#module" + t).find("ul li a span").first().css("display")=="none") {
        $("#addAttr" + t + " input[value='1']").attr("checked", "checked");
    } else if ($("#module" + t).find("ul li a img").first().css("display") == "none" && $("#module" + t).find("ul li a span").first().css("display") == "block") {
        $("#addAttr" + t + " input[value='2']").attr("checked", "checked");
    } else {
        $("#addAttr" + t + " input[value='3']").attr("checked", "checked");
    }
    var htmldiv = "";
    $("#module" + t).find("ul li").each(function () {
        var p = $(this).attr("n");
        var fontColor = $(this).find("a span").css("color");
        var bgColor = $(this).find("a span").css("background");
        var imgSrc = $(this).find("a img").attr("src");
        var hrefval = $(this).find("a").attr("href");
        htmldiv += "<table id='nav_li_attr" + p + "' class='nav_li_attr' n='" + p + "' cellpadding='0' cellspacing='0' border='0'>" +
					  "<tr><td>导航图片:</td><td><button type='button' onclick='chooseImg(this,\"DaoHang\")'>选择图片</button></td><td><div class='changeBtn'><span onclick='moveUp(this)'>上移</span><span onclick='moveDwon(this)'>下移</span><span onclick='moveDel(this)'>删除</span></div></td></tr>" +
					  "<tr><td></td><td><img src='" + imgSrc + "'></td></tr>" +
					  "<tr><td></td><td style='color:#828282'>建议尺寸:150px × 150px</td></tr>" +
					  "<tr><td>导航名称:</td><td colspan='2'><input type='text' value='" + $(this).find("span").text() + "' class='name_shop' /><input type='text' id='hue-demo' class='form-control demo' data-control='hue' value='" + RGBToHex(fontColor) + "'></td></tr>" +
					  "<tr><td>背景颜色:</td><td><input type='text' id='demo-hue' class='form-control demo' data-control='hue' value='" + RGBToHex(bgColor) + "'></td></tr>" +
					  "<tr><td>导航链接:</td><td><input type='text' value='" + hrefval + "' class='href_shop' /></td><td><select onchange='selectchg(this)'>" +
                          "<option value ='/Vshop/index.aspx'>首页</option>" +
                          "<option value ='/Vshop/ProductSearch.aspx'>分类页面</option>" +
                          "<option value ='/Vshop/ShoppingCart.aspx'>购物车</option>" +
                          "<option value ='/Vshop/ProductList.aspx'>商品列表</option>" +
                          "<option value ='/Vshop/ArticleCategory.aspx'>新闻列表</option>" +
                          "<option value ='/Vshop/MemberCenter.aspx'>用户中心</option>" +
                          "<option value ='/Vshop/groupbuylist.aspx'>团购活动</option>" +
                          "<option value ='/Vshop/CountDownproductlist.aspx'>限时限购</option>" +
                          "<option value ='/Vshop/cutdownlist.aspx '>砍价活动</option>" +
                          "<option value ='/Vshop/myvantages.aspx'>积分兑换</option>" +
                          "<option value ='/Vshop/myredpager.aspx'>我的优惠券</option>" +
                          "<option value ='/Vshop/SaleService.aspx'>售后服务</option>" +
                          "<option value =''>自定义链接</option>" +
                      "</select></td></tr>" +
				  "</table>";
    });
    $("#addAttr" + t + " p").before(htmldiv);

    $("#addAttr" + t + " .name_shop").each(function () {
        $(this).change(function () {
            var p = $(this).parents("table").attr("n");
            $("#module" + t + " #nav_li" + p + " span").html($(this).val());
        });
    });
    $("#addAttr" + t + " #hue-demo").each(function () {
        $(this).change(function () {
            var p = $(this).parents("table").attr("n");
            $("#module" + t + " #nav_li" + p + " span").css("color", $(this).val());
        });
    });
    $("#addAttr" + t + " .href_shop").each(function () {
        $(this).change(function () {
            var p = $(this).parents("table").attr("n");
            $("#module" + t + " #nav_li" + p + " a").attr("href", $(this).val());
        });
    });
    $("#addAttr" + t + " #demo-hue").each(function () {
        $(this).change(function () {
            var p = $(this).parents("table").attr("n");
            $("#module" + t + " #nav_li" + p + " .wb_txt").css("background-color", $(this).val());
        });
    });
    hrefset(t);
}
function hrefset(t) {
    $("#module" + t + " ul li").each(function () {
        var p = $(this).attr("n");
        switch ($(this).find("a").attr("href")) {
            case "/Vshop/index.aspx":
                $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='/Vshop/index.aspx']").attr("selected", "selected");
                break;
            case "/Vshop/ProductSearch.aspx":
                $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='/Vshop/ProductSearch.aspx']").attr("selected", "selected");
                break;
            case "/Vshop/ShoppingCart.aspx":
                $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='/Vshop/ShoppingCart.aspx']").attr("selected", "selected");
                break;
            case "/Vshop/ProductList.aspx":
                $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='/Vshop/ProductList.aspx']").attr("selected", "selected");
                break;
            case "/Vshop/ArticleCategory.aspx":
                $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='/Vshop/ArticleCategory.aspx']").attr("selected", "selected");
                break;
            case "/Vshop/MemberCenter.aspx":
                $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='/Vshop/MemberCenter.aspx']").attr("selected", "selected");
                break;
            case "/Vshop/groupbuylist.aspx":
                $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='/Vshop/groupbuylist.aspx']").attr("selected", "selected");
                break;
            case "/Vshop/CountDownproductlist.aspx":
                $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='/Vshop/CountDownproductlist.aspx']").attr("selected", "selected");
                break;
            case "/Vshop/cutdownlist.aspx":
                $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='/Vshop/cutdownlist.aspx']").attr("selected", "selected");
                break;
            case "/Vshop/myvantages.aspx":
                $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='/Vshop/myvantages.aspx']").attr("selected", "selected");
                break;
            case "/Vshop/myredpager.aspx":
                $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='/Vshop/myredpager.aspx']").attr("selected", "selected");
                break;
            case "/Vshop/SaleService.aspx":
                $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='/Vshop/SaleService.aspx']").attr("selected", "selected");
                break;
            default:
                $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='']").attr("selected", "selected");
        }
    });
}
//文本数据绑定
function wbAttrBind(t) {
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
					  "<table class='nav_li_attr' cellpadding='0' cellspacing='0' border='0'>" +
					  "<tr><td><script id='editor" + t + "' type='text/plain' style='width:500px;min-height:200px;'>" + $('#module'+t+' .wb_txt').html() + "</script></td></tr>" +
					  "</table>" +
				  "</div>";
    $(divhtml).appendTo(".attribute_frame");
    EditorU(t);
}
//搜索数据绑定
function ssAttrBind(t) {
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
                  "<p>可随意插入任何页面和位置，方便会员快速搜索商品。</p>" +
				  "</div>";
    $(divhtml).appendTo(".attribute_frame");
}
//滚动公告数据绑定
function gdAttrBind(t) {
    var fontColor = $("#module" + t + " .wb_txt a").css("color");
    var aHref = $("#module" + t + " .wb_txt a").attr("href");
    var atxt = $("#module" + t + " .wb_txt #scroll_begin").text().trim();
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
					  "<table class='nav_li_attr' cellpadding='0' cellspacing='0' border='0'>" +
						  "<tr><td>文本内容：</td><td><textarea rows='3' cols='50' >" + atxt + "</textarea></td></tr>" +
						  "<tr><td>文本链接：</td><td><input type='text' class='href_wb' value='"+aHref+"'></td></tr>" +
						  "<tr><td>文本颜色：</td><td><input type='text' id='hue-demo' class='form-control demo' data-control='hue' value='" + RGBToHex(fontColor) + "'></td></tr>" +
					  "</table>" +
				  "</div>";
    $(divhtml).appendTo(".attribute_frame");
    ScrollImgLeft();
}
//图片数据绑定
function tpAttrBind(t) {
    n = 1;
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
					  "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
						  "<tr><td align='right'>显示方式：</td><td><input type='radio' name='mode" + t + "' value='two' />一行一张</td><td><input type='radio' name='mode" + t + "' value='three' />一行两张</td><td><input type='radio' name='mode" + t + "' value='four' />一行三张</td></tr>" +
						  "<tr><td align='right'>显示文字：</td><td><input type='radio' name='txt" + t + "' value='yes' />是</td><td><input type='radio' name='txt" + t + "' value='no' />否</td><td></td></tr>" +
					  "</table>" +
					  "<p class='increase' onclick='addTP(this)'>增加</p>" +
				  "</div>";
    $(divhtml).appendTo(".attribute_frame");
    if ($("#module" + t).find("ul").is(".tp_style1")) {
        $("#addAttr" + t + " input[value='two']").attr("checked", "checked");
    } else if ($("#module" + t).find("ul").is(".tp_style2")) {
        $("#addAttr" + t + " input[value='three']").attr("checked", "checked");
    } else {
        $("#addAttr" + t + " input[value='four']").attr("checked", "checked");
    }
    if ($("#module" + t).find("ul li span").first().css("display") == "block") {
        $("#addAttr" + t + " input[value='yes']").attr("checked", "checked");
    } else {
        $("#addAttr" + t + " input[value='no']").attr("checked", "checked");
    }
    var htmldiv = "";
    $("#module" + t).find("ul li").each(function () {
        var p = $(this).attr("n");
        var atxt = $(this).find("span").text();
        var fontColor = $(this).find("span").css("color");
        var imgSrc = $(this).find("img").attr("src");
        var aHref = $("#module" + t + " .wb_txt a").attr("href");
        var hrefval = $(this).find("a").attr("href");
        htmldiv += "<table id='nav_li_attr" + p + "' class='nav_li_attr' n='" + p + "' cellpadding='0' cellspacing='0' border='0'>" +
					  "<tr><td>图片:</td><td width='238px'><button onclick='chooseImg(this,\"TuPian\")' type='button' >选择图片</button></td><td><div class='changeBtn'><span onclick='moveUp(this)'>上移</span><span onclick='moveDwon(this)'>下移</span><span onclick='moveDel(this)'>删除</span></div></td></tr>" +
					  "<tr><td></td><td><img src='" + imgSrc + "'/></td></tr>" +
					  "<tr><td>文字说明:</td><td><input type='text' value='" + atxt + "' class='name_shop' /></td></tr>" +
					  "<tr><td></td><td><input type='text' id='hue-demo' class='form-control demo' data-control='hue' value='" + RGBToHex(fontColor) + "'></td></tr>" +
					  "<tr><td>图片链接:</td><td><input type='text' value='" + hrefval + "' class='href_shop' /><td><select onchange='selectchg(this)'>" +
                          "<option value ='/Vshop/index.aspx'>首页</option>" +
                          "<option value ='/Vshop/ProductSearch.aspx'>分类页面</option>" +
                          "<option value ='/Vshop/ShoppingCart.aspx'>购物车</option>" +
                          "<option value ='/Vshop/ProductList.aspx'>商品列表</option>" +
                          "<option value ='/Vshop/ArticleCategory.aspx'>新闻列表</option>" +
                          "<option value ='/Vshop/MemberCenter.aspx'>用户中心</option>" +
                          "<option value ='/Vshop/groupbuylist.aspx'>团购活动</option>" +
                          "<option value ='/Vshop/CountDownproductlist.aspx'>限时限购</option>" +
                          "<option value ='/Vshop/cutdownlist.aspx '>砍价活动</option>" +
                          "<option value ='/Vshop/myvantages.aspx'>积分兑换</option>" +
                          "<option value ='/Vshop/myredpager.aspx'>我的优惠券</option>" +
                          "<option value ='/Vshop/SaleService.aspx'>售后服务</option>" +
                          "<option value =''>自定义链接</option>" +
                      "</td></select></td></tr>" +
				   "</table>";
    });
    $("#addAttr" + t + " p").before(htmldiv);
    $("#addAttr" + t + " .name_shop").each(function () {
        $(this).change(function () {
            var p = $(this).parents("table").attr("n");
            alert(t);
            alert(p);
            $("#module" + t + " #nav_li" + p + " span").html($(this).val());
        });
    });
    $("#addAttr" + t + " #hue-demo").each(function () {
        $(this).change(function () {
            var p = $(this).parents("table").attr("n");
            $("#module" + t + " #nav_li" + p + " span").css("color", $(this).val());
        });
    });
    $("#addAttr" + t + " .href_shop").each(function () {
        $(this).change(function () {
            var k = $(this).parents("table").attr("n");
            $("#module" + t + " #nav_li" + k + " a").attr("href", $(this).val());
        });
    });
    hrefset(t);
}
//幻灯片数据绑定
function hdpAttrBind(t) {
    n = 1;
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
                  "<p class='increase' onclick='addHDP(this)'>增加</p></div>";
    $(divhtml).appendTo(".attribute_frame");
    var htmldiv = "";
    $("#module" + t).find("ul li").each(function () {
        var p = $(this).attr("n");
        var imgSrc = $(this).find("img").attr("src");
        var aHref = $(this).find("a").attr("href");
        var hrefval = $(this).find("a").attr("href");
        htmldiv += "<table id='nav_li_attr" + p + "' class='nav_li_attr' n='" + p + "' cellpadding='0' cellspacing='0' border='0'>" +
                "<tr><td>幻灯片图片:</td><td><button onclick='chooseImg(this,\"HuanDeng\")' type='button' >选择图片</button></td><td><div class='changeBtn'><span onclick='moveUp(this)'>上移</span><span onclick='moveDwon(this)'>下移</span><span onclick='moveDel(this)'>删除</span></div></td></tr>" +
                "<tr><td></td><td><img src='" + imgSrc + "' /></td></tr>" +
                "<tr><td></td><td style='color:#828282'>建议尺寸:750px × 400px</td></tr>" +
				"<tr><td>幻灯片链接:</td><td><input type='text' value='" + hrefval + "' class='href_shop' /></td><td><select onchange='selectchg(this)'>" +
                          "<option value ='/Vshop/index.aspx'>首页</option>" +
                          "<option value ='/Vshop/ProductSearch.aspx'>分类页面</option>" +
                          "<option value ='/Vshop/ShoppingCart.aspx'>购物车</option>" +
                          "<option value ='/Vshop/ProductList.aspx'>商品列表</option>" +
                          "<option value ='/Vshop/ArticleCategory.aspx'>新闻列表</option>" +
                          "<option value ='/Vshop/MemberCenter.aspx'>用户中心</option>" +
                          "<option value ='/Vshop/groupbuylist.aspx'>团购活动</option>" +
                          "<option value ='/Vshop/CountDownproductlist.aspx'>限时限购</option>" +
                          "<option value ='/Vshop/cutdownlist.aspx '>砍价活动</option>" +
                          "<option value ='/Vshop/myvantages.aspx'>积分兑换</option>" +
                          "<option value ='/Vshop/myredpager.aspx'>我的优惠券</option>" +
                          "<option value ='/Vshop/SaleService.aspx'>售后服务</option>" +
                    "<option value =''>自定义链接</option>" +
                "</select></td></tr>" +
             "</table>";
    });
    $("#addAttr" + t + " p").before(htmldiv);
    $("#addAttr" + t + " .href_shop").each(function () {
        $(this).change(function () {
            var k = $(this).parents("table").attr("n");
            $("#module" + t + " #nav_li" + k + " a").attr("href", $(this).val());
        });
    });
    hrefset(t);
}
//商品列表数据绑定
function splbAttrBind(t) {
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
					  "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
						  "<tr><td align='right'>显示方式：</td><td><input type='radio' name='composition" + t + "' value='manner1' checked='checked' />小图</td><td><input type='radio' name='composition" + t + "' value='manner2' />大图</td><td><input type='radio' name='composition" + t + "' value='manner3'/>列表</td><td><input type='radio' name='composition" + t + "' value='manner4' />一大两小</td></tr>" +
  						  "<tr><td align='right'>显示数量：</td><td><input type='radio' name='bit" + t + "' value='6' checked='checked' />6个</td><td><input type='radio' name='bit" + t + "' value='12' />12个</td><td><input type='radio' name='bit" + t + "' value='18' />18个</td></tr>" +
                          "<tr><td align='right'>字体颜色:</td><td colspan='4'><input type='text' class='form-control demo list-color' data-control='hue' value='#000'></td></tr>" +
                          "<tr><td align='right'>背景颜色:</td><td colspan='4'><input type='text' id='demo-hue' class='form-control demo list-bgcolor' data-control='hue' value='#fff'></td></tr>" +
  						  "<tr><td align='right'>第一优先级：</td><td colspan='4'><select id='show1' name='firstPriority'>" +
						      "<option value='1'>序号越大越靠前</option>" +
						      "<option value='2'>最热的排在前面</option>" +
						      "<option value='3'>创建时间越晚越靠前</option>" +
						      "<option value='4'>创建时间越早越靠前</option>" +
						      "<option value='5' selected='selected'>销量越高越靠前</option>" +
						      "<option value='6'>销量越低越靠前</option>" +
  						  "</select></td></tr>" +
  						  "<tr><td align='right'>第二优先级：</td><td colspan='4'><select id='show2' name='firstPriority'>" +
						      "<option value='1'>序号越大越靠前</option>" +
						      "<option value='2'>最热的排在前面</option>" +
						      "<option value='3' selected='selected'>创建时间越晚越靠前</option>" +
						      "<option value='4'>创建时间越早越靠前</option>" +
						      "<option value='5'>销量越高越靠前</option>" +
						      "<option value='6'>销量越低越靠前</option>" +
  						  "</select></td></tr>" +
  						  "<tr><td align='right'>第三优先级：</td><td colspan='4'><select id='show3' name='firstPriority'>" +
						      "<option value='1'>序号越大越靠前</option>" +
						      "<option value='2' selected='selected'>最热的排在前面</option>" +
						      "<option value='3'>创建时间越晚越靠前</option>" +
						      "<option value='4'>创建时间越早越靠前</option>" +
						      "<option value='5'>销量越高越靠前</option>" +
						      "<option value='6'>销量越低越靠前</option>" +
  						  "</select></td></tr>" +
					  "</table>" +
				  "</div>";
    $(divhtml).appendTo(".attribute_frame");
    $("#addAttr" + t + " .list-bgcolor").val(RGBToHex($("#module" + t + " ul").eq(0).css("background-color")));
    $("#addAttr" + t + " .list-color").val(RGBToHex($("#module" + t + " ul li p").css("color")));
    $("#addAttr" + t + " .list-bgcolor").change(function () {
        var m = $(this).parents("table").parent().attr("i");
        $("#module" + m + " ul").css("background", $(this).val());
    });
    $("#addAttr" + t + " .list-color").change(function () {
        var m = $(this).parents("table").parent().attr("i");
        $("#module" + m + " ul p").css("color", $(this).val());
    });
    if ($("#module" + t).find("ul").is(".manner1")) {
        $("#addAttr" + t + " input[value='manner1']").attr("checked", "checked");
    } else if ($("#module" + t).find("ul").is(".manner2")) {
        $("#addAttr" + t + " input[value='manner2']").attr("checked", "checked");
    } else if ($("#module" + t).find("ul").is(".manner3")) {
        $("#addAttr" + t + " input[value='manner3']").attr("checked", "checked");
    } else {
        $("#addAttr" + t + " input[value='manner4']").attr("checked", "checked");
    }
    if ($("#module" + t + " ul li").length == 6) {
        $("#addAttr" + t + " input[value='6']").attr("checked", "checked");
    } else if ($("#module" + t + " ul li").length == 12) {
        $("#addAttr" + t + " input[value='12']").attr("checked", "checked");
    } else {
        $("#addAttr" + t + " input[value='18']").attr("checked", "checked");
    }

    var sorts = $("#module" + t).attr("sort").split('♦');
        $("#show1 option[value=" + sorts[0] + "]").attr("selected", "selected");
        $("#show2 option[value=" + sorts[1] + "]").attr("selected", "selected");
        $("#show3 option[value=" + sorts[2] + "]").attr("selected", "selected");
}

//空白数据绑定
function kbAttrBind(t) {
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
					  "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
						  "<tr><td align='right'>高度：</td><td><div class='sliding'></div></td><td width='30px'><span class='amount'></span>px</td></tr>" +
					  "</table>" +
				  "</div>";
    $(divhtml).appendTo(".attribute_frame");
}