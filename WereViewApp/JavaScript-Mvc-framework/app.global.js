; $.app = $.app || {};
$.app.global = {
    /**
     * global methods collection 
     * execute methods as per necessary, 
     * ** these methods will NOT run automatically. **
     */
    documentFullSpinnerHide: function () {
        var $bodyStart = $.byId("body-start");
        var fixedClass = "body-fixed";
        var flexible = "body-flexible";

        if ($bodyStart.length > 0) {
            if ($bodyStart.hasClass(fixedClass)) {
                var $loadingBar = $.byId("loading-bar-full-screen");
                $loadingBar.addClass("animated").removeClass("fadeIn").addClass("fadeOut");
                setTimeout(function () {
                    $loadingBar.hide();
                    $bodyStart.removeClass(fixedClass).addClass(flexible);
                }, 1500);
            }
        }
    },

    documentFullSpinnerShow: function (message) {
        var $bodyStart = $.byId("body-start");
        var fixedClass = "body-fixed";
        var flexible = "body-flexible";
        if (!message) {
            message = "...Please wait...";
        }
        if ($bodyStart.length > 0) {
            var $content = $.byId("full-screen-spinner-content");
            var $loadingBar = $.byId("loading-bar-full-screen");
            $bodyStart.removeClass(flexible).addClass(fixedClass);
            $content.html(message);
            $loadingBar.addClass("animated").removeClass("fadeOut").addClass("fadeIn");
            $loadingBar.show();
        }
    }
};