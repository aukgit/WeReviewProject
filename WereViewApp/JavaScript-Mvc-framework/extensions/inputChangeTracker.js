/// <reference path="urls.js" />
/// <reference path="constants.js" />
/// <reference path="country-phone.js" />
/// <reference path="devOrg.js" />
/// <reference path="initialize.js" />
/// <reference path="jQueryExtend.js" />
/// <reference path="jsonCombo.js" />
/// <reference path="regularExp.js" />
/// <reference path="selectors.js" />
/// <reference path="upload.js" />
/// <reference path="../../Content/Scripts/jquery-2.1.4.js" />
/// <reference path="../../Content/Scripts/jquery-2.1.4.intellisense.js" />
/// <reference path="../schema/url.js" />
/// <reference path="../schema/schema.js" />
/// <reference path="../jQueryExtend.fn.js" />
/// <reference path="../ProtoType/Array.js" />

; $.app = $.app || {};
$.app.inputChangeTracker = {
    list: {
        $inputs: null, // array
        initalTexts: null // array
    },

    createTracker: function ($inputs) {
        /// <summary>
        /// Creates a tracker to track input elements which are changed afterwards.
        /// </summary>
        /// <param name="$inputs" type="type"></param>
        var tracker = $.app.schema.createNestedClone($.app.inputChangeTracker);
        delete tracker.createTracker;
        var list = tracker.list;
        list.$inputs = $inputs;
        list.initalTexts = $inputs.toArrayWithValues();
        return tracker;
    },

    getChangedInputs: function () {
        /// <summary>
        /// Get all inputs array which are changed at moment of calling this method.
        /// </summary>
        /// <returns type=""></returns>
        var list = this.list,
            $inputs = this.list.$inputs,
            len = $inputs.length;
        var changedInputsList = [];
        for (var i = 0; i < len; i++) {
            var input = $inputs[i],
                currentText = input.value,
                previousValue = list.initalTexts[i];
            if (currentText !== previousValue) {
                // different.
                changedInputsList.push(input);
            }
        }
        return $(changedInputsList);
    },
    getChangedInputsAttrArray: function (attr) {
        /// <summary>
        /// Get an array of the given attribute values for changed inputs.
        /// </summary>
        /// <param name="attr" type="type">Give a attr name.</param>
        /// <returns type="">Get an array of the given attribute values for changed inputs.</returns>
        var $changedInputs = this.getChangedInputs();
        var attrArray = new Array($changedInputs.length);
        for (var i = 0; i < $changedInputs.length; i++) {
            attrArray[i] = $changedInputs.attr(attr);
        }
        return attrArray;
    },
    setChangedInputsAttr: function (attr, value) {
        /// <summary>
        /// Set common attribute value to all the changed input elements.
        /// </summary>
        /// <param name="attr" type="type"></param>
        /// <param name="value" type="type"></param>
        var $changedInputs = this.getChangedInputs();
        for (var i = 0; i < $changedInputs.length; i++) {
            var input = $changedInputs[i];
            input.setAttribute(attr, value);
        }
    }
};