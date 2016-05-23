; $.app = $.app || {};
$.app.global = {
    /**
     * global methods collection 
     * execute methods as per necessary, 
     * ** these methods will NOT run automatically. **
     */
    vars: {

    },
    documentFullSpinnerHide: function () {
        var self = $.app.global,
            $bodyStart = $.findCachedId("body-start"),
            fixedClass = "body-fixed",
            flexible = "body-flexible";

        if ($bodyStart.length > 0) {
            if ($bodyStart.hasClass(fixedClass)) {
                var $loadingBar = $.findCachedId("loading-bar-full-screen");
                $bodyStart.removeClass(fixedClass).addClass(flexible);
                $loadingBar.addClass("animated").removeClass("fadeIn").addClass("fadeOut");
                setTimeout(function () {
                    $loadingBar.hide();
                    $bodyStart.removeClass(fixedClass).addClass(flexible);
                }, 1500);
            }
        }
    },
    enterToNextInputFocus: function ($form, submitAtLast, atLastFocusOnFirst) {
        /// <summary>
        /// Focus to next input if it was visible.
        /// </summary>
        /// <param name="$form" type="type"></param>
        /// <param name="submitAtLast" type="type"></param>
        /// <param name="atLastFocusOnFirst" type="type"></param>
        $form.find("input:text:first-child").focus();

        //var binders = formSelector + " input[type='text']:visible," +
        //    formSelector + " input[type='password']:visible," +
        //    formSelector + " input[type='numeric']:visible," +
        //    formSelector + " input[type='email']:visible," +
        //    //formSelector + " textarea:visible," +
        //    formSelector + " button.selectpicker[type='button']:visible," +
        //    formSelector + " select:visible";
        var binders = "input[type='text']:visible," +
                     "input[type='password']:visible," +
                     "input[type='numeric']:visible," +
                     "input[type='email']:visible," +
                    //formSelector + " textarea:visible," +
                     "button.selectpicker[type='button']:visible," +
                     "select:visible";
        $form.on("keypress", binders, function (e) {
            // var codeAbove = d.keyCode || d.which;
            // console.log("above code :" + codeAbove);
            var code = e.keyCode || e.which;
            // console.log("inside code :" + code);
            if (code === 13) { // Enter key
                e.preventDefault(); // Skip default behavior of the enter key
                var $elements = $form.find(binders);
                //console.log($elements);
                var n = $elements.length;
                var nextIndex = $elements.index(this) + 1;
                if (nextIndex < n) {
                    $elements[nextIndex].focus();
                } else {
                    $elements[nextIndex - 1].blur();
                    if (submitAtLast === true) {
                        $form.submit();
                    } else if (atLastFocusOnFirst === true) {
                        $elements[0].focus();
                    }
                }
            }
        });
    },
    enterToNextInputFocusWithoutTags: function ($form, submitAtLast, isDynamicSelector, atLastFocusOnFirst) {
        $form.find("input:text:first-child").focus();
        var binders = "input[type='text']:visible," +
                     "input[type='password']:visible," +
                     "input[type='numeric']:visible," +
                     "input[type='email']:visible," +
                     "input[type='text']:not(.bootstrap-tagsinput)," +
                    //formSelector + " textarea:visible," +
                     "button.selectpicker[type='button']:visible," +
                     "select:visible";
        var $elements = [];
        var keyPressEvent = function (e) {
            // var codeAbove = d.keyCode || d.which;
            // console.log("above code :" + codeAbove);
            var code = e.keyCode || e.which;
            // console.log("inside code :" + code);
            if (code === 13) { // Enter key
                e.preventDefault(); // Skip default behavior of the enter key
                if (isDynamicSelector === true || $elements.length === 0) {
                    $elements = $form.find(binders);
                }
                //console.log($elements);
                var n = $elements.length;
                var nextIndex = $elements.index(this) + 1;
                if (nextIndex < n) {
                    $elements[nextIndex].focus();
                } else {
                    $elements[nextIndex - 1].blur();
                    if (submitAtLast === true) {
                        $form.submit();
                    } else if (atLastFocusOnFirst === true) {
                        $elements[0].focus();
                    }
                }
            }
        };
        if (isDynamicSelector) {
            $form.on("keypress", binders, keyPressEvent);
        } else {
            var $inputs = $form.find(binders);
            $inputs.on('keypress', keyPressEvent);
        }
    },
    documentFullSpinnerShow: function (message) {
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

    isGivenUrlMatchedDomain: function (url, domain) {
        var regex = new RegExp("^(?:https:\/\/|http:\/\/)*(www\.)*(?:" + domain + ")+(\/)?", "ig");
        var found = url.match(regex);
        return !$.isEmpty(found) && found.length === 1;
    }


};