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
        /// toggle classes from the given $element, order doesn't matter..
        /// </summary>
        /// <param name="$element" type="type">jquery element</param>
        /// <param name="classes" type="type">Use spaces( ) to combine and give classes names.</param>
        if (this.length > 0) {
            var classList = classes.split(" ");
            for (var i = 0; i < classList.length; i++) {
                var _class = classList[i];
                this.toggleClass(_class);
            }
        }
    },

    anyClassesExist: function (classes) {
        /// <summary>
        /// toggle classes from the given $element, order doesn't matter..
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
        /// toggle classes from the given $element, order doesn't matter..
        /// </summary>
        /// <param name="$element" type="type">jquery element</param>
        /// <param name="classes" type="type">Use spaces( ) to combine and give classes names.</param>
        var result = false;
        if (this.length > 0) {
            result = true;
            var classList = classes.split(" ");
            for (var i = 0; i < classList.length; i++) {
                var _class = classList[i];
                if (!this.hasClass(_class)) {
                    result = false;
                }
            }
        }
        return result;
    },
    toggleAttrValue: function (attr) {
        /// <summary>
        /// toggle given attribute value to "true" => "false" or "false" => "true".
        /// If not present then default insert true.
        /// </summary>
        /// <param name="attr" type="type">attribute name</param>
        if (this.length > 0) {
            var val = this.attr(attr);
            if (val === "true") {
                this.attr(attr, "false");
            } else {
                this.attr(attr, "true");
            }
        }
    },
    isBoolAttr: function (attr) {
        /// <summary>
        /// checks if the given attribute value is not "false".
        /// If attr not present then true.
        /// If no element present then false.
        /// </summary>
        /// <param name="attr" type="type">attribute name</param>
        if (this.length > 0) {
            var val = this.attr(attr);
            return val !== "false";
        }
        return false;
    },
    setBoolTrueAttr: function (attr) {
        /// <summary>
        /// checks if the given attribute value is "false".
        /// If attr not present then true.
        /// </summary>
        /// <param name="attr" type="type">attribute name</param>
        if (this.length > 0) {
            this.attr(attr, "true");
        }
    },
    setBoolFalseAttr: function (attr) {
        /// <summary>
        /// checks if the given attribute value is "false".
        /// If attr not present then true.
        /// </summary>
        /// <param name="attr" type="type">attribute name</param>
        if (this.length > 0) {
            this.attr(attr, "false");
        }
    }
});