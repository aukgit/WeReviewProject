; $.app = $.app || {};
$.app.initializeMethods = {
    /**
     * runs all the methods after initialize method.
     */
    initialize: function() {
        var self = $.app.initializeMethods;

        var keys = Object.keys(self);
        for (var i = 0; i < keys.length; i++) {
            var key = keys[i];
            if (key !== "initialize") {
                // execute all other than "initialize" method
                var functionsOrMethods = self[key];
                $.executeFunction(functionsOrMethods);
            }
        }
    },
    toolTipShow: function() {
        var $tooltipItems = $('.tooltip-show');
        if ($tooltipItems.length > 0) {
            $tooltipItems.tooltip({ container: 'body' });
        }

    },
    seoHide: function() {
        var $seoHideItems = $(".seo-hide");
        if ($seoHideItems.length > 0) {
            $seoHideItems.hide();
        }
    },
    menuEnable: function() {
        var menuPage = $("#menu-item-edit-page");
        if (menuPage.length > 0) {
            var div = $("#hasDropdownDiv");
            div.hide();
            $("#HasDropDown").click(function() {
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
        var options = {
            scaleColor: false,
            trackColor: 'rgba(266,144,0,0.0)',
            barColor: '#ff7200',
            lineWidth: 2,
            lineCap: 'butt',
            size: 253
        };
        $().jetmenu();
        window.addEventListener('DOMContentLoaded', function () {
            var charts = [];
            [].forEach.call(document.querySelectorAll('.chart'), function (el) {
                charts.push(new EasyPieChart(el, options));
            });
        });

        var wow = new WOW({
            boxClass: 'wow',      // animated element css class (default is wow)
            animateClass: 'animated', // animation css class (default is animated)
            offset: 100,          // distance to the element when triggering the animation (default is 0)
            mobile: false        // trigger animations on mobile devices (true is default)
        });
        wow.init();
    }
};