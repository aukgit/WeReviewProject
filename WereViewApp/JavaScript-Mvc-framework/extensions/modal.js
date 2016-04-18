; $.app = $.app || {};
$.app.modal = {
    $modalStacks: [],
    $modalIds: [],
    push: function ($modal) {
        var self = $.app.modal,
            list = self.$modalStacks;
        list.push($modal);
    },
    pop: function () {
        var self = $.app.modal,
            list = self.$modalStacks,
            $modal = list[list.length - 1];
        list.pop();
        return $modal;
    },
    hidePrevious: function () {
        var self = $.app.modal,
           list = self.$modalStacks,
           $modal = list[list.length - 2];
        if (!$.isEmpty($modal)) {
            $modal.modal('hide');
        }
    },
    showPrevious: function () {
        var self = $.app.modal,
            len = self.$modalStacks.length,
            list = self.$modalStacks;
        var $modal = list[len-2]; // get previous one.
        if (!$.isEmpty($modal)) {
            $modal.modal('hide');
            setTimeout(function () {
                $modal.modal('show');
            }, 400);
        }
    },
    show: function ($modal, $modalBody, url, spinnerShowMethod, spinnerHideMethod, onBeforeRequest,onComplete) {
        /// <summary>
        /// Shows modal and track it..
        /// Calling showPrevious will display previous modal.
        /// </summary>
        /// <param name="$modal"></param>
        /// <param name="$modalBody"></param>
        /// <param name="url"></param>
        /// <param name="spinnerShowMethod"></param>
        /// <param name="spinnerHideMethod"></param>
        /// <param name="onComplete"></param>
        /// <returns type=""></returns>

        //console.log(url);
        $.executeFunction(onBeforeRequest);
        $.executeFunction(spinnerShowMethod);
        var self = $.app.modal;
        jQuery.ajax({
            method: "Get", // by default "GET"
            url: url,
            dataType: "Html" //, // "Text" , "HTML", "xml", "script" 
        }).done(function (response) {
            $.executeFunction(spinnerHideMethod);
            $modalBody.html(response);
            $modal.modal("show");
            self.push($modal);
            $.executeFunction(onComplete);
        }).fail(function (jqXHR, textStatus, exceptionMessage) {
            console.log("Request failed: " + exceptionMessage);
        });
    }

}