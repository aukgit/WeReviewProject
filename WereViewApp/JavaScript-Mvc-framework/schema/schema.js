;$.app = $.app || {};
;$.app.schema = {
    create: function (schema) {
        /// <summary>
        /// (non-nested faster) create deep copy of the schema 
        /// </summary>
        /// <param name="schema" type="type">Give a schema type from the schema folder.</param>
        return $.nonNestedClone(schema);
    },
    createNestedClone: function (schema) {
        /// <summary>
        /// (nested , a little bit slower) create deep copy of the schema  
        /// </summary>
        /// <param name="schema" type="type">Give a schema type from the schema folder.</param>
        return $.nestedClone(schema);
    }
};