/// <reference path="developers-organism.component.js" />
/// <reference path="developers-organism.dynamicSelect.js" />
/// <reference path="developers-organism.upload.js" />
/// <reference path="faster-jQuery.js" />
/// <reference path="WeReviewApps.js" />
/// <reference path="../star-rating.js" />
/// <reference path="../validation.js" />
/// <reference path="../underscore.js" />
/// <reference path="developers-organism.country-phone.js" />
/// <reference path="../ckeditor.js" />

/*!
 * Written by Alim Ul Karim
 * Developers Organism
 * Dated : 14 June 2015
 * Version : 1.2
 * https://www.facebook.com/DevelopersOrganism
 * mailto:info@developers-organism.com
*/

$.devOrg = $.devOrg || {};

$.devOrg.runner = function () {
    $.devOrg.Constants = {
        registerForm: $("form.register-form"),
        userName: "UserName",
        email: "Email",
        phoneNumberSelector: "#Phone",
        slash: "/",
        // "/Validator/"
        validatorCommonUrl: "/Validator/",
        //"/Validator/Username"
        usernameValidationUrl: "/Validator/Username",
        //"/Validator/Email"        
        emailAddressValidationUrl: "/Validator/Email",
        countryJsonUrl: "/Content/Scripts/Data/country-info-select-ready.json", // look like this /Partials/GetTimeZone/CountryID
        timeZoneJsonUrl: "/Partials/GetTimeZone", // look like this /Partials/GetTimeZone/CountryID
        languageJsonUrl: "/Partials/GetLanguage" // look like this /Partials/GetTimeZone/CountryID
    };


    if ($.devOrg.Constants.registerForm.length > 0) {
        // country , timezone, and phone initialize
        //$.devOrg
        //    .countryTimezonePhoneComponent
        //    .initialize($.devOrg.Constants.countryJsonUrl,
        //        $.devOrg.Constants.timeZoneJsonUrl,
        //        $.devOrg.Constants.languageJsonUrl,
        //        true // retrieve as html, to have the processed version  , make it false and change the url.
        //        );

        //$.devOrg.validateInputFromServer("#" + $.devOrg.Constants.userName,
        //                                  $.devOrg.Constants.usernameValidationUrl,
        //                                  $.devOrg.Constants.userName,
        //                                  true,
        //                                  false,
        //                                  3);
        //$.devOrg.validateInputFromServer("#" + $.devOrg.Constants.email,
        //                                  $.devOrg.Constants.emailAddressValidationUrl,
        //                                  $.devOrg.Constants.email,
        //                                  false,
        //                                  false,
        //                                  4);

        $.devOrg.enterToNextTextBox(".register-form", false);
        //$.devOrg.uxFriendlySlide("form.register-form", true);


        $.devOrg.bootstrapComboSelectbyFindingValue("select.country-combo", '1');

    }

    // load dynamic and depended select or combo
    $.devOrg.dynamicSelect.initialize();

    // make tables look nice with pagination

    $("select.selectpicker").selectpicker();

    $.devOrg.bootstrapComboSelectIndex("select.selectpicker", 0);


    ///Implement .rating-5,.rating-10
    $.devOrg.ratingMordernize();
    $.devOrg.bootstrapTabsMordernize(".make-it-tab");


}

$(function () {
    $.devOrg.runner();
});