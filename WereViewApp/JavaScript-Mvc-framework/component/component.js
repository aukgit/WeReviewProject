
; $.app = $.app || {};
/**
 * Only initialize components if it has "Component-Enable" Hidden has the named 
 */
; $.app.component = {
    id: "Component-Enable",
    /**
     * 
     * @returns {} 
     */
    initialize: function () {
        var self = $.app.component,
            id = self.id,
            listOfComponents = self.list,
            load = self.load;

        var $field = $.byId(id);
        if ($field.length > 0) {
            var loadingComponents = $field.val().split(","); // list of components to load.
            for (var i = 0; i < loadingComponents.length; i++) {
                var componentName = loadingComponents[i];
                var component = listOfComponents[componentName];
                $.executeFunction(component);
            }
        }
    },
};