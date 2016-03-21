/// <reference path="../../../jQueryExtend.js" />
/// <reference path="../../../extensions/spinner.js" />
/// <reference path="../../../extensions/ajax.js" />
/// <reference path="../../../extensions/pagination.js" />
/// <reference path="../../../extensions/selectors.js" />
/// <reference path="../../../extensions/urls.js" />
/// <reference path="../../../extensions/constants.js" />
/// <reference path="../../../extensions/ajax.js" />
/// <reference path="../../../controllers/controllers.js" />
/// <reference path="../../../controllers/initialize.js" />
/// <reference path="../../../app.global.js" />
/// <reference path="../../../app.js" />
/// <reference path="../../../app.run.js" />
/// <reference path="../../../byId.js" />
/// <reference path="D:\Working\GitHub\WereViewProject\WereViewApp\Content/Scripts/jquery-2.1.4.js" />

;$.app.controllers = $.app.controllers || {};
$.app.controllers.navItemsController = {
    // any thing related to controllers.
    pageId: "navitems-controller",
    $pageElement: null,
    initialize: function () {
        var controllers = $.app.controllers,
            current = controllers.navItemsController;
        if (controllers.isCurrentPage(current)) {
            controllers.execute(current);
        }
    },
    isDebugging: true,
    getPage: function() {
        return $.app.controllers.navItemsController.$pageElement;
    },
    actions: {
        /// <summary>
        /// Represents the collection of actions exist inside a controller.
        /// </summary>
        list: function () {
            /// <summary>
            /// Represents list action page.
            /// Refers to the data-action attribute.
            /// </summary>
            /// <returns type=""></returns>
            var self = $.app.controllers.navItemsController,
                $page = self.getPage(),
                urlSchema = $.app.urls.getGeneralUrlSchema(false, ["Add", "SaveOrder"]); // pass nothing will give add,edit,save,delete url
            // urlSchema.edit  will give edit url.
            // in the 
            console.log(urlSchema);
        }
    }

}

