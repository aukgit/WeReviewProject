; $.app = $.app || {};
$.app.global = {
    /**
     * global methods collection 
     * execute methods as per necessary, 
     * ** these methods will NOT run automatically. **
     */
    vars: {

    },
    documentFullSpinnerHide: function() {
        var self = $.app.global,
            $bodyStart = $.findCachedId("body-start"),
            fixedClass = "body-fixed",
            flexible = "body-flexible";

        if ($bodyStart.length > 0) {
            if ($bodyStart.hasClass(fixedClass)) {
                var $loadingBar = $.findCachedId("loading-bar-full-screen");
                $bodyStart.removeClass(fixedClass).addClass(flexible);
                $loadingBar.addClass("animated").removeClass("fadeIn").addClass("fadeOut");
                setTimeout(function() {
                    $loadingBar.hide();
                    $bodyStart.removeClass(fixedClass).addClass(flexible);
                }, 1500);
            }
        }
    },

    documentFullSpinnerShow: function(message) {
        var $bodyStart = $.findCachedId("body-start");
        var fixedClass = "body-fixed";
        var flexible = "body-flexible";
        if (!message) {
            message = "...Please wait...";
        }
        if ($bodyStart.length > 0) {
            var $content = $.findCachedId("full-screen-spinner-content");
            var $loadingBar = $.findCachedId("loading-bar-full-screen");
            $bodyStart.removeClass(flexible).addClass(fixedClass);
            $content.html(message);
            $loadingBar.addClass("animated").removeClass("fadeOut").addClass("fadeIn");
            $loadingBar.show();
        }
    },

    isGivenUrlMatchedDomain: function(url, domain) {
        var regex = new RegExp("^(?:https:\/\/|http:\/\/)*(www\.)*(?:" + domain + ")+(\/)?", "ig");
        return url.match(regex);
    }
    

};