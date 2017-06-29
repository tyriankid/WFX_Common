/// <reference path="jquery-1.6.4.min.js" />



function vShop_RegionSelector(containerId, onSelected, defaultRegionText) {
    /// <param name="onSelected" type="function">选择地址后回调,包括两个参数，依次为址址和地址编码</param>

    var regionHandleUrl='/Vshop/RegionHandler.aspx';
    init();
    var address = '';
    var code = 0;


    function init() {
        if (!defaultRegionText)
            defaultRegionText = '请选择省市区';
        var text = '<div class="btn-group bmargin">\
        <button id="address-check-btn" type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">'+defaultRegionText+'<span class="caret"></span></button>\
        <ul name="province" class="dropdown-menu" role="menu"></ul>\
        <ul name="city" class="dropdown-menu hide" role="menu"></ul>\
        <ul name="district" class="dropdown-menu hide" role="menu"></ul>\
        </div>';

        $('#' + containerId).html(text);

        getRegin("province", 0, function (noSub) { bind(noSub); });
    }

    function getRegin(regionType, parentRegionId, callback) {
        /// <param name="regionType" type="String">"province-省,city-市,district-区"</param>
        var text = '';
        
        if (!parentRegionId) {
            parentRegionId = 0;
            address = '';
        }
        jQuery.ajax({
            type: "get",
            async: false,
            url: regionHandleUrl,
            data: { action: 'getregions', parentId: parentRegionId },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                var noSub = false;
                if (data.Status == 'OK') {
                    if (regionType != "province") {
                        text += '<li><a href="#" name="-1"> (返回上一级) </a></li>';
                    }
                    $.each(data.Regions, function (i, province) {
                        text += '<li><a href="#" name="' + province.RegionId + '">' + province.RegionName + '</a></li>';
                    });
                    $('#' + containerId + ' ul[name="' + regionType + '"]').html(text);

                }
                else if (data.Status == 0)
                    noSub = true;
                callback(noSub);
            }
        });
    }

    function bind(noSub) {


        $('#' + containerId + ' ul li a').unbind('click');
        $('#' + containerId + ' ul li a').click(function () {
            var currentUl = $(this).parent().parent();
            var regionId = $(this).attr('name');


            var nextRegionUl = currentUl.next();
            var prevRegionUl = currentUl.prev();
            var nextRegionType = nextRegionUl ? $(nextRegionUl).attr('name') : '';

            //返回上一级处理
            if (regionId == "-1") {
                currentUl.addClass('hide');
                prevRegionUl.removeClass('hide');
                setTimeout(function () {
                    $(".btn-group").addClass('open');
                }, 1);
                var listAddress = address.split(" ");
                if (currentUl.attr('name') == "city") {
                    listAddress = {};
                }
                else if (currentUl.attr('name') == "district") {
                    listAddress.splice(1, 2);
                }
                var str = "";
                for (var i = 0; i < listAddress.length; i++) {
                    str += listAddress[i] + " ";
                }
                address = str;
                return;
            }

            address += $(this).html() + " ";
            //如果没有进入提交状态,并且还有下一个地址选择
            if (!noSub && nextRegionType ) {
                code = $(this).attr('name');
                getRegin(nextRegionType, regionId, function (noSub) {
                    currentUl.addClass('hide');
                    if (noSub && !isGoback) {
                        var first = currentUl.parent().find('ul').first();
                        $(first).removeClass('hide');
                        onSelected(address, code);
                        address = '';
                        bind();
                    }
                    else {
                        nextRegionUl && !noSub && $(nextRegionUl).removeClass('hide');
                        bind();
                        setTimeout(function () {
                            $(".btn-group").addClass('open');
                        }, 1);
                    }
                });
            }
            //否则就是选完提交
            else {
                var first = currentUl.parent().find('ul').first();
                $(first).removeClass('hide');
                currentUl.addClass('hide');
                code = $(this).attr('name');
                onSelected(address, code);
                address = '';
            }
        });

    }
} 