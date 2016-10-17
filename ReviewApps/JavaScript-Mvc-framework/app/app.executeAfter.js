/// <reference path="byId.js" />
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
        }, 2500);
    },
    /**
     * bind anchor click prevention 
     * @returns {} 
     */
    captureAnchorAndShowSpinner: function () {
        var $anchors = $.findCached("a:link");
        $anchors.click(function (e) {
            var $link = $(this),
                href = $link.attr("href");
            if (!$.isEmpty(href)) {
                var startsWith = href[0];
                var isInvalidUrl = href === "" || startsWith === "" || startsWith === "#" || href.indexOf("javascript") > -1;
                if (!isInvalidUrl) {
                    e.preventDefault();
                    $.app.global.documentFullSpinnerShow("...Please Wait...");
                    window.location = $link.attr("href");
                }
            }
        });
    },
    
};