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
        initalTexts: null, // array
        $changedInputs: null, // array
        textAfterInital: null // array
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
        var list = this.list;
        list.textAfterInital = list.$inputs.toArrayWithValues();
        var differentIndexesList = list.initalTexts.getDifferentIndexes(list.textAfterInital);
        list.$changedInputs = -1; // remove previous item
        list.$changedInputs = new Array(differentIndexesList.length);
        for (var i = 0; i < differentIndexesList.length; i++) {
            var index = differentIndexesList[i];
            list.$changedInputs[i] = list.$inputs[index];
        }
        return list.$changedInputs;
    }
};