
/*---------------------------------------------------------/

                       ☀ 唐明明20151015 ☀

/---------------------------------------------------------*/


var isClickRed = false;
$(document).ready(function() {
    // 点击redbutton按钮时执行以下全部
    $('.chai_btn').click(function () {
        if (isClickRed) return;
        isClickRed = true;
        // 在带有red样式的div中添加shake-chunk样式
        $('.imgbox').addClass('tongRotate');
        //抽红包，成功后显示下面
        $('.red-jg h5').html("");
        getPrize();
        
    });
});


function setResult(no) {
    // 点击按钮500毫秒后执行以下操作
    setTimeout(function () {
        // 在带有imgbox样式的div中删除shake-chunk样式
        $('.imgbox').removeClass('shake-chunk');
        // 将imgboxbutton按钮隐藏
        $('.chai_btn').css("display", "none");
        //设置抽奖结果
        if (no > 0) {
            $('.red-jg h5').html(no + "<span>元</span>");
            // 修改red 下 span   背景图
            $('.red > span').css("background-image", "url(../Templates/vshop/common/src/red-y.png)");
        }
        else {
            // 修改red 下 span   背景图
            $('.red > span').css("background-image", "url(../Templates/vshop/common/src/red-x.png)");
        }
        // 修改red-jg的css显示方式为块
        $('.red-jg').css("display", "block");

    }, 500);
}








