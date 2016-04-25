; $.app = $.app || {};
; $.app.spinner = {
    id: 'loading-bar',
    $spinner: [],
    spinnerDisplayTypeId: 1,
    type: {
        HtmlTemplate: 1, // renders spinner from Html element
        JsTemplate: 2  // render Html by generating Html from JavaScript.
    },
    initialize: function () {
        var self = $.app.spinner;
        self.$spinner = $.byId(self.id);
        if (!$.isFunc($.blockUI)) {
            throw new Error("Spinner requires jQueryUI Block + Animate.css library. Please download and add those to your project.");
        }
    },
    setMessage : function (tooltipMessage, contentMessage) {
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
    }
};