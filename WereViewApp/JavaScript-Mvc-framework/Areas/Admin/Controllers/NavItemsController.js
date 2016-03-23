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
/// <reference path="../../../extensions/inputChangeTracker.js" />
/// <reference path="../../../ProtoType/Array.js" />
/// <reference path="../../../extensions/spinner.js" />

;$.app.controllers = $.app.controllers || {};
$.app.controllers.navItemsController = {
    // any thing related to controllers.
    pageId: "navitems-controller",
    $pageElement: null,
    prop: {
        /// populated from bindEvents.orderingTextBoxChange
        tracker: null,
        formId: "form-id-"
    },
    isDebugging: true,
    initialize: function () {
        var controllers = $.app.controllers,
            current = controllers.navItemsController;
        if (controllers.isCurrentPage(current)) {
            controllers.execute(current);
        }
    },
    getPage: function() {
        return $.app.controllers.navItemsController.$pageElement;
    },
    config :  function() {
        
    },
    actions: {
        /// <summary>
        /// Represents the collection of actions exist inside a controller.
        /// </summary>
        list: function() {
            /// <summary>
            /// Represents list action page.
            /// Refers to the data-action attribute.
            /// </summary>
            /// <returns type=""></returns>
            var self = $.app.controllers.navItemsController,
                $page = self.getPage(),
                urlSchema = $.app.urls.getGeneralUrlSchema(false, ["SaveOrder"]); // pass nothing will give Create,Edit,Delete,Index url
            // urlSchema.edit  will give edit url.


            // create tracker
            var $allInputs = $(".ordering-textbox");
            self.prop.tracker = $.app.inputChangeTracker.createTracker($allInputs);

            // bind events
            self.bindEvents.saveOrderButtonClick(urlSchema.SaveOrder);



            console.log(urlSchema);
        }
    },

    bindEvents: {
        saveOrderButtonClick: function(saveingUrl) {
            var $saveBtn = $.byId("save-order-btn");
            var self = $.app.controllers.navItemsController,
                $page = self.getPage(),
                prop = self.prop,
                tracker = prop.tracker,
                formIdFormat = prop.formId;

            var getFormsData = function (ids, formIdFormat) {
                var formArray = new Array(ids.length);
                for (var i = 0; i < ids.length; i++) {
                    var id = ids[i],
                        $form = $.byId(formIdFormat + id);
                    formArray[i] = $.serializeToJson($form);
                }
                return JSON.stringify(formArray);
            }

            $saveBtn.click(function(e) {
                e.preventDefault();
                // changed inputs ids array, only contains id values.
                var idsArray = tracker.getChangedInputsAttrArray("data-id");
                var data = getFormsData(idsArray, formIdFormat);
                var isInTestingMode = true;
                jQuery.ajax({
                    method: "POST", // by default "GET"
                    url: saveingUrl,
                    data: data, // PlainObject or String or Array
                    dataType: "JSON", //, // "Text" , "HTML", "xml", "script" 
                    contentType: "application/json", // must add this line for server json submit
                }).done(function (response) {
                    if (isInTestingMode) {
                        console.log(response);
                    }
                }).fail(function (jqXHR, textStatus, exceptionMessage) {
                    console.log("Request failed: " + exceptionMessage);
                }).always(function () {
                    console.log("complete");
                });
                console.log(idsArray);
                console.log(data);
            });
        }
    }

}

