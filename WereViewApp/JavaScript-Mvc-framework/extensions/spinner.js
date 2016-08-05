/// <reference path="../jQueryExtend.fn.js" />
/// <reference path="../jquery-2.1.4.js" />
/// <reference path="../byId.js" />
/// <reference path="../jquery-2.1.4.intellisense.js" />
;$.app.spinner = {
    id: 'loading-bar',
    $spinner: [],
    spinnerDisplayTypeId: 1,
    type: {
        HtmlTemplate: 1, // renders spinner from Html element
        JsTemplate: 2 // render Html by generating Html from JavaScript.
    },
    prop: {
        spinnerClass: "fa-spin-custom fa-spinner",
        spinnerVisibleAttr: "data-is-spinner-visible"
    },
    initialize: function () {
        var self = $.app.spinner;
        self.$spinner = $.byId(self.id);
        if (!$.isFunc($.blockUI)) {
            throw new Error("Spinner requires jQueryUI Block + Animate.css library. Please download and add those to your project.");
        }
    },
    setMessage: function (contentMessage) {
        /// <summary>
        /// Set message on spinner
        /// </summary>
        /// <param name="tooltipMessage">tooltipMessage message</param>
        /// <param name="contentMessage">content message</param>
        var self = $.app.spinner,
            $loadingbar = self.get(),
            $content = $loadingbar.find(".spinner-content");


        if ($.isEmpty(contentMessage)) {
            contentMessage = "Please wait!";
        }

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
        self.show(null, $blockingElement, $elementToHide, onBlockExecuteMethod);
    },
    show: function (message, $blockingElement, $elementToHide, onBlockExecuteMethod) {
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
        self.setMessage(message);

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
        /// <param name="$e" type="type">Element to toggle hide.</param>
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
    isSpinnerVisibleAt: function ($btn) {
        if ($btn.length > 0) {
            var self = $.app.spinner,
                prop = self.prop,
                attr = prop.spinnerVisibleAttr;
            return $btn.isBoolAttr(attr);
        }
        return false;
    },
    toggleSpinnerWithBtnPlusUIBlock: function ($btn, $currentIcon, $blockingUI, message, onCompleteFunction, spinnerClasses, nonSpinnerClasses, commonClass, right, hideOnSpinnerOnSpinnerClassesRemoved) {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="$btn" type="type">Where to add the spinner.</param>
        /// <param name="$currentIcon" type="type">$ element if any icon present in the btn. This element will be hidden when spinner css is added.</param>
        /// <param name="spinnerClasses" type="type">custom spinner classes. if not given default one will be set : fa-spin-custom fa-spinner</param>
        /// <param name="nonSpinnerClasses" type="type">custom classes to be displayed when spinner is disabled. If not given nothing will happen. if given then it will be added with the i.spinner when by toggling</param>
        /// <param name="right" type="type">if place in right or left. by default left.</param>
        /// <param name="hideOnSpinnerOnSpinnerClassesRemoved" type="type">Hide the spinner icon when toggled. If true then when spinner class is removed this spinner icon object will be hidden and nonSpinnerClasses will have no effect on the system.</param>
        /// <param name="$blockingUI" type="type">Blocking ui</param>
        /// <param name="message" type="type">Message to display when blocks the ui</param>
        /// <param name="onCompleteFunction" type="type">onCompletetion function.</param>
        var self = $.app.spinner;
        var isSpinnerVisible = self.toggleSpinnerWithBtn($btn, $currentIcon, spinnerClasses, nonSpinnerClasses, commonClass, right, hideOnSpinnerOnSpinnerClassesRemoved);
        if (isSpinnerVisible === true) {
            self.show(message, $blockingUI, null, onCompleteFunction);
        } else {
            self.hide($blockingUI, null);
        }
    },
    toggleSpinnerWithBtn: function ($btn, $currentIcon, spinnerClasses, nonSpinnerClasses, commonClass, right, hideOnSpinnerOnSpinnerClassesRemoved) {
        /// <summary>
        /// Attach spinner icon inside a button or anchor or any div tag.
        /// </summary>
        /// <param name="$btn" type="type">Where to add the spinner.</param>
        /// <param name="$currentIcon" type="type">$ element if any icon present in the btn. This element will be hidden when spinner css is added.</param>
        /// <param name="spinnerClasses" type="type">custom spinner classes. if not given default one will be set : fa-spin-custom fa-spinner</param>
        /// <param name="nonSpinnerClasses" type="type">custom classes to be displayed when spinner is disabled. If not given nothing will happen. if given then it will be added with the i.spinner when by toggling</param>
        /// <param name="right" type="type">if place in right or left. by default left.</param>
        /// <param name="hideOnSpinnerOnSpinnerClassesRemoved" type="type">Hide the spinner icon when toggled. If true then when spinner class is removed this spinner icon object will be hidden and nonSpinnerClasses will have no effect on the system.</param>
        if ($btn !== undefined && $btn.length > 0) {
            var $spinner,
                self = $.app.spinner,
                prop = self.prop,
                attr = prop.spinnerVisibleAttr,
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
                var currentlySpinnerDisplaying = $btn.isBoolAttr(attr);
                $spinner.toggleClasses(spinnerClasses); // toggle spinner visible/invisible classes.
                if (currentlySpinnerDisplaying) {
                    // currently spinner is visible , now make it invisible.
                    $btn.setBoolFalseAttr(attr);
                } else {
                    // currently spinner is not visible, make it visible.
                    $btn.setBoolTrueAttr(attr);
                }
                if (!$.isEmpty(nonSpinnerClasses)) {
                    $spinner.toggleClasses(nonSpinnerClasses);
                }
            } else {
                // creating the spinner
                commonClass = $.setDefaultOnEmpty(commonClass, "");
                $spinner = $("<i>", { class: "spinner-icon " + commonClass + " " + spinnerClasses });
                $btn.$attachtedSpinner = $spinner;
                if (right === true) {
                    $btn.append($spinner);
                } else {
                    $btn.prepend($spinner);
                }
                $btn.setBoolTrueAttr(attr);
            }
            if (!$.isEmpty($currentIcon) && $currentIcon.length > 0) {
                $currentIcon.toggleClass("hide");
            }
            return $btn.isBoolAttr(attr);
        }
        return false;
    }
};