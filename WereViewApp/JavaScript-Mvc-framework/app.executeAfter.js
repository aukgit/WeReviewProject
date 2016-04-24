/// <reference path="byId.js" />
/// <reference path="D:\Working (SSD)\GitHub\WereViewProject\WereViewApp\Content/Scripts/toastr.js" />
/// <reference path="D:\Working (SSD)\GitHub\WereViewProject\WereViewApp\Content/Scripts/jquery-2.1.4.js" />
/// <reference path="D:\Working (SSD)\GitHub\WereViewProject\WereViewApp\Content/Scripts/jquery.blockUI.js" />
/// <reference path="D:\Working (SSD)\GitHub\WereViewProject\WereViewApp\Content/Scripts/jquery-2.1.4.intellisense.js" />
/// <reference path="app.global.js" />
; $.app = $.app || {};
/**
 * method which runs after all the initialization is done.
 */
$.app.executeAfter = {
    /**
     * method which runs after all the initialization is done.
     * @returns {} 
     */
    documentSpinnerHide: function () {
        $.app.global.documentFullSpinnerHide();
        var timer = setTimeout(function () {
            $.app.global.documentFullSpinnerHide();
            clearTimeout(timer);
        }, 800);
    },
    captureAnchorAndShowSpinner: function () {
        var $anchors = $.findCached("a:link");
        $anchors.click(function (e) {
            e.preventDefault();
            var $link = $(this);
            $.app.global.documentFullSpinnerShow("... Please Wait ...");
            window.location = $link.attr("href");
        });
    },
    owlCaroselComponentEnable: function () {
        var ownComponentEnable = $.findCachedId("owl-carousel-component");
        if (ownComponentEnable.length > 0) {
            $("div.app-suggested-list-items-mobile,div.featured-apps-list-items").owlCarousel({
                responsiveClass: true,
                navigation: true,
                navigationText: [
                  "<i class='fa fa-chevron-circle-left'></i>",
                  "<i class='fa fa-chevron-circle-right'></i>"
                ],
                items: 1, //10 items above 1000px browser width
                //itemsDesktop: [1152, 6], //5 items between 1000px and 901px
                //itemsDesktopSmall: [900, 4], // betweem 900px and 601px
                //itemsTablet: [600, 3], //2 items between 600 and 0
                //itemsMobile: [450, 2],
                itemsCustom: [370, 1]
            });
        }
    }
};