/// <reference path="byId.js" />
/// <reference path="D:\Working (SSD)\GitHub\WereViewProject\WereViewApp\Content/Scripts/toastr.js" />
/// <reference path="D:\Working (SSD)\GitHub\WereViewProject\WereViewApp\Content/Scripts/jquery-2.1.4.js" />
/// <reference path="D:\Working (SSD)\GitHub\WereViewProject\WereViewApp\Content/Scripts/jquery.blockUI.js" />
/// <reference path="D:\Working (SSD)\GitHub\WereViewProject\WereViewApp\Content/Scripts/jquery-2.1.4.intellisense.js" />
; $.app = $.app || {};
/**
* runs all the methods after initialize method.
*/
$.app.executeBefore = {
    /**
     * runs all the methods after initialize method.
     */
    toasterComponentSetup: function () {
        if (!$.isEmpty(toastr)) {
            toastr.options = {
                "closeButton": false,
                "debug": false,
                "newestOnTop": true,
                "progressBar": true,
                "positionClass": "toast-bottom-left",
                "preventDuplicates": true,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }
    },
    toolTipShow: function () {
        var $tooltipItems = $('.tooltip-show');
        if ($tooltipItems.length > 0) {
            $tooltipItems.tooltip({ container: 'body' });
        }

    },
    seoHide: function () {
        var $seoHideItems = $(".seo-hide");
        if ($seoHideItems.length > 0) {
            $seoHideItems.hide();
        }
    },
    menuEnable: function () {
        $().jetmenu();
        var menuPage = $("#menu-item-edit-page");
        if (menuPage.length > 0) {
            var div = $("#hasDropdownDiv");
            div.hide();
            $("#HasDropDown").click(function () {
                if (this.checked) {
                    div.show('slow');
                } else {
                    div.hide('slow');
                }
            });
        }
    },
    bootstrapTableComponentEnable: function () {
        var $tables = $("table.bootstrap-table-do");
        if ($tables.length > 0) {
            $tables.bootstrapTable();
        }
    },
    datePickerComponentEnable: function () {
        if ($.isFunc($.datetimepicker)) {
            $(".datetimepicker-start").datetimepicker({
                pickDate: true,                 //en/disables the date picker
                pickTime: true,                 //en/disables the time picker
                useMinutes: true,               //en/disables the minutes picker
                useSeconds: true,               //en/disables the seconds picker
                useCurrent: true,               //when true, picker will set the value to the current date/time     
                minuteStepping: 1,               //set the minute stepping
                defaultDate: "",                 //sets a default date, accepts js dates, strings and moment objects
                disabledDates: [],               //an array of dates that cannot be selected
                enabledDates: [],                //an array of dates that can be selected
                sideBySide: true              //show the date and time picker side by side

            });

            $(".datepicker-start").datetimepicker({
                pickDate: true,                 //en/disables the date picker
                pickTime: false,                 //en/disables the time picker
                useMinutes: false,               //en/disables the minutes picker
                useSeconds: false,               //en/disables the seconds picker
                useCurrent: true,               //when true, picker will set the value to the current date/time     
                minuteStepping: 1,               //set the minute stepping
                defaultDate: "",                 //sets a default date, accepts js dates, strings and moment objects
                disabledDates: [],               //an array of dates that cannot be selected
                enabledDates: [],                //an array of dates that can be selected
                sideBySide: true              //show the date and time picker side by side
            });
        }
    },
    tagComponentEnable: function () {
        var $processForm = $.byId("server-validation-form");
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
    transactionStatusEnable: function () {
        var $transaction = $.byId("transaction-container"),
            hideTimeOut = parseInt($transaction.attr("data-hide-duration"));

        var hideStatus = function () {
            $transaction.attr("data-shown", "true");
            $transaction.hide(500);
        };
        var timer = setTimeout(hideStatus, hideTimeOut);

        var stopTimer = function () {
            clearTimeout(timer);
        }

        $transaction.click(function () {
            stopTimer();
            hideStatus();
        });
    },
    loadWow: function () {
        var wow = new WOW({
            boxClass: 'wow',      // animated element css class (default is wow)
            animateClass: 'animated', // animation css class (default is animated)
            offset: 100,          // distance to the element when triggering the animation (default is 0)
            mobile: false        // trigger animations on mobile devices (true is default)
        });
        wow.init();
    },

    ratingComponentEnable: function () {
        var $frontPageRatings = $.findCached(".rating-5-front");
        if ($frontPageRatings.length > 0) {
            $frontPageRatings.rating({
                showClear: false,
                showCaption: false
            });
        }

        var $detailPageRatingDisplayItems = $.findCached(".rating-5-page-details");
        if ($detailPageRatingDisplayItems.length > 0) {
            $detailPageRatingDisplayItems.rating({
                showClear: false,
                showCaption: true,
                starCaptions: {
                    0: "0",
                    0.5: "0.5",
                    1: "1",
                    1.5: "1.5",
                    2: "2",
                    2.5: "2.5",
                    3: "3",
                    3.5: "3.5",
                    4: "4",
                    4.5: "4.5",
                    5: "5"
                },
                starCaptionClasses: {
                    0: 'label label-danger',
                    0.5: 'label label-danger',
                    1: 'label label-danger',
                    1.5: 'label label-warning',
                    2: 'label label-warning',
                    2.5: 'label label-info',
                    3: 'label label-info',
                    3.5: 'label label-primary',
                    4: 'label label-primary',
                    4.5: 'label label-success',
                    5: 'label label-success'
                }
            });
        }
    },

    
};