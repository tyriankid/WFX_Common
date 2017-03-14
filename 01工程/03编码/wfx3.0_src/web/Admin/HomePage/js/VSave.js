//○●♢♦
//分隔说明：○模块之间   ●属性之间  ♢子属性之间    ♦内容之间

//模块之间->模块内容->属性、子内容(区分属性)

//DaoHang●Style1●ShowText●Content♢名称♢链接♢前景色 ○ GunDong●Style1●ShowText●Content♢名称♢链接♢前景色●....
function save() {
    
    if (check()) {
        var res = "";
        $(".gridly").children("div").each(function () {
            var modelName = $(this).attr("name");
            res += modelName + "●";//模块code
            switch (modelName) {
                
                case "VDaoHang":
                    var i = $(this).attr("i");
                    res += $("#addAttr" + i).find(".amount_height").html() + "●";//导航文字高度  |高度|
                    res += $(this).find("li:eq(0) img").css("display") + "●";//是否显示图片  |是否显示图标|
                    res += $(this).find("li:eq(0) span").css("display")+"●";                                                            //|是否显示文字|
                    res += $(this).find("ul").attr("class") + "●";//一行几列的样式           |显示的样式|
                    res += "●";                                                            //|显示个数|
                    //***********************内容开始***********************                  |显示内容|
                    $(this).find("li").each(function () {
                        res += $(this).find("img").attr("src") + "♦";//图片路径
                        res += $(this).find("span").html().rep() + "♦";//导航名称
                        res += $(this).find("a").css("background-color") + "♦";//导航文字背景
                        res += $(this).find("span").css("color") + "♦";//导航文字颜色
                        res += $(this).find("a").attr("href") + "♦";//导航链接
                        res += $(this).height() / $(this).parent().height()*100;
                        res += "♢";
                    });
                    res += "●";
                    //***********************内容结束*********************** 
                    var i = $(this).attr("i");
                    res += $("#addAttr" + i + " .amount_width").html() + "●";
                    var t = $(this).position().top / $("#panelHomePage").height() * 100;
                    res += $(this).position().top / $("#panelHomePage").height() * 100 + "%" + "♦" + $(this).position().left / $("#panelHomePage").width() * 100 + "%";
                    break;
                case "VWenBen":
                    var i = $(this).attr("i");
                    res += "●";              //|高度|                  
                    res +="●";//                                                             |是否显示图标|
                    res += "●";                                                            //|是否显示文字|
                    res += "●";//一                                                          |显示的样式|
                    res += "●";                                                            //|显示个数|
                    //***********************内容开始***********************                  |显示内容|
                    res += $(this).find(".wb_txt").html().rep() + "●";
                    //***********************内容结束*********************** 
                    res += "●";
                    res += $(this).position().top / $("#panelHomePage").height() * 100 + "%" + "♦" + $(this).position().left / $("#panelHomePage").width() * 100 + "%";
                    break;
                case "VGunDong":
                    res += $(this).find("#scroll_div").css("height") + "●";//                |高度|?????
                    res += "●";//                                                             |是否显示图标|
                    res += "●";                                                            //|是否显示文字|
                    res += "●";//一                                                          |显示的样式|
                    res += "●";                                                            //|显示个数|
                    //***********************内容开始***********************                  |显示内容|
                    res += $(this).find("#scroll_begin a").html().rep() + "♦" + $(this).find("#scroll_begin a").css("color") + "♦" + $(this).find("#scroll_begin a").attr("href") + "●";
                    //***********************内容结束*********************** 
                    res += "●";
                    res += $(this).position().top / $("#panelHomePage").height() * 100 + "%" + "♦" + $(this).position().left / $("#panelHomePage").width() * 100 + "%";
                    break;
                case "VTuPian":
                    res += "●";//                                                             |高度|
                    res += "●";//                                                             |是否显示图标|
                    res += $(this).find("ul li a span:eq(0)").css("display")+"●";           //|是否显示文字|
                    res +=$(this).find("ul").attr("class")+ "●";//一                          |显示的样式|
                    res += "●";                                                            //|显示个数|
                    //***********************内容开始***********************                  |显示内容|
                    $(this).find("ul li").each(function () {
                        //图片  文字   链接
                        res += $(this).find("img").attr("src") + "♦";//图片路径
                        res += $(this).find("span").html().rep() + "♦";//导航名称
                        res += $(this).find("a").attr("href");//链接
                        res += "♢";
                    });
                    res += "●";
                    //***********************内容结束*********************** 
                    res += "●";
                    res += $(this).position().top / $("#panelHomePage").height() * 100 + "%" + "♦" + $(this).position().left / $("#panelHomePage").width() * 100 + "%";
                    break;
                case "VHuanDeng":
                    res += "●";//                                                             |高度|
                    res += "●";//                                                             |是否显示图标|
                    res += "●";           //|是否显示文字|
                    res += "●";//一                          |显示的样式|
                    res += "●";                                                            //|显示个数|
                    //***********************内容开始***********************                  |显示内容|
                    $(this).find("ul li").each(function () {
                        //图片   链接
                        res += $(this).find("img").attr("src") + "♦";//图片路径
                        res += $(this).find("a").attr("href");//链接
                        res += "♢";
                    });
                    res += "●";
                    res += "●";
                    res += $(this).position().top / $("#panelHomePage").height() * 100 + "%" + "♦" + $(this).position().left / $("#panelHomePage").width() * 100 + "%";
                    break;
               
                case "BeiJing":
                    res += "●";//                                                             |高度|
                    res += "●";//                                                             |是否显示图标|
                    res += "●";           //|是否显示文字|
                    res += "●";//一                          |显示的样式|
                    res += "●";                                                            //|显示个数|
                    //***********************内容开始***********************                  |显示内容|
                    res += $(this).find("img").attr("src");
                    //***********************内容结束*********************** 
                    break;
                
                
            }
            res += "○";
        });
        $("#txtRes").val(res);
        return true;;
    } else {
        return false;
    }
  
}


function check() {
    var msgStr = "";
    $(".gridly").children("div").each(function () {
        var modelName = $(this).attr("name");
        switch (modelName) {
            case "VDaoHang":
                var lilength = $(this).find("li").length;
                if (lilength <= 0) {
                    msgStr += "导航模块至少得有一个导航~<br/>";
                } else {
                    $(this).find("li").each(function () {
                        if ($(this).find("span").html().rep() == "") {
                            msgStr += "有导航的名称为空了~<br/>";
                        }
                        if ($(this).find("a").attr("href") == "") {
                            msgStr += "有导航的链接为空了~<br/>";
                        }
                    });
                }
                break;
            case "VWenBen":
                if ($(this).find(".wb_txt").html().rep() == "") {
                    msgStr += "文本模块的文本不能为空~<br/>";
                }
                break;
            case "VGunDong":
                if ($(this).find("#scroll_begin a").html().rep() == "") {
                    msgStr += "滚动公告的文本不能为空~<br/>";
                }
                if ($(this).find("#scroll_begin a").attr("href") == "") {
                    msgStr += "滚动公告的链接不能为空~<br/>";
                }
                break;
            case "VTuPian":
                if ($(this).find("ul li").length <= 0) {
                    msgStr += "图片模块必须至少上传一张图片~<br/>";
                }
                break;
            case "VHuanDeng":
                if ($(this).find("ul li").length <= 0) {
                    msgStr += "幻灯片必须至少上传一张图片~<br/>";
                }
                $(this).find("ul li a img").each(function () {
                    if ($(this).attr("src") == "") {
                        msgStr += "幻灯片模块中的图片不能为空~<br/>";
                    }
                });
                break;
        }
    });
    if (msgStr != "") {
        $("#msg").html(msgStr).dialog({
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
    } else {
        return true;
    }
}

String.prototype.rep= function(){
    return this.replace(/[●○♢♦]/g, "");;
}