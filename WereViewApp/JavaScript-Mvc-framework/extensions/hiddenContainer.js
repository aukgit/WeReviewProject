$.app.hiddenContainer = {
    $hiddenContainer: null,
    $hiddenFieldDictionary: [],
    hiddenFieldNamesDictionary: [],
    initialize: function () {
        /// <summary>
        /// Initialize hidden container if exist.
        /// </summary>
        /// <returns type="">returns hidden container.</returns>
        var app = $.app.hiddenContainer;
        self.$hiddenContainer = $.byId("hidden-fields-container");
        self.$hiddenFieldDictionary = -1;
        self.$hiddenFieldDictionary = [];
        self.hiddenFieldNamesDictionary = -1;
        self.hiddenFieldNamesDictionary = [];
        return self.$hiddenContainer;
    },
    isHiddenContainerExist: function () {
        return !$.isEmpty($.app.hiddenContainer.$hiddenContainer);
    },
    _getHiddenFieldDictionary: function (nameOfHiddenField) {
        /// <summary>
        /// Get dictionary hidden field values.
        /// </summary>
        /// <param name="nameOfHiddenField"></param>
        /// <returns type="return $ type object.">null or jquery obejct.</returns>
        var self = $.app.hiddenContainer;
        if (nameOfHiddenField) {
            var namesDictionary = self.hiddenFieldNamesDictionary;
            for (var i = 0; i < namesDictionary.length; i++) {
                var hiddenName = namesDictionary[i];
                if (hiddenName === nameOfHiddenField) {
                    return self.$hiddenFieldDictionary[i];
                }
            }
        }
        return null;
    },
    _addHiddenFieldToDictionary: function ($field) {
        /// <summary>
        /// Only adds the item to the dictionary ($hiddenFieldDictionary, hiddenFieldNamesDictionary)
        /// </summary>
        /// <param name="$field">jQuery object.</param>
        /// <returns type=""></returns>
        var app = $.app.hiddenContainer;
        self.$hiddenFieldDictionary.push($field);
        self.hiddenFieldNamesDictionary.push($field.attr("name"));
    },
    getHiddenField: function (nameOfHiddenField) {
        /// <summary>
        /// Get the hidden field value, if possible get it from dictionary object.
        /// Make sure that you put every hidden field inside #hidden-fields-container container
        /// </summary>
        /// <param name="nameOfHiddenField"></param>
        /// <returns type="return $ type object.">get attribute values $returnedObject.attr() or null</returns>
        var self = $.app.hiddenContainer;
        if (self.isHiddenContainerExist()) {
            var $container = self.$hiddenContainer,
                $field = self._getHiddenFieldDictionary(nameOfHiddenField);
            if ($field) {
                // not null
                return $field;
            } else {
                // is null the get id from DOM
                $field = $.byId(nameOfHiddenField);
                if ($field.length === 0) {
                    $field = $container.find("[name='" + nameOfHiddenField + "']");
                }
                self._addHiddenFieldToDictionary($field);
                return $field;
            }
        }
        return null;
    },
    setHiddenValue: function (nameOfHiddenField, val) {
        /// <summary>
        /// Get the hidden field value, if possible get it from dictionary object.
        /// Make sure that you put every hidden field inside #hidden-fields-container container
        /// </summary>
        /// <param name="nameOfHiddenField"></param>
        /// <returns type="return $ type object.">get attribute values $returnedObject.attr() or null</returns>
        var app = $.app.hiddenContainer;
        if (self.isHiddenContainerExist()) {
            var $field = self.getHiddenField(nameOfHiddenField);
            if ($field.length > 0) {
                $field.val(val);
                return $field;
            }
        }
        return null;
    }
}