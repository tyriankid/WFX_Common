$(function(){
	$(".gridly").children("div").each(function(){
		var t=$(this).attr("i");
		switch($(this).attr("name")){
		    case "ShuRuKuang":
				wzAttrBind(t);
			break;
		    case "ShiJian":
		        rqAttrBind(t);
		    break;
		    case "XuanXiang":
		        wbtpAttrBind(t);
		    break;
		    case "WenBen":
		        wbAttrBind(t);
			break;
		    case "TuPian":
		        tpAttrBind(t);
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

//文字数据绑定
function wzAttrBind(t) {
    n = 1;
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
				        "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
                        "<tr><td colspan='2' align='center'><h4>文字控件</h4><td></tr>" +
				        "<tr><td align='right'>标题：</td><td><input type='text' name='title' value='" + $("#module" + t + " h3").html() + "' maxlength='100'></td></tr>" +
				        "<tr><td align='right'>副标题：</td><td><input type='text' name='subtitle' value='" + $("#module" + t + " h4").html() + "' maxlength='100'></td></tr>" +
                        "<tr><td align='right'>文本框高度：</td><td><label><input type='radio' name='input_type" + t + "' value='0'>单行</label><label><input type='radio' name='input_type" + t + "' value='1'>多行</label></td></tr>" +
                        "<tr><td align='right'>是否必填：</td><td><label><input name='required" + t + "' type='checkbox'>必须填写</label></td></tr>" +
				        "</table>" +
			        "</div>";
    $(divhtml).appendTo(".attribute_frame");
    if ($("#module" + t + " input").attr("type") == "text") {
        $("#addAttr" + t + " input[value='0']").attr("checked", "checked");
    }else{
        $("#addAttr" + t + " input[value='1']").attr("checked", "checked");
    }
    if ($("#module" + t).attr("bt") == "true") {
        $("#addAttr" + t + " input[name='required" + t + "']").attr("checked", true);
        $("#module" + t + " h3").after("<span class='must'>(必填)</span>");
    } else {
        $("#addAttr" + t + " input[name='required" + t + "']").attr("checked", false);
        $("#module" + t + " .must").remove();
    }
}
//日期数据绑定
function rqAttrBind(t) {
    n = 1;
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
					  "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
                        "<tr><td colspan='2' align='center'><h4>日期控件</h4><td></tr>" +
						"<tr><td align='right'>标题：</td><td><input type='text' name='title' value='" + $("#module" + t + " h3").html() + "' maxlength='100'></td></tr>" +
						"<tr><td align='right'>副标题：</td><td><input type='text' name='subtitle' value='" + $("#module" + t + " h4").html() + "' maxlength='100'></td></tr>" +
                        "<tr><td align='right'>是否必填：</td><td><label><input name='required" + t + "' type='checkbox' value='0'>必须填写</label></td></tr>" +
					  "</table>" +
				  "</div>";
    $(divhtml).appendTo(".attribute_frame");
    if ($("#module" + t).attr("bt") == "true") {
        $("#addAttr" + t + " input[name='required" + t + "']").attr("checked", true);
        $("#module" + t + " h3").after("<span class='must'>(必填)</span>");
    } else {
        $("#addAttr" + t + " input[name='required" + t + "']").attr("checked", false);
        $("#module" + t + " .must").remove();
    }
}
//投票模块数据绑定
function wbtpAttrBind(t) {
    n = 1;
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
					  "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
                        "<tr><td colspan='2' align='center'><h4>文本投票</h4><td></tr>" +
						"<tr><td align='right'>标题：</td><td><input type='text' name='title' value='" + $("#module" + t + " h3").html() + "' maxlength='100'></td></tr>" +
						"<tr><td align='right'>副标题：</td><td><input type='text' name='subtitle' value='" + $("#module" + t + " h4").html() + "' maxlength='100'></td></tr>" +
                        "<tr><td align='right'>单选/多选：</td><td><label><input type='radio' name='input_choice" + t + "' value='0'>单选</label><label><input type='radio' name='input_choice" + t + "' value='1'>多选</label></td></tr>" +
                        "<tr><td align='right'>是否必填：</td><td><label><input name='required" + t + "' type='checkbox' value=''>必须填写</label></td></tr>" +
                        "<tr><td colspan='3' align='center' class='line-bg'><span>选项</span></td></tr>" +
					  "</table>" +
                      "<p class='increase' onclick='addWBTP(this)'>增加</p>" +
				  "</div>";
    $(divhtml).appendTo(".attribute_frame");
    if ($("#module" + t).attr("bt") == "true") {
        $("#addAttr" + t + " input[name='required" + t + "']").attr("checked", true);
        $("#module" + t + " h3").after("<span class='must'>(必填)</span>");
    } else {
        $("#addAttr" + t + " input[name='required" + t + "']").attr("checked", false);
        $("#module" + t + " .must").remove();
    }
    if ($("#module" + t + " ul li input").attr("type") == "radio") {
        $("#addAttr" + t + " input[value='0']").attr("checked", true);
        $("#module" + t + " ul li").css("background-position-y", "10px");
    } else {
        $("#addAttr" + t + " input[value='1']").attr("checked", true);
        $("#module" + t + " ul li").css("background-position-y", "-29px");
    }
    var htmldiv = "";
    $("#module" + t + " ul li").each(function () {
        var p = $(this).attr("n");
        htmldiv += "<tr id='nav_li_attr" + p + "' n='" + p + "'><td align='right'>选项名称：</td><td><input type='text' name='options' value='" + $(this).find("span").html() + "' maxlength='100'></td><td><div class='changeBtn' style='width: 80px'><span onclick='voteDel(this)'>删除</span></div></td></tr>";
    });
    $(htmldiv).appendTo("#addAttr" + t + " table");
}
//图片数据绑定
function tpAttrBind(t) {
    n = 1;
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
					  "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
						  "<tr><td align='right'>显示文字：</td><td><input type='radio' name='txt' value='yes' />是</td><td><input type='radio' name='txt' value='no' />否</td><td></td></tr>" +
					  "</table>" +
					  "<p class='increase' onclick='addTP(this)'>增加</p>" +
				  "</div>";
    $(divhtml).appendTo(".attribute_frame");
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