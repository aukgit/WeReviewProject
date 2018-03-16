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
        initalTexts: null,  // array
        idsOrNames: null // array
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
        list.idsOrNames = tracker.getAllInputsIdsOrNameArray();
        return tracker;
    },
    isChanged: function ($input) {
        var item = this.getInputfromListWithInitialText($input);
        if (item !== null) {
            var currentText = item.$input.val();
            if (item.initText !== currentText) {
                return true;
            }
        }
        return false;
    },
    getInputfromListWithInitialText: function ($input) {
        var textArr = this.list.initalTexts,
            findingId = this.getInputIdOrName($input);
        for (var i = 0; i < textArr.length; i++) {
            var currentInputId = this.list.idsOrNames[i];
            if (findingId === currentInputId) {
                return {
                    $input: $input,
                    initText: textArr[i]
                }
            }
        }
        return null;
    },

    getInputIdOrName: function ($input) {
        var name;
        if (!$.isEmpty($input.length)) {
            name = $input.attr("id");
            if ($.isEmpty(name)) {
                name = $input.attr("name");
            }
            return name;
        } else {
            name = $input.id;
            if ($.isEmpty(name)) {
                name = $input.getAttribute("name");
            }
        }
        return name;
    },
    getInputIdOrNameByIndex: function (index) {
        return this.list.idsOrNames[index];
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
                // different 
                changedInputsList.push(input);
            }
        }
        return $(changedInputsList);
    },
    getUnchangedInputs: function () {
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
            if (currentText === previousValue) {
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
            attrArray[i] = $changedInputs[i].getAttribute(attr);
        }
        return attrArray;
    },
    getUnchangedInputsAttrArray: function (attr) {
        /// <summary>
        /// Get an array of the given attribute values for changed inputs.
        /// </summary>
        /// <param name="attr" type="type">Give a attr name.</param>
        /// <returns type="">Get an array of the given attribute values for changed inputs.</returns>
        var $changedInputs = this.getUnchangedInputs();
        var attrArray = new Array($changedInputs.length);
        for (var i = 0; i < $changedInputs.length; i++) {
            attrArray[i] = $changedInputs[i].getAttribute(attr);
        }
        return attrArray;
    },
    getAllInputsIdsOrNameArray: function () {
        /// <summary>
        /// Get an array of the given attribute values for changed inputs.
        /// </summary>
        /// <param name="attr" type="type">Give a attr name.</param>
        /// <returns type="">Get an array of the given attribute values for changed inputs.</returns>
        var $inputs = this.list.$inputs;
        var attrArray = new Array($inputs.length);
        for (var i = 0; i < $inputs.length; i++) {
            var input = $inputs[i];
            var idOrName = input.id;
            if ($.isEmpty(idOrName)) {
                idOrName = input.getAttribute("name");
            }
            attrArray[i] = idOrName;
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