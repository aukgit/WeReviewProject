/// <reference path="../app.executeBefore.js" />
/// <reference path="../app.executeAfter.js" />
/// <reference path="../attachInitialize.js" />
; $.app = $.app || {};

$.app.initilizeClasses = [
    $.app.executeBefore
];

$.app.initialize = function () {
    /// <summary>
    /// Run all modules.
    /// First runs config.setup() then all initialize methods in all the controllers.js. 
    /// Then run app.initializeMethods.initialize();
    /// </summary>

    var app = $.app;
    app.config.setup(); // runs all configuration before running all initialize methods.

    $.attachInitializeToArrayAndExecute(app.initilizeClasses);

    // run controller module
    app.controllers.initialize(); // runs all controllers modules.
};

$.app.initializeController = function (controllerCsv) {
    /// <summary>
    /// Run only the controller methods + initialize hidden container , spinner.
    /// First runs config.setup() then all initialize methods in all the controllers.js. 
    /// Then run app.initializeMethods.initialize();
    /// </summary>
    /// <param name="controllerCsv" type="string">controller name csv string.</param>

    var app = $.app;
    app.config.setup(); // runs all configuration before running all initialize methods.
    // run controller module
    app.controllers.initialize(controllerCsv); // run only specific controllers modules.
};

$.app.initializeControllerWithExcuteBeforeMethods = function (controllerCsv) {
    /// <summary>
    /// Run only the controller methods + initialize hidden container , spinner.
    /// First runs config.setup() then all initialize methods in all the controllers.js. 
    /// Then run app.initializeMethods.initialize();
    /// </summary>
    /// <param name="controllerCsv" type="string">controller name csv string.</param>

    var app = $.app;
    app.config.setup(); // runs all configuration before running all initialize methods.

    $.executeArrayOfInitilizeMethods(app.initilizeClasses);

    // run controller module
    app.controllers.initialize(controllerCsv); // run only specific controllers modules.
};