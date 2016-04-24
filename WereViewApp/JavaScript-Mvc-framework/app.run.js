/// <reference path="app.initialize.js" />
/// <reference path="app.js" />
/// <reference path="app.run.js" />
/// <reference path="jQueryExtend.js" />
/// <reference path="../Controllers/controllers.js" />
/// <reference path="../Controllers/initialize.js" />
/// <reference path="../Controllers/programsearch.js" />
/// <reference path="../jquery-2.1.4-vsdoc.js" />
/// <reference path="../find-byId.js" />
/// <reference path="attachInitialize.js" />
; $.app = $.app || {};
$.app.initialize = $.app.initialize || {};
$(document).ready(function () {
    $.app.initialize(); // initialize everything
});

window.onload= function() {
    var app = $.app;
    $.attachAndExecuteInitialize(app.executeAfter);
}