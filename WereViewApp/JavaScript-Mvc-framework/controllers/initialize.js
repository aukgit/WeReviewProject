/// <reference path="../extensions/ajax.js" />
/// <reference path="../extensions/clone.js" />
/// <reference path="../extensions/constants.js" />
/// <reference path="../extensions/hiddenContainer.js" />
/// <reference path="../extensions/inputChangeTracker.js" />
/// <reference path="../extensions/modal.js" />
/// <reference path="../extensions/pagination.js" />
/// <reference path="../extensions/regularExp.js" />
/// <reference path="../extensions/selectors.js" />
/// <reference path="../extensions/spinner.js" />
/// <reference path="../libs/DevOrgPlugins/WeReviewApps.js" />
/// <reference path="../libs/jquery.blockUI.js" />
/// <reference path="../extensions/urls.js" />
/// <reference path="../libs/toastr.js" />
/// <reference path="../libs/underscore.js" />
/// <reference path="../byId.js" />
/// <reference path="../controllers.js" />
/// <reference path="../jQueryCaching.js" />
/// <reference path="../jQueryExtend.fn.js" />
/// <reference path="../app.global.js" />
/// <reference path="../jQueryExtend.js" />
/// <reference path="../schema/hashset.js" />
/// <reference path="../attachInitialize.js" />
/// <reference path="../schema/schema.js" />
/// <reference path="../libs/jQuery/jquery-2.2.3.intellisense.js" />
/// <reference path="../schema/url.js" />
/// <reference path="../Prototype/Array.js" />

//; $.app.controllers = $.app.controllers || {};
$.app.controllers.initialize = function (controllerName) {
    /// <summary>
    /// Run all modules inside controllers.
    /// </summary>
    var app = $.app,
        controllersList = app.controllers,
        runAll = true,
        keys = [],
        key,
        pageId,
        i,
        controllers = app.controllers,
        currentController,
        bindingEventsNames,
        binding = app.events.binding,
        addController = false;
    if ($.isEmpty(controllerName)) {
        keys = Object.keys(controllersList);
    } else {
        keys = controllerName.split(",");
        addController = true;
    }

    for (i = 0; i < keys.length; i++) {
        key = keys[i];
        if (addController === true) {
            currentController = controllersList[key + "Controller"];
        } else {
            currentController = controllersList[key];
        }
        pageId = currentController["pageId"];
        if (!$.isEmpty(pageId)) {
            if (controllers.isCurrentPage(currentController)) {
                controllers.execute(currentController, runAll);
                bindingEventsNames = controllers.getPageBindings(currentController);
                if (bindingEventsNames === "*") {
                    // binds all bindings
                    binding.executeAll(currentController);
                } else if (bindingEventsNames !== "") {
                    // binds specific events using csv
                    binding.execute(currentController, bindingEventsNames);
                }
                $.executeFunction(currentController["initialize"]);
            }
        }
    }
}