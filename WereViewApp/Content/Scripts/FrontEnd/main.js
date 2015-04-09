$(document).ready(function ($) {
    "use strict";
    /*-----------------------------------------------------------------------------------*/
    /*    Parallax
    /*-----------------------------------------------------------------------------------*/

    jQuery.stellar({
        horizontalScrolling: false,
        scrollProperty: 'scroll',
        positionProperty: 'position'
    });
    /*-----------------------------------------------------------------------------------*/
    /*	Easy Pie Chart
    /*-----------------------------------------------------------------------------------*/
    var options = {
        scaleColor: false,
        trackColor: 'rgba(266,144,0,0.0)',
        barColor: '#ff7200',
        lineWidth: 2,
        lineCap: 'butt',
        size: 253
    };
    $().jetmenu();
    window.addEventListener('DOMContentLoaded', function () {
        var charts = [];
        [].forEach.call(document.querySelectorAll('.chart'), function (el) {
            charts.push(new EasyPieChart(el, options));
        });
    });
    //jQuery('.skillbar').each(function () {
    //    jQuery(this).find('.skillbar-bar').animate({
    //        width: jQuery(this).attr('data-percent')
    //    }, 4000);
    //});





    /*-----------------------------------------------------------------------------------*/
    /* 	FULL SCREEN
    /*-----------------------------------------------------------------------------------*/
    //$('.full-screen').superslides({
    //});





    /*-----------------------------------------------------------------------------------*/
    /* 	ANIMATION
    /*-----------------------------------------------------------------------------------*/
    var wow = new WOW({
        boxClass: 'wow',      // animated element css class (default is wow)
        animateClass: 'animated', // animation css class (default is animated)
        offset: 100,          // distance to the element when triggering the animation (default is 0)
        mobile: false        // trigger animations on mobile devices (true is default)
    });
    wow.init();



    /*-----------------------------------------------------------------------------------*/
    /*    PRODUCTS IMAGE SLIDES
    /*-----------------------------------------------------------------------------------*/

    //$('#imageGallery').lightSlider({
    //    gallery: true,
    //    auto: true,
    //    item: 1,
    //    thumbItem: 9,
    //    slideMargin: 0,
    //    loop: true,
    //    mode: "slide"
    //});









    /*-----------------------------------------------------------------------------------*/
    /*    STICKY NAVIGATION
    /*-----------------------------------------------------------------------------------*/

    $(".sticky").sticky({ topSpacing: 0 });






    /*-----------------------------------------------------------------------------------*/
    /*  ISOTOPE PORTFOLIO
    /*-----------------------------------------------------------------------------------*/

    //var $container = $('.portfolio-wrapper .items');
    //$container.imagesLoaded(function () {
    //    $container.isotope({
    //        itemSelector: '.item',
    //        layoutMode: 'fitRows'
    //    });
    //});
 
});