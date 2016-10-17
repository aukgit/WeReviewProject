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
/// <reference path="../../../extensions/inputChangeTracker.js" />
/// <reference path="../../../ProtoType/Array.js" />
/// <reference path="../../../extensions/spinner.js" />
/// <reference path="../app.executeAfter.js" />
/// <reference path="../app.executeBefore.js" />
/// <reference path="../app.global.js" />
/// <reference path="../app.config.js" />
/// <reference path="../jQueryCaching.js" />
/// <reference path="../jQueryExtend.js" />
/// <reference path="../jQueryExtend.fn.js" />

$.app.controllers.tagsController = {
    // any thing related to controllers.
    pageId: "tags-controller",
    $pageElement: null,
    prop: {
        /// populated from bindEvents.orderingTextBoxChange
        tracker: null,
        formId: "form-id-",
        youtubePlayableBtnId: "apps-preview"
    },
    isDebugging: true,
    initialize: function () {
        /// <summary>
        /// anything to config
        /// </summary>
    },
    getPage: function() {
        return $.app.controllers.tagsController.$pageElement;
    },
    config :  function() {
        
    },
    actions: {
        /// <summary>
        /// Represents the collection of actions exist inside a controller.
        /// </summary>
        index: function () {
            /// <summary>
            /// Represents list action page.
            /// Refers to the data-action attribute.
            /// </summary>
            /// <returns type=""></returns>
            //var self = $.app.controllers.tagsController,
            //    $page = self.getPage(),
            //    prop = self.prop;
                //urlSchema = $.app.urls.getGeneralUrlSchema(false, ["SaveOrder"]); // pass nothing will give Create,Edit,Delete,Index url
            // urlSchema.edit  will give edit url.

            //console.log($page);
            //console.log($page.attr("data-hello"));
            
            // create tracker

        }
    },

    bindEvents: {
        
    }

}

