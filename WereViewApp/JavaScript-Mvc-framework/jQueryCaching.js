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