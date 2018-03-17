;$.app.config={setup:function(){/// <summary> /// Setup all configuration. /// </summary> var app=$.app;:var classesToCallInitialize=[ app.hiddenContainer,app.spinner,$.jQueryCaching,$.app.component ];:$.executeArrayOfInitilizeMethods(classesToCallInitialize);:$.ajaxPrefilter(function (options,originalOptions,jqXHR){options.async=true};$.app.executeAfter={documentSpinnerHide:function (){$.app.global.documentFullSpinnerHide();:var timer=setTimeout(function (){$.app.global.documentFullSpinnerHide();:clearTimeout(timer)},captureAnchorAndShowSpinner:function (){:var $anchors=$.findCached("a:link");:$anchors.click(function (e){var $link=$(this),href=$link.attr("href");:if (!$.isEmpty(href)){var startsWith=href[0];:var isInvalidUrl=href==="" || startsWith==="" || startsWith==="#" || href.indexOf("javascript")>-1;:if (!isInvalidUrl){e.preventDefault();:$.app.global.documentFullSpinnerShow("...Please Wait...");:window.location=$link.attr("href")};$.app.executeBefore={toasterComponentSetup:function (){if (!$.isEmpty(toastr)){toastr.options={"closeButton":false,"debug":false,"newestOnTop":true,"progressBar":true,"positionClass":"toast-bottom-left","preventDuplicates":true,"showDuration":"300","hideDuration":"1000","timeOut":"5000","extendedTimeOut":"1000","showEasing":"swing","hideEasing":"linear","showMethod":"fadeIn","hideMethod":"fadeOut"},dynamicDependedCombo:function (){:// load dynamic and depended select or combo var $selectPickers=$(".selectpicker");:if ($selectPickers.length>0){$.devOrg.dynamicSelect.initialize();:$selectPickers.selectpicker();:$.devOrg.bootstrapComboSelectIndex($selectPickers,0)},elasticTextAreas:function (){:var $textAreas=$("textarea");if ($textAreas.length>0){$textAreas.filter(".big-multiline").focus(function (){$(this).animate({'height':'300px','width':'630px','max-width':'630px'}).blur(function (){$(this).animate({'height':'auto','width':'294px','max-width':'294px'});//making textarea's elastic
            $textAreas.elastic().trigger('update');
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
        var $menuPage = $("#menu-item-edit-page");
        if ($menuPage.length > 0) {
            var $div = $("#hasDropdownDiv"),
                $hasDropDown = $("#HasDropDown");
            $div.hide();
            $hasDropDown.click(function () {
                if (this.checked) {
                    $div.show('slow');
                } else {
                    $div.hide('slow');
                }
            });
        }
    },
    bootstrapTableComponentEnable: function () {
        var $tables = $(".bootstrap-table-do");
        if ($tables.length > 0) {
            $tables.bootstrapTable();
        }
    },
    datePickerComponentEnable: function () {
        var $dateTimePicker = $(".datetimepicker-start"),
            options = {
                pickDate: true, //en/disables the date picker
                pickTime: true, //en/disables the time picker
                useMinutes: true, //en/disables the minutes picker
                useSeconds: true, //en/disables the seconds picker
                useCurrent: true, //when true, picker will set the value to the current date/time     
                minuteStepping: 1, //set the minute stepping
                defaultDate: "", //sets a default date, accepts js dates, strings and moment objects
                disabledDates: [], //an array of dates that cannot be selected
                enabledDates: [], //an array of dates that can be selected
                sideBySide: true //show the date and time picker side by side
            };
        if ($dateTimePicker.length > 0) {
            $dateTimePicker.datetimepicker(options);
        }
        var $datePicker = $(".datepicker-start");
        if ($datePicker.length > 0) {
            options.pickTime = false;
            options.useMinutes = false;
            options.useSeconds = false;
            $datePicker.datetimepicker(options);
        }
    },
    transactionStatusEnable: function () {
        var $transaction = $("#transaction-container"),
            hideTimeOut = 0;
        if ($transaction.length !== 0) {
            if ($transaction.length > 0) {
                hideTimeOut = parseInt($($transaction[0]).attr("data-hide-duration"));
            }

            var hideStatus = function () {
                $transaction.each(function (index) {
                    var $this = $(this);
                    $this.attr("data-shown", "true")
                        .hide(500);
                });
            };
            var timer = setTimeout(hideStatus, hideTimeOut);

            var stopTimer = function () {
                clearTimeout(timer);
            }

            $transaction.click(function () {
                stopTimer();
                hideStatus();
            });
        }
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
$.app.global = {
    
    vars: {

    },
    documentFullSpinnerHide: function () {
        var self = $.app.global,
            $bodyStart = $.findCachedId("body-start"),
            fixedClass = "body-fixed",
            flexible = "body-flexible";

        if ($bodyStart.length > 0) {
            if ($bodyStart.hasClass(fixedClass)) {
                var $loadingBar = $.findCachedId("loading-bar-full-screen");
                $bodyStart.removeClass(fixedClass).addClass(flexible);
                $loadingBar.addClass("animated").removeClass("fadeIn").addClass("fadeOut");
                setTimeout(function () {
                    $loadingBar.hide();
                    $bodyStart.removeClass(fixedClass).addClass(flexible);
                }, 1500);
            }
        }
    },
    enterToNextInputFocus: function ($form, submitAtLast, atLastFocusOnFirst) {
        /// <summary>
        /// Focus to next input if it was visible.
        /// </summary>
        /// <param name="$form" type="type"></param>
        /// <param name="submitAtLast" type="type"></param>
        /// <param name="atLastFocusOnFirst" type="type"></param>
        $form.find("input:text:first-child").focus();

        //var binders = formSelector + " input[type=text]:visible," +
        //    formSelector + " input[type=password]:visible," +
        //    formSelector + " input[type=numeric]:visible," +
        //    formSelector + " input[type=email]:visible," +
        //    //formSelector + " textarea:visible," +
        //    formSelector + " button.selectpicker[type=button]:visible," +
        //    formSelector + " select:visible";
        var binders = "input[type=text]:visible," +
                     "input[type=password]:visible," +
                     "input[type=numeric]:visible," +
                     "input[type=email]:visible," +
                    //formSelector + " textarea:visible," +
                     "button.selectpicker[type=button]:visible," +
                     "select:visible";
        $form.on("keypress", binders, function (e) {
            // var codeAbove = d.keyCode || d.which;
            // console.log("above code :" + codeAbove);
            var code = e.keyCode || e.which;
            // console.log("inside code :" + code);
            if (code === 13) { // Enter key
                e.preventDefault(); // Skip default behavior of the enter key
                var $elements = $form.find(binders);
                //console.log($elements);
                var n = $elements.length;
                var nextIndex = $elements.index(this) + 1;
                if (nextIndex < n) {
                    $elements[nextIndex].focus();
                } else {
                    $elements[nextIndex - 1].blur();
                    if (submitAtLast === true) {
                        $form.submit();
                    } else if (atLastFocusOnFirst === true) {
                        $elements[0].focus();
                    }
                }
            }
        });
    },
    enterToNextInputFocusWithoutTags: function ($form, submitAtLast, isDynamicSelector, atLastFocusOnFirst) {
        $form.find("input:text:first-child").focus();
        var binders = "input[type=text]:visible," +
                     "input[type=password]:visible," +
                     "input[type=numeric]:visible," +
                     "input[type=email]:visible," +
                     "input[type=text]:not(.bootstrap-tagsinput)," +
                    //formSelector + " textarea:visible," +
                     "button.selectpicker[type=button]:visible," +
                     "select:visible";
        var $elements = [];
        var keyPressEvent = function (e) {
            // var codeAbove = d.keyCode || d.which;
            // console.log("above code :" + codeAbove);
            var code = e.keyCode || e.which;
            // console.log("inside code :" + code);
            if (code === 13) { // Enter key
                e.preventDefault(); // Skip default behavior of the enter key
                if (isDynamicSelector === true || $elements.length === 0) {
                    $elements = $form.find(binders);
                }
                //console.log($elements);
                var n = $elements.length;
                var nextIndex = $elements.index(this) + 1;
                if (nextIndex < n) {
                    $elements[nextIndex].focus();
                } else {
                    $elements[nextIndex - 1].blur();
                    if (submitAtLast === true) {
                        $form.submit();
                    } else if (atLastFocusOnFirst === true) {
                        $elements[0].focus();
                    }
                }
            }
        };
        if (isDynamicSelector) {
            $form.on("keypress", binders, keyPressEvent);
        } else {
            var $inputs = $form.find(binders);
            $inputs.on('keypress', keyPressEvent);
        }
    },
    documentFullSpinnerShow: function (message) {
        var $bodyStart = $.findCachedId("body-start");
        var fixedClass = "body-fixed";
        var flexible = "body-flexible";
        if (!message) {
            message = "...Please wait...";
        }
        if ($bodyStart.length > 0) {
            var $content = $.findCachedId("full-screen-spinner-content");
            var $loadingBar = $.findCachedId("loading-bar-full-screen");
            $bodyStart.removeClass(flexible).addClass(fixedClass);
            $content.html(message);
            $loadingBar.addClass("animated").removeClass("fadeOut").addClass("fadeIn");
            $loadingBar.show();
        }
    },

    isGivenUrlMatchedDomain: function (url, domain) {
        var regex = new RegExp("^(?:https:\/\/|http:\/\/)*(www\.)*(?:" + domain + ")+(\/)?", "ig");
        var found = url.match(regex);
        return !$.isEmpty(found) && found.length === 1;
    }


};
$.app = {
    isDebugging: true,
    getProcessForm: function () {
        /// <summary>
        /// Get the processing form.
        /// </summary>
        var app = $.app,
              selectors = app.selectors,
              ids = selectors.ids,
              $processForm = $.findCachedId(ids.processForm);

        return $processForm;
    }
};
$(document).ready(function () {
    $.app.initialize(); // initialize everything
});

window.onload= function() {
    var app = $.app;
    $.attachAndExecuteInitialize(app.executeAfter);
}
/// <reference path="app.js" />
/// <reference path="byId.js" />
/// <reference path="jQueryExtend.js" />
/// <reference path="jQueryExtend.fn.js" />
/// <reference path="D:\Working (SSD)\GitHub\WereViewProject\WereViewApp\Content/Scripts/jquery-2.1.4.js" />


$.attachInitialize = function ($object, force) {
    /// <summary>
    /// Attach a initializer method which will call all the other method except for initialize.
    /// </summary>
    /// <param name="$object" type="type">Json object</param>
    /// <param name="force" type="type">Force to add this new method.</param>
    var name = "initialize";

    var initMethod = function () {
        var self = $object;
        var keys = Object.keys(self);
        for (var i = 0; i < keys.length; i++) {
            var key = keys[i];
            if (key !== name) {
                // execute all other than "initialize" method
                var functionsOrMethods = self[key];
                $.executeFunction(functionsOrMethods); // execute only if it is function.
            }
        }
    }
    if (!$.isEmpty($object)) {
        var initialize = $object[name];
        if (force === true) {
            $object[name] = initMethod;
        } else if ($.isEmpty(initialize)) {
            $object[name] = initMethod;
        }
    }
}

$.attachAndExecuteInitialize = function($object, force) {
    var name = "initialize";
    $.attachInitialize($object, force);
    $object[name].apply();
};


$.attachInitializeToArray = function (array, force) {
    /// <summary>
    /// Attach a initializer method which will call all the other method except for initialize.
    /// </summary>
    /// <param name="$object" type="type">Json object</param>
    /// <param name="force" type="type">Force to add this new method.</param>    if (!$.isEmpty(array)) {
        for (var i = 0; i < array.length; i++) {
            var element = array[i];
            $.attachInitialize(element, force);
        }
    }

}


$.attachInitializeToArrayAndExecute = function (array, force) {
    /// <summary>
    /// Attach a initializer method which will call all the other method except for initialize.
    /// </summary>
    /// <param name="$object" type="type">Json object</param>
    /// <param name="force" type="type">Force to add this new method.</param>
    var name = "initialize";
    if (!$.isEmpty(array)) {
        for (var i = 0; i < array.length; i++) {
            var element = array[i];
            $.attachInitialize(element, force);
            element[name].apply();
        }
    }
}


$.executeArrayOfInitilizeMethods = function (array) {
    /// <summary>
    /// Attach a initializer method which will call all the other method except for initialize.
    /// </summary>
    /// <param name="$object" type="type">Json object</param>
    /// <param name="force" type="type">Force to add this new method.</param>
    var name = "initialize";
    if (!$.isEmpty(array)) {
        for (var i = 0; i < array.length; i++) {
            var element = array[i];
            element[name].apply();
        }
    }
}

;$.byId = function(elementIdString) {
    /// <summary>
    /// Get your element by id, there is no need to use #.
    /// However if there is a hash then it will be removed.
    /// </summary>
    /// <param name="findElementById">Your element id, there is no need to use #</param>
    /// <returns>jQuery object , check length property to understand if any exist</returns>
    if (elementIdString !== undefined && elementIdString !== null && elementIdString !== "" && typeof elementIdString === 'string') {
        var elementsById;
        if (elementIdString.charAt(0) !== "#") {
            elementsById = document.getElementById(elementIdString);
            return $(elementsById);
        } else {
            var newId = elementIdString.slice(1, elementIdString.length);
            elementsById = document.getElementById(newId);
            return $(elementsById);
        }
    }
    return $(null);
};
$.app.controllers = {
    // any thing related to controllers.
    isCurrentPage: function (controllerExtendedTypeObject) {
        /// <summary>
        /// Returns true/false if the page is executable or not.
        /// </summary>
        /// <param name="controllerExtendedTypeObject">Controller extended object , which must have pageId, $pageElement prop</param>
        /// <returns type=""></returns>
        var pageId = controllerExtendedTypeObject.pageId,
            $page = $.app.controllers.getPage(controllerExtendedTypeObject);
        controllerExtendedTypeObject.$pageElement = $.byId(pageId);
        $page = controllerExtendedTypeObject.$pageElement;
        return !$.isEmpty($page);
    },
    getPage: function (controllerExtendedTypeObject) {
        return controllerExtendedTypeObject.$pageElement;
    },
    getPageActionName: function (controllerExtendedTypeObject) {
        /// <summary>
        /// Get which mvc action it is referring to.
        /// </summary>
        /// <param name="controllerExtendedTypeObject"></param>
        /// <returns type="">String of name(i.e. Index, Edit etc... which set in the data-action attribute.)</returns>
        var $page = $.app.controllers.getPage(controllerExtendedTypeObject);
        if (!$.isEmpty($page)) {
            return $page.attr("data-action");
        }
        return "";
    },
    getPageBindings: function (controllerExtendedTypeObject) {
        /// <summary>
        /// Get binding events names
        /// </summary>
        /// <param name="controllerExtendedTypeObject"></param>
        /// <returns type="">String of name(i.e. Index, Edit etc... which set in the data-action attribute.)</returns>
        var $page = $.app.controllers.getPage(controllerExtendedTypeObject);
        if (!$.isEmpty($page)) {
            var value = $page.attr("data-event-binding");
            if (!$.isEmpty(value)) {
                return value;
            }
        }
        return "";
    },
    execute: function (controllerExtendedTypeObject, runAll) {
        /// <summary>
        /// execute all the actions if in the same page and page is related to that action.
        /// </summary>
        /// <param name="controllerExtendedTypeObject">Controller extended object , which must have actions array of methods.</param>
        /// <returns type=""></returns>
        var actions = controllerExtendedTypeObject.actions,
            actionNames = Object.keys(actions),
            pageAction = $.app.controllers.getPageActionName(controllerExtendedTypeObject),
            needToStop = runAll === false,
           isOneRan = false;

        for (var i = 0; i < actionNames.length; i++) {
            var actionName = actionNames[i],
                action = actions[actionName];
            if (actionName === pageAction) {
                if ($.executeFunction(action)) {
                    isOneRan = true;
                }
                if (needToStop === true) {
                    return true;
                }
            }
        }
        return isOneRan;
    }
};

$.app.initilizeClasses = [
    $.app.executeBefore
];

$.app.initialize = function () {
    /// <summary>
    /// Run all modules.
    /// First runs config.setup() then all initialize methods in all the controllers.js. 
    /// Then run app.initializeMethods.initialize();
    /// </summary>

    var app = $.app;
    app.config.setup(); // runs all configuration before running all initialize methods.

    $.attachInitializeToArrayAndExecute(app.initilizeClasses);

    // run controller module
    app.controllers.initialize(); // runs all controllers modules.
};

$.app.initializeController = function (controllerCsv) {
    /// <summary>
    /// Run only the controller methods + initialize hidden container , spinner.
    /// First runs config.setup() then all initialize methods in all the controllers.js. 
    /// Then run app.initializeMethods.initialize();
    /// </summary>
    /// <param name="controllerCsv" type="string">controller name csv string.</param>

    var app = $.app;
    app.config.setup(); // runs all configuration before running all initialize methods.
    // run controller module
    app.controllers.initialize(controllerCsv); // run only specific controllers modules.
};

$.app.initializeControllerWithExcuteBeforeMethods = function (controllerCsv) {
    /// <summary>
    /// Run only the controller methods + initialize hidden container , spinner.
    /// First runs config.setup() then all initialize methods in all the controllers.js. 
    /// Then run app.initializeMethods.initialize();
    /// </summary>
    /// <param name="controllerCsv" type="string">controller name csv string.</param>

    var app = $.app;
    app.config.setup(); // runs all configuration before running all initialize methods.

    $.executeArrayOfInitilizeMethods(app.initilizeClasses);

    // run controller module
    app.controllers.initialize(controllerCsv); // run only specific controllers modules.
};
/// <reference path="devOrg.js" />
/// <reference path="initialize.js" />
/// <reference path="jQueryExtend.js" />
/// <reference path="jsonCombo.js" />
/// <reference path="regularExp.js" />
/// <reference path="selectors.js" />
/// <reference path="upload.js" />
/// <reference path="urls.js" />
/// <reference path="country-phone.js" />
/// <reference path="constants.js" />
/// <reference path="byId.js" />
/// <reference path="../jQuery/jquery-2.1.4.js" />
/// <reference path="../jQuery/jquery-2.1.4-vsdoc.js" />
/// <reference path="schema/hashset.js" />
;
$.jQueryCaching = {
    hashset: null,
    
    initialize: function (capacity, force) {
        var hashset = $.app.schema.hashset;

        if (!capacity) {
            capacity = 350;
        }
        var self = $.jQueryCaching;
        if (self.hashset === null || force === true) {
            self.hashset = hashset.create(capacity);
        }
    }
}

$.findCached = function (selector, force) {
    /// <summary>
    /// get jquery searched items, if exist in the 
    /// </summary>
    /// <param name="selector" type="type"></param>
    var self = $.jQueryCaching;
    var $e;
    if (force === true) {
        $e = $(selector);
        self.hashset.addUnique(selector, $e, true);
        return $e;
    } else {
        var item = self.hashset.getItemObject(selector);
        if (item === null) {
            $e = $(selector);
            self.hashset.addUnique(selector, $e, true);
            return $e;
        }
        return item.value;
    }
}
$.findCachedId = function (id, force) {
    /// <summary>
    /// get jquery searched items, if exist in the 
    /// </summary>
    /// <param name="selector" type="type"></param>
    var self = $.jQueryCaching;
    var $e;
    if (force === true) {
        $e = $.byId(id);
        self.hashset.addUnique(id, $e, true);
        return $e;
    } else {
        var item = self.hashset.getItemObject(id);
        if (item === null) {
            $e = $.byId(id);
            self.hashset.addUnique(id, $e, true);
            return $e;
        }
        return item.value;
    }
}
/// <reference path="devOrg.js" />
/// <reference path="initialize.js" />
/// <reference path="jQueryExtend.js" />
/// <reference path="jsonCombo.js" />
/// <reference path="regularExp.js" />
/// <reference path="selectors.js" />
/// <reference path="upload.js" />
/// <reference path="urls.js" />
/// <reference path="country-phone.js" />
/// <reference path="constants.js" />
/// <reference path="byId.js" />
/// <reference path="../jQuery/jquery-2.1.4.js" />
/// <reference path="../jQuery/jquery-2.1.4-vsdoc.js" />
;
$.fn.extend({
    getClassesList: function () {
        /// <summary>
        /// jQuery element get all classes as an array.
        /// </summary>
        /// <returns type="array">array list of classes.</returns>
        return $.getClassesList(this);
    },
    isEmpty: function () {
        /// <summary>
        /// Compare any object to null , unidentified or empty then returns true/false.
        /// </summary>
        /// <param name="variable"> Anything can be possible.</param>
        /// <returns type="boolean">True/False</returns>
        return $.isEmpty(this);
    },
    getSelectorElement: function () {
        /// <summary>
        /// get $(selector) element if data-selector attribute exists with the element.
        /// </summary>
        var selector = this.attr("data-selector");
        if ($.isEmpty(selector) === false) {
            //exist
            return $(selector);
        }
        return [];
    },
    getUrlString: function () {
        /// <summary>
        /// get attr("data-url") property.
        /// </summary>
        var url = this.attr("data-url");
        if ($.isEmpty(url) === false) {
            //exist
            return url;
        }
        return "";
    },
    getReferenceIdElement: function () {
        /// <summary>
        /// get $("#id") element if data-ref-id attribute exists.
        /// </summary>
        var id = this.attr("data-ref-id");
        if ($.isEmpty(id) === false) {
            //exist
            return $.byId(id);
        }
        return [];
    },
    disableElement: function () {
        this.attr("disabled", "disabled");
    },
    enableElement: function () {
        this.removeAttr("disabled");
    },
    isDisabledElement: function () {
        return this.hasAttr("disabled");
    },
    toArrayWithValues: function () {
        var len = this.length,
            array = new Array(len);
        for (var i = 0; i < len; i++) {
            array[i] = this[i].value;
        }
        return array;
    },
    multiformSerialize: function () {
        var len = this.length,
            array = [];
        for (var i = 0; i < len; i++) {
            var $from = $(this[i]);
            array.push($.serializeToJson($from));
        }
        return array;
    },
    toggleClasses: function (classes) {
        /// <summary>
        /// toggle classes from the given $element, order doesn't matter.. /// </summary> /// <param name="$element" type="type">jquery element</param> /// <param name="classes" type="type">Use spaces() to combine and give classes names.</param> if (this.length>0){:var classList=classes.split(" ");:for (var i=0;:i < classList.length;:i++){var _class=classList[i];:this.toggleClass(_class)},anyClassesExist:function (classes){:/// <summary> /// toggle classes from the given $element,order doesn't matter..
        /// </summary>
        /// <param name="$element" type="type">jquery element</param>
        /// <param name="classes" type="type">Use spaces( ) to combine and give classes names.</param>
        if (this.length > 0) {
            var classList = classes.split(" ");
            for (var i = 0; i < classList.length; i++) {
                var _class = classList[i];
                if (this.hasClass(_class)) {
                    return true;
                }
            }
        }
        return false;
    },

    allClassesExist: function (classes) {
        /// <summary>
        /// toggle classes from the given $element, order doesn't matter.. /// </summary> /// <param name="$element" type="type">jquery element</param> /// <param name="classes" type="type">Use spaces() to combine and give classes names.</param> var result=false;:if (this.length>0){result=true;:var classList=classes.split(" ");:for (var i=0;:i < classList.length;:i++){var _class=classList[i];:if (!this.hasClass(_class)){result=false},toggleAttrValue:function (attr){:/// <summary> /// toggle given attribute value to "true"=> "false" or "false"=> "true". /// If not present then default insert true. /// </summary> /// <param name="attr" type="type">attribute name</param> if (this.length>0){var val=this.attr(attr);:if (val==="true"){this.attr(attr,"false")}else{this.attr(attr,"true")},isBoolAttr:function (attr){:/// <summary> /// checks if the given attribute value is not "false". /// If attr not present then true. /// If no element present then false. /// </summary> /// <param name="attr" type="type">attribute name</param> if (this.length>0){var val=this.attr(attr);:return val!=="false"},setBoolTrueAttr:function (attr){/// <summary> /// checks if the given attribute value is "false". /// If attr not present then true. /// </summary> /// <param name="attr" type="type">attribute name</param> if (this.length>0){this.attr(attr,"true")},setBoolFalseAttr:function (attr){/// <summary> /// checks if the given attribute value is "false". /// If attr not present then true. /// </summary> /// <param name="attr" type="type">attribute name</param> if (this.length>0){this.attr(attr,"false")});/// <reference path="devOrg.js" /> /// <reference path="initialize.js" /> /// <reference path="jQueryExtend.js" /> /// <reference path="jsonCombo.js" /> /// <reference path="regularExp.js" /> /// <reference path="selectors.js" /> /// <reference path="upload.js" /> /// <reference path="urls.js" /> /// <reference path="country-phone.js" /> /// <reference path="constants.js" /> /// <reference path="byId.js" /> /// <reference path="../jQuery/jquery-2.1.4.js" /> /// <reference path="../jQuery/jquery-2.1.4-vsdoc.js" />;$.isEmpty=function (variable){/// <summary> /// Compare any object to null,unidentified or empty then returns true/false. /// </summary> /// <param name="variable"> Anything can be possible.</param> /// <returns type="boolean">True/False</returns> return variable===undefined || variable===null || variable==='' || variable.length===0};$.setDefaultBoolOnEmpty=function (variable,;$.setDefaultOnEmpty=function (variable,defaultValue){/// <summary> /// Compare any object to null,unidentified or empty then sets the default value to that object and then returns /// </summary> /// <param name="variable"> Anything can be possible.</param> /// <returns type="boolean">True/False</returns> if (variable===undefined || variable===null || variable==='' || variable.length===0){variable=defaultValue};$.getCommonClasses=function (classesWithSpace1,classesWithSpace2){var len=list1.length>list2.length ? list1.length:list2.length;var workingList=list1.length>list2.length ? list1:list2;var otherList=list1.length>list2.length ? list2:list1;:for (var i=0;:i < len;:i++){var item=workingList[i];:if (otherList.indexof(item)>-1){// present as common common.push(item)}$.getUnCommonClasses=function (classesWithSpace1,classesWithSpace2){var len=list1.length>list2.length ? list1.length:list2.length;var workingList=list1.length>list2.length ? list1:list2;var otherList=list1.length>list2.length ? list2:list1;:for (var i=0;:i < len;:i++){var item=workingList[i];:if (otherList.indexof(item)===-1){// not present uncommon unCommon.push(item)}item=otherList.length < i ? otherList[i]:null;if (item!==null && workingList.indexof(item)===-1){// not present uncommon unCommon.push(item)}$.getClassesList=function ($jQueryObject){/// <summary> /// jQuery element get all classes as an array. /// </summary> /// <param name="$jQueryObject">Any jQuery object.</param> /// <returns type="array">array list of classes.</returns> if ($jQueryObject.length===1){return $jQueryObject.attr("class").split(/\s+/)};$.getArrayExcept=function (givenArray,excludingArray){:/// <summary> /// givenArray=['a','b','c'],excludingArray=['b','c'],results=['a'] /// </summary> /// <param name="givenArray" type="array">Full list of items (in array format).</param> /// <param name="excludingArray" type="array">List of items which needs to be excluded from the list (in array format).</param> /// <returns type="array">an array after excluding the items from the given list.</returns> "use strict";:if ($.isEmpty(givenArray)){return []}if ($.isEmpty(excludingArray)){return givenArray}var len=givenArray.length;var results=[];for (var i=0;i < len;i++){if (excludingArray.indexOf(givenArray[i])===-1){// not found results.push(givenArray[i])};$.isString=function (variable){/// <summary> /// Checks wheater it is a string type or not. /// </summary> /// <param name="variable"></param> /// <returns type="boolean">true/false</returns> return typeof variable==='string'};$.returnUrlWithSlash=function (url){:/// <summary> /// First checks if slash exist at the bottom or not. /// </summary> /// <param name="url" type="string">Give an url.</param> /// <returns type="string">Url with slash at the bottom or empty string if type doesn't match or null or undefined.</returns>
    if ($.isEmpty(url) === false && $.isString(url)) {
        var len = url.length;
        if (url[len - 1] !== '/') {
            url += "/";
            return url;
        }
    }
    return "";
};
$.getFriendlyUrlSlug = function (str) {
    /// <summary>
    /// Returns friendly url slug from given string
    /// Hello & World -> hello-world
    /// </summary>
    /// <param name="str">Give an string "Hello & World"</param>

    var regularExpressions = $.app.regularExp;
    if ($.isEmpty(str) === false) {
        //"[^A-Za-z0-9_\.~]+"
        var regexString = regularExpressions.friendlyUrl;
        str = str.trim();
        var regExp = new RegExp(regexString, 'gi');
        return str.replace(regExp, "-");
    }
    return "";
};

$.htmlToSquareTag = function ($jQueryInputText) {
    /// <summary>
    /// Any HTML tag to square tag inside the input text.
    /// <iframe width="560" height="315" src="//www.youtube.com/embed/ob-P2a6Mrjs" frameborder="0" allowfullscreen> to Square
    /// </summary>
    /// <param name="$jQueryInput">jQuery element.</param>
    var currentText = $jQueryInputText.val();
    //currentText = currentText.toLowerCase();
    var reg = new RegExp("<" + tag, 'gi');
    currentText = currentText.replace(reg, "[" + tag);
    reg = new RegExp("</" + tag + ">", 'gi');
    currentText = currentText.replace(reg, "[/" + tag + "]");
    currentText = currentText.replace(">", "]");
    $jQueryInputText.val(currentText);
};


$.squareToHtmlTag = function ($jQueryInput, tag) {
    /// <summary>
    /// Any square tag to html tag inside the input text.
    /// [iframe width="560" height="315" src="//www.youtube.com/embed/ob-P2a6Mrjs" frameborder="0" allowfullscreen] to html
    /// </summary>
    /// <param name="$jQueryInput">jQuery element.</param>
    var currentText = $jQueryInput.val();
    //currentText = currentText.toLowerCase();
    var reg = new RegExp("\\[" + tag, 'gi');
    currentText = currentText.replace(reg, "<" + tag);
    reg = new RegExp("\\[/" + tag + "\\]", 'gi');
    currentText = currentText.replace(reg, "</" + tag + ">");
    currentText = currentText.replace("]", ">");
    $jQueryInput.val(currentText);
};
//validation modification
$.checkValidInputs = function ($inputsCollection, starRatingLabel, invalidStarRatingCss) {
    /// <summary>
    /// Check all the inputs jQuery validations.
    /// Also mark to red when invalid by the default valid method. 
    /// Bootstrap star rating is also validated in custom way.
    /// </summary>
    /// <param name="$inputsCollection" type="jQuery element">All input collection. </param>
    /// <param name="starRatingLabel">Can be null or full html for the label to be injected when star rating is not selected or rated.</param>
    /// <param name="invalidStarRatingCss" type="json with css properties">When null: {'text-shadow': "2px 2px red"}</param>
    /// <returns type="boolean">true/false</returns>
    "use strict";

    var $currentInput = null;
    var length = $inputsCollection.length;
    var labelHtml = starRatingLabel;
    if ($.isEmpty(labelHtml)) {
        labelHtml = "<label class='label label-danger small-font-size'>Please rate first.</label>";
    }

    if ($.isEmpty(invalidStarRatingCss)) {
        invalidStarRatingCss = {
            'text-shadow': "2px 2px red"
        };
    }
    if (length > 0) {
        for (var i = 0; i < length; i++) {
            $currentInput = $($inputsCollection[i]);

            if ($currentInput.hasClass("common-rating")) {
                var $ratingContainer = $currentInput.closest(".rating-container");
                var $wholeContainer = $ratingContainer.closest(".star-rating");

                if ($currentInput.val() === "0") {
                    $ratingContainer.css(invalidStarRatingCss);
                    if (!$wholeContainer.attr("data-warned")) {
                        $wholeContainer.append(labelHtml);
                        $wholeContainer.attr("data-warned", "true");
                    }
                    return false;
                } else {
                    // when star rating is valid then 
                    // remove the injected label and make it normal
                    $ratingContainer.css({
                        'text-shadow': "none"
                    });

                    if ($wholeContainer.attr("data-warned")) {
                        // removing injected label.
                        $wholeContainer.find("label").remove();
                        $wholeContainer.attr("data-warned", "false");
                    }
                }
            }
            if (!$currentInput.valid()) {
                return false;
            }
        }
    }
    return true;
};

$.isJson = function (obj) {
    if (!$.isEmpty(obj) && !$.isArray(obj) && typeof obj !== 'string' && typeof obj !== 'function') {
        return Object.keys(obj).length > 0;
    }
    return false;
};
$.getHiddenField = function (name) {
    /// <summary>
    /// Get hidden field object from cache if possible.
    /// </summary>
    /// <param name="name">Name of the field</param>
    /// <returns type=""></returns>
    return $.app.hiddenContainer.getHiddenField(name);
};

$.getHiddenValue = function (name) {
    /// <summary>
    /// Get string value of the hidden field.
    /// </summary>
    /// <param name="name">Name of the field</param>
    /// <returns type="">Get string value of the hidden field. If not found then empty string "".</returns>
    var $field = $.app.hiddenContainer.getHiddenField(name);
    if (!$.isEmpty($field)) {
        return $field.val();
    }
    return "";
};

$.setHiddenValue = function (name, val) {
    /// <summary>
    /// Get string value of the hidden field.
    /// </summary>
    /// <param name="name">Name of the field</param>
    /// <param name="val">value of the field</param>
    /// <returns type="">Get string value of the hidden field. If not found then empty string "".</returns>
    return $.app.hiddenContainer.setHiddenValue(name, val);
};


$.isFunc = function (func) {
    /// <summary>
    /// Is it it a function.
    /// </summary>
    /// <param name="func">Anything</param>
    /// <returns type="">Returns true/false</returns>
    return typeof func === "function";
};
$.executeFunction = function (func) {
    /// <summary>
    /// Execute only if it is a function
    /// </summary>
    /// <param name="func">Anything</param>
    /// <returns type="">Returns true/false</returns>
    if (typeof func === "function") {
        func.apply();
        return true;
    }
    return false;
};

$.executeFunctionWithArguments = function (func, argumentsArray) {
    /// <summary>
    /// Execute only if it is a function.
    /// Catch the arguments with arguments variable inside the function.
    /// </summary>
    /// <param name="func">Anything</param>
    /// <param name="argumentsArray">Pass an array of arguments.</param>
    /// <returns type="">Returns true/false</returns>
    if (typeof func === "function") {
        func.apply(null, argumentsArray);
        return true;
    }
    return false;
};


$.getJsonToQueryString = function (url, json, isQuestionMarkRequired) {
    /// <summary>
    /// Returns a concatenated url with those json array value pair
    /// </summary>
    /// <param name="url"></param>
    /// <param name="json">
    ///    any json  {name: 'value', name2: 'value' },
    /// </param>
    /// <param name="isQuestionMarkRequired">add ? after given url or else add &</param>
    /// <returns type="">returns a url string.</returns>
    if (url !== null && url !== undefined) {
        if (isQuestionMarkRequired) {
            url += "?";
        } else {
            url += "&";
        }
        var keys = Object.keys(json),
            len = keys.length,
            arr = new Array(len);
        for (var i = 0; i < len; i++) {
            var key = keys[i],
                value = json[key];
            arr[i] = key + "=" + value + "";
        }
        url += arr.join("&");
        return url;
    }

    return "";
};

$.applyAutoResizeMultiline = function ($container) {
    /// <summary>
    /// Apply auto size on the elements which has elastic or autosize-enabled class.
    /// </summary>
    /// <param name="$container">can be null, if given the filter will be done only inside that container.</param>
    /// <returns type=""></returns>

    var $autoSizableElements;
    var selectors = ".elastic,.autosize,.multiline-text";
    if (!$.isEmpty($container)) {
        $autoSizableElements = $container.find(selectors);
    } else {
        $autoSizableElements = $(selectors);
    }
    if (!$.isEmpty($autoSizableElements)) {
        $autoSizableElements.elastic();
    }
};
$.hideEmptyFields = function ($container) {
    /// <summary>
    /// Hide elements which has empty input fields.
    /// </summary>
    /// <param name="$container">can be null, if given the filter will be done only inside that container.</param>
    /// <returns type=""></returns>

    var $inputs;
    var selectors = "input[value='']";
    var $formGroups;
    if (!$.isEmpty($container)) {
        $inputs = $container.find(selectors);
        $formGroups = $container.find(".form-group");
    } else {
        $inputs = $(selectors);
        $formGroups = $(".form-group");
    }
    if (!$.isEmpty($inputs)) {
        for (var i = 0; i < $inputs.length; i++) {
            var $input = $($inputs[i]),
                name = $input.attr("data-prop"),
                selector = "[data-prop='" + name + "']",$formGroup=$formGroups.filter(selector);:$formGroup.hide();://console.log($formGroup);://console.log(selector);://console.log(name)};$.getjQueryElementsByArrayOfSelectors=function (arrayOfSelectors){:/// <summary> /// Get jquery elements by passing array of selectors. /// </summary> /// <param name="arrayOfSelectors" type="type"></param> /// <returns type=""></returns> var results=[];:for (var i=0;:i < arrayOfSelectors.length;:i++){var selector=arrayOfSelectors[i];:var $elems=$(selector);:for (var j=0;:j < $elems.length;:j++){var elem=$elems[i];:results.push(elem)}$.serializeToJson=function ($singleForm){;var formItemsArray=$singleForm.serializeArray();for (var i=0;i < formItemsArray.length;i++){:var item=formItemsArray[i];:result[item.name]=item.value}