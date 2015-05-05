/// <reference path="developers-organism.component.js" />
/// <reference path="developers-organism.dynamicSelect.js" />
/// <reference path="developers-organism.upload.js" />
/// <reference path="faster-jQuery.js" />
/// <reference path="WeReviewApps.js" />
/// <reference path="../star-rating.js" />
/// <reference path="../validation.js" />
/// <reference path="../underscore.js" />

$.devOrg.countryTimezonePhoneComponent = {

    countryUrl: "",
    timezoneUrl: "",
    languageUrl: "",

    countryComboSelector: ".form-control.selectpicker.country-combo",
    countryComboDivInnerSelector: ".country-combo-div",
    countryDropDownItemsSelector: "ul.dropdown-menu.inner.selectpicker",
    btnSelector: "button.btn.dropdown-toggle.selectpicker.btn-success.flag-combo",

    initialize: function (countryUrl, timeZoneUrl, languageUrl) {
        $.devOrg.countryTimezonePhoneComponent.countryUrl = countryUrl;
        $.devOrg.countryTimezonePhoneComponent.timezoneUrl = timeZoneUrl;
        $.devOrg.countryTimezonePhoneComponent.languageUrl = languageUrl;
        var $countryInnerDiv = $.queryAll($.devOrg.countryTimezonePhoneComponent.countryComboDivInnerSelector);
        console.log($countryInnerDiv);
        // first generate country
        if ($countryInnerDiv.length > 0) {
            $.ajax({
                method: "Get", // by default "GET"
                url: $.devOrg.countryTimezonePhoneComponent.countryUrl,
                dataType: "json" //, // "Text" , "HTML", "xml", "script" 
            }).done(function (response) {
                console.log(response);

                $countryInnerDiv.append(response);

            }).fail(function (jqXHR, textStatus, ex) {
                console.log("Request failed: " + ex);
            });

        }
    },
    setupRefreshingCountryFlag: function () {
        $.devOrg.countryFlagRefresh($.devOrg.Constants.countryComboSelector,
                                    $.devOrg.Constants.countryDropDownItemsSelector,
                                    $.devOrg.Constants.btnSelector);


    },
    setupDependableCombos: function () {
        /// <summary>
        /// Phone, Timezone and language
        /// </summary>
        // fix phone code and make country select to slectpicker()
        $.devOrg.countryRelatedToPhone($.devOrg.countryTimezonePhoneComponent.countryComboSelector,
                                        $.devOrg.countryTimezonePhoneComponent.countryDropDownItemsSelector,
                                        $.devOrg.countryTimezonePhoneComponent.btnSelector,
                                        $.devOrg.countryTimezonePhoneComponent.phoneNumberSelector);

        //country dependable load
        // set json { display = "display text", id= "value }
        $.devOrg.smartDependableCombo("select.country-combo", //selecting parent combo
                                      ".timezone-main", // must given : container for the time-zone
                                      ".timezone-combo-div", // must given : where to place the combo inside the container
                                      $.devOrg.countryTimezonePhoneComponent.timezoneUrl, // url
                                      "UserTimeZoneID", //combo-name
                                      "", //id
                                      "btn-success", //class
                                      ""
                                      );
        // set json { display = "display text", id= "value }
        $.devOrg.smartDependableCombo("select.country-combo",   //selecting parent combo
                                      ".language-main",         // must given : container
                                      ".language-combo-div",    // must given : where to place the combo inside the container
                                      $.devOrg.countryTimezonePhoneComponent.languageUrl, // url
                                      "CountryLanguageID", //combo-name
                                      "",//id
                                      "btn-success",//class
                                      ""
                                      );
    }

}