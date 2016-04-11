Array.prototype.isEqual = function(array) {
    /// <summary>
    ///     Returns bool based on if both array contains same items.
    /// </summary>
    /// <param name="array" type="type">Array list</param>
    /// <returns type="">
    ///     Returns true/false based if both of those array are same or not. If not same then false. If both empty
    ///     then also return true.
    /// </returns>
    "use strict";

    var isEmpty = this.length === 0;
    var arrayIsEmpty = array === undefined || array === null || array.length === 0;
    if (isEmpty === arrayIsEmpty && isEmpty === true) {
        return true;
    } else {
        if (this.length !== array.length) {
            return false;
        } else {
            // length is same , now loop through
            for (var i = 0; i < this.length; i++) {
                if (this[i] !== array[i]) {
                    return false;
                }
            }
            return true; // both arrays are same.
        }
    }
};

Array.prototype.getDifferentIndexes = function(array) {
    /// <summary>
    ///     Returns array of indexes which are different between two arrays.
    /// </summary>
    /// <param name="array" type="type">Must pass same length array. Otherwise exception will be thrown.</param>
    /// <returns type="">Return an array of indexes  which are different between two arrays</returns>
    "use strict";
    var isEmpty = this.length === 0;
    var arrayIsEmpty = array === undefined || array === null || array.length === 0;
    if (isEmpty === arrayIsEmpty && isEmpty !== false) {
        return [];
    } else {
        var results = [];
        if (this.length !== array.length) {
            throw new Error("Array indexes are not same.");
        } else {
            // length is same , now loop through
            for (var i = 0; i < this.length; i++) {
                if (this[i] !== array[i]) {
                    results.push(i);
                }
            }
            return results;
        }
    }
};