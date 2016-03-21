/// <reference path="../extensions/clone.js" />

; $.app = $.app || {};
; $.app.schema.hashset = {
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

    add: function (id, items) {
        /// <summary>
        /// First parameter is id and all other is parameter item.
        /// </summary>
        /// <param name="args" type="type"></param>
        var isIdEmpty = (id === undefined || id === null);
        console.log(this);
        console.log(this.list);
        if (isIdEmpty === false) {
            var list = this.list,
                count = list.count,
                ids = list.ids,
                arr = list.array;
            // argument passed
            if (this.isPossibleToAddNew()) {
                ids[count] = id;
                arr[count] = items;
                this.list.count++;
            } else {
                ids.push(id);
                arr.push(items);
                this.list.count++;
            }
        } else {
            throw new Error("No id parameter given, so can't add new item to the hash-list.");
        }
    },
    getItemIndex: function (id) {
        /// <summary>
        /// Find and get the item from the list by id.
        /// </summary>
        /// <param name="id" type="type"></param>
        return this.list.ids.indexOf(id);
    },
    getItemValue: function (id) {
        /// <summary>
        /// Find and get the item from the list by id.
        /// </summary>
        /// <param name="id" type="type"></param>
        var index = this.getItemIndex(id);
        if (index > -1) {
            // found
            return this.list.array[index];
        }
        return null;
    },
    getItemObject: function (id) {
        /// <summary>
        /// Find and get the item from the list by id.
        /// </summary>
        /// <param name="id" type="type"></param>
        var index = this.getItemIndex(id);
        if (index > -1) {
            // found
            return {
                value: this.list.array[index],
                index: index,
                id: id
            };
        }
        return null;
    },
    removeItem: function (id) {
     
        var isIdEmpty = (id === undefined || id === null);
        console.log(this);
        console.log(this.list);
        if (isIdEmpty === false) {
            var item = this.getItemObject(id);
            if (item !== null) {
                // found
                re
            }
        } else {
            throw new Error("No id parameter given, so can't remove item from hash-list.");
        }
    },

    isPossibleToAddNew: function () {
        var list = this.list,
            count = list.count,
            increment = count + 1;
        return increment <= this.capacity;
    },

    getList: function () {
        return this.list;
    },
    Count: function () {
        return this.list.count;
    },



};