$.app.controllers = $.app.controllers || {};
$.app.controllers.homeController = {
    // any thing related to controllers.
    pageId: "home",
    $pageElement: null,
    initialize: function () {
        var controllers = $.app.controllers,
            current = controllers.homeController;
        if (controllers.isCurrentPage(current)) {
            controllers.execute(current);
        }
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