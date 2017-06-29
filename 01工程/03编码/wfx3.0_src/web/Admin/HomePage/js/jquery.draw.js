var i = $(".gridly").children().length + 1;
$(function () {
    $(".draw li").each(function () {
        $(this).dblclick(function () {
            //导航模块--------导航模块--------导航模块
            if ($(this).is(".navigation")) {
                dh();
                dhAttr();
            }
            //搜索栏模块-----搜索栏模块------搜索栏模块
            if ($(this).is(".search")) {
                ss();
                ssAttr();
            }
            //文本模块--------文本模块--------文本模块
            if ($(this).is(".text")) {
                wb();
                wbAttr();
                EditorU(i);//富文本编辑器
            }
            //公告模块--------公告模块--------公告模块
            if ($(this).is(".notice")) {
                gg();
                ggAttr();
                ScrollImgLeft();//文字滚动
            }
            //图片模块--------图片模块--------图片模块
            if ($(this).is(".picture")) {
                tp();
                tpAttr();
            }
            //轮播图模块-----轮播图模块------轮播图模块
            if ($(this).is(".slide")) {
                hdp();
                hdpAttr();
            }
            //商品模块---商品模块--商品模块
            if ($(this).is(".goods")) {
                sp();
                spAttr();
            }
            //商品列表模块---商品列表模块--商品列表模块
            if ($(this).is(".list")) {
                splb();
                splbAttr();
            }
            //空白模块--------空白模块--------空白模块
            if ($(this).is(".space")) {
                kb();
                kbAttr();
            }
            //店招模块--------店招模块--------店招模块
            if ($(this).is(".signboard")) {
                dz();
                dzAttr();
            }
            //视频模块--------视频模块--------视频模块
            //if($(this).is(".video")){
            //	alert(9);
            //}
            //模块拖动
            //  		$('.gridly').gridly({
            //			    base: 50, // px 
            //			    gutter:5, // px
            //			    columns: 3
            //			});

            integrate("#module" + i, i);

            pos("#module" + i, i);  //双击后立即定位	

            $("#module" + i).click(function () {
                var j = $(this).attr("i");
                integrate(this, j);
            });
            i++;

            choseColor();
        });
    });
    $(".pitch").css("border", "none");
    $(".pitch p").hide();

    $(".splb ul li a img").each(function () {
        $(this).width($(this).parent().width());
        $(this).height($(this).parent().width());
    });
});
/*-----------页面一些方法--------------页面一些方法-------------------页面一些方法-----------------页面一些方法----------------*/
//一些操作整合
function integrate(e, j) {
    //隐藏所有属性模块，显示当前选中模块属性
    $(".addAttr").hide();
    $("#addAttr" + j).show();
    //模块添加删除按钮、选中状态
    if ($("#module" + j).find(".pitch").length == 0) {
        $("#module" + j).prepend("<div class='pitch'><p><a onclick='integrate()'>编辑</a><a onclick='delcon(this)'>删除</a></p></div>");
    }
    $("#module" + j).siblings().find(".pitch").css("border", "none");
    $("#module" + j).siblings().find(".pitch p").hide();
    $("#module" + j).find(".pitch").css("border", "2px #f90 dashed");
    $("#module" + j).find(".pitch p").show();
    //查询当前模块所处页面位置，设置模块属性位置
    pos(e, j);
    //jqueryUI滑块
    var dhheight = $("#module" + j + " .wb_txt").height();
    $("#addAttr" + j + " .sliding").slider({
        range: "max",
        min: 30,
        max: 100,
        value: dhheight,
        slide: function (event, ui) {
            $("#addAttr" + j + " .amount").text(ui.value);
            $("#module" + j + " .wb_txt").css("height", ui.value);
            $("#module" + j + " .wb_txt").css("line-height", ui.value + "px");
        }
    });
    $("#addAttr" + j + " .amount").text($("#addAttr" + j + " .sliding").slider("value"));
    $("#module" + j + " .wb_txt").css("height", $("#addAttr" + j + " .sliding").slider("value"));
    $("#module" + j + " .wb_txt").css("height", $("#addAttr" + j + " .sliding").slider("value") + "px");

    //属性切换按钮
    $("#addAttr" + j + " input").click(function () {
        if ($(this).attr("name") == "way" + j + "") {
            switch ($(this).attr("value")) {
                case "two":
                    $("#module" + j + " ul").attr("class", "dh_style2");
                    break;
                case "three":
                    $("#module" + j + " ul").attr("class", "dh_style3");
                    break;
                case "four":
                    $("#module" + j + " ul").attr("class", "dh_style4");
                    break;
                case "five":
                    $("#module" + j + " ul").attr("class", "dh_style5");
                    break;
            }
        }
        if ($(this).attr("name") == "displaySet" + j + "") {
            if ($(this).attr("value") == 1) {
                $("#module" + j + " ul li a img").css("display", "block");
                $("#module" + j + " ul li a span").css("display", "none");
            } else if ($(this).attr("value") == 2) {
                $("#module" + j + " ul li a img").css("display", "none");
                $("#module" + j + " ul li a span").css("display", "block");
            } else {
                $("#module" + j + " ul li a img").css("display", "block");
                $("#module" + j + " ul li a span").css("display", "block");
            }
        }
        if ($(this).attr("name") == "alignment" + j + "") {
            switch ($(this).attr("value")) {
                case "left":
                    $("#module" + j + " #scroll_begin a").css("text-align", "left");
                    break;
                case "center":
                    $("#module" + j + " #scroll_begin a").css("text-align", "center");
                    break;
                case "right":
                    $("#module" + j + " #scroll_begin a").css("text-align", "right");
                    break;
            }
        }
        if ($(this).attr("name") == "mode" + j + "") {
            switch ($(this).attr("value")) {
                case "two":
                    $("#module" + j + " ul").attr("class", "tp_style1");
                    break;
                case "three":
                    $("#module" + j + " ul").attr("class", "tp_style2");
                    break;
                case "four":
                    $("#module" + j + " ul").attr("class", "tp_style3");
                    break;
            }
        }
        if ($(this).attr("name") == "txt" + j + "") {
            if ($(this).attr("value") == "yes") {
                $("#module" + j + " ul li a span").css("display", "block");
            } else {
                $("#module" + j + " ul li a span").css("display", "none");
            }
        }
        if ($(this).attr("name") == "composition" + j + "") {
            alert($(this).attr("value"));
            switch ($(this).attr("value")) {
                case "manner1":
                    $("#module" + j + " ul").attr("class", "manner1");
                    break;
                case "manner2":
                    $("#module" + j + " ul").attr("class", "manner2");
                    break;
                case "manner3":
                    $("#module" + j + " ul").attr("class", "manner3");
                    break;
                case "manner4":
                    $("#module" + j + " ul").attr("class", "manner4");
                    break;
                case "manner5":
                    $("#module" + j + " ul").attr("class", "manner5");
                    break;
                case "manner6":
                    $("#module" + j + " ul").attr("class", "manner6");
                    break;
            }
        }
    });
    //属性改变按钮
    $("#addAttr" + j + " #hue-demo").change(function () {
        //店招文本
        $("#module" + j + " .shop_name").css("color", $(this).val());
        //滚动文本
        $("#module" + j + " #scroll_begin a").css("color", $(this).val());
        $("#module" + j + " #scroll_end a").css("color", $(this).val());
    });
    $("#addAttr" + j + " .name_shop").change(function () {
        //店招文本
        $("#module" + j + " .shop_name").html($(this).val());
    });
    $("#addAttr" + j + " textarea").change(function () {
        //滚动文本
        $("#module" + j + " #scroll_begin a").html($(this).val());
        $("#module" + j + " #scroll_end a").html($(this).val());
    });
    $("#addAttr" + j + " .href_wb").change(function () {
        $("#module" + j + " #scroll_begin a").attr("href", $(this).val());
        $("#module" + j + " #scroll_end a").attr("href", $(this).val());
    });
}
function pos(e, j) {
    //查询当前模块所处页面位置，设置模块属性位置
    var y = $(e).offset().top;
    var x = $(e).offset().left;
    $("#addAttr" + j).css("top", y);
    $("#tabbox" + j).css("top", y);
    $("html,body").animate({ scrollTop: y }, 0);
}
//富文本编辑器
function EditorU(j) {
    var ue = UE.getEditor('editor' + j, {
        toolbars: [[
            'fullscreen', 'source', '|', 'undo', 'redo', '|',
            'bold', 'italic', 'underline', 'fontborder', 'strikethrough', 'superscript', 'subscript', 'removeformat', 'formatmatch', 'autotypeset', 'blockquote', 'pasteplain', '|', 'forecolor', 'backcolor', 'insertorderedlist', 'insertunorderedlist', 'selectall', 'cleardoc', '|',
            'rowspacingtop', 'rowspacingbottom', 'lineheight', '|',
            'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|',
            'directionalityltr', 'directionalityrtl', 'indent', '|',
            'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify', '|', 'touppercase', 'tolowercase', '|',
            'link', 'unlink', '|', 'imagenone', 'imageleft', 'imageright', 'imagecenter', '|',
            'simpleupload', 'insertimage', 'emotion', 'scrawl', 'music', 'attachment', '|',
            'horizontal', 'date', 'time', 'spechars', '|',
            'inserttable', 'deletetable', 'mergecells', 'mergeright', 'mergedown', 'splittocells', 'splittorows', 'splittocols', 'charts',
        ]],
        autoHeightEnabled: true,
        autoFloatEnabled: true,
        initialStyle: 'p{line-height:1em; font-size: 16px; }',
        initialContent: '',
        autoClearinitialContent: true,
        focus: false,
    });
    ue.addListener("contentChange", function () {
        var s = ue.getContent();
        if ($(s).length == "") {
            var a = "<p>点此编辑『富文本』内容 ——&gt;</p><p>你可以对文字进行<strong>加粗</strong>、<em>斜体</em>、<span style='text-decoration: underline;'>下划线</span>、<span style='text-decoration: line-through;'>删除线</span>、文字<span style='color: rgb(0, 176, 240);'>颜色</span>、<span style='background-color: rgb(255, 192, 0); color: rgb(255, 255, 255);'>背景色</span>、以及字号<span style='font-size: 20px;'>大</span><span style='font-size: 14px;'>小</span>等简单排版操作。</p><p>还可以在这里加入表格了</p><table><tbody><tr><td width='93' valign='top' style='word-break: break-all;'>中奖客户</td><td width='93' valign='top' style='word-break: break-all;'>发放奖品</td><td width='93' valign='top' style='word-break: break-all;'>备注</td></tr><tr><td width='93' valign='top' style='word-break: break-all;'>猪猪</td><td width='93' valign='top' style='word-break: break-all;'>内测码</td><td width='93' valign='top' style='word-break: break-all;'><em><span style='color: rgb(255, 0, 0);'>已经发放</span></em></td></tr><tr><td width='93' valign='top' style='word-break: break-all;'>大麦</td><td width='93' valign='top' style='word-break: break-all;'>积分</td><td width='93' valign='top' style='word-break: break-all;'><a href='javascript: void(0);' target='_blank'>领取地址</a></td></tr></tbody></table><p style='text-align: left;'><span style='text-align: left;'>也可在这里插入图片、并对图片加上超级链接，方便用户点击。</span></p>"
            $("#module" + j + " .wb_txt").html(a);
        } else {
            $("#module" + j + " .wb_txt").html(s);
        }
    });
}
//上移按钮
function moveUp(e) {
    var a = $(e).parents("table").parent().attr("i");
    var b = $(e).parents("table").attr("n");
    var c = $(e).parents("table").prev("table").not(".nature");
    var d = $("#module" + a + " #nav_li" + b + "").prev("li");
    $(e).parents("table").insertBefore(c);
    $("#module" + a + " #nav_li" + b + "").insertBefore(d);
}
//下移按钮
function moveDwon(e) {
    var a = $(e).parents("table").parent().attr("i");
    var b = $(e).parents("table").attr("n");
    var c = $(e).parents("table").next("table").not(".nature");
    var d = $("#module" + a + " #nav_li" + b + "").next("li");
    $(e).parents("table").insertAfter(c);
    $("#module" + a + " #nav_li" + b + "").insertAfter(d);
}
//删除按钮
function moveDel(e) {
    var a = $(e).parents("table").parent().attr("i");
    var b = $(e).parents("table").attr("n");
    $(e).parents("table").remove();
    $("#module" + a + " #nav_li" + b + "").remove();
}
//垃圾桶删除方法
function delcon(e) {
    var r = confirm("是否确认删除");
    if (r == true) {
        var modeldiv = $(e).parent().parent().parent();
        var j = $(modeldiv).attr("i");
        $(modeldiv).remove();
        $("#addAttr" + j).remove();
        $("#tabbox" + j).remove();
    } else {
        return false;
    }
}
//颜色选择器
function choseColor() {
    $('.demo').each(function () {
        $(this).minicolors({
            control: $(this).attr('data-control') || 'hue',
            defaultValue: $(this).attr('data-defaultValue') || '',
            inline: $(this).attr('data-inline') === 'true',
            letterCase: $(this).attr('data-letterCase') || 'lowercase',
            opacity: $(this).attr('data-opacity'),
            position: $(this).attr('data-position') || 'bottom left',
            change: function (hex, opacity) {
                if (!hex) return;
                if (opacity) hex += ', ' + opacity;
                try {
                    console.log(hex);
                } catch (e) { }
            }
        });
    });
}
//图片上传后图显示
function onFileChange(sender, j) {
    var filename = sender.value;
    if (filename == "") {
        return "";
    }
    var ExName = filename.substr(filename.lastIndexOf(".") + 1).toUpperCase();

    if (ExName == "JPG" || ExName == "BMP" || ExName == "GIF" || ExName == "PNG") {
        //导航图片
        var n = $(sender).parents("table").attr("n");
        $("#nav_li" + n).find("img").attr("src", window.URL.createObjectURL(sender.files[0]));
        $("#nav_li_attr" + n).find("img").attr("src", window.URL.createObjectURL(sender.files[0]));
        //   	$("#nav_li"+n).attr("src",window.URL.createObjectURL(sender.files[0]));
        ///document.getElementById("img_Photo").src = window.URL.createObjectURL(sender.files[0]);
    }
    else {
        alert('请选择正确的图片格式！');
        sender.value = null;
        return false;
    }
}
function onChangeFile(sender, j) {
    var filename = sender.value;
    if (filename == "") {
        return "";
    }
    var ExName = filename.substr(filename.lastIndexOf(".") + 1).toUpperCase();
    if (ExName == "JPG" || ExName == "BMP" || ExName == "GIF" || ExName == "PNG") {
        //店招图片
        var m = $(sender).parents("div").attr("i");
        $("#module" + m).find("img").eq(j - 1).attr("src", window.URL.createObjectURL(sender.files[0]));
        $("#addAttr" + m).find("img").eq(j - 1).attr("src", window.URL.createObjectURL(sender.files[0]));
        ///document.getElementById("img_Photo").src = window.URL.createObjectURL(sender.files[0]);
    }
    else {
        alert('请选择正确的图片格式！');
        sender.value = null;
        return false;
    }
}
//超链接传值
function selectchg(e) {
    var a = $(e).parents("table").attr("n");
    var b = $(e).parents("table").parent().attr("i");
    $(e).parent().prev().find("input").val($(e).val());
    $("#module" + b + " ul #nav_li" + a + " a").attr("href", $(e).val());
}
/*-----------模块添加--------------模块添加-------------------模块添加-----------------模块添加----------------*/
//导航模块--------导航模块--------导航模块
function dh() {
    var divhtml = "<div id='module" + i + "' i='" + i + "' name='DaoHang' class='small dh'>" +
        "<ul class='dh_style4'>" +
        "</ul>" +
        "</div>";
    $(divhtml).appendTo(".gridly");
}
function dhAttr() {
    var divhtml = "<div id='addAttr" + i + "' class='addAttr' i='" + i + "'>" +
        "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
        "<tr><td align='right'>显示方式：</td><td><input type='radio' name='way" + i + "' value='two' />一行两列</td><td><input type='radio' name='way" + i + "' value='three' />一行三列</td><td><input type='radio' name='way" + i + "' value='four' checked='checked' />一行四列</td><td><input type='radio' name='way" + i + "' value='five' checked='checked' />一行五列</td></tr>" +
        "<tr><td align='right'>显示设置：</td><td><input type='radio' name='displaySet" + i + "' value='1' />图片</td><td><input type='radio' name='displaySet" + i + "' value='2' />文字</td><td><input type='radio' name='displaySet" + i + "' value='3' checked='checked' />图文</td></tr>" +
        "<tr><td align='right'>高度：</td><td colspan='2'><div class='sliding'></div></td><td><span class='amount'></span>px</td></tr>" +
        "</table>" +
        "<p class='increase' onclick='addDH(this)'>增加</p>" +
        "</div>";
    $(divhtml).appendTo(".attribute_frame");
}
function addDH(e) {
    var m = $(e).parents("div").attr("i");
    var n = 1;
    if ($("#module" + m + " ul li").length != 0) {
        var max = 0;
        $("#module" + m + " ul li").each(function () {
            var a = $(this).attr("n");
            if (a > max) {
                max = a;
                n = parseInt(max) + 1;
            }
        });
    }
    var divhtml1 = "<li id='nav_li" + n + "' n='" + n + "'><a href><img src='img/daohang.png' /><span class='wb_txt' style='display:none;'>导航</span></a></li>";
    var divhtml2 = "<li id='nav_li" + n + "' n='" + n + "'><a href><img src='img/daohang.png' style='display:none;' /><span class='wb_txt'>导航</span></a></li>";
    var divhtml3 = "<li id='nav_li" + n + "' n='" + n + "'><a href><img src='img/daohang.png' /><span class='wb_txt'>导航</span></a></li>";
    if ($("#addAttr" + m + " input[name='displaySet" + m + "']:checked").attr("value") == 1) {
        $(divhtml1).appendTo("#module" + m + " ul");
    }
    if ($("#addAttr" + m + " input[name='displaySet" + m + "']:checked").attr("value") == 2) {
        $(divhtml2).appendTo("#module" + m + " ul");
    }
    if ($("#addAttr" + m + " input[name='displaySet" + m + "']:checked").attr("value") == 3) {
        $(divhtml3).appendTo("#module" + m + " ul");
    }

    if ($("#addAttr" + m + " input[name='displaySet" + m + "']:checked").attr("value") == 1) {
        $("#module" + m + " ul li a img").css("display", "block");
        $("#module" + m + " ul li a span").css("display", "none");
    } else if ($("#addAttr" + m + " input[name='displaySet" + m + "']:checked").attr("value") == 2) {
        $("#module" + m + " ul li a img").css("display", "none");
        $("#module" + m + " ul li a span").css("display", "block");
    } else {
        $("#module" + m + " ul li a img").css("display", "block");
        $("#module" + m + " ul li a span").css("display", "block");
    }
    $("#module" + m + " .wb_txt").css("height", $("#addAttr" + m + " .sliding").slider("value"));
    $("#module" + m + " .wb_txt").css("line-height", $("#addAttr" + m + " .sliding").slider("value") + "px");
    var htmldiv = "<table id='nav_li_attr" + n + "' class='nav_li_attr' n='" + n + "' cellpadding='0' cellspacing='0' border='0'>" +
        "<tr><td>导航图片:</td><td><button type='button' onclick='chooseImg(this,\"DaoHang\")'>选择图片</button></td><td><div class='changeBtn'><span onclick='moveUp(this," + n + ")'>上移</span><span onclick='moveDwon(this," + n + ")'>下移</span><span onclick='moveDel(this," + n + ")'>删除</span></div></td></tr>" +
        "<tr><td></td><td><img src='img/daohang.png'/></td></tr>" +
        "<tr><td></td><td style='color:#828282'>建议尺寸:150px × 150px</td></tr>" +
        "<tr><td>导航名称:</td><td colspan='2'><input type='text' value='导航名称' class='name_shop' /><input type='text' id='hue-demo' class='form-control demo' data-control='hue' value='#000'></td></tr>" +
        "<tr><td>背景颜色:</td><td><input type='text' id='demo-hue' class='form-control demo' data-control='hue' value='#fff'></td></tr>" +
        "<tr><td>导航链接:</td><td><input type='text' value='' class='href_shop' /></td><td><select onchange='selectchg(this)'>" +
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
        "<option value ='' selected>自定义链接</option>" +
        "</select></td></tr>" +
        "</table>";
    $("#addAttr" + m + " p").before(htmldiv);
    $("#addAttr" + m + " #nav_li_attr" + n + " .name_shop").change(function () {
        var k = $(this).parents("table").attr("n");
        $("#module" + m + " #nav_li" + k + " span").html($(this).val());
    });
    $("#addAttr" + m + " #nav_li_attr" + n + " #hue-demo").change(function () {
        var k = $(this).parents("table").attr("n");
        $("#module" + m + " #nav_li" + k + " span").css("color", $(this).val());
    });
    $("#addAttr" + m + " #nav_li_attr" + n + " .href_shop").change(function () {
        var k = $(this).parents("table").attr("n");
        $("#module" + m + " #nav_li" + k + " a").attr("href", $(this).val());
    });
    $("#addAttr" + m + " #nav_li_attr" + n + " #demo-hue").change(function () {
        var k = $(this).parents("table").attr("n");
        $("#module" + m + " #nav_li" + k + " .wb_txt").css("background-color", $(this).val());
    });
    //导航文本颜色选择器
    choseColor();
}
//搜索模块--------搜索模块--------搜索模块
function ss() {
    var divhtml = "<div id='module" + i + "' i='" + i + "' name='SouSuo' class='small ss'>" +
        "<div class='search_div'><input class='search' type='text' placeholder='商品搜索：请输入商品关键字' />" +
        "<button type='submit' class='searchBtn'></button></div></div>";
    $(divhtml).appendTo(".gridly");
}
function ssAttr() {
    var divhtml = "<div id='addAttr" + i + "' class='addAttr' i='" + i + "'>" +
        "<p>可随意插入任何页面和位置，方便会员快速搜索商品。</p>" +
        "</div>";
    $(divhtml).appendTo(".attribute_frame");
}
//文本模块--------文本模块--------文本模块
function wb() {
    var divhtml = "<div id='module" + i + "' i='" + i + "' name='WenBen' class='small wb'>" +
        "<div class='wb_txt'>" +
        "<p>点此编辑『富文本』内容 ——&gt;</p><p>你可以对文字进行<strong>加粗</strong>、<em>斜体</em>、<span style='text-decoration: underline;'>下划线</span>、<span style='text-decoration: line-through;'>删除线</span>、文字<span style='color: rgb(0, 176, 240);'>颜色</span>、<span style='background-color: rgb(255, 192, 0); color: rgb(255, 255, 255);'>背景色</span>、以及字号<span style='font-size: 20px;'>大</span><span style='font-size: 14px;'>小</span>等简单排版操作。</p><p>还可以在这里加入表格了</p><table><tbody><tr><td width='93' valign='top' style='word-break: break-all;'>中奖客户</td><td width='93' valign='top' style='word-break: break-all;'>发放奖品</td><td width='93' valign='top' style='word-break: break-all;'>备注</td></tr><tr><td width='93' valign='top' style='word-break: break-all;'>猪猪</td><td width='93' valign='top' style='word-break: break-all;'>内测码</td><td width='93' valign='top' style='word-break: break-all;'><em><span style='color: rgb(255, 0, 0);'>已经发放</span></em></td></tr><tr><td width='93' valign='top' style='word-break: break-all;'>大麦</td><td width='93' valign='top' style='word-break: break-all;'>积分</td><td width='93' valign='top' style='word-break: break-all;'><a href='javascript: void(0);' target='_blank'>领取地址</a></td></tr></tbody></table><p style='text-align: left;'><span style='text-align: left;'>也可在这里插入图片、并对图片加上超级链接，方便用户点击。</span></p>" +
        "</div>" +
        "</div>";
    $(divhtml).appendTo(".gridly");
}
function wbAttr() {
    var divhtml = "<div id='addAttr" + i + "' class='addAttr' i='" + i + "'>" +
        "<table class='nav_li_attr' cellpadding='0' cellspacing='0' border='0'>" +
        "<tr><td><script id='editor" + i + "' type='text/plain' style='width:500px;min-height:200px;'></script></td></tr>" +
        "</table>" +
        "</div>";
    $(divhtml).appendTo(".attribute_frame");
}
//公告模块--------公告模块--------公告模块
function gg() {
    var divhtml = "<div id='module" + i + "' i='" + i + "' name='GunDong' class='small dh'>" +
        "<div id='gongao'>" +
        "<div id='scroll_div' class='scroll_div wb_txt'>" +
        "<div id='scroll_begin'><a href></a></div>" +
        "<div id='scroll_end'><a href></a></div>" +
        "</div>" +
        "</div>" +
        "</div>";
    $(divhtml).appendTo(".gridly");
}
function ggAttr() {
    var divhtml = "<div id='addAttr" + i + "' class='addAttr' i='" + i + "'>" +
        "<table class='nav_li_attr' cellpadding='0' cellspacing='0' border='0'>" +
        "<tr><td>文本内容：</td><td><textarea rows='3' cols='50' >添加滚动字幕，文字长度要超过屏幕宽度才会滚动哟。</textarea></td></tr>" +
        "<tr><td>文本链接：</td><td><input type='text' class='href_wb' value='http://'></td></tr>" +
        "<tr><td>文本颜色：</td><td><input type='text' id='hue-demo' class='form-control demo' data-control='hue' value='#ff6161'></td></tr>" +
        "</table>" +
        "</div>";
    $(divhtml).appendTo(".attribute_frame");
}
function ScrollImgLeft() {
    //公告滚动
    var speed = 30;
    var scroll_begin = document.getElementById("scroll_begin");
    var scroll_end = document.getElementById("scroll_end");
    var scroll_div = document.getElementById("scroll_div");
    scroll_end.innerHTML = scroll_begin.innerHTML;

    function Marquee() {
        if (scroll_end.offsetWidth - scroll_div.scrollLeft <= 0)
            scroll_div.scrollLeft -= scroll_begin.offsetWidth;
        else
            scroll_div.scrollLeft++;
    }
    var MyMar = setInterval(Marquee, speed);
    scroll_div.onmouseover = function () {
        clearInterval(MyMar);
    }
    scroll_div.onmouseout = function () {
        MyMar = setInterval(Marquee, speed);
    }
}
//图片模块--------图片模块--------图片模块
function tp() {
    var divhtml = "<div id='module" + i + "' i='" + i + "' name='TuPian' class='small tp'>" +
        "<ul class='tp_style1'>" +
        "</ul>" +
        "</div>";
    $(divhtml).appendTo(".gridly");
}
function tpAttr() {
    var divhtml = "<div id='addAttr" + i + "' class='addAttr' i='" + i + "'>" +
        "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
        "<tr><td align='right'>显示方式：</td><td><input type='radio' name='mode" + i + "' value='two' checked='checked' />一行一张</td><td><input type='radio' name='mode" + i + "' value='three' />一行两张</td><td><input type='radio' name='mode" + i + "' value='four' />一行三张</td></tr>" +
        "<tr><td align='right'>显示文字：</td><td><input type='radio' name='txt" + i + "' value='yes' checked='checked' />是</td><td><input type='radio' name='txt" + i + "' value='no' />否</td><td></td></tr>" +
        "</table>" +
        "<p class='increase' onclick='addTP(this)'>增加</p>" +
        "</div>";
    $(divhtml).appendTo(".attribute_frame");
}
function addTP(e) {
    var m = $(e).parents("div").attr("i");
    var n = 1;
    if ($("#module" + m + " ul li").length != 0) {
        var max = 0;
        $("#module" + m + " ul li").each(function () {
            var a = $(this).attr("n");
            if (a > max) {
                max = a;
                n = parseInt(max) + 1;
            }
        });
    }
    var divhtml = "<li id='nav_li" + n + "' n='" + n + "'><a href><img src='img/banner.png'/><span>文字说明</span></a></li>";
    $(divhtml).appendTo("#module" + m + " ul");
    var htmldiv = "<table id='nav_li_attr" + n + "' class='nav_li_attr' n='" + n + "' cellpadding='0' cellspacing='0' border='0'>" +
        "<tr><td>图片:</td><td width='238px'><button onclick='chooseImg(this,\"TuPian\")' type='button' >选择图片</button></td><td><div class='changeBtn'><span onclick='moveUp(this," + n + ")'>上移</span><span onclick='moveDwon(this," + n + ")'>下移</span><span onclick='moveDel(this," + n + ")'>删除</span></div></td></tr>" +
        "<tr><td></td><td><img src='img/banner.png'/></td></tr>" +
        "<tr><td>文字说明:</td><td><input type='text' value='导航名称' class='name_shop' /></td></tr>" +
        "<tr><td></td><td><input type='text' id='hue-demo' class='form-control demo' data-control='hue' value='#ff6161'></td></tr>" +
        "<tr><td>图片链接:</td><td><input type='text' value='' class='href_shop' /><td><select onchange='selectchg(this)'>" +
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
        "<option value ='' selected>自定义链接</option>" +
        "</td></select></td></tr>" +
        "</table>";
    $("#addAttr" + m + " p").before(htmldiv);
    if ($("#addAttr" + m + " input[name='txt']:checked").attr("value") == "yes") {
        $("#module" + m + " span").css("display", "block");
    } else {
        var k = $(this).parents("table").attr("n");
        $("#module" + m + " span").css("display", "none");
    }
    $("#addAttr" + m + " #nav_li_attr" + n + " .name_shop").change(function () {
        var k = $(this).parents("table").attr("n");
        $("#module" + m + " #nav_li" + k + " span").html($(this).val());
    });
    $("#addAttr" + m + " #nav_li_attr" + n + " #hue-demo").change(function () {
        var k = $(this).parents("table").attr("n");
        $("#module" + m + " #nav_li" + k + " span").css("color", $(this).val());
    });
    $("#addAttr" + m + " #nav_li_attr" + n + " .href_shop").change(function () {
        var k = $(this).parents("table").attr("n");
        $("#module" + m + " #nav_li" + k + " a").attr("href", $(this).val());
    });
    //文本颜色选择器
    choseColor();
}
//幻灯片模块-----幻灯片模块-----幻灯片模块
function hdp() {
    var divhtml = "<div id='module" + i + "' i='" + i + "' name='HuanDeng' class='small hdp'>" +
        "<ul id='slides" + i + "' class='slides'>" +
        "</ul>" +
        "</div>";
    $(divhtml).appendTo(".gridly");
}
function hdpAttr() {
    var divhtml = "<div id='addAttr" + i + "' class='addAttr' i='" + i + "'>" +
        "<p class='increase' onclick='addHDP(this)'>增加</p>" +
        "</div>";
    $(divhtml).appendTo(".attribute_frame");
}
function addHDP(e) {
    var m = $(e).parents("div").attr("i");
    var n = 1;
    if ($("#module" + m + " ul li").length != 0) {
        var max = 0;
        $("#module" + m + " ul li").each(function () {
            var a = $(this).attr("n");
            if (a > max) {
                max = a;
                n = parseInt(max) + 1;
            }
        });
    }
    var divhtml = "<li id='nav_li" + n + "' n='" + n + "'><a href><img src /></a></li>";
    $(divhtml).appendTo("#module" + m + " ul");
    var htmldiv = "<table id='nav_li_attr" + n + "' class='nav_li_attr' n='" + n + "' cellpadding='0' cellspacing='0' border='0'>" +
        "<tr><td>幻灯片图片:</td><td><button onclick='chooseImg(this,\"HuanDeng\")' type='button' >选择图片</button></td><td><div class='changeBtn'><span onclick='moveUp(this," + n + ")'>上移</span><span onclick='moveDwon(this," + n + ")'>下移</span><span onclick='moveDel(this," + n + ")'>删除</span></div></td></tr>" +
        "<tr><td></td><td><img /></td></tr>" +
        "<tr><td></td><td style='color:#828282'>建议尺寸:750px × 400px</td></tr>" +
        "<tr><td>幻灯片链接:</td><td><input type='text' value='幻灯片链接' class='href_shop' /></td><td><select onchange='selectchg(this)'>" +
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
        "<option value ='' selected>自定义链接</option>" +
        "</select></td></tr>" +
        "</table>";
    $("#addAttr" + m + " p").before(htmldiv);
    $("#addAttr" + m + " #nav_li_attr" + n + " .href_shop").change(function () {
        var k = $(this).parents("table").attr("n");
        $("#module" + m + " #nav_li" + k + " a").attr("href", $(this).val());
    });
}
//商品列表模块--------商品列表模块--------商品列表模块
function splb() {
    var divhtml = "<div id='module" + i + "' i='" + i + "' name='LieBiao' class='small splb'>" +
        "<ul class='manner1'>" +
        "<li><a><img src='img/sc.png'><p>第一个商品</p><p><span>￥100</span><span>原价：￥200</span></p></a></li>" +
        "<li><a><img src='img/sc.png'><p>第二个商品</p><p><span>￥200</span><span>原价：￥300</span></p></a></li>" +
        "<li><a><img src='img/sc.png'><p>第三个商品</p><p><span>￥300</span><span>原价：￥400</span></p></a></li>" +
        "<li><a><img src='img/sc.png'><p>第四个商品</p><p><span>￥400</span><span>原价：￥500</span></p></a></li>" +
        "</ul>" +
        "</div>";
    $(divhtml).appendTo(".gridly");
}
function splbAttr() {
    var divhtml = "<div id='addAttr" + i + "' class='addAttr' i='" + i + "'>" +
        "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
        "<tr><td align='right'>1显示方式：</td><td><input type='radio' name='composition" + i + "' value='manner1' checked='checked' />小图</td><td><input type='radio' name='composition" + i + "' value='manner2' />大图</td><td><input type='radio' name='composition" + i + "' value='manner3'/>列表</td><td><input type='radio' name='composition' value='manner4' />一大两小</td><td><input type='radio' name='composition" + i + "' value='manner5' />一行三列</td></tr>" +
        "<tr><td align='right'>显示数量：</td><td><input type='radio' name='bit" + i + "' value='6' checked='checked' />6个</td><td><input type='radio' name='bit" + i + "' value='12' />12个</td><td><input type='radio' name='bit" + i + "' value='18' />18个</td></tr>" +
        "<tr><td align='right'>字体颜色:</td><td colspan='4'><input type='text' class='form-control demo list-color' data-control='hue' value='#000' style='border:none;'></td></tr>" +
        "<tr><td align='right'>背景颜色:</td><td colspan='4'><input type='text' class='form-control demo list-bgcolor' data-control='hue' value='#fff'></td></tr>" +
        "<tr><td align='right'>第一优先级：</td><td colspan='4'><select name='firstPriority'>" +
        "<option value='1'>序号越大越靠前</option>" +
        "<option value='2'>最热的排在前面</option>" +
        "<option value='3'>创建时间越晚越靠前</option>" +
        "<option value='4'>创建时间越早越靠前</option>" +
        "<option value='5' selected='selected'>销量越高越靠前</option>" +
        "<option value='6'>销量越低越靠前</option>" +
        "</select></td></tr>" +
        "<tr><td align='right'>第二优先级：</td><td colspan='4'><select name='firstPriority'>" +
        "<option value='1'>序号越大越靠前</option>" +
        "<option value='2'>最热的排在前面</option>" +
        "<option value='3' selected='selected'>创建时间越晚越靠前</option>" +
        "<option value='4'>创建时间越早越靠前</option>" +
        "<option value='5'>销量越高越靠前</option>" +
        "<option value='6'>销量越低越靠前</option>" +
        "</select></td></tr>" +
        "<tr><td align='right'>第三优先级：</td><td colspan='4'><select name='firstPriority'>" +
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
    $("#addAttr" + i + " .list-bgcolor").change(function () {
        var m = $(this).parents("table").parent().attr("i");
        $("#module" + m + " ul").css("background", $(this).val());
    });
    $("#addAttr" + i + " .list-color").change(function () {
        var m = $(this).parents("table").parent().attr("i");
        $("#module" + m + " ul p").css("color", $(this).val());
    });
}
//空白模块--------空白模块--------空白模块
function kb() {
    var divhtml = "<div id='module" + i + "' i='" + i + "' name='KongBai' class='small kb'>" +
        "<div class='wb_txt'></div>" +
        "</div>";
    $(divhtml).appendTo(".gridly");
}
function kbAttr() {
    var divhtml = "<div id='addAttr" + i + "' class='addAttr' i='" + i + "'>" +
        "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
        "<tr><td align='right'>高度：</td><td><div class='sliding'></div></td><td width='30px'><span class='amount'></span>px</td></tr>" +
        "</table>" +
        "</div>";
    $(divhtml).appendTo(".attribute_frame");
}
//商品模块--------商品模块--------商品模块
function sp() {
    var divhtml = "<div id='module" + i + "' i='" + i + "' name='ShangPin' class='small sp'>" +
        "<ul class='manner1'>" +
        "</ul>" +
        "</div>";
    $(divhtml).appendTo(".gridly");
}
function spAttr() {
    var divhtml = "<div id='addAttr" + i + "' class='addAttr' i='" + i + "'>" +
        "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
        "<tr><td align='right'>显示方式：</td><td><input type='radio' name='composition" + i + "' value='manner1' checked='checked' />小图</td><td><input type='radio' name='composition" + i + "' value='manner2' />大图</td><td><input type='radio' name='composition" + i + "' value='manner3'/>列表</td><td><input type='radio' name='composition" + i + "' value='manner4' />一大两小</td><td><input type='radio' name='composition" + i + "' value='manner5' />一行三列</td><td><input type='radio' name='composition" + i + "' value='manner6' />一行四列</td></tr>" +
        "</table>" +
        "<p class='increase' onclick='addSP(this)'>增加</p>" +
        "</div>";
    $(divhtml).appendTo(".attribute_frame");
}
function addSP(e) {

    var m = $(e).parents("div").attr("i");
    var n = 1;
    if ($("#module" + m + " ul li").length != 0) {
        var max = 0;
        $("#module" + m + " ul li").each(function () {
            var a = $(this).attr("n");
            if (a > max) {
                max = a;
                n = parseInt(max) + 1;
            }
        });
    }

    var divhtml = "<li id='nav_li" + n + "' n='" + n + "'><a href><img productid='0' src='img/banner.png'/><l role='prjName' style='color: #212121;'></l><l role='prjPrice' style='color:#D9690B;'></l></a></li>";

    $(divhtml).appendTo("#module" + m + " ul");
    //<td><div class='changeBtn'><span onclick='moveUp(this," + n + ")'>上移</span><span onclick='moveDwon(this," + n + ")'>下移</span><span onclick='moveDel(this," + n + ")'>删除</span></div></td>
    var htmldiv =
        "<table id='nav_li_attr" + n + "' class='nav_li_attr' n='" + n + "' cellpadding='0' cellspacing='0' border='0'>" +
        "<tr><td>商品:</td><td width='238px'><button onclick='chooseProduct(this,\"ShangPin\")' type='button' >选择商品</button></td><td><div class='changeBtn'><span onclick='moveUp(this," + n + ")'>上移</span><span onclick='moveDwon(this," + n + ")'>下移</span><span onclick='moveDel(this," + n + ")'>删除</span></div></td></tr>" +
        "<tr><td></td><td><img productid='0' src='img/banner.png'/><l role='prjName' style='color: #212121;'></l><l role='prjPrice'  style='color:#D9690B;'></l></td></tr>" +
        "</table>";
    $("#addAttr" + m + " p").before(htmldiv);


    /*
    var m = $(e).parents("div").attr("i");
    var n = 1;
    if ($("#module" + m + " ul li").length != 0) {
        n = parseInt($("#module" + m + " ul li:last-child").attr("n")) + 1;
    }
    var divhtml = "<li class='nav_li" + n + "' n='" + n + "'><a><img src='img/sc.png'><p>第四个商品</p><p><span>￥400</span><span>原价：￥500</span></p></a></li>";
    $(divhtml).appendTo("#module" + m + " ul");
    var htmldiv = "<div id='nav_li_attr" + n + "' n='" + n + "' class='delsp'><img src='img/sc.png'><span onclick='delsp(this);'>删除</span></div>";
    $("#addAttr" + m + " p").before(htmldiv);
    */
}
function delsp(e) {
    var a = $(e).parent().parent().attr("i");
    var b = $(e).parent().attr("n");
    $(e).parent().remove();
    $("#module" + a + " .nav_li" + b + "").remove();
}

