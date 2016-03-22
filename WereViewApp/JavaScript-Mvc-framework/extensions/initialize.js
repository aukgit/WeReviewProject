; $.app = $.app || {};
$.app.initialize = function () {
    /// <summary>
    /// Run all modules.
    /// First runs config.setup() then all initialize methods in all the controllers.js. 
    /// Then run app.initializeMethods.initialize();
    /// </summary>
    
    var app = $.app;
    app.initHiddenContainer();
    app.config.setup(); // runs all configuration before running all initialize methods.
    app.initializeMethods.initialize(); // initialize and run everything in the initializeMethods file.
    // run controller module
    app.spinner.initialize(); // runs all controllers modules.
    app.controllers.initialize(); // runs all controllers modules.

};
