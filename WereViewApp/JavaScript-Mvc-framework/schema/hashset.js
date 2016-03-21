/// <reference path="../extensions/clone.js" />

; $.app = $.app || {};
;$.app.schema.hashset = {
    capacity: 1,
    list: {
        array: null,
        ids: null,
        count: 0
    },
    create: function (capacity) {
        /// <summary>
        /// create a new hash-set with the given capacity.
        /// </summary>
        /// <param name="schema" type="type">Give a schema type from the schema folder.</param>
        var hashset = $.app.schema.createNestedClone($.app.schema.hashset);
        delete hashset.create;
        if ($.isEmpty(capacity)) {
            hashset.capacity = 25;
        } else {
            hashset.capacity = capacity;
        }
        hashset.list.array = new Array(hashset.capacity);
        hashset.list.ids = new Array(hashset.capacity);
        return hashset;
    },

    add : function(id, args) {
        /// <summary>
        /// First parameter is id and all other is parameter item.
        /// </summary>
        /// <param name="args" type="type"></param>
        var isIdEmpty = (id === undefined || id === null);
        console.log(this);
        console.log(this.list);
        if (isIdEmpty === false) {
            // argument passed
        } else {
            throw new Error("No id parameter given, so can't add new item to the hash-list.");
        }
    },

    isPossibleToAddNew : function() {
        
    }
};