//选取图片的一些操作
var chooseImgBtn;
var CurrentModel = "";
function chooseImg(e, model) {
    chooseImgBtn = e;
    CurrentModel = model;
    $(".layoutImg").show();
}
//选取商品的一些操作
var chooseImgBtn;
var CurrentModel = "";
function chooseProduct(e, model) {
    chooseImgBtn = e;
    CurrentModel = model;
    $(".layoutProduct").show();
}

function setImg(url, name, price, productid) {
    switch (CurrentModel) {
        case "DaoHang":
            $(chooseImgBtn).parents("tr").next().find("img").attr("src", url);
            var i = $(chooseImgBtn).parents("table").parent().attr("i");
            var n = $(chooseImgBtn).parents("table").attr("n");
            $("#module" + i + " #nav_li" + n + " img").attr("src", url);
            break;
        case "TuPian":
            $(chooseImgBtn).parents("tr").next().find("img").attr("src", url);
            var i = $(chooseImgBtn).parents("table").parent().attr("i");
            var n = $(chooseImgBtn).parents("table").attr("n");
            $("#module" + i + " #nav_li" + n + " img").attr("src", url);
            break;
        case "HuanDeng":
            $(chooseImgBtn).parents("tr").next().find("img").attr("src", url);
            var i = $(chooseImgBtn).parents("table").parent().attr("i");
            var n = $(chooseImgBtn).parents("table").attr("n");
            $("#module" + i + " #nav_li" + n + " img").attr("src", url);
            break;
        case "ShangPin":
            //右侧域赋值
            var $img = $(chooseImgBtn).parents("tr").next().find("img");//商品图片
            $img.attr("src", url)//给商品图片赋值
            $img.next().text(name);//给商品名赋值
            $img.next().next().text(price);//给商品价格赋值
            var i = $(chooseImgBtn).parents("table").parent().attr("i");
            var n = $(chooseImgBtn).parents("table").attr("n");
            //左侧浏览区赋值
            $("#module" + i + " #nav_li" + n + " img").attr("src", url);
            $("#module" + i + " #nav_li" + n + " img").next().text(name);
            $("#module" + i + " #nav_li" + n + " img").next().next().text(price);
            $("#module" + i + " #nav_li" + n + " img").attr("productid", productid);
            //alert(productid);
            break;
    }
}
function closechsImg() {
    $(".layoutImg").hide();
}
function closeSelectPrj() {
    $(".layoutProduct").hide();
}
function setIframeUrl(url) {
    $(".layoutImg").find("iframe").attr("src", url);
}
function setIframeUrlProduct(url) {
    $(".layoutProduct").find("iframe").attr("src", url);
}
//选取图片的一些操作END

