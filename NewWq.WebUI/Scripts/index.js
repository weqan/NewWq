$(function(){
    $(window).on("resize", function () {
        //获取窗口的宽度
        let clientW = $(window).width();
        //console.log(clientW);

        //设置临界值
        let isShowBigImage = (clientW >= 800);

        //获取所有的item
        let $allItems = $("#carousel-wq .item");
        console.log($allItems);

        //遍历
        $allItems.each(function (index, item) {
            //取出图片的路径
            let src = isShowBigImage ? $(item).data("lg-img") : $(item).data("sm-img");
            let imgUrl = 'url("' + src + '")'; 
            //设置背景
            $(item).css({
                backgroundImage: imgUrl
            });           

            //设置img标签
            if (!isShowBigImage) {
                let $img = "<img src='" + src + "'>";
                $(item).empty().append($img);                
            } else {                                  
                $(item).empty();
            }


        })
    });

    $(window).trigger("resize");


    //工具提示
    $('[data-toggle="tooltip"]').tooltip();


    //导航处理
    let allList = $("#wq-target li");

    $(allList[1]).on("click", function () {
        $("html,body").animate({ scrollTop: $("#wq_hot").offset().top }, 1000);
    });
    $(allList[3]).on("click", function () {
        $("html,body").animate({ scrollTop: $("#wq_link").offset().top }, 1000);
    });





});



//Dropdown扩展方法
+(function ($, window, undefined) {  
    var $allDropdowns = $();    
    $.fn.dropdownHover = function (options) {
      
        $allDropdowns = $allDropdowns.add(this.parent());

        return this.each(function () {
            var $this = $(this).parent(),
                defaults = {
                    delay: 500,
                    instantlyCloseOthers: true
                },
                data = {
                    delay: $(this).data('delay'),
                    instantlyCloseOthers: $(this).data('close-others')
                },
                options = $.extend(true, {}, defaults, options, data),
                timeout;

            $this.hover(function () {
                if (options.instantlyCloseOthers === true)
                    $allDropdowns.removeClass('open');

                window.clearTimeout(timeout);
                $(this).addClass('open');
            }, function () {
                timeout = window.setTimeout(function () {
                    $this.removeClass('open');
                }, options.delay);
            });
        });
    };

    $('[data-hover="dropdown"]').dropdownHover();
})(jQuery, this);