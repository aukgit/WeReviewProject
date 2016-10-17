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

$.app.controllers.homeController = {
    // any thing related to controllers.
    pageId: "home",
    $pageElement: null,
    initialize: function () {
        //anything to config

    },
    isDebugging: true,
    actions: {
        /// <summary>
        /// Represents the collection of actions exist inside a controller.
        /// </summary>
        index: function () {
            /// <summary>
            /// Represents index action page.
            /// Refers to the data-action attribute.
            /// </summary>
            /// <returns type=""></returns>
            var self = $.app.controllers.homeController,
                urlSchema = $.app.url.getGeneralUrlSchema(); // pass nothing will give add,edit,save,delete url
            
        }

    }
}