////店招模块--------店招模块--------店招模块
//function dz(){
//	var divhtml = "<div id='module"+i+"' i='"+i+"' name='DianZhao' class='small dz'>"+
//					  "<div class='shop_banner_box'>"+
//						  "<div class='shop_banner'><img src='img/banner.png' /></div>"+
//						  "<div class='shop_logo'><img src='img/banner.png' /></div>"+
//						  "<div class='shop_name'>店铺名称</div>"+
//					  	  "<div class='shop_desc'>宝贝数量</div>"+
//					  "</div>"+
//				  "</div>";
//	$(divhtml).appendTo(".gridly");
//};
//function dzAttr(){
//	var divhtml = "<div id='addAttr"+i+"' class='addAttr' i='"+i+"'>"+
//					  "<table class='nav_li_attr' cellpadding='0' cellspacing='0' border='0'>"+
//						  "<tr><td>店招图片:</td><td><input onchange='onChangeFile(this,1)' type='file' value='上传图片'/></td></tr>"+
//						  "<tr><td></td><td><img src='img/banner.png'/></td></tr>"+
//						  "<tr><td></td><td>建议尺寸:650px × 277px</td></tr>" +
//						  "<tr><td>店铺LOGO:</td><td><input onchange='onChangeFile(this,2)' type='file' value='上传图片' /></td></tr>"+
//						  "<tr><td></td><td><img src='img/banner.png'/></td></tr>"+
//						  "<tr><td></td><td>建议尺寸:160px × 160px</td><td></td></tr>" +
//						  "<tr><td>店铺名称:</td><td><input type='text' value='店铺名称' class='name_shop' /></td></tr>"+
//						  "<tr><td></td><td><input type='text' id='hue-demo' class='form-control demo' data-control='hue' value='#ff6161'></td></tr>"+
//						  "<tr><td></td><td>建议10个汉字以内</td></tr>"+
//					  "</table>"+
//				  "</div>";
//	$(divhtml).appendTo(".attribute_frame");
//};