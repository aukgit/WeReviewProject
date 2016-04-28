/// <reference path="../jQueryExtend.fn.js" />
/// <reference path="../jquery-2.1.4.js" />
/// <reference path="../byId.js" />
/// <reference path="../jquery-2.1.4.intellisense.js" />
; $.app = $.app || {};
;
$.app.spinner = {
    id: 'loading-bar',
    $spinner: [],
    spinnerDisplayTypeId: 1,
    type: {
        HtmlTemplate: 1, // renders spinner from Html element
        JsTemplate: 2 // render Html by generating Html from JavaScript.
    },
    prop: {
        spinnerClass: "fa-spin-custom fa-spinner"
    },
    initialize: function () {
        var self = $.app.spinner;
        self.$spinner = $.byId(self.id);
        if (!$.isFunc($.blockUI)) {
            throw new Error("Spinner requires jQueryUI Block + Animate.css library. Please download and add those to your project.");
        }
    },
    setMessage: function (tooltipMessage, contentMessage) {
        /// <summary>
        /// Set message on spinner
        /// </summary>
        /// <param name="tooltipMessage">tooltipMessage message</param>
        /// <param name="contentMessage">content message</param>
        var self = $.app.spinner,
            $loadingbar = self.get(),
            $anchor = $loadingbar.find("a:first"),
            $span = $anchor.find("span"),
            $content = $loadingbar.find(".spinner-content");

        if ($.isEmpty(tooltipMessage)) {
            tooltipMessage = "Please wait while processing! Any interruption may hamper the process.";
        }
        if ($.isEmpty(contentMessage)) {
            contentMessage = "Please wait!";
        }

        $anchor.attr("title", tooltipMessage)
            .attr("data-original-title", tooltipMessage);
        $span.attr("data-display", tooltipMessage)
            .attr("title", tooltipMessage);
        if ($.isEmpty(contentMessage) === false) {
            $content.attr("title", contentMessage)
                .html(contentMessage);
        } else {
            $content.attr("title", "")
                .html("");
        }
    },
    quickShow: function ($blockingElement, $elementToHide, onBlockExecuteMethod) {
        /// <summary>
        /// Show a spiner with default messages.
        /// </summary>
        /// <param name="$blockingElement" type="type">Element which to block.</param>
        /// <param name="$elementToHide" type="type">Element which to hide during the display of the spinner.</param>
        /// <param name="onBlockExecuteMethod" type="type">An event to execute when the element is blocked.</param>
        var self = $.app.spinner;
        self.show(null, null, $blockingElement, $elementToHide, onBlockExecuteMethod);
    },
    show: function (tooltip, message, $blockingElement, $elementToHide, onBlockExecuteMethod) {
        /// <summary>
        /// show spinner and block UI
        /// </summary>
        /// <param name="message" type="type">
        /// set message to the loading spinner.
        /// </param>
        /// <param name="$blockingElement" type="type">
        /// If any $element is given then UI will only be blocked $element. If none given then whole screen will be blocked.
        /// </param>
        /// <param name="$elementToHide" type="type">
        /// If any $elementToHide is given then this element will be hidden when the UI is blocked.
        /// </param>
        var self = $.app.spinner,
            $spinner = self.get();
        self.setMessage(tooltip, message);

        if (!$.isEmpty($elementToHide)) {
            $elementToHide.hide();
        }
        var options = {
            message: $spinner,
            onBlock: onBlockExecuteMethod
        };
        if (!$.isEmpty($blockingElement)) {
            $blockingElement.block(options);
        } else {
            // block whole screen
            $.blockUI(options);
        }
    },

    hide: function ($unBlockingElement, $elementToDisplay) {
        /// <summary>
        /// hide spinner and unblock the UI
        /// </summary>
        /// <param name="$unBlockingElement" type="type"></param>
        /// <param name="$elementToDisplay" type="type"></param>
        var self = $.app.spinner;
        if (!$.isEmpty($unBlockingElement)) {
            $unBlockingElement.unblock();
        } else {
            // unblock whole screen
            $.unblockUI();
        }
        if (!$.isEmpty($elementToDisplay)) {
            $elementToDisplay.show("slow");
        }
    },


    get: function () {
        /// <summary>
        /// Get the spinner element.
        /// </summary>
        /// <returns type="">Returns $.app.spinner.$spinner</returns>
        return $.app.spinner.$spinner;
    },

    toogleSpinnerClass: function ($e, newClasses, hideOnSpinnerOnSpinnerClassesRemoved) {
        /// <summary>
        /// Toggle spinner classes on the given $element.
        /// </summary>
        /// <param name="$e" type="type"></param>
        /// <param name="newClasses" type="type">use spaces for multiple classes</param>
        /// <param name="hideOnSpinnerOnSpinnerClassesRemoved" type="type">true/false if the spinner element should hide when removing the spinner classes.</param>
        var self = $.app.spinner,
            prop = self.prop,
            spinnerClass = prop.spinnerClass;
        if ($e.length > 0) {
            if (!$e.hasClass("fa")) {
                spinnerClass += " fa";
            }
            if (hideOnSpinnerOnSpinnerClassesRemoved === true) {
                $e.toggleClass("hide");
            }
            $e.toggleClasses(spinnerClass);
            $e.toggleClasses(newClasses);
        }
    },

    toggleSpinnerWithBtn: function ($btn, $currentIcon, spinnerClasses, nonSpinnerClasses, right, hideOnSpinnerOnSpinnerClassesRemoved) {
        /// <summary>
        /// Attach spinner icon replacing a existing icon.
        /// </summary>
        /// <param name="$btn" type="type">Where to add the spinner.</param>
        /// <param name="$currentIcon" type="type">$ element if any icon present in the btn.</param>
        /// <param name="spinnerClasses" type="type">custom spinner classes.</param>
        /// <param name="nonSpinnerClasses" type="type">custom classes to be displayed when spinner is disabled.</param>
        /// <param name="right" type="type">if place in right or left. by default left.</param>
        /// <param name="hideOnSpinnerOnSpinnerClassesRemoved" type="type">Hide the spiner icon when toggled.</param>
        if ($btn.length > 0) {
            var $spinner,
                self = $.app.spinner,
                prop = self.prop,
                attr = "data-has-spinner",
                spinnerClass = prop.spinnerClass;
            if ($.isEmpty(spinnerClasses)) {
                if (!$btn.hasClass("fa")) {
                    spinnerClass += " fa";
                }
                spinnerClasses = spinnerClass;
            }

            if (!$.isEmpty($btn.$attachtedSpinner)) {
                $spinner = $btn.$attachtedSpinner;
                if (hideOnSpinnerOnSpinnerClassesRemoved === true) {
                    $spinner.toggleClass("hide");
                }
                var currentlySpinnerDisplaying = $btn.attr(attr) === "1";
                $spinner.toggleClasses(spinnerClasses);
                if (currentlySpinnerDisplaying) {
                    if (!$.isEmpty(nonSpinnerClasses)) {
                        $spinner.toggleClasses(nonSpinnerClasses);
                    }
                    $btn.attr(attr, "0");
                } else {
                    $btn.attr(attr, "1");
                }
               
            } else {
                $spinner = $("<i>", { class: "spinner-icon " + spinnerClasses });
                $btn.$attachtedSpinner = $spinner;
                if (right === true) {
                    $btn.append($spinner);
                } else {
                    $btn.prepend($spinner);
                }
                $btn.attr(attr, "1");
            }
            if (!$.isEmpty($currentIcon) && $currentIcon.length > 0) {
                $currentIcon.toggleClass("hide");
            }

        }
    }
};