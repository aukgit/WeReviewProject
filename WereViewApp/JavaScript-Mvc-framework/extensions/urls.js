/// <reference path="urls.js" />
/// <reference path="constants.js" />
/// <reference path="country-phone.js" />
/// <reference path="devOrg.js" />
/// <reference path="initialize.js" />
/// <reference path="jQueryExtend.js" />
/// <reference path="jsonCombo.js" />
/// <reference path="regularExp.js" />
/// <reference path="selectors.js" />
/// <reference path="upload.js" />
/// <reference path="D:\Working\GitHub\WereViewProject\WereViewApp\Content/Scripts/jquery-2.1.4.js" />
/// <reference path="D:\Working\GitHub\WereViewProject\WereViewApp\Content/Scripts/jquery-2.1.4.intellisense.js" />
/// <reference path="../schema/url.js" />
; $.app = $.app || {};
$.app.urls = {
    /*
     * hostUrl will be retrieved from hidden field "#host-url"
     * Contains a slash at the end.
     */
    hostUrl: null,

    validator: "Validator/",
    usernameValidation: "Username",
    emailValidation: "Email",
    timeZoneJson: "Services/GetTimeZone", // look like this /Partials/GetTimeZone/CountryID
    languageJson: "Services/GetLanguage", // look like this /Partials/GetTimeZone/CountryID
    getHostUrl: function () {
        /// <summary>
        /// Retrieve host url from host-url id hidden field
        /// Return host url with a slash at the bottom.
        /// </summary>
        /// <returns type="">Returns the host url.</returns>
        var self = $.app.urls;
        var hostUrl = self.hostUrl;

        if ($.isEmpty(hostUrl)) {
            var dev = $.app,
                selectors = dev.selectors;
            var id = selectors.hostFieldId;
            var $hostUrlHidden = $.byId(id);
            if ($hostUrlHidden.length > 0) {
                var url = $hostUrlHidden.val();
                self.hostUrl = $.returnUrlWithSlash(url);
            }
        }
        return self.hostUrl;
    },

    getAbsUrl: function (givenUrl) {
        /// <summary>
        /// Given url shouldn't have any slash at the begining.
        /// </summary>
        /// <param name="givenUrl">url shouldn't have any slash at the begining.</param>
        /// <returns type="">Return absolute url containing host name and url.</returns>
        var self = $.app.urls;
        var hostUrl = self.hostUrl;
        if (!$.isEmpty(hostUrl)) {
            return hostUrl + givenUrl;
        }
        hostUrl = self.getHostUrl();
        return hostUrl + givenUrl;
    },


    getAbsValidatorUrl: function (url) {
        /// <summary>
        /// Returns absolute url of a validation
        /// </summary>
        /// <param name="url"></param>
        /// <returns type="string">returns absolute url.</returns>
        var self = $.app.urls;

        var urlCombined = self.validator + url;
        return self.getAbsUrl(urlCombined);

    },

    getGeneralUrlSchema: function (shouldGetDefaultSchema, otherUrlsList) {
        /// <summary>
        /// Generate a general url schema , which contains
        /// It will look for hidden fields : edit-url, add-url, delete-url, save-url
        /// </summary>
        /// <param name="shouldGetDefaultSchema" type="bool">
        /// T/F , T/undefined : gets the default schmea.
        /// </param>
        /// <param name="otherUrlsList" type="type">
        /// Array of list items containing new url names.
        /// If null then only return url schema with add,edit,save,remove urls.
        /// For example, retrieving "edit-url" hidden value pass "edit".
        /// </param>
        /// <returns type="$.app.schema.url">
        /// Returns a url schema object from schema folder's url (schema).
        /// </returns>
        var urlSchema, i, urlName;
        if ($.isEmpty(shouldGetDefaultSchema) || shouldGetDefaultSchema === true) {
            urlSchema = $.app.schema.create($.app.schema.url);
            var keys = Object.keys(urlSchema);
            for (i = 0; i < keys.length; i++) {
                urlName = keys[i];
                urlSchema[urlName] = $.getHiddenValue(urlName + "-url");
            }
        } else {
            urlSchema = {};
        }

        if (!$.isEmpty(otherUrlsList)) {
            for (i = 0; i < otherUrlsList.length; i++) {
                urlName = otherUrlsList[i];
                urlSchema[urlName] = $.getHiddenValue(urlName + "-url");
            }
        }
        return urlSchema;
    }
};