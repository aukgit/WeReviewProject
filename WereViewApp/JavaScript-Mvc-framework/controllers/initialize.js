/// <reference path="../jQueryExtend.js" />
;
$.app.controllers = $.app.controllers || {};
$.app.controllers.initialize = function() {
    /// <summary>
    ///     Run all modules inside controllers.
    /// </summary>
    var self = $.app.controllers;

    var keys = Object.keys(self);
    for (var i = 0; i < keys.length; i++) {
        var key = keys[i];
        var nameSpace = self[key];
        if ($.isJson(nameSpace)) {
            var initialize = nameSpace["initialize"];
            $.executeFunction(initialize);
        }
    }
};