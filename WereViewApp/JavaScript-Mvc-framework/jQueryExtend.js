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

$.isEmpty = function (variable) {
    /// <summary>
    /// Compare any object to null , unidentified or empty then returns true/false.
    /// </summary>
    /// <param name="variable"> Anything can be possible.</param>
    /// <returns type="boolean">True/False</returns>
    return variable === undefined || variable === null || variable === '' || variable.length === 0;
};

$.setDefaultOnEmpty = function (variable, defaultValue) {
    /// <summary>
    /// Compare any object to null , unidentified or empty then sets the default value to that object and then returns
    /// </summary>
    /// <param name="variable"> Anything can be possible.</param>
    /// <returns type="boolean">True/False</returns>
    if (variable === undefined || variable === null || variable === '' || variable.length === 0) {
        variable = defaultValue;
    }
    return variable;
};

/**
 * gets the common classes from the list.
 * @param {} classesWithSpace1 : classes with spaces eg. "Hello world"
 * @param {} classesWithSpace2  : classes with spaces eg "world Hello v"
 * @returns {} return a array list of common classes eg. ["Hello",  "world"]
 */
$.getCommonClasses = function(classesWithSpace1, classesWithSpace2) {
    var list1 = classesWithSpace1.split(" "),
        list2 = classesWithSpace2.split(" "),
        common = [];
    var len = list1.length > list2.length ? list1.length : list2.length;
    var workingList = list1.length > list2.length ? list1 : list2;
    var otherList = list1.length > list2.length ? list2 : list1;

    for (var i = 0; i < len; i++) {
        var item = workingList[i];
        if (otherList.indexof(item) > -1) {
            // present as common
            common.push(item);
        }
    }
    return common;
}


/**
 * gets the uncommon classes from the list.
 * @param {} classesWithSpace1 : classes with spaces eg. "Hello world"
 * @param {} classesWithSpace2  : classes with spaces eg "world Hello v"
 * @returns {} return a array list of common classes eg. ["v"]
 */
$.getUnCommonClasses = function (classesWithSpace1, classesWithSpace2) {
    var list1 = classesWithSpace1.split(" "),
        list2 = classesWithSpace2.split(" "),
        unCommon = [];
    var len = list1.length > list2.length ? list1.length : list2.length;
    var workingList = list1.length > list2.length ? list1 : list2;
    var otherList = list1.length > list2.length ? list2 : list1;

    for (var i = 0; i < len; i++) {
        var item = workingList[i];
        if (otherList.indexof(item) === -1) {
            // not present uncommon
            unCommon.push(item);
        }
        item = otherList.length < i ? otherList[i]  : null;
        if (item !== null && workingList.indexof(item) === -1) {
            // not present uncommon
            unCommon.push(item);
        }
    }
    return unCommon;
}
/**
 *  @returns array of classes names.
 */
$.getClassesList = function ($jQueryObject) {
    /// <summary>
    /// jQuery element get all classes as an array.
    /// </summary>
    /// <param name="$jQueryObject">Any jQuery object.</param>
    /// <returns type="array">array list of classes.</returns>
    if ($jQueryObject.length === 1) {
        return $jQueryObject.attr("class").split(/\s+/);
    }
    return null;
};

$.getArrayExcept = function(givenArray, excludingArray) {
    /// <summary>
    /// givenArray = ['a','b','c'] , excludingArray=['b','c'], results=['a']
    /// </summary>
    /// <param name="givenArray" type="array">Full list of items (in array format).</param>
    /// <param name="excludingArray" type="array">List of items which needs to be excluded from the list (in array format).</param>
    /// <returns type="array">an array after excluding the items from the given list.</returns>
    "use strict";
    if ($.isEmpty(givenArray)) {
        return [];
    }
    if ($.isEmpty(excludingArray)) {
        return givenArray;
    }

    var len = givenArray.length;
    var results = [];
    for (var i = 0; i < len; i++) {
        if (excludingArray.indexOf(givenArray[i]) === -1) {
            // not found
            results.push(givenArray[i]);
        }
    }
    return results;
};


$.isString = function (variable) {
    /// <summary>
    /// Checks wheater it is a string type or not.
    /// </summary>
    /// <param name="variable"></param>
    /// <returns type="boolean">true/false</returns>
    return typeof variable === 'string';
};

$.returnUrlWithSlash = function (url) {
    /// <summary>
    /// First checks if slash exist at the bottom or not.
    /// </summary>
    /// <param name="url" type="string">Give an url.</param>
    /// <returns type="string">Url with slash at the bottom or empty string if type doesn't match or null or undefined.</returns>
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
/**
 * single input IFRAME code HTML  to Square
 */
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

/**
 * single input IFRAME code Square  to HTML
 */
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
$.checkValidInputs = function($inputsCollection, starRatingLabel, invalidStarRatingCss) {
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
                var $ratingContainer = $currentInput.closest("div.rating-container");
                var $wholeContainer = $ratingContainer.closest("div.star-rating");

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
$.getHiddenField = function(name) {
    /// <summary>
    /// Get hidden field object from cache if possible.
    /// </summary>
    /// <param name="name">Name of the field</param>
    /// <returns type=""></returns>
    return $.app.hiddenContainer.getHiddenField(name);
};

$.getHiddenValue = function(name) {
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

$.setHiddenValue = function(name, val) {
    /// <summary>
    /// Get string value of the hidden field.
    /// </summary>
    /// <param name="name">Name of the field</param>
    /// <param name="val">value of the field</param>
    /// <returns type="">Get string value of the hidden field. If not found then empty string "".</returns>
    return $.app.hiddenContainer.setHiddenValue(name, val);
};


$.isFunc = function(func) {
    /// <summary>
    /// Is it it a function.
    /// </summary>
    /// <param name="func">Anything</param>
    /// <returns type="">Returns true/false</returns>
    return typeof func === "function";
};
$.executeFunction = function(func) {
    /// <summary>
    /// Execute only if it is a function
    /// </summary>
    /// <param name="func">Anything</param>
    /// <returns type="">Returns true/false</returns>
    if (typeof func === "function") {
        func.apply();
    }
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

$.applyAutoResizeMultiline = function($container) {
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
$.hideEmptyFields = function($container) {
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
                selector = "[data-prop='" + name + "']",
                $formGroup = $formGroups.filter(selector);
            $formGroup.hide();
            //console.log($formGroup);
            //console.log(selector);
            //console.log(name);
        }
    }
};

/**
 *  
 * @param {} arrayOfSelectors  : Pass array of selectors
 * @returns {} jquery elements
 */
$.getjQueryElementsByArrayOfSelectors = function (arrayOfSelectors) {
    /// <summary>
    /// Get jquery elements by passing array of selectors.
    /// </summary>
    /// <param name="arrayOfSelectors" type="type"></param>
    /// <returns type=""></returns>
    var results = [];
    for (var i = 0; i < arrayOfSelectors.length; i++) {
        var selector = arrayOfSelectors[i];
        var $elems = $(selector);
        for (var j = 0; j < $elems.length; j++) {
            var elem = $elems[i];
            results.push(elem);
        }
    }
    return $(results);
}

/**
 * Convert single form to json object.
 * @param {} $singleForm 
 * @returns {} 
 */
$.serializeToJson = function($singleForm) {
    var result = {};
    var formItemsArray = $singleForm.serializeArray();
    for (var i = 0; i < formItemsArray.length; i++) {
        var item = formItemsArray[i];
        result[item.name] = item.value;
    }
    return result;
}