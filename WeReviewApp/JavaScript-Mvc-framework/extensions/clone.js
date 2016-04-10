$.nestedClone = function (schema) {
    /// <summary>
    /// (Little bit slow) Created nested cloned object. It will not work for recursive pointing object.
    /// Cloning test by Alim : http://jsperf.com/js-cloning-performance-test
    /// </summary>
    /// <param name="schema" type="type"></param>
    /// <returns type=""></returns>
    var schemaCopy;

    // Handle the 3 simple types, and null or undefined
    if (null == schema || "object" != typeof schema) return schema;

    // Handle Date
    if (schema instanceof Date) {
        schemaCopy = new Date();
        schemaCopy.setTime(schema.getTime());
        return schemaCopy;
    }

    // Handle Array
    if (schema instanceof Array) {
        schemaCopy = new Array(schema.length);
        for (var i = 0, len = schema.length; i < len; i++) {
            schemaCopy[i] = $.nestedClone(schema[i]);
        }
        return schemaCopy;
    }

    // Handle Object
    if (schema instanceof Object) {
        schemaCopy = {};
        for (var attr in schema) {
            if (schema.hasOwnProperty(attr)) schemaCopy[attr] = $.nestedClone(schema[attr]);
        }
        return schemaCopy;
    }

    throw new Error("Unable to create the given schema type! Its type isn't supported.");
}

$.nonNestedClone = function (schema) {
    /// <summary>
    /// (Very fast) Created non-nested cloned object. It will not create nested json objects.
    /// Cloning test by Alim : http://jsperf.com/js-cloning-performance-test
    /// </summary>
    /// <param name="schema" type="type"></param>
    /// <returns type=""></returns>
    var schemaCopy;

    // Handle the 3 simple types, and null or undefined
    if (null == schema || "object" != typeof schema) return schema;

    // Handle Date
    if (schema instanceof Date) {
        schemaCopy = new Date();
        schemaCopy.setTime(schema.getTime());
        return schemaCopy;
    }

    // Handle Array
    if (schema instanceof Array) {
        schemaCopy = new Array(schema.length);
        for (var i = 0, len = schema.length; i < len; i++) {
            schemaCopy[i] = schema[i];
        }
        return schemaCopy;
    }

    // Handle Object
    if (schema instanceof Object) {
        schemaCopy = {};
        for (var attr in schema) {
            if (schema.hasOwnProperty(attr)) schemaCopy[attr] = schema[attr];
        }
        return schemaCopy;
    }

    throw new Error("Unable to create the given schema type! Its type isn't supported.");
}