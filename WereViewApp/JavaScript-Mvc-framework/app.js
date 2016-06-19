/// <reference path="libs/jQuery/jquery-2.2.3.js" />
/// <reference path="libs/jQuery/jquery-2.2.3.intellisense.js" />
/// <reference path="byId.js" />
/// <reference path="schema/schema.js" />
/// <reference path="schema/hashset.js" />
/// <reference path="schema/url.js" />
/// <reference path="extensions/constants.js" />
/// <reference path="extensions/selectors.js" />

/*!
 * JavaScript Mvc framework.
 * Version                      : 2.0
 * Last Modified                : 19 Jun 2016  
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
 * JavaScript Mvc framework works with convention :
 * JavaScript Framework how it is implemented :  http://bit.ly/1KdWSHD | http://bit.ly/1KdX0qq 
 */


; $.app = $.app || {};
$.app = {
    isDebugging: true,
    getProcessForm: function () {
        /// <summary>
        /// Get the processing form.
        /// </summary>
        var app = $.app,
              selectors = app.selectors,
              ids = selectors.ids,
              $processForm = $.findCachedId(ids.processForm);

        return $processForm;
    }
};