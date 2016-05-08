/// <reference path="../libs/jQuery/jquery-2.2.3.js" />
/// <reference path="../libs/jQuery/jquery-2.2.3.intellisense.js" />
/// <reference path="../libs/jquery.blockUI.js" />
/// <reference path="../extensions/ajax.js" />
/// <reference path="../extensions/clone.js" />
/// <reference path="../extensions/constants.js" />
/// <reference path="../extensions/hiddenContainer.js" />
/// <reference path="../extensions/initialize.js" />
/// <reference path="../extensions/inputChangeTracker.js" />
/// <reference path="../extensions/modal.js" />
/// <reference path="../extensions/pagination.js" />
/// <reference path="../extensions/regularExp.js" />
/// <reference path="../extensions/selectors.js" />
/// <reference path="../extensions/spinner.js" />
/// <reference path="../extensions/urls.js" />
/// <reference path="AccountController.js" />
/// <reference path="AppController.js" />
/// <reference path="controllers.js" />
/// <reference path="homeController.js" />
/// <reference path="initialize.js" />
/// <reference path="../component/component.list.js" />
/// <reference path="../component/component.js" />


;$.app.controllers = $.app.controllers || {};
$.app.controllers.accountController = {
    // any thing related to controllers.
    pageId: "account-controller",
    $pageElement: null,
    prop: {
        /// populated from bindEvents.orderingTextBoxChange
        tracker: null,
        formId: "form-id-",
        youtubePlayableBtnId: "apps-preview"
    },
    isDebugging: true,
    initialize: function () {
        var controllers = $.app.controllers,
            current = controllers.accountController;
        if (controllers.isCurrentPage(current)) {
            controllers.execute(current);
        }
    },
    getPage: function() {
        return $.app.controllers.accountController.$pageElement;
    },
    config :  function() {
        
    },
    actions: {
        /// <summary>
        /// Represents the collection of actions exist inside a controller.
        /// </summary>
        register: function () {
            /// <summary>
            /// Represents list action page.
            /// Refers to the data-action attribute.
            /// </summary>
            /// <returns type=""></returns>
            var self = $.app.controllers.accountController,
                $page = self.getPage(),
                prop = self.prop;
            //$form = ;
            // urlSchema = $.app.urls.getGeneralUrlSchema(false, ["SaveOrder"]); // pass nothing will give Create,Edit,Delete,Index url
            // urlSchema.edit  will give edit url.
            var $form = $page.find(".register-form");
            $form.submit(function(e) {
                e.preventDefault();
            });
        }
    },

    bindEvents: {
        
    }

}

