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
                case "DianZhao":
                    res += $(this).find(".shop_banner img").attr("src") + "●";//店招背景
                    res += $(this).find(".shop_logo img").attr("src") + "●";//Logo图片
                    res += $(this).find(".shop_name").html().rep() + "●";//店名
                    res += $(this).find(".shop_name").css("color");//店名颜色
                    break;
                case "DaoHang":
                    res += $(this).find("li:eq(0) span").css("height") + "●";//导航文字高度  |高度|
                    res += $(this).find("li:eq(0) img").css("display") + "●";//是否显示图片  |是否显示图标|
                    res += $(this).find("li:eq(0) span").css("display")+"●";                                                            //|是否显示文字|
                    res += $(this).find("ul").attr("class") + "●";//一行几列的样式           |显示的样式|
                    res += "●";                                                            //|显示个数|
                    //***********************内容开始***********************                  |显示内容|
                    $(this).find("li").each(function () {
                        res += $(this).find("img").attr("src") + "♦";//图片路径
                        res += $(this).find("span").html().rep() + "♦";//导航名称
                        res += $(this).find("span").css("background-color") + "♦";//导航文字背景
                        res += $(this).find("span").css("color") + "♦";//导航文字颜色
                        res += $(this).find("a").attr("href") ;//导航链接
                        res += "♢";
                    });
                    //***********************内容结束*********************** 
                    break;
                case "WenBen":
                    res += $(this).find(".wb_txt").css("height") + "●";//                    |高度|
                    res +="●";//                                                             |是否显示图标|
                    res += "●";                                                            //|是否显示文字|
                    res += "●";//一                                                          |显示的样式|
                    res += "●";                                                            //|显示个数|
                    //***********************内容开始***********************                  |显示内容|
                    res += $(this).find(".wb_txt").html().rep();
                    //***********************内容结束*********************** 
                    break;
                case "GunDong":
                    res += $(this).find("#scroll_div").css("height") + "●";//                |高度|
                    res += "●";//                                                             |是否显示图标|
                    res += "●";                                                            //|是否显示文字|
                    res += "●";//一                                                          |显示的样式|
                    res += "●";                                                            //|显示个数|
                    //***********************内容开始***********************                  |显示内容|
                    res += $(this).find("#scroll_begin a").html().rep() + "♦" + $(this).find("#scroll_begin a").css("color") + "♦" + $(this).find("#scroll_begin a").attr("href");
                    //***********************内容结束*********************** 
                    break;
                case "TuPian":
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
                    //***********************内容结束*********************** 
                    
                    break;
                case "HuanDeng":
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
                    break;
                case "SouSuo":
                    break;
                case "LieBiao":
                    res += "●";
                    res += "●";
                    res += "●";
                    res += $(this).children("ul").attr("class") + "●";                                                     //|显示的样式|
                    var i = $(this).attr("i");
                    res +=$("#addAttr" + i + " table tr:eq(1)").find("input[type='radio']:checked").attr("value")+ "●";//|显示个数|
                    //***********************内容开始***********************                 
                    res += $("#addAttr" + i + " table select:eq(0)").val() + "♦" + $("#addAttr" + i + " table select:eq(1)").val() + "♦" + $("#addAttr" + i + " table select:eq(2)").val()+"●";
                    //***********************内容结束*********************** 
                    //***********************保留字段使用，用来处理颜色】
                    res += $(this).children("ul").css("background") + "♦" + $("#addAttr" + i + " table tr:eq(2)").find("input").val();
                    break;
                case "ShangPin":
                    res += "●";
                    res += "●";
                    res += "●";
                    res += $(this).children("ul").attr("class") + "●";                                                     //|显示的样式|
                    res += "●";//|显示个数|
                    //***********************内容开始***********************                 
                    res += $("#addAttr" + i + " table select:eq(0)").val() + "♦" + $("#addAttr" + i + " table select:eq(1)").val() + "♦" + $("#addAttr" + i + " table select:eq(2)").val();
                    //***********************内容结束*********************** 
                    break;
                case "KongBai":
                    res += $(this).children(".wb_txt").css("height")+ "●";//                                                             |高度|
                    res += "●";//                                                             |是否显示图标|
                    res += "●";           //|是否显示文字|
                    res += "●";//一                          |显示的样式|
                    res += "●";
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
            case "DianZhao":
                var shopName = $(this).find(".shop_name").html().rep();
                if (shopName == "") {
                    msgStr += "店铺名称不能为空<br/>";
                } 
                break;
            case "DaoHang":
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
            case "WenBen":
                if ($(this).find(".wb_txt").html().rep() == "") {
                    msgStr += "文本模块的文本不能为空~<br/>";
                }
                break;
            case "GunDong":
                if ($(this).find("#scroll_begin a").html().rep() == "") {
                    msgStr += "滚动公告的文本不能为空~<br/>";
                }
                if ($(this).find("#scroll_begin a").attr("href") == "") {
                    msgStr += "滚动公告的链接不能为空~<br/>";
                }
                break;
            case "TuPian":
                if ($(this).find("ul li").length <= 0) {
                    msgStr += "图片模块必须至少上传一张图片~<br/>";
                }
                break;
            case "HuanDeng":
                if ($(this).find("ul li").length <= 0) {
                    msgStr += "幻灯片必须至少上传一张图片~<br/>";
                }
                $(this).find("ul li a img").each(function () {
                    if ($(this).attr("src") == "") {
                        msgStr += "幻灯片模块中的图片不能为空~<br/>";
                    }
                });
                break;
            case "SouSuo":
                break;
            case "ShangPin":
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