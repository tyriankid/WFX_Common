// JavaScript Document

//ѡ����һ���˵���ʾ
//selecttype
function ShowMenuLeft(firstnode, secondnode, threenode) {
    $.ajax({
        url: "Menu.xml?date=" + new Date(),
        dataType: "xml",
        type: "GET",
        async: false,
        timeout: 10000,
        error: function (xm, msg) {
            alert("loading xml error");
        },
        success: function (xml) {
            $("#menu_left").html('');
            var curenturl = null;
            var curentmenuaspan = null;
            curenturl = $(xml).find("Module[Title='" + firstnode.replace(/\s/g, "") + "']").attr("Link");
            if (secondnode != null) {
                curenturl = secondnode;
            }

            var currRootMenuID = GetRootMenuID(firstnode);
            $(xml).find("Module[Title='" + firstnode.replace(/\s/g, "") + "'] Item").each(function (i) {
                $menutitle = $('<div class="hishop_menutitle"></div>');
                $menuaspan = $("<em id='menu" + currRootMenuID + "_" + i + "' style='background-image:url(images/menu" + currRootMenuID + "_" + i + ".png);'></em><span onclick='ShowSecond(this)'>" + $(this).attr("Title") + "</span>"); //��ȡ������������
                $menutitle.append($menuaspan);
                $(this).find("PageLink").each(function (k) {
                    var link_href = $(this).attr("Link");
                    var link_title = $(this).attr("Title");
                    $alink = $("<i id='menu" + currRootMenuID + "_" + i + "_" + k + "'></i><a href='" + link_href + "' target='frammain' style='display:none'>" + link_title + "</a>");

                    if (link_href == curenturl) {
                        $alink.addClass("curent");
                        curentmenuaspan = $menuaspan;
                    }
                    $menutitle.append($alink);
                    $menutitle.append('<div class="clean"></div>');
                });
                if ($.cookie("Vshop-Manager2") != null && $.cookie("Vshop-Manager2") != "undefined" && $.cookie("Vshop-Manager2") == "1" && currRootMenuID == 7 && $(this).attr("Title") == "��ȫ����") {
                    $alink = $("<i id='menu" + currRootMenuID + "_" + i + "_99'></i><a href='ManageMenu.aspx' target='frammain'>��̨ģ������</a>");
                    $menutitle.append($alink);
                    $menutitle.append('<div class="clean"></div>');
                }
                $("#menu_left").append($menutitle);
            });
            $("#menu_arrow").attr("class", "open_arrow");
            $("#menu_arrow").css("display", "block");
            $(".hishop_menu_scroll").css("display", "block");
            $(".hishop_content_r").css("left", 200);
            if (threenode != null) {
                curenturl = threenode;
            }
            $("#frammain").attr("src", curenturl);
            $("#frammain").width($(window).width() - 168);
            ShowSecond(curentmenuaspan);
        }
    });
    $(".hishop_menu a:contains('" + firstnode + "')").addClass("hishop_curent").siblings().removeClass("hishop_curent");
}

function GetRootMenuID(menuName) {
    var currID = 0;
    switch (menuName) {
        case "΢����":
            currID = 0;
            break;
        case "΢��Ա":
            currID = 1;
            break;
        case "΢Ӫ��":
            currID = 2;
            break;
        case "΢��Ʒ":
            currID = 3;
            break;
        case "΢����":
            currID = 4;
            break;
        case "΢����":
            currID = 5;
            break;
        case "΢ͳ��":
            currID = 6;
            break;
        case "ϵͳ����":
            currID = 7;
            break;
    }
    return currID;
}

//������ҹر���
function ExpendMenuLeft() {
    var clientwidth = $(window).width() - 7;
    if ($(".hishop_menu_scroll").is(":hidden")) {//���չ��
        $("#menu_arrow").attr("class", "open_arrow");
        $(".hishop_menu_scroll").css("display", "block");
        $(".hishop_content_r").css("left", 200);
        $("#frammain").width(clientwidth - 200);
    } else {//�������
        $("#menu_arrow").attr("class", "close_arrow");
        $(".hishop_menu_scroll").css("display", "none");
        $(".hishop_content_r").css("left", 7);
        $("#frammain").width(clientwidth)
    }

}

//��������˵�
function ShowSecond(sencond) {
    if ($(sencond).siblings("a:hidden") != null && $(sencond).siblings("a:hidden").length > 0) {
        $(sencond).siblings("a").css("display", "block");
        $(sencond).parent().attr("class", "hishop_menutitleEx");
    } else {
        $(sencond).siblings("a").css("display", "none");
        $(sencond).parent().attr("class", "hishop_menutitle");
    }
}

//����Ӧ�߶�
function AutoHeight() {
    var clientheight = $(this).height() - 40;
    var clientwidth = $(this).width() - 15;
    $(".hishop_menu_scroll").height(clientheight);
    $(".hishop_content_r").height(clientheight);
    if (!$(".hishop_menu_scroll").is(":hidden")) {
        clientwidth = clientwidth - 200;
    }
    $("#frammain").width(clientwidth);
}


//���ڱ仯
$(window).resize(function () {
    AutoHeight();
});

//���ڼ���
$(function () {
    AutoHeight();
    $("#menu_left a").live("click", function () {
        $("#menu_left a").removeClass("curent");
        $(this).addClass("curent");
    });
    LoadTopLink();

    /*if ($.cookie("guide") == null || $.cookie("guide") == "undefined" || $.cookie("guide") != 1) {
        DialogFrame('help/vshopindex.html', '������', 750, 450);
    }*/
});


function LoadTopLink() {
    $.ajax({
        url: 'LoginUser.ashx?action=login&timestamp=' + new Date().getTime(),
        dataType: 'json',
        type: 'GET',
        timeout: 5000,
        //		error: function(xm, msg) {
        //			alert(msg);
        //		},
        success: function (siteinfo) {
            document.title = siteinfo.sitename;
            //$(".hishop_banneritem a:eq(0)").text(siteinfo.sitename);
            //$(".hishop_banneritem a:eq(1)").text(siteinfo.username);

        }
    });
}
