$(function(){
	$(".gridly").children("div").each(function(){
		var t=$(this).attr("i");
		switch($(this).attr("name")){
		    case "VDaoHang":
		        dhAttrBind(t);
			break;
		    case "VWenBen":
		        wbAttrBind(t);
			break;
		    case "VGunDong":
		        gdAttrBind(t);
			break;
		    case "VTuPian":
		        tpAttrBind(t);
			break;
		    case "VHuanDeng":
		        hdpAttrBind(t);
			break;
		    case "VBeiJing":
		        bgAttrBind(t);
			break;
		}
		$(this).click(function () {
			integrate(this,t);
		});

	    //高度

		if ($("#module" + t).attr("name") == "VGunDong") {
		    var dhheight = $("#module" + t + " .wb_txt").height();
		    $("#addAttr" + t + " .sliding").slider({
		        range: "max",
		        min: 0,
		        max: 100,
		        value: dhheight,
		        slide: function (event, ui) {
		            $("#addAttr" + t + " .amount_height").text(ui.value);
		            $("#module" + j).css("height", ui.value);
		            $("#module" + t + " .wb_txt").css("height", ui.value);
		            $("#module" + t + " .wb_txt").css("line-height", ui.value + "px");
		        }
		    });
		    $("#addAttr" + t + " .amount_height").text($("#addAttr" + t + " .sliding").slider("value"));
		    $("#module" + t + " .wb_txt").css("height", $("#addAttr" + t + " .sliding").slider("value"));
		    $("#module" + t + " .wb_txt").css("line-height", $("#addAttr" + t + " .sliding").slider("value") + "px");
		} else {
		    $("#module" + t + " ul").height("100%");
		    var dhheight = $("#module" + t).height() / ($("#module" + t).parent().height()) * 100;
		    $("#addAttr" + t + " .sliding").slider({
		        range: "max",
		        min: 0,
		        max: 300,
		        value: dhheight,
		        slide: function (event, ui) {
		            $("#addAttr" + t + " .amount_height").text(ui.value);
		            $("#module" + t).css("height", ui.value + "%");
		            $("#module" + t).css("line-height", ui.value + "%");
		            percentage(t);
		        }
		    });
		    $("#addAttr" + t + " .amount_height").text($("#addAttr" + t + " .sliding").slider("value"));
		    $("#module" + t).css("height", $("#addAttr" + t + " .sliding").slider("value") + "%");
		    $("#module" + t).css("line-height", $("#addAttr" + t + " .sliding").slider("value") + "%");
		}
	    //宽度
		var dhwidth = $("#module" + t).width() / ($("#module" + t).parent().width()) * 100;
		$("#addAttr" + t + " .widthing").slider({
		    range: "max",
		    min: 20,
		    max: 100,
		    value: dhwidth,
		    slide: function (event, ui) {
		        $("#addAttr" + t + " .amount_width").text(ui.value);
		        $("#module" + t).css("width", ui.value + "%");
		    }
		});
		$("#addAttr" + t + " .amount_width").text($("#addAttr" + t + " .widthing").slider("value"));
		$("#module" + t).css("width", $("#addAttr" + t + " .widthing").slider("value") + "%");

		percentage(t);
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
//导航数据绑定
function dhAttrBind(t) {
    n = 1;
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
					  "<table class='nature' cellpadding='0' cellspacing='0' border='0'>"+
                      "<tr><td align='right'>显示方式：</td><td><input type='radio' name='way' value='one' />一行一列</td><td><input type='radio' name='way' value='two' />一行两列</td><td><input type='radio' name='way' value='three' />一行三列</td><td><input type='radio' name='way' value='four'  />一行四列</td></tr>" +
                      "<tr><td align='right'>显示设置：</td><td><input type='radio' name='displaySet' value='1' />图片</td><td><input type='radio' name='displaySet' value='2' />文字</td></tr>" +
                      "<tr><td align='right'>高度设置：</td><td colspan='3'><div class='sliding'></div></td><td><span class='amount_height'></span>%</td></tr>" +
                      "<tr><td align='right'>宽度设置：</td><td colspan='3'><div class='widthing'></div></td><td><span class='amount_width'></span>%</td></tr>" +
                  "</table><p class='increase' onclick='addDH(this)'>增加</p></div>";
    $(divhtml).appendTo(".attribute_frame");
    if ($("#module" + t).find("ul").is(".dh_style4")) {
        $("#addAttr" + t + " input[value='four']").attr("checked", "checked");
    } else if ($("#module" + t).find("ul").is(".dh_style3")) {
        $("#addAttr" + t + " input[value='three']").attr("checked", "checked");
    } else if ($("#module" + t).find("ul").is(".dh_style2")) {
        $("#addAttr" + t + " input[value='two']").attr("checked", "checked");
    }else{
        $("#addAttr" + t + " input[value='one']").attr("checked", "checked");
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
        var bgColor = $(this).find("a").css("background-color");
        var imgSrc = $(this).find("a img").attr("src");
        var hrefval = $(this).find("a").attr("href");
        htmldiv += "<table id='nav_li_attr" + p + "' class='nav_li_attr' n='" + p + "' cellpadding='0' cellspacing='0' border='0'>" +
					  "<tr><td>导航图片:</td><td><button type='button' onclick='chooseImg(this,\"VDaoHang\")'>选择图片</button></td><td><div class='changeBtn'><span onclick='moveUp(this)'>上移</span><span onclick='moveDwon(this)'>下移</span><span onclick='moveDel(this)'>删除</span></div></td></tr>" +
					  "<tr><td></td><td><img src='" + imgSrc + "'></td></tr>" +
					  "<tr><td></td><td style='color:#828282'>建议尺寸:150px × 150px</td></tr>" +
					  "<tr><td>导航名称:</td><td colspan='2'><input type='text' value='" + $(this).find("span").text() + "' class='name_shop' /><input type='text' id='hue-demo' class='form-control demo' data-control='hue' value='" + RGBToHex(fontColor) + "'></td></tr>" +
					  "<tr><td>背景颜色:</td><td><input type='text' id='demo-hue' class='form-control demo' data-control='hue' value='" + RGBToHex(bgColor) + "'></td></tr>" +
					  "<tr><td>导航链接:</td><td><input type='text' value='" + hrefval + "' class='href_shop' /></td><td><select onchange='selectchg(this)'>" +
                        "<option value ='/Vshop/WgwArticleCategory.aspx'>分类列表</option>" +
                        "<option value ='2'>文章列表</option>" +
                        "<option value ='1'>文章详情</option>" +
                        "<option value ='3'>二级页链接</option>" +
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
            $("#module" + t + " #nav_li" + p + " a").css("background-color", $(this).val());
        });
    });
    hrefset(t);
}
function hrefset(t) {
    $("#module" + t + " ul li").each(function () {
        var p = $(this).attr("n");
        if($(this).find("a").attr("href") == "/Vshop/WgwArticleCategory.aspx"){
            $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='/Vshop/WgwArticleCategory.aspx']").attr("selected", "selected");
        } else if ($(this).find("a").attr("href").indexOf("WgwArticles") > 0) {
            $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='2']").attr("selected", "selected");
        } else if ($(this).find("a").attr("href").indexOf("WgwArticleDetails") > 0) {
            $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='1']").attr("selected", "selected");
        } else if ($(this).find("a").attr("href").indexOf("SecondPage") > 0) {
            $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='3']").attr("selected", "selected");
        } else if ($(this).find("a").attr("href") == "/Vshop/SaleService.aspx") {
            $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='/Vshop/SaleService.aspx']").attr("selected", "selected");
        } else {
            $("#addAttr" + t + " #nav_li_attr" + p + " select option[value ='']").attr("selected", "selected");
        }
    });
}
//文本数据绑定
function wbAttrBind(t) {
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
					  "<table class='nav_li_attr' cellpadding='0' cellspacing='0' border='0'>" +
					  "<tr><td><script id='editor" + t + "' type='text/plain' style='width:500px;min-height:200px;'>" + $('#module' + t + ' .wb_txt').html() + "</script></td></tr>" +
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
    ScrollImgLeft(t);
}
//图片数据绑定
function tpAttrBind(t) {
    n = 1;
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
					  "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
						  "<tr><td align='right'>显示方式：</td><td><input type='radio' name='mode' value='two' />一行一张</td><td><input type='radio' name='mode' value='three' />一行两张</td><td><input type='radio' name='mode' value='four' />一行三张</td></tr>" +
						  "<tr><td align='right'>显示文字：</td><td><input type='radio' name='txt' value='yes' />是</td><td><input type='radio' name='txt' value='no' />否</td><td></td></tr>" +
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
					  "<tr><td>字体颜色:</td><td><input type='text' id='hue-demo' class='form-control demo' data-control='hue' value='" + RGBToHex(fontColor) + "'></td></tr>" +
					  "<tr><td>图片链接:</td><td><input type='text' value='" + hrefval + "' class='href_shop' /><td><select onchange='selectchg(this)'>" +
                          "<option value ='/Vshop/WgwArticleCategory.aspx'>分类列表</option>" +
                          "<option value ='2'>文章列表</option>" +
                          "<option value ='1'>文章详情</option>" +
                          "<option value ='3'>二级页链接</option>" +
                          "<option value ='/Vshop/SaleService.aspx'>售后服务</option>" +
                          "<option value =''>自定义链接</option>" +
                      "</td></select></td></tr>" +
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
                    "<option value ='/Vshop/WgwArticleCategory.aspx'>分类列表</option>" +
                    "<option value ='2'>文章列表</option>" +
                    "<option value ='1'>文章详情</option>" +
                    "<option value ='3'>二级页链接</option>" +
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
//空白数据绑定
function kbAttrBind(t) {
    var divhtml = "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
					  "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
						  "<tr><td align='right'>高度：</td><td><div class='sliding'></div></td><td width='30px'><span class='amount_height'></span>px</td></tr>" +
					  "</table>" +
				  "</div>";
    $(divhtml).appendTo(".attribute_frame");
}
//背景数据绑定
function bgAttrBind(t) {
    var htmldiv = "";
    $("#module" + t).find("img").each(function () {
        var imgSrc = $(this).attr("src");
        htmldiv += "<div id='addAttr" + t + "' class='addAttr' i='" + t + "'>" +
                  "<table class='nav_li_attr' cellpadding='0' cellspacing='0' border='0'>" +
				    "<tr><td>幻灯片图片:</td><td><button onclick='chooseImg(this,\"BeiJing\")' type='button' >选择图片</button></td></tr>" +
				    "<tr><td></td><td><img src='" + imgSrc + "' /></td></tr>" +
					"<tr><td></td><td style='color:#828282'>建议尺寸:750px × 400px</td></tr>" +
					"<tr><td>幻灯片链接:</td><td><input type='text' value='幻灯片链接' class='href_shop' /></td></tr>" +
				 "</table>" +
				  "</div>";
    });
    $(htmldiv).appendTo(".attribute_frame");
}