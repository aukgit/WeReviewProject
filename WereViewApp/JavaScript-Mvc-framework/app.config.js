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
        app.hiddenContainer.initialize(); // initialize hidden container module.
        app.spinner.initialize(); // initialize spinner module.
    }
};