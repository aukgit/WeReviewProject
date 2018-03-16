/// <reference path="extensions/hiddenContainer.js" />
; $.app = $.app || {};
; $.app.config = {
    /**
     * app configuration settings.
     * Runs before initializing everything.
     * 
     */
    setup: function() {
        /// <summary>
        /// Setup all configuration.
        /// </summary>
        var app = $.app;
        var classesToCallInitialize = [
            app.hiddenContainer,
            app.spinner,
            $.jQueryCaching,
            $.app.component
        ];
        $.executeArrayOfInitilizeMethods(classesToCallInitialize);

        $.ajaxPrefilter(function (options, originalOptions, jqXHR) {
            options.async = true;
        });
    }
};