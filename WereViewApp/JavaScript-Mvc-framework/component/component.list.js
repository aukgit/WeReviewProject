/// <reference path="../libs/jQuery/jquery-2.2.3.js" />
/// <reference path="../libs/jQuery/jquery-2.2.3.intellisense.js" />
/// <reference path="../jQueryCaching.js" />
/// <reference path="../app.js" />
/// <reference path="../Prototype/Array.js" />
/// <reference path="../extensions/selectors.js" />
; $.app = $.app || {};
; $.app.component = $.app.component || {};
; $.app.component.list = $.app.component.list || {};

/**
 * Loads this component if this string value present in the hidden field of "Component-Enable"
 * Please add a hidden with id "Component-Enable"
 * <input id="Component-Enable" value="revolution-gallery,form-validation" />
 * @returns {} 
 */

$.app.component.list = {
    "revolution-gallery": function () {
        var $frontPageGallyery = $(".tp-banner");
        if ($frontPageGallyery.length > 0) {
            $frontPageGallyery.show().revolution({
                dottedOverlay: "none",
                delay: 5000,
                startwidth: 960,
                startheight: 320,
                hideThumbs: 10,
                fullWidth: "off",
                navigationType: "bullet",
                navigationStyle: "preview2",
                forceFullWidth: "off"
            });
        }
    },
    "form-validation": function () {
        var app = $.app,
            $processForm = app.getProcessForm();

        if ($processForm.length > 0) {
            $processForm.serverValidate({
                crossDomain: false,
                multipleRequests: true,
                checkValidationBeforeSendingRequest: true,
                dontSendSameRequestTwice: false,
                disableInputOnValidation: false,
                focusPersistIfNotValid: false,
                hideOnValidation: false
            });
        }
    },
    "tag": function () {
        var app = $.app,
            $processForm = app.getProcessForm();

        if ($processForm.length > 0) {
            var $createdTags = $(".tag-inputs");
            if ($createdTags.length > 0) {
                var $tokenField = $processForm.find("[name='__RequestVerificationToken']"),
                    token = $tokenField.val();
                for (var i = 0; i < $createdTags.length; i++) {
                    var $tagsInput = $($createdTags[0]),
                        urlToPost = $tagsInput.attr("data-url");
                    //
                    $tagsInput.tagsinput({
                        freeInput: true,
                        trimValue: true,
                        typeahead: {
                            source: function (query) {
                                return $.post(urlToPost, { id: query, __RequestVerificationToken: token }).done(function (response) {
                                    //console.log("tags:");
                                    //console.log("response:");
                                    //console.log(response);
                                });
                            }
                        },
                        onTagExists: function (item, $tag) {
                            if ($.isEmpty($tag)) {
                                $tag.hide.fadeIn();
                            }
                        }
                    });
                }
            }

        }
    },
    "isotop": function () {
        var $isotopContainer = $("ul.search-page-apps-list");
        if ($isotopContainer.length > 0) {
            var $filterIsotopItems = $('.filter li a');
            if ($filterIsotopItems.length > 0) {
                $filterIsotopItems.click(function () {
                    $('.filter').find("li a").removeClass('active');
                    $(this).addClass('active');
                    var selector = $(this).attr('data-filter');

                    $isotopContainer.isotope({
                        filter: selector
                    });
                    return false;
                });
            }
        }
    },
    "convert-youtube-link-to-embed": function () {
        /// <summary>
        /// Add id "youtube-link-convert" on input or put the input as "#server-validation-form .youtube-link-convert"
        /// then it will work.
        /// </summary>
        var app = $.app,
            id = "youtube-link-convert",
            $anyInputs = $.findCachedId(id),
            $processForm = app.getProcessForm(),
            $inputs = [],
            isGivenUrlMatchedDomain = app.global.isGivenUrlMatchedDomain;

        if ($anyInputs.length > 0) {
            $inputs = $anyInputs;
        } else if ($processForm.length > 0) {
            $inputs = $processForm.find("." + id);
        }

        if ($inputs.length > 0) {
            $inputs.blur(function () {
                var $this = $(this),
                    text = $this.val();
                if (!$.isEmpty(text) && isGivenUrlMatchedDomain("youtu\.be|youtube\.com")) {
                    text.replace(/(?:https:\/\/|http:\/\/)?(?:www\.)?(?:youtube\.com|youtu\.be)\/(?:watch\?v=)?(.+)/g, '<iframe width="420" height="345" src="http://www.youtube.com/embed/$1" frameborder="0" allowfullscreen></iframe>');
                    $this.val(text);
                }
            });
        }
    },
    "owl-carousel": function () {
        var owlOptions = {
            responsiveClass: true,
            navigation: true,
            navigationText: [
                "<i class='fa fa-chevron-circle-left'></i>",
                "<i class='fa fa-chevron-circle-right'></i>"
            ],
            items: 1, //10 items above 1000px browser width
            //itemsDesktop: [1152, 6], //5 items between 1000px and 901px
            //itemsDesktopSmall: [900, 4], // betweem 900px and 601px
            //itemsTablet: [600, 3], //2 items between 600 and 0
            //itemsMobile: [450, 2],
            itemsCustom: [370, 1]
        };

        $(".app-suggested-list-items-mobile").owlCarousel(owlOptions);
        $(".featured-apps-list-items").owlCarousel(owlOptions);

        var $suggestionCarosel = $(".owl-list");
        if ($suggestionCarosel.length > 0) {
            $suggestionCarosel.owlCarousel({
                navigation: true,
                navigationText: [
                    "<i class='fa fa-chevron-circle-left'></i>",
                    "<i class='fa fa-chevron-circle-right'></i>"
                ],
                items: 7, //10 items above 1000px browser width
                itemsDesktop: [1152, 6], //5 items between 1000px and 901px
                itemsDesktopSmall: [966, 5], // betweem 900px and 601px
                itemsTabletSmall: [730, 4],
                itemsTablet: [600, 3], //2 items between 600 and 0
                //itemsCustom: [[0, 2], [435, 3], [450, 2], [600, 3], [730, 4], [900, 5],  [950, 6]], // [[740, 6], [1000, 8], [1200, 10], [1600, 16]]
                itemsMobile: [450, 2]
            });
        }

        var $appsPreview = $.findCachedId("apps-preview");
        if ($appsPreview.length > 0) {
            $appsPreview.owlCarousel({
                slideSpeed: 300,
                paginationSpeed: 400,
                singleItem: true,
                items: 1,
                itemsDesktop: false,
                itemsDesktopSmall: false,
                itemsTablet: false,
                itemsMobile: false,
                stopOnHover: true,
                navigation: true, // Show next and prev buttons
                pagination: false,
                autoHeight: true,
                navigationText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"]
            });
        }
    }
}
