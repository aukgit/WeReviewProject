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
            listOfComponents = self.list, // list of components function resides inside the  component.list.js file.
            //load = self.load,
            $field = $.byId(id),
            extractComponentNameAndParameters = function (componentStringName) {
                /// <summary>
                /// extract component name and parameters
                /// </summary>
                /// <param name="componentStringName" type="type">component(param1,param2)</param>
                /// <returns type="">returns {name:"componentName", parameters: []}</returns>
                var result = {
                    name: "",
                    parameters: []
                };
                var parameterStartingIndex = componentStringName.indexOf("(");
                if (parameterStartingIndex === -1) {
                    // no parameters found
                    result.name = componentStringName;
                } else {
                    result.name = componentStringName.substr(0, parameterStartingIndex);
                    var len = componentStringName.length - parameterStartingIndex - 1 - 1; // -1 more for removing the last parentthesis
                    var paramsString = componentStringName.substr(parameterStartingIndex + 1, len);
                    result.parameters = paramsString.split(",");
                    for (var j = 0; j < result.parameters.length; j++) {
                        var param = result.parameters[j];
                        var captionFoundIndex = param.indexOf(":");
                        if (captionFoundIndex > - 1) {
                            // caption exist
                            len = param.length - 1 - captionFoundIndex;
                            // removing caption
                            result.parameters[j] = param.substr(captionFoundIndex + 1, len);
                        }
                    }
                }
                return result;
            };

        if ($field.length > 0) {
            // If separator is changed then must change the separator in the htmlhelper ComponentsEnableFor method.
            var seperator = "|";
            var loadingComponents = $field.val().split(seperator),
                executeFunction = $.executeFunction,
                executeFunctionWithArguments = $.executeFunctionWithArguments; // list of components to load.
            for (var i = 0; i < loadingComponents.length; i++) {
                var componentNameParam = loadingComponents[i];
                componentNameParam = extractComponentNameAndParameters(componentNameParam);
                var component = listOfComponents[componentNameParam.name];
                if (componentNameParam.parameters.length > 0) {
                    executeFunctionWithArguments(component, componentNameParam.parameters);
                } else {
                    executeFunction(component);
                }
            }
        }
    },
};