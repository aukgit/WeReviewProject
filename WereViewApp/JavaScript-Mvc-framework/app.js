/*!
 * JavaScript Mvc framework.
 * Version                      : 1.5
 * Last Modified                : 21 Mar 2016  
 * 
 * Copyright (c) Md. Alim Ul Karim
 * Source Code Available at     : https://github.com/aukgit/JavaScript-MVC-Framework
 * Linkedin profile             : https://bd.linkedin.com/in/alimkarim
 * Facebook profile             : https://fb.com/alim.karim
 * Available under MIT license  : https://opensource.org/licenses/MIT
 * Facebook Page                : https://www.facebook.com/DevelopersOrganism
 * Mail to                      : info{at}developers-organism.com
 * Download                     : https://github.com/aukgit/JavaScript-MVC-Framework/archive/master.zip
 * 
 */

/**
 * JavaScript Mvc framework works with convention :
 * JavaScript Framework how it is implemented :  http://bit.ly/1KdWSHD | http://bit.ly/1KdX0qq 
 */
; $.app = $.app || {};
$.app = {
    isDebugging: true,
    $hiddenContainer: null,
    $hiddenFieldDictionary: [],
    hiddenFieldNamesDictionary: [],
    initHiddenContainer: function() {
        /// <summary>
        /// Initialize hidden container if exist.
        /// </summary>
        /// <returns type="">returns hidden container.</returns>
        var app = $.app;
        app.$hiddenContainer = $.byId("hidden-fields-container");
        app.$hiddenFieldDictionary = [];
        app.hiddenFieldNamesDictionary = [];
        return app.$hiddenContainer;
    },
    isHiddenContainerExist: function() {
        return !$.isEmpty($.app.$hiddenContainer);
    },
    _getHiddenFieldDictionary: function(nameOfHiddenField) {
        /// <summary>
        /// Get dictionary hidden field values.
        /// </summary>
        /// <param name="nameOfHiddenField"></param>
        /// <returns type="return $ type object.">null or jquery obejct.</returns>

        if (nameOfHiddenField) {
            var namesDictionary = $.app.hiddenFieldNamesDictionary;
            for (var i = 0; i < namesDictionary.length; i++) {
                var hiddenName = namesDictionary[i];
                if (hiddenName === nameOfHiddenField) {
                    return $.app.$hiddenFieldDictionary[i];
                }
            }
        }
        return null;
    },
    _addHiddenFieldToDictionary: function($field) {
        /// <summary>
        /// Only adds the item to the dictionary ($hiddenFieldDictionary, hiddenFieldNamesDictionary)
        /// </summary>
        /// <param name="$field">jQuery object.</param>
        /// <returns type=""></returns>
        var app = $.app;
        app.$hiddenFieldDictionary.push($field);
        app.hiddenFieldNamesDictionary.push($field.attr("name"));
    },
    getHiddenField: function(nameOfHiddenField) {
        /// <summary>
        /// Get the hidden field value, if possible get it from dictionary object.
        /// Make sure that you put every hidden field inside #hidden-fields-container container
        /// </summary>
        /// <param name="nameOfHiddenField"></param>
        /// <returns type="return $ type object.">get attribute values $returnedObject.attr() or null</returns>
        var app = $.app;
        if (app.isHiddenContainerExist()) {
            var $container = app.$hiddenContainer,
                $field = app._getHiddenFieldDictionary(nameOfHiddenField);
            if ($field) {
                // not null
                return $field;
            } else {
                // is null the get id from DOM
                $field = $.byId(nameOfHiddenField);
                if ($field.length === 0) {
                    $field = $container.find("[name='" + nameOfHiddenField + "']");
                }
                app._addHiddenFieldToDictionary($field);
                return $field;
            }
        }
        return null;
    },
    setHiddenValue: function(nameOfHiddenField, val) {
        /// <summary>
        /// Get the hidden field value, if possible get it from dictionary object.
        /// Make sure that you put every hidden field inside #hidden-fields-container container
        /// </summary>
        /// <param name="nameOfHiddenField"></param>
        /// <returns type="return $ type object.">get attribute values $returnedObject.attr() or null</returns>
        var app = $.app;
        if (app.isHiddenContainerExist()) {
            var $field = app.getHiddenField(nameOfHiddenField);
            if ($field.length > 0) {
                $field.val(val);
                return $field;
            }
        }
        return null;
    }
};