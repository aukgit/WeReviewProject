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
/// <reference path="../app.js" />

$.app.controllers.reviewsController = {
    // any thing related to controllers.
    pageId: "reviews-controller",
    $pageElement: null,
    prop: {
        /// populated from bindEvents.orderingTextBoxChange
        tracker: null
    },
    isDebugging: true,
    initialize: function () {
        //anything to config
    },
    getPage: function () {
        return $.app.controllers.reviewsController.$pageElement;
    },
    config: function () {

    },
    actions: {
        /// <summary>
        /// Represents the collection of actions exist inside a controller.
        /// </summary>
        //write: function () {
        //    /// <summary>
        //    /// Represents list action page.
        //    /// Refers to the data-action attribute.
        //    /// </summary>
        //    /// <returns type=""></returns>
        //    //var self = $.app.controllers.reviewsController,
        //    //    $page = self.getPage(),
        //    //    prop = self.prop,
        //    //    urlSchema = $.app.urls.getGeneralUrlSchema(false, ["SaveOrder"]); // pass nothing will give Create,Edit,Delete,Index url
        //    // urlSchema.edit  will give edit url.

        //    //console.log(this);

        //}
    },

    bindEvents: {
        reviewFormSubmit: function (evt, $form) {
            evt.preventDefault(); //stop from submitting.
            console.log($form);
            var currformData = 0,
                url = $form.attr("action"),
                $inputs = $form.find("input"),
                $btn = $.findCachedId("review-submit-btn"),
                $icon = $btn.find("i"),
                $comment = $("#Comments"),
                commentValue = $comment.val();
            if ($.devOrg.checkValidInputs($inputs) && !$.isEmpty(commentValue)) {
                currformData = $form.serializeArray();
                $.ajax({
                    type: "POST",
                    dataType: "json",
                    url: url,
                    data: currformData,
                    beforeSend: function () {
                        $.app.spinner
                            .toggleSpinnerWithBtnPlusUIBlock($btn, $icon);
                    }
                }).done(function (response) {
                    console.log(response);
                    var isDone = response.isDone;
                    if (isDone) {
                        // reload the page, because we can't change the review from here.
                        location.reload(true);
                        //$container.fadeOut("slow");
                    } else {
                        toastr["error"]("Sorry! Failed to update or post the review.", "Failed");
                    }
                }).fail(function (jqXHR, textStatus, exceptionMessage) {
                    console.log("Request failed: " + exceptionMessage);
                }).always(function () {
                    $.app.spinner
                      .toggleSpinnerWithBtnPlusUIBlock($btn, $icon);
                });
            }
        }
    }

}

