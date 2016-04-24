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

;$.app.controllers = $.app.controllers || {};
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
        var controllers = $.app.controllers,
            current = controllers.appController;
        if (controllers.isCurrentPage(current)) {
            controllers.execute(current);
        }
    },
    getPage: function() {
        return $.app.controllers.appController.$pageElement;
    },
    config :  function() {
        
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
        }
    }

}

