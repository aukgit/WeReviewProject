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
    /**
     * 
     * @param {} capacity  : default is 350
     * @returns {} 
     */
    initialize: function (capacity, force) {
        if (!capacity) {
            capacity = 350;
        }
        var self = $.jQueryCaching;
        if (self.hashset === null || force === true) {
            self.hashset = $.app.hashset.create(capacity);
        }
    }
}

$.getC = function (selector, force) {
    /// <summary>
    /// get jquery searched items, if exist in the 
    /// </summary>
    /// <param name="selector" type="type"></param>
    var self = $.jQueryCaching;
    if (force === true) {
        var $e = $(selector);
        self.hashset.addUnique(selector, $e, true);
    } else {
        var item = self.hashset.getItemObject(selector);
    }
}