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
    countryFieldName: "CountryID",

    countryComboSelector: ".form-control.selectpicker.country-combo",
    countryComboDivInnerSelector: ".country-combo-div",
    countryDropDownItemsSelector: "ul.dropdown-menu.inner.selectpicker",
    btnSelector: "button.btn.dropdown-toggle.selectpicker.btn-success.flag-combo",
    isCountryRetriveAsHtml: false,
    getCountryComboOptionsStringFromJson: function (jsonItems, extraHtmlWithEachElement, itemClasses) {
        /// <summary>
        /// Generates and append "option" items to the given $select. 
        /// </summary>
        /// <param name="jsonItems">must contain display and id value for every 'option' item.</param>
        /// <param name="extraHtmlWithEachElement">add the extra html content with option display value</param>
        /// <param name="itemClasses">add classes with each option.</param>
        if (_.isEmpty(itemClasses)) {
            itemClasses = "";
        }
        if (_.isEmpty(extraHtmlWithEachElement)) {
            extraHtmlWithEachElement = "";
        }
        if (jsonItems.length > 0) {
            var length = jsonItems.length;
            var options = new Array(length + 5);
            var selected = " selected='selected' ";
            var optionStarting = "<option class='" + itemClasses;
            var optionEnding = "</option>";
            for (var i = 0; i < length; i++) {
                if (i !== 0 && selected !== "") {
                    selected = ""; //only first one will be selected
                }
                options[i] = optionStarting +
                             jsonItems[i].countryCode +
                             "' title='" + "| " +
                             jsonItems[i].display +
                             "'" +
                             selected +
                             "value='" +
                             jsonItems[i].id +
                             "'>" +
                             extraHtmlWithEachElement +
                             jsonItems[i].display +
                             optionEnding;
            }
            return options.join("");
        }
        return "";
    },
    getCountryWholeComboStringWithJsonItems: function (jsonItems, comboName, comboClass, additionalAttributesWithCombo, extraHtmlWithEachElement, eachOptionItemClasses) {
        /// <summary>
        /// Returns a full country combo/select based on json items
        /// Developer should inject this into document
        /// </summary>
        /// <param name="comboName">Name of the combo/select</param>
        /// <param name="comboClass">By defaul "flag-country-combo flag" will be added with your custom classes for the combo/select</param>
        /// <param name="comboId">Just pass the id or give null, it will automatically formatted</param>
        /// <param name="stringOptionItems">Option items passed as an string</param>
        /// <param name="additionalAttributes">Add additional attributes with the select, however user have to format it. Eg. id='hello' </param>
        /// <param name="jsonItems">must contain display,id,countryCode value for every 'option' item.</param>
        /// <param name="extraHtmlWithEachElement">add the extra html content with option display value</param>
        /// <param name="itemClasses">add classes with each option.</param>
        if (_.isEmpty(eachOptionItemClasses)) {
            eachOptionItemClasses = "flag-country-combo flag ";
        } else {
            eachOptionItemClasses += " flag-country-combo flag ";
        }
        if (_.isEmpty(comboClass)) {
            comboClass = "btn-success country-combo flag-combo fc-af";
        } else {
            comboClass += " country-combo flag-combo fc-af";
        }
        var optionsString = $.devOrg.countryTimezonePhoneComponent.getCountryComboOptionsStringFromJson(jsonItems, extraHtmlWithEachElement, eachOptionItemClasses);
        var comboString = $.devOrg.getComboString(comboName, comboClass, comboName, optionsString, additionalAttributesWithCombo);
        return comboString;
    },
    initialize: function (countryUrl, timeZoneUrl, languageUrl, retriveAsHtml) {
        /// <summary>
        /// Initialize country , timezone and phone number fields
        /// </summary>
        /// <param name="countryUrl"></param>
        /// <param name="timeZoneUrl"></param>
        /// <param name="languageUrl"></param>
        /// <param name="retriveAsHtml">boolean : should retrieve only html or process the json. True means no processing.</param>

        $.devOrg.countryTimezonePhoneComponent.countryUrl = countryUrl;
        $.devOrg.countryTimezonePhoneComponent.timezoneUrl = timeZoneUrl;
        $.devOrg.countryTimezonePhoneComponent.languageUrl = languageUrl;
        $.devOrg.countryTimezonePhoneComponent.isCountryRetriveAsHtml = retriveAsHtml;
        var comboName = $.devOrg.countryTimezonePhoneComponent.countryFieldName;

        var $countryInnerDiv = $($.devOrg.countryTimezonePhoneComponent.countryComboDivInnerSelector);
        //console.log($countryInnerDiv);
        // first generate country
        if ($countryInnerDiv.length > 0) {
            $.ajax({
                method: "Get", // by default "GET"
                url: $.devOrg.countryTimezonePhoneComponent.countryUrl,
                dataType: "text" //, // "Text" , "HTML", "xml", "script" 
            }).done(function (response) {
                //console.log(response);
                var comboString;
                if (retriveAsHtml === false) {
                    comboString = $.devOrg
                        .countryTimezonePhoneComponent
                        .getCountryWholeComboStringWithJsonItems(response, comboName, "", "", ""); //var innerHtmlDiv = $countryInnerDiv.html();
                    //var wholeComboHtmlString = comboString + innerHtmlDiv;
                } else {
                    comboString = response; // html
                }

                $countryInnerDiv.prepend(comboString);
                $countryInnerDiv.find("select:first-child").selectpicker();

                // also make the country combo to select picker
                $.devOrg.countryTimezonePhoneComponent.setupRefreshingCountryFlag();
                //setup other dependables
                $.devOrg.countryTimezonePhoneComponent.setupDependableCombos();

                //console.log(comboString);


            }).fail(function (jqXHR, textStatus, ex) {
                console.log("Request failed: " + ex);
            });

        }
    },
    setupRefreshingCountryFlag: function () {
        $.devOrg.countryFlagRefresh($.devOrg.countryTimezonePhoneComponent.countryComboSelector,
                                    $.devOrg.countryTimezonePhoneComponent.countryDropDownItemsSelector,
                                    $.devOrg.countryTimezonePhoneComponent.btnSelector);


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