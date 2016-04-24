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
    setItem: function (id, items) {
        /// <summary>
        /// Add items uniquely by the given id and item is the hash item could be array or json or anything.
        /// </summary>
        /// <param name="id" type="type"></param>
        /// <param name="items" type="type"></param>
        var isIdEmpty = (id === undefined || id === null);
        if (isIdEmpty === false) {
            var item = this.getItemObject(id);
            if (item !== null) {
                // item not found in the existing list.
                this.list.array[item.index] = items;
                return true;
            }
            return false;
        }
        throw new Error("No id parameter given to set.");
    },
    setItemByIndex: function (index, id, items) {
        /// <summary>
        /// Add items uniquely by the given id and item is the hash item could be array or json or anything.
        /// </summary>
        /// <param name="id" type="type"></param>
        /// <param name="items" type="type"></param>
        var isIndexEmpty = (index === undefined || index === null);
        if (isIndexEmpty === false) {
            if (index <= this.list.count) {
                this.list.array[index] = items;
                this.list.ids[index] = id;
            } else {
                throw new Error("Sorry ! (index : " + index + ", id: " + id + ") given index is out of boundary.");
            }
        }
    },
    addUnique: function (id, items, modifyIfExist) {
        /// <summary>
        /// Add items uniquely by the given id and item is the hash item could be array or json or anything.
        /// </summary>
        /// <param name="id" type="type"></param>
        /// <param name="items" type="Anything : array, json or anything else."></param>
        /// <returns type="bool">Returns if the item is added to the list. If not unique then returns false.</returns>
        var isIdEmpty = (id === undefined || id === null);
        if (isIdEmpty === false) {
            var index = this.getItemIndex(id);
            if (index === -1) {
                // item not found in the existing list.
                this.add(id, items);
                return true;
            } else if (modifyIfExist) {
                this.setItemByIndex(index, id, items);
                return true;
            }
        } else {
            throw new Error("No id parameter given, so can't add new item to the hash-list.");
        }
        return false;
    },
    add: function (id, items) {
        /// <summary>
        /// First parameter is id and item is the hash item could be array or json or any item.
        /// </summary>
        /// <param name="args" type="type"></param>
        /// <returns type=""></returns>
        var isIdEmpty = (id === undefined || id === null);
        // console.log(this);
        // console.log(this.list);
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
    isIdExist: function (id) {
        /// <summary>
        /// Returns true/false based on the if the id exist or not.
        /// </summary>
        /// <param name="id" type="type"></param>
        /// <returns type=""></returns>
        return this.getItemIndex(id) > -1;
    },
    getItemIndex: function (id) {
        /// <summary>
        /// Find and get the item from the list by id.
        /// </summary>
        /// <param name="id" type="type"></param>
        for (var i = 0; i < this.list.count; i++) {
            if (this.list.ids[i] === id) {
                return i;
            }
        }
        return -1;
    },
    getItemValue: function (id) {
        /// <summary>
        /// Find and get the item from the list by id.
        /// </summary>
        /// <param name="id" type="type"></param>
        /// <r
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
        /// <summary>
        /// Remove the hash item from the list.
        /// </summary>
        /// <param name="id" type="type"></param>
        /// <returns type="">
        /// Returns {  
        ///    value: this.list.array[index],
        ///    index: index,
        ///    id: id
        /// };
        /// </returns>
        var isIdEmpty = (id === undefined || id === null);
        if (isIdEmpty === false) {
            var item = this.getItemObject(id);
            if (item !== null) {
                // found
                var list = this.list,
                    ids = list.ids,
                    arr = list.array;
                ids.splice(item.index, 1);
                arr.splice(item.index, 1);
                this.list.count--;
                this.capacity = ids.length;
                return item;
            }
        }
        throw new Error("No id found to remove the element from the list.");
        return null;
    },

    isPossibleToAddNew: function () {
        /// <summary>
        /// Private : Is it possible to add items with item in the array.
        /// </summary>
        /// <returns type="">Return true/false if we can add a item by count++</returns>
        var list = this.list,
            count = list.count,
            increment = count + 1;
        return increment <= this.capacity;
    },

    getList: function () {
        /// <summary>
        /// Get this.list;
        /// </summary>
        /// <returns type="">Get this.list.</returns>
        return this.list;
    },
    getIds: function () {
        /// <summary>
        /// Get this.list;
        /// </summary>
        /// <returns type="">Get this.list.</returns>
        return this.list.ids;
    },

    getItems: function () {
        /// <summary>
        /// Get this.list;
        /// </summary>
        /// <returns type="">Get this.list.</returns>
        return this.list.array;
    },
    count: function () {
        /// <summary>
        /// 
        /// </summary>
        /// <returns type="">Get this.list.count</returns>
        return this.list.count;
    }

};