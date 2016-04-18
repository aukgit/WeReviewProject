/// <reference path="../find-byId.js" />
/// <reference path="../jquery-2.1.4-vsdoc.js" />
/// <reference path="../application-js/jQueryExtend.js" />
/// <reference path="../application-js/app.initialize.js" />
/// <reference path="../application-js/app.js" />
/// <reference path="../application-js/app.run.js" />
/// <reference path="controllers.js" />
/// <reference path="../detect-browser.js" />
/// <reference path="initialize.js" />
/// <reference path="programsearch.js" />
;$.app = $.app || {};
$.app.controllers = {
    // any thing related to controllers.
    isCurrentPage: function (controllerExtendedTypeObject) {
        /// <summary>
        /// Returns true/false if the page is executable or not.
        /// </summary>
        /// <param name="controllerExtendedTypeObject">Controller extended object , which must have pageId, $pageElement prop</param>
        /// <returns type=""></returns>
        var pageId = controllerExtendedTypeObject.pageId,
            $page = $.app.controllers.getPage(controllerExtendedTypeObject);
        controllerExtendedTypeObject.$pageElement = $.byId(pageId);
        $page = controllerExtendedTypeObject.$pageElement;
        return !$.isEmpty($page);
    },
    getPage: function (controllerExtendedTypeObject) {
        return controllerExtendedTypeObject.$pageElement;
    },
    getPageActionName: function (controllerExtendedTypeObject) {
        /// <summary>
        /// Get which mvc action it is referring to.
        /// </summary>
        /// <param name="controllerExtendedTypeObject"></param>
        /// <returns type="">String of name(i.e. Index, Edit etc... which set in the data-action attribute.)</returns>
        var $page = $.app.controllers.getPage(controllerExtendedTypeObject);
        if (!$.isEmpty($page)) {
            return $page.attr("data-action");
        }
        return "";
    },
    execute: function (controllerExtendedTypeObject, runAll) {
        /// <summary>
        /// execute all the actions if in the same page and page is related to that action.
        /// </summary>
        /// <param name="controllerExtendedTypeObject">Controller extended object , which must have actions array of methods.</param>
        /// <returns type=""></returns>
        var actions = controllerExtendedTypeObject.actions,
            actionNames = Object.keys(actions),
            pageAction = $.app.controllers.getPageActionName(controllerExtendedTypeObject),
            needToStop = runAll === false;

        for (var i = 0; i < actionNames.length; i++) {
            var actionName = actionNames[i],
                action = actions[actionName];
            if (actionName === pageAction) {
                $.executeFunction(action);
                if (needToStop === true) {
                    return;
                }
            }
        }
    }
}