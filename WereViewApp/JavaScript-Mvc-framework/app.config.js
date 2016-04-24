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
        var classes = [
            app.hiddenContainer,
            app.spinner,
            $.jQueryCaching
        ];
        $.executeArrayOfInitilizeMethods(classes);
    }
};