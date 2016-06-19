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


//;$.app.controllers = $.app.controllers || {};
$.app.controllers.appController = {
    // any thing related to controllers.
    pageId: "app-controller",
    $pageElement: null,
    prop: {
        /// populated from bindEvents.orderingTextBoxChange
        tracker: null,
        formId: "form-id-",
        youtubePlayableBtnId: "apps-preview"
    },
    isDebugging: true,
    initialize: function () {
        //anything to config
    },
    getPage: function () {
        return $.app.controllers.appController.$pageElement;
    },
    config: function () {

    },
    actions: {
        /// <summary>
        /// Represents the collection of actions exist inside a controller.
        /// </summary>
        SingleAppDisplay: function () {
            /// <summary>
            /// Represents list action page.
            /// Refers to the data-action attribute.
            /// </summary>
            /// <returns type=""></returns>
            var self = $.app.controllers.appController,
                $page = self.getPage(),
                prop = self.prop,
                urlSchema = $.app.urls.getGeneralUrlSchema(false, ["SaveOrder"]); // pass nothing will give Create,Edit,Delete,Index url
            // urlSchema.edit  will give edit url.


            // bind events
            self.bindEvents.youtubePlayBtnClick();



            $.frontEndAppDetailsPage = {
                $showMoreBtnContainer: [],
                $showMoreBtns: [],
                $showLessBtns: [],
                $moreExcert: [],
                execute: function () {
                    this.$showMoreBtnContainer = $(".show-more-btns-container");
                    this.$showMoreBtns = $(".see-more-btn");
                    this.$showLessBtns = $(".less-btn");
                    this.$moreExcert = $(".more");
                    if (this.$moreExcert.length > 0) {
                        this.$moreExcert.hide();
                    }

                    var $numberElement = $(".app-viewed-numbers");
                    if ($numberElement.length > 0) {
                        $numberElement.number(true);
                    }

                    this.$showMoreBtns.click(function () {
                        var $this = $(this);
                        var moreReference = $this.attr("data-ref");
                        var dataId = $this.attr("data-id");
                        var dataRefSelector;
                        var dataIdSelector = _.isUndefined(dataId) === false ? "[data-id='" + dataId + "']" : "";
                        if (_.isUndefined(moreReference) === false) {
                            dataRefSelector = "[data-ref='" + moreReference + "']" + dataIdSelector + ":first";

                            var $specificMoreExcertFound = $.frontEndAppDetailsPage.$moreExcert.filter(dataRefSelector);
                            if ($specificMoreExcertFound.length > 0) {
                                $specificMoreExcertFound.show("slow");
                                $specificMoreExcertFound.css("display", "inline");
                            }
                            var $moreBtnContainer = $.frontEndAppDetailsPage.$showMoreBtnContainer.filter(dataRefSelector);
                            if ($moreBtnContainer.length > 0) {
                                $moreBtnContainer.hide("slow");
                            }
                        }
                    });

                    this.$showLessBtns.click(function () {
                        var $this = $(this);
                        var moreReference = $this.attr("data-ref");
                        var dataId = $this.attr("data-id");
                        var dataRefSelector;
                        var dataIdSelector = _.isUndefined(dataId) === false ? "[data-id='" + dataId + "']" : "";
                        if (_.isUndefined(moreReference) === false) {
                            dataRefSelector = "[data-ref='" + moreReference + "']" + dataIdSelector + ":first";

                            var $specificMoreExcertFound = $.frontEndAppDetailsPage.$moreExcert.filter(dataRefSelector);
                            if ($specificMoreExcertFound.length > 0) {
                                $specificMoreExcertFound.hide("slow");
                            }
                            var $moreBtnContainer = $.frontEndAppDetailsPage.$showMoreBtnContainer.filter(dataRefSelector);
                            if ($moreBtnContainer.length > 0) {
                                $moreBtnContainer.show("slow");
                            }
                        }
                    });
                }
            };
            $.frontEndAppDetailsPage.execute();

            // create tracker

        }
    },

    bindEvents: {
        youtubePlayBtnClick: function () {
            var self = $.app.controllers.appController,
                $page = self.getPage(),
                prop = self.prop;

            var $youtubeVideoContainer = $.findCachedId(prop.youtubePlayableBtnId);
            if ($youtubeVideoContainer.length === 1) {
                $youtubeVideoContainer.find(".playable-btn").click(function () {
                    var $iframe = $youtubeVideoContainer.find("iframe");
                    var $this = $(this);
                    if ($iframe.length === 1) {
                        $iframe[0].src += "?rel=0&controls=1&autoplay=1";
                        $this.hide("slow");
                        $this.unbind("click");//or some other way to make sure that this only happens once
                    }
                });
            }
        },
        getReviewForm: function (e, $this) {
            e.preventDefault();
            var $container = $this.getReferenceIdElement();
            var cls = "already-embedded",
                reviewsControllerPageId = "reviews-controller",
                spinnerMessage = null; // 
            if (!$container.hasClass(cls)) {
                $container.addClass(cls);
                $container.hide();
                // inputs to load the review write form only via url
                var reqVerifyFieldsArray = $("#review-request-fields").find("input").serializeArray();
                //console.log(reqVerifyFields);
                $.ajax({
                    type: "POST",
                    dataType: "html",
                    url: $this.getUrlString(),
                    data: reqVerifyFieldsArray,
                    success: function (response) {
                        //var $response = $(response);
                        $container.html(response);
                        $container.show("slow");
                        //var $form = $response.filter("form");
                        //displayModal(htmlResponse);
                        var $form = $.byId(reviewsControllerPageId);
                        if ($form.hasClass("write")) {
                            //stop submitting and go through the processes and pages
                            $.devOrg.uxFriendlySlide("#" + reviewsControllerPageId,
                                true,
                                true //don't submit
                            );
                        }
                        $.app.controllers.initialize("reviews"); //init reviews controller.
                        // Now from reviews controller there is form submit event in the bindEvents json
                        // that method will take care of the rest and submit the form to the appropriate section.
                    },
                    beforeSend: function () {
                        $.app.spinner.show(spinnerMessage);
                    }
                }).always(function () {
                    $.app.spinner.hide();
                });
            } else {
                $container.toggle("slow");

            }
        }

    }
}
