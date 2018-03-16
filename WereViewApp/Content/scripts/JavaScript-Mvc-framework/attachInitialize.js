/// <reference path="app.js" />
/// <reference path="byId.js" />
/// <reference path="jQueryExtend.js" />
/// <reference path="jQueryExtend.fn.js" />
/// <reference path="D:\Working (SSD)\GitHub\WereViewProject\WereViewApp\Content/Scripts/jquery-2.1.4.js" />

/**
 * Attach and initial method with the class and execute the method.
 * @param {} $object : attach a initial method to excute all the methods inside that class.
 * @param {} force : if force then it will attach the initial method and execute if exist or not.
 * @returns {} 
 */
$.attachInitialize = function ($object, force) {
    /// <summary>
    /// Attach a initializer method which will call all the other method except for initialize.
    /// </summary>
    /// <param name="$object" type="type">Json object</param>
    /// <param name="force" type="type">Force to add this new method.</param>
    var name = "initialize";

    var initMethod = function () {
        var self = $object;
        var keys = Object.keys(self);
        for (var i = 0; i < keys.length; i++) {
            var key = keys[i];
            if (key !== name) {
                // execute all other than "initialize" method
                var functionsOrMethods = self[key];
                $.executeFunction(functionsOrMethods); // execute only if it is function.
            }
        }
    }
    if (!$.isEmpty($object)) {
        var initialize = $object[name];
        if (force === true) {
            $object[name] = initMethod;
        } else if ($.isEmpty(initialize)) {
            $object[name] = initMethod;
        }
    }
}

$.attachAndExecuteInitialize = function($object, force) {
    var name = "initialize";
    $.attachInitialize($object, force);
    $object[name].apply();
};

/**
 * Attach initialize methods to array of elements.
 * @param {} array of json classes
 * @param {} force 
 * @returns {} 
 */
$.attachInitializeToArray = function (array, force) {
    /// <summary>
    /// Attach a initializer method which will call all the other method except for initialize.
    /// </summary>
    /// <param name="$object" type="type">Json object</param>
    /// <param name="force" type="type">Force to add this new method.</param>\
    if (!$.isEmpty(array)) {
        for (var i = 0; i < array.length; i++) {
            var element = array[i];
            $.attachInitialize(element, force);
        }
    }

}

/**
 * Attach initialize methods to array of elements and then execute the initialize method.
 * @param {} array of json classes
 * @param {} force 
 * @returns {} 
 */
$.attachInitializeToArrayAndExecute = function (array, force) {
    /// <summary>
    /// Attach a initializer method which will call all the other method except for initialize.
    /// </summary>
    /// <param name="$object" type="type">Json object</param>
    /// <param name="force" type="type">Force to add this new method.</param>
    var name = "initialize";
    if (!$.isEmpty(array)) {
        for (var i = 0; i < array.length; i++) {
            var element = array[i];
            $.attachInitialize(element, force);
            element[name].apply();
        }
    }
}

/**
 * excute initialize methods from the array of elements.
 * @param {} array of json classes
 * @param {} force 
 * @returns {} 
 */
$.executeArrayOfInitilizeMethods = function (array) {
    /// <summary>
    /// Attach a initializer method which will call all the other method except for initialize.
    /// </summary>
    /// <param name="$object" type="type">Json object</param>
    /// <param name="force" type="type">Force to add this new method.</param>
    var name = "initialize";
    if (!$.isEmpty(array)) {
        for (var i = 0; i < array.length; i++) {
            var element = array[i];
            element[name].apply();
        }
    }
}
