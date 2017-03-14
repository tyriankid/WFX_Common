var i = $(".gridly").children().length + 1;
$(function(){
    $(".draw li").each(function(){
    	$(this).dblclick(function(){
    		//模块拖动
//  		$('.gridly').gridly({
//			    base: 50, // px 
//			    gutter:5, // px
//			    columns: 3
    	    //			});
    	    //文字模块--------文字模块--------文字模块
    	    if ($(this).is(".character")) {
    	        wz();
    	        wzAttr();
    	    }
    	    //日期模块--------日期模块--------日期模块
    	    if ($(this).is(".calendar")) {
    	        rq();
    	        rqAttr();
    	    }
    	    //文本投票--------文本投票--------文本投票
    	    if ($(this).is(".text-voting")) {
    	        wbtp();
    	        wbtpAttr();
    	        addWBTP();
    	    }
    	    //图片模块--------图片模块--------图片模块
    	    if ($(this).is(".picture")) {
    	        tp();
    	        tpAttr();
    	    }
    	    //文本模块--------文本模块--------文本模块
    	    if ($(this).is(".text")) {
    	        wb();
    	        wbAttr();
    	        EditorU(i);//富文本编辑器
    	    }
			
			integrate("#module"+i,i);
			
			pos("#module"+i,i);  //双击后立即定位	
			
			$("#module"+i).click(function(){
				var j = $(this).attr("i");
				integrate(this,j);
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
    $("#addAttr"+j).show();
    //模块添加删除按钮、选中状态
    if ($("#module" + j).find(".pitch").length == 0) {
        $("#module" + j).prepend("<div class='pitch'><p><a onclick='integrate()'>编辑</a><a onclick='delcon(this)'>删除</a></p></div>");
    }
    $("#module"+j).siblings().find(".pitch").css("border","none");
    $("#module"+j).siblings().find(".pitch p").hide();
    $("#module" + j).find(".pitch").css("border", "2px #f90 dashed");
    $("#module" + j).find(".pitch p").show();
    //查询当前模块所处页面位置，设置模块属性位置
    pos(e,j);
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
    $("#addAttr"+j+" input").click(function(){
        if ($(this).attr("name") == "input_type"+j){
            if ($(this).attr("value") == "0") {
                $("#module" + j + " .wz textarea").remove();
                $("#module" + j + " .wz input").remove();
                $("#module" + j + " .wz").append("<input type='text' class='w_txt' disabled=''>");
            } else {
                $("#module" + j + " .wz input").remove();
                $("#module" + j + " .wz textarea").remove();
                $("#module" + j + " .wz").append("<textarea class='w_txt' disabled=''></textarea>");
            }
        }
        if ($(this).attr("name") == "input_choice" + j) {
            if ($(this).attr("value") == "0") {
                $("#module" + j + " ul li input").attr("type", "radio");
                $("#module" + j + " ul li").css("background-position-y", "10px");
            } else {
                $("#module" + j + " ul li input").attr("type", "checkbox");
                $("#module" + j + " ul li").css("background-position-y", "-29px");
            }
        }
        if ($(this).attr("name") == "required" + j) {
            if($(this).is(":checked")){
                $("#module" + j).attr("bt", "true");
                $("#module" + j + " h3").after("<span class='must'>(必填)</span>");
            } else {
                $("#module" + j).attr("bt", "false");
                $("#module" + j + " .must").remove();
            }
        }
		if ($(this).attr("name") == "txt") {
		    if ($(this).attr("value") == "yes") {
		        $("#module" + j + " ul li a span").css("display", "block");
		    } else {
		        $("#module" + j + " ul li a span").css("display", "none");
		    }
		}
	});
	//属性改变按钮
	$("#addAttr" + j + " input").change(function () {
	    if ($(this).attr("name") == "title") {
	        $("#module" + j + " .wz h3").text($(this).val());
	    }
	    if ($(this).attr("name") == "subtitle") {
	        $("#module" + j + " .wz h4").text($(this).val());
	    }
	});

	$("#module" + j + " ul li").each(function () {
	    $(this).click(function () {
	        var p = $(this).parents(".small").attr("i");
	        if ($("#addAttr" + p + " input[name='input_choice" + p + "']:checked").attr("value") == 0) {
	            $(this).siblings().css("background-position-y", "10px");
	            $(this).css("background-position-y", "-66px");
	        } else {
	            if ($(this).css("background-position-y") == "-29px") {
	                $(this).css("background-position-y", "-115px");
	            } else{
	                $(this).css("background-position-y", "-29px");
	            }
	        }
	    });
	});


    //日期控件
	var currYear = (new Date()).getFullYear();
	var opt = {};
	opt.date = { preset: 'date' };
	opt.datetime = { preset: 'datetime' };
	opt.time = { preset: 'time' };
	opt.default = {
	    theme: 'android-ics light', //皮肤样式
	    display: 'modal', //显示方式 
	    mode: 'scroller', //日期选择模式
	    dateFormat: 'yyyy-mm-dd',
	    lang: 'zh',
	    showNow: true,
	    nowText: "今天",
	    startYear: currYear - 10, //开始年份
	    endYear: currYear + 10 //结束年份
	};

	$("#appDate" + j).mobiscroll($.extend(opt['date'], opt['default']));

    //文本投票
	$("#addAttr" + j + " input[name='options']").change(function () {
	    var k = $(this).parents("tr").attr("n");
	    $("#module" + j + " #nav_li" + k + " span").html($(this).val());
	});
}
function pos(e,j){
	//查询当前模块所处页面位置，设置模块属性位置
	var y = $(e).offset().top;
	var x = $(e).offset().left;
	$("#addAttr"+j).css("top",y);
	$("#tabbox"+j).css("top",y);
	$("html,body").animate({scrollTop:y},0);
}
//富文本编辑器
function EditorU(j) {
    var ue = UE.getEditor('editor'+j,{
		toolbars: [[
            'fullscreen', 'source', '|', 'undo', 'redo', '|',
            'bold', 'italic', 'underline', 'fontborder', 'strikethrough', 'superscript', 'subscript', 'removeformat', 'formatmatch', 'autotypeset', 'blockquote', 'pasteplain', '|', 'forecolor', 'backcolor', 'insertorderedlist', 'insertunorderedlist', 'selectall', 'cleardoc', '|',
            'rowspacingtop', 'rowspacingbottom', 'lineheight', '|',
            'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|',
            'directionalityltr', 'directionalityrtl', 'indent', '|',
            'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify', '|', 'touppercase', 'tolowercase', '|',
            'link', 'unlink', '|', 'imagenone', 'imageleft', 'imageright', 'imagecenter', '|',
            'simpleupload', 'insertimage', 'emotion', 'scrawl', 'music', 'attachment','|',
            'horizontal', 'date', 'time', 'spechars','|',
            'inserttable', 'deletetable','mergecells', 'mergeright', 'mergedown', 'splittocells', 'splittorows', 'splittocols', 'charts',
		]],
		autoHeightEnabled: true,
		autoFloatEnabled: true,
		initialStyle: 'p{line-height:1em; font-size: 16px; }',
		initialContent: '',
        autoClearinitialContent:true,
        focus: false,
    });
	ue.addListener("contentChange",function(){
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
    var a =$(e).parents("table").parent().attr("i");
    var b = $(e).parents("table").attr("n");
	$(e).parents("table").remove();
	$("#module"+a+" #nav_li"+b+"").remove();
}
function voteDel(e) {
    var a = $(e).parents(".addAttr").attr("i");
    var b = $(e).parents("tr").attr("n");
    $(e).parents("tr").remove();
    $("#module" + a + " #nav_li" + b + "").remove();
}
//垃圾桶删除方法
function delcon(e){
	var r = confirm("是否确认删除");
	if(r==true){
		var modeldiv=$(e).parent().parent().parent();
		var j=$(modeldiv).attr("i");
		$(modeldiv).remove();
		$("#addAttr"+j).remove();
		$("#tabbox"+j).remove();
	}else{
		return false;
	}
}
//颜色选择器
function choseColor(){
    $('.demo').each( function() {
		$(this).minicolors({
			control: $(this).attr('data-control') || 'hue',
			defaultValue: $(this).attr('data-defaultValue') || '',
			inline: $(this).attr('data-inline') === 'true',
			letterCase: $(this).attr('data-letterCase') || 'lowercase',
			opacity: $(this).attr('data-opacity'),
			position: $(this).attr('data-position') || 'bottom left',
			change: function(hex, opacity) {
				if( !hex ) return;
				if( opacity ) hex += ', ' + opacity;
				try {
					console.log(hex);
				} catch(e) {}
			}
		});
    });
}
//图片上传后图显示
function onFileChange(sender,j) {
    var filename = sender.value;
    if (filename == "") {
        return "";
    }
    var ExName = filename.substr(filename.lastIndexOf(".") + 1).toUpperCase();

    if (ExName == "JPG" || ExName == "BMP" || ExName == "GIF" || ExName == "PNG") {
     	//导航图片
     	var n =$(sender).parents("table").attr("n");
     	$("#nav_li"+n).find("img").attr("src",window.URL.createObjectURL(sender.files[0]));
     	$("#nav_li_attr"+n).find("img").attr("src",window.URL.createObjectURL(sender.files[0]));
//   	$("#nav_li"+n).attr("src",window.URL.createObjectURL(sender.files[0]));
        ///document.getElementById("img_Photo").src = window.URL.createObjectURL(sender.files[0]);
    }
    else {
        alert('请选择正确的图片格式！');
        sender.value = null;
        return false;
    }
}
function onChangeFile(sender,j){
    var filename = sender.value;
    if (filename == "") {
        return "";
    }
    var ExName = filename.substr(filename.lastIndexOf(".") + 1).toUpperCase();
    if (ExName == "JPG" || ExName == "BMP" || ExName == "GIF" || ExName == "PNG") {
    	//店招图片
     	var m=$(sender).parents("div").attr("i");
     	$("#module"+m).find("img").eq(j-1).attr("src",window.URL.createObjectURL(sender.files[0]));
     	$("#addAttr"+m).find("img").eq(j-1).attr("src",window.URL.createObjectURL(sender.files[0]));
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
    $("#module" + b + " ul #nav_li" + a + " a").attr("href",$(e).val());
}
/*-----------模块添加--------------模块添加-------------------模块添加-----------------模块添加----------------*/
//文字模块--------文字模块--------文字模块
function wz() {
    var divhtml = "<div id='module" + i + "' i='" + i + "' name='ShuRuKuang' class='small'>" +
					  "<div class='vote'><div class='wz'><h3>未命名的文字调查标题</h3><h4></h4>" +
                      "<textarea class='w_txt' disabled=''></textarea></div></div>"+
				  "</div>";
    $(divhtml).appendTo(".gridly");
}
function wzAttr() {
    var divhtml = "<div id='addAttr" + i + "' class='addAttr' i='" + i + "'>" +
					  "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
                        "<tr><td colspan='2' align='center'><h4>文字控件</h4><td></tr>" +
						"<tr><td align='right'>标题：</td><td><input type='text' name='title' value='未命名的文字调查标题' maxlength='100'></td></tr>" +
						"<tr><td align='right'>副标题：</td><td><input type='text' name='subtitle' value='' maxlength='100'></td></tr>" +
                        "<tr><td align='right'>文本框高度：</td><td><label><input type='radio' name='input_type" + i + "' value='0'>单行</label><label><input type='radio' name='input_type" + i + "' value='1' checked=''>多行</label></td></tr>" +
                        "<tr><td align='right'>是否必填：</td><td><label><input name='required" + i + "' type='checkbox'>必须填写</label></td></tr>" +
					  "</table>" +
				  "</div>";
    $(divhtml).appendTo(".attribute_frame");
}
//日期模块--------日期模块--------日期模块
function rq() {
    var divhtml = "<div id='module" + i + "' i='" + i + "' name='ShiJian' class='small'>" +
					  "<div class='vote'><div class='wz'><h3>未命名的日期</h3><h4></h4>" +
                      "<input value='' readonly='readonly' name='appDate' id='appDate"+i+"' type='text'></div></div>"+
				  "</div>";
    $(divhtml).appendTo(".gridly");
}
function rqAttr() {
    var divhtml = "<div id='addAttr" + i + "' class='addAttr' i='" + i + "'>" +
					  "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
                        "<tr><td colspan='2' align='center'><h4>日期控件</h4><td></tr>" +
						"<tr><td align='right'>标题：</td><td><input type='text' name='title' value='未命名的日期' maxlength='100'></td></tr>" +
						"<tr><td align='right'>副标题：</td><td><input type='text' name='subtitle' value='' maxlength='100'></td></tr>" +
                        "<tr><td align='right'>是否必填：</td><td><label><input name='required" + i + "' type='checkbox' value='0'>必须填写</label></td></tr>" +
					  "</table>" +
				  "</div>";
    $(divhtml).appendTo(".attribute_frame");
}
//投票模块--------投票模块--------投票模块
function wbtp() {
    var divhtml = "<div id='module" + i + "' i='" + i + "' name='XuanXiang' class='small'>" +
					  "<div class='vote'><div class='wz'><h3>未命名的投票</h3><h4></h4></div><ul>" +
                      "<li id='nav_li1' n='1' style='background-position:15px 10px;'><label><input type='radio' name='multiple' /><span>未命名的选项</span></label></li>" +
                      "<li id='nav_li2' n='2' style='background-position:15px 10px;'><label><input type='radio' name='multiple' /><span>未命名的选项</span></label></li>" +
				  "</ul></div></div>";
    $(divhtml).appendTo(".gridly");
}
function wbtpAttr() {
    var divhtml = "<div id='addAttr" + i + "' class='addAttr' i='" + i + "'>" +
					  "<table class='nature' cellpadding='0' cellspacing='0' border='0'>" +
                        "<tr><td colspan='2' align='center'><h4>文本投票</h4><td></tr>" +
						"<tr><td align='right'>标题：</td><td><input type='text' name='title' value='未命名的投票' maxlength='100'></td></tr>" +
						"<tr><td align='right'>副标题：</td><td><input type='text' name='subtitle' value='' maxlength='100'></td></tr>" +
                        "<tr><td align='right'>单选/多选：</td><td><label><input type='radio' name='input_choice" + i + "' value='0' checked>单选</label><label><input type='radio' name='input_choice" + i + "' value='1'>多选</label></td></tr>" +
                        "<tr><td align='right'>是否必填：</td><td><label><input name='required" + i + "' type='checkbox' value='0'>必须填写</label></td></tr>" +
                        "<tr><td colspan='3' align='center' class='line-bg'><span>选项</span></td></tr>" +
                        "<tr id='nav_li_attr1' n='1'><td align='right'>选项名称：</td><td><input type='text' name='options' value='未命名的选项' maxlength='100'></td><td><div class='changeBtn' style='width: 80px'><span onclick='voteDel(this)'>删除</span></div></td></tr>" +
                        "<tr id='nav_li_attr2' n='2'><td align='right'>选项名称：</td><td><input type='text' name='options' value='未命名的选项' maxlength='100'></td><td><div class='changeBtn' style='width: 80px'><span onclick='voteDel(this)'>删除</span></div></td></tr>" +
					  "</table>" +
                      "<p class='increase' onclick='addWBTP(this)'>增加</p>" +
				  "</div>";
    $(divhtml).appendTo(".attribute_frame");
}
function addWBTP(e) {
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
    var divhtml1 = "<li id='nav_li" + n + "' n='" + n + "' style='background-position:15px 10px;'><label><input type='radio' name='multiple' /><span>未命名的选项</span></label></li>";
    var divhtml2 = "<li id='nav_li" + n + "' n='" + n + "' style='background-position:15px -29px;'><label><input type='checkbox' name='multiple' /><span>未命名的选项</span></label></li>";
    if ($("#addAttr" + m + " input[name='input_choice" + m + "']:checked").attr("value") == "0") {
        $(divhtml1).appendTo("#module" + m + " ul");
    } else {
        $(divhtml2).appendTo("#module" + m + " ul");
    }
    
    var htmldiv = "<tr id='nav_li_attr" + n + "' n='" + n + "'><td align='right'>选项名称：</td><td><input type='text' name='options' value='未命名的选项' maxlength='100'></td><td><div class='changeBtn' style='width: 80px'><span onclick='voteDel(this)'>删除</span></div></td></tr>";
    $("#addAttr" + m + " table").append(htmldiv);
    
    $("#addAttr" + m + " #nav_li_attr" + n + " input[name='options']").change(function () {
        $("#module" + m + " #nav_li" + n + " span").html($(this).val());
    });
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
						  //"<tr><td align='right'>显示方式：</td><td><input type='radio' name='mode' value='two' checked='checked' />一行一张</td><td><input type='radio' name='mode' value='three' />一行两张</td><td><input type='radio' name='mode' value='four' />一行三张</td></tr>" +
						  "<tr><td align='right'>显示文字：</td><td><input type='radio' name='txt' value='yes' checked='checked' />是</td><td><input type='radio' name='txt' value='no' />否</td><td></td></tr>" +
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
					  "<tr><td>图片:</td><td width='138px'><button onclick='chooseImg(this,\"TuPian\")' type='button' >选择图片</button></td><td><div class='changeBtn'><span onclick='moveUp(this," + n + ")'>上移</span><span onclick='moveDwon(this," + n + ")'>下移</span><span onclick='moveDel(this," + n + ")'>删除</span></div></td></tr>" +
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



//选取图片的一些操作
var chooseImgBtn;
var CurrentModel = "";
function chooseImg(e,model) {
    chooseImgBtn = e;
    CurrentModel = model;
    $(".layoutImg").show();
}
function setImg(url) {
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
    }
}
function closechsImg() {
    $(".layoutImg").hide();
}
function setIframeUrl(url){
    $(".layoutImg").find("iframe").attr("src",url);
}
//选取图片的一些操作END

