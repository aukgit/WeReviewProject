/// <reference path="faster-jQuery.js" />
/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../jquery-2.1.3.intellisense.js" />
/// <reference path="../bootstrap.min.js" />
/// <reference path="../bootstrap-table.js" />
/// <reference path="../jquery.validate.js" />
/// <reference path="../modernizr-2.8.3.js" />
/// <reference path="../star-rating.js" />
/// <reference path="../moment.js" />
/// <reference path="../underscore.js" />


/*!
 * Written by Alim Ul Karim
 * Developers Organism
 * https://www.facebook.com/DevelopersOrganism
 * mailto:info@developers-organism.com
*/
$(function() {

    $.fn.extend({
        // jQuery element get all classes
        getAllClasses: function() {
            if (this.length === 1) {
                return this.attr("class").split(/\s+/);
            }
            return null;
        }
    });


    $.devOrg = {


        // get all the classes from an jQuery element
        getAllClasses: function(jQueryHtmlElement) {
            return jQueryHtmlElement.getAllClasses();
        },

        // allClassesArray = ['a','b','c'] , exceptClassesArray=['b','c'], results=['a']
        getClassesExcept: function(allClassesArray, exceptClassesArray) {
            if (allClassesArray === null || allClassesArray === undefined) {
                return [];
            }

            if (exceptClassesArray === null || exceptClassesArray === undefined) {
                return allClassesArray;
            }
            var len = allClassesArray.length;
            var results = [];
            for (var i = 0; i < len; i++) {
                if (exceptClassesArray.indexOf(allClassesArray[i]) === -1) {
                    // not found
                    results.push(allClassesArray[i]);
                }
            }
            return results;
        },
        // all Selectors are jQuery Selector Text  only.
        // selectpicker will be called inside function, no need to call outside.
        countryFlagRefresh: function(countrySelector, dropDownItemsSelector, dropDownBtnSelector) {
            var countryBox = $(countrySelector).selectpicker(); // only select a select element then apply the custom bootstrap selector
            var dropDownItems = $(dropDownItemsSelector); // getting generated dropdown items from the custom bootstrap selector
            var dropDownBtn = $(dropDownBtnSelector); // generated new button from the selectpicker option
            var skippingClassesAnchor = ["flag-country-combo", "flag"];
            var skippingClassesForBtn = ["btn", "dropdown-toggle", "selectpicker", "btn-success", "flag-combo"];

            // console.log(dropDownItems.length);
            countryBox.change(function(e) {
                var listItem = dropDownItems.find("li.selected");
                var anchorItem = listItem.find("a");
                var listOfAllAnchorClasses = anchorItem.getAllClasses();
                var listOfAllClassesdropDownBtn = dropDownBtn.getAllClasses();
                var flagClass = $.devOrg.getClassesExcept(listOfAllAnchorClasses, skippingClassesAnchor);
                var btnFlagClass = $.devOrg.getClassesExcept(listOfAllClassesdropDownBtn, skippingClassesForBtn);
                for (var i = 0; i < btnFlagClass.length; i++) {
                    dropDownBtn.removeClass(btnFlagClass[i]);
                }
                dropDownBtn.addClass("fc-" + flagClass[0]);
            });
        },
        // countryFlagRefresh must be called first or selectpicker must be called first
        // all Selectors are jQuery Selector Text  only.
        countryRelatedToPhone: function(countrySelector, dropDownItemsSelector, dropDownBtnSelector, phoneNumberInputSelector) {
            var countryBox = $(countrySelector);
            var dropDownItems = $(dropDownItemsSelector);
            var dropDownBtn = $(dropDownBtnSelector);
            var phoneNumberBox = $(phoneNumberInputSelector);
            var previousCallingCode = "";

            function selectChangeState() {
                // console.log("executed");
                var listItem = dropDownItems.find("li.selected");
                var spanText = listItem.find("a > span").text().toString();
                var newCallingCode = $.devOrg.getTextBetween(spanText, "(", ")");
                var getWrittenPhoneNumber = phoneNumberBox.val();
                // console.log(listItem);
                newCallingCode = $.devOrg.replaceStartsWith(newCallingCode, "+", "");
                if ((!_.isEmpty(getWrittenPhoneNumber) && !_.isEmpty(previousCallingCode))
                    && $.devOrg.isStartsWith(getWrittenPhoneNumber, previousCallingCode)) {
                    getWrittenPhoneNumber = $.devOrg.replaceStartsWith(getWrittenPhoneNumber, previousCallingCode, newCallingCode);
                } else {
                    getWrittenPhoneNumber = newCallingCode + getWrittenPhoneNumber;
                }
                previousCallingCode = newCallingCode;
                phoneNumberBox.val(getWrittenPhoneNumber);
            }

            countryBox.ready(selectChangeState).change(selectChangeState);
            //phoneNumberBox.keyup(selectChangeState);
            // $("#selectID option")[index].selected = true;
        },

        getTextBetween: function(givenString, startSequence, endingSequence) {
            if (_.isString(givenString)) {
                var index1 = givenString.indexOf(startSequence);
                if (index1 > -1) {
                    var index2 = givenString.indexOf(endingSequence);
                    if (index2 > -1) {
                        // exist
                        return givenString.substr(index1 + 1, index2 - index1 - 1);
                    }
                }
            }
            return null;
        },
        // parentjQueryCombo = passJqueryElement , mainDivContainerSelector = ".something-main", innerDivSelectorForPlacingCombo= ".somthing-combo-div"
        // it would be better to execute parentjQueryCombo as selectpicker or have a selectpicker class.
        // No combo will appear , even the main div will disappear if no item is received from the link.
        // json sender should sends as id and text only.
        smartDependableCombo: function(parentjQuerySelector, mainDivContainerSelector, innerDivSelectorForPlacingCombo, urlToGetJson, placeComboName, placedComboId, placedComboClass, placedComboAdditionalClassesWithItems, placedComboAdditionalHtmlWithEachItem) {
            var parentjQueryCombo = $(parentjQuerySelector);
            if (_.isEmpty(parentjQueryCombo)) {
                console.error.log("error raised from developers organism component's smartDependableCombo that no parent is detected.");
                return; // nothing exist in parent.
            }
            var mainDiv = $(mainDivContainerSelector);
            var innerDiv = mainDiv.find(innerDivSelectorForPlacingCombo);

            function hideDiv() {
                if (mainDiv.length > 0) {
                    mainDiv.hide();
                } else {
                    console.error.log("devOrg->smartDependableCombo: main div not found for '" + mainDivContainerSelector + "'");
                }
            }

            hideDiv();

            function showDiv() {
                // remove select if exist.
                var options = innerDiv.find("select, div.bootstrap-select");
                if (options.length > 0) {
                    options.remove();
                }
                mainDiv.show("slow");
            }

            function createCombo(response) {
                if (!_.isEmpty(placedComboId)) {
                    placedComboId = " id='" + placedComboId + "' ";
                } else {
                    placedComboId = "";
                }
                if (_.isEmpty(placedComboClass)) {
                    placedComboClass = "";
                }

                if (_.isEmpty(placeComboName)) {
                    placeComboName = "";
                } else {
                    placeComboName = " name='" + placeComboName + "' ";
                }


                innerDiv.prepend("<select " + placeComboName + " class='devOrgSmartCombo form-control " + placedComboClass + " selectpicker'" + placedComboId + "data-style='" + placedComboClass + "' data-live-search='true'></select>");
                var combo = innerDiv.find("select");
                $.devOrg.appenedComboElement(combo, response, placedComboAdditionalHtmlWithEachItem, placedComboAdditionalClassesWithItems);
                combo.selectpicker();
            }

            parentjQueryCombo.change(function() {
                var parentComboValue = parentjQueryCombo.val();
                var actualUrl = urlToGetJson + "/" + parentComboValue;
                $.ajax({
                    type: "POST",
                    dataType: "JSON",
                    url: actualUrl,
                    success: function(response) {
                        if (response.length === 0) {
                            hideDiv();
                            return;
                        }
                        innerDiv = $(mainDivContainerSelector + " " + innerDivSelectorForPlacingCombo);
                        // items exist.
                        showDiv(); //remove inner options if exist any
                        createCombo(response); // create if necessary and then append options to it.
                    },
                    error: function(xhr, status, error) {
                        hideDiv();
                    }
                });
            });
        },
        // listOfItems = expected a json item with id and text property
        // extraHtmlWithEachElement : represents like below
        // <option .. > extraHtmlWithEachElement Item </option>
        appenedComboElement: function(combo, listOfItems, extraHtmlWithEachElement, itemClasses) {
            // followed by the best practice : http:// allthingscraig.com/blog/2012/09/28/best-practice-appending-items-to-the-dom-using-jquery/
            if (_.isEmpty(itemClasses)) {
                itemClasses = "";
            }
            if (_.isEmpty(extraHtmlWithEachElement)) {
                extraHtmlWithEachElement = "";
            }
            if (listOfItems.length > 0) {
                var length = listOfItems.length;
                var options = new Array(length + 5);
                var selected = " Selected='selected' ";
                var optionStarting = "<option class='devorgCombo-item " + itemClasses + "'";
                var optionEnding = "</option>";
                for (var i = 0; i < length; i++) {
                    if (i === 0) {
                        selected = "";
                    }
                    options[i] = optionStarting + selected + "value='" + listOfItems[i].id + "'>" + extraHtmlWithEachElement + listOfItems[i].text + optionEnding;
                }
                combo.append(options.join(""));
            }
        },
        bootstrapComboSelectbyFindingValue: function(comboSelector, searchForvalue) {
            $(comboSelector).selectpicker("val", searchForvalue).trigger("change");
        },
        bootstrapComboSelectIndex: function(comboSelector, index) {
            var combo = $(comboSelector + ">option");
            if (combo.length > 0 && index <= (combo.length - 1)) {

                var itemFound = $(combo[index]);
                var value = itemFound.val();
                $.devOrg.bootstrapComboSelectbyFindingValue(comboSelector, value);
            }
        },

        // givenString "Example ( Hello )" 
        // startsWith= "Example" ; returns true.
        isStartsWith: function(givenString, startsWith) {
            if (_.isString(givenString)) {
                var subtringOfGiventext = givenString.substr(0, startsWith.length);
                if (subtringOfGiventext === startsWith) {
                    return true;
                }
            }
            return false;
        },

        replaceStartsWith: function(givenString, findStartsWith, replaceString) {
            if (_.isString(givenString) && !_.isEmpty(findStartsWith)) {
                var subtringOfGiventext = givenString.substr(0, findStartsWith.length);
                if (subtringOfGiventext === findStartsWith) {
                    var nextStringIndex = findStartsWith.length;
                    var otherHalftext = givenString.substr(nextStringIndex, givenString.length - nextStringIndex);
                    return replaceString + otherHalftext;
                }
            }
            return givenString;
        },

        // jquery formSelector, submitAtLast:true/false
        enterToNextTextBox: function(formSelector, submitAtLast) {
            $(formSelector + " input:text:first").focus();
            var binders = formSelector + " input[type='text']:visible," +
                formSelector + " input[type='password']:visible," +
                formSelector + " input[type='numeric']:visible," +
                formSelector + " input[type='email']:visible," +
                //formSelector + " textarea:visible," +
                formSelector + " button.selectpicker[type='button']:visible," +
                formSelector + " select:visible";
            $(document).on("keypress", binders, function(e) {
                // var codeAbove = d.keyCode || d.which;
                // console.log("above code :" + codeAbove);
                var code = e.keyCode || e.which;
                // console.log("inside code :" + code);
                if (code === 13) { // Enter key
                    e.preventDefault(); // Skip default behavior of the enter key
                    var n = $(binders).length;
                    var nextIndex = $(binders).index(this) + 1;
                    if (nextIndex < n) {
                        $(binders)[nextIndex].focus();
                    } else {
                        $(binders)[nextIndex - 1].blur();
                        if (submitAtLast === true) {
                            $(formSelector).submit();
                        }
                    }
                }
            });
        },

        validateTextInputBasedOnRegEx: function(jQuerySelectorforTextBox, stringRegEx, msgOnInvalidPattern) {
            /// <summary>
            ///     Validate text input while typing with ASP.NET jquery validation.
            ///     Only the attributes with the text. No event is bound.
            /// </summary>
            /// <param name="jQuerySelectorforTextBox">string:jQuery Selector</param>
            /// <param name="stringRegEx">string: Regular expression to validate the textinput</param>
            $(jQuerySelectorforTextBox).attr("data-val-regex-pattern", stringRegEx);
            if (!_.isEmpty(msgOnInvalidPattern)) {
                $(jQuerySelectorforTextBox).attr("data-val-regex", msgOnInvalidPattern);
            }

        },

        reSetupjQueryValidate: function(jQueryFormSelector) {
            /// <summary>
            ///     call after setting new reg ex via validateTextInputBasedOnRegEx
            /// </summary>
            /// <param name="jQueryFormSelector"></param>
            var $form = $(jQueryFormSelector)
                .removeData("validator") /* added by the raw jquery.validate plugin */
                .removeData("unobtrusiveValidation");
            /* added by the jquery unobtrusive plugin */
            $.validator.unobtrusive.parse($form);
        },

        validateTextInputsBasedOnHiddenSpansGiven: function(formSelector) {
            /// <summary>
            ///     inComplete
            ///     There should be span for each of inputs that needs to be modified or changed.
            ///     hidden span with data-name="same as input"
            ///     data-display, data-reg, data-reg-fail-msg, data-min,data-max,data-range-failed-msg
            ///     data-placeholder, data-class
            /// </summary>
            /// <param name="formSelector">jQuery form selector</param>
            var $form = $(formSelector);
            var dataRegSpanAttr = "data-reg";
            var dataRegMsgSpanAttr = "data-reg-fail-msg";
            var dataMinSpanAttr = "data-min";
            var dataMaxSpanAttr = "data-max";
            var dataRangeMsgSpanAttr = "data-min";
            var dataDisplaySpanAttr = "data-range-failed-msg";
            var dataMinSpanAttr = "data-min";

            if ($form.length > 0) {
                var binders = "input[type='text']," +
                    "input[type='password']," +
                    "input[type='email']," +
                    "input[type='numeric']," +
                    "select:visible";
                var $inputs = $form.find(binders);
                for (var i = 0; i < $inputs.length; i++) {
                    var $input = $($inputs[i]);
                    var $nameOftheInput = $input.attr("name");
                    // now find the span based on the name
                    var $span = $form.find("span[data-name='" + $nameOftheInput + "']:hidden");
                    if ($span.length > 0) {
                        // set the attr to the input.

                        // setting reg ex

                    }
                }
            }
        },

        validateInputFromServer: function(jQuerytextBoxSelector, validationUrl, internalValidatorSpanClassName, isAlwaysFocusUntilValid, isDisable, minChars, isSubmitTheWholeForm, onInvalidStringStatementInCrossMark, onValidStringStatementInCheckMark, $formGiven, maxTryLimit) {
            /// <summary>
            ///     Made validation easy on the fly with a server response.
            /// </summary>
            /// <param name="jQuerytextBoxSelector">string: jQuery Selector</param>
            /// <param name="validationURL">string: Url to validate</param>
            /// <param name="internalValidatorSpanClassName">Propertyname (refer to the class of validation span) Indicating class.</param>
            /// <param name="isAlwaysFocusUntilValid">Boolean: Keep the focus until it's valid. By default: false</param>
            /// <param name="isDisable">Boolean: Disable the textbox after being validated. By default : false.</param>
            /// <param name="minChars">number: min chars to send the request</param>
            /// <param name="isSubmitTheWholeForm">Boolean:Rather than submitting the small form submit the whole related closet form.</param>
            /// <param name="onInvalidStringStatementInCrossMark">invalid statement show on the cross mark.</param>
            /// <param name="onValidStringStatementInCheckMark">
            ///     valid statement show on the check mark. By default: fieldDisplayname +
            ///     is available and valid
            /// </param>
            //if (_.isEmpty(isSubmitTheWholeForm)) {
            //    isSubmitTheWholeForm = false;
            //}

            //if (_.isEmpty(isAlwaysFocusUntilValid)) {
            //    isAlwaysFocusUntilValid = false;
            //}

            //if (_.isEmpty(isDisable)) {
            //    isDisable = false;
            //}

            var sentRequestCount = 0;
            var $userTextbox = $(jQuerytextBoxSelector);
            if ($userTextbox.length > 0) {
                $userTextbox.removeAttr("isDisable");

                if (!isSubmitTheWholeForm) {
                    $userTextbox.keyup(function() {
                        $.queryAll("#validation #id").val($userTextbox.val());
                        // console.log(user);
                    }).focus(function() {
                        $.queryAll("#validation #id").val($userTextbox.val());
                        // console.log(user);
                    });
                }

                $userTextbox.blur(function() {
                    if (!isSubmitTheWholeForm) {
                        $.queryAll("#validation #id").val($userTextbox.val());
                    }
                    var $passingText = $userTextbox.val();
                    if (_.isEmpty($passingText) || $passingText.length < minChars) {
                        // if empty text then don't send.
                        return;
                    }
                    if (_.isEmpty(onValidStringStatementInCheckMark)) {
                        onValidStringStatementInCheckMark = "is available and valid.";
                    }
                    if (_.isEmpty(onInvalidStringStatementInCrossMark)) {
                        onInvalidStringStatementInCrossMark = "is not valid or already exist. Your input can't contain ( [ ] ' , * & ? \" ) or space or any other special character for this data-type.";
                    }

                    // Validation should be a formData underlying the original from with 
                    // only antiforgery token and a hidden id field
                    // whatever is typed in that selected text box will be pushed into
                    // this formData
                    var formData;
                    if ($formGiven === null || $formGiven === undefined || $formGiven.length == 0) {
                        if (!isSubmitTheWholeForm) {
                            formData = $.byId("validation").serialize();
                        } else {
                            formData = $userTextbox.closest("form").serializeArray();
                        }
                    } else {
                        formData = $formGiven.serializeArray();
                    }

                    //console.log(formData);

                    var validatorName = "span.CustomValidation." + internalValidatorSpanClassName;
                    var token = $("input[name=__RequestVerificationToken]").val();
                    var processingState1 = "glyphicon-refresh";
                    var processingState2 = "glyphicon-spin";
                    var isHideClass = "hide";
                    var colorGreen = "green";
                    var colorRed = "red";
                    var correctState = "glyphicon-ok";
                    var incorrectState = "glyphicon-remove";
                    var $validatorBox = $(validatorName);
                    var displayName = $validatorBox.attr("data-display");
                    var correctStateTitle = displayName + " " + onValidStringStatementInCheckMark;
                    var invalidAttrName = "data-invalid";
                    var incorrectStateTitle = displayName + " " + onInvalidStringStatementInCrossMark;
                    var tooltipName = "a.CustomValidation." + internalValidatorSpanClassName + ".tooltip-show";
                    var $tooltipBox = $(tooltipName);

                    // console.log($("#validation #id").val());
                    $validatorBox.removeClass(incorrectState).removeClass(correctState);


                    // if no processing state then add it
                    if (!$validatorBox.hasClass(processingState1)) {
                        $validatorBox.addClass(processingState1);
                    }

                    if (!$validatorBox.hasClass(processingState2)) {
                        $validatorBox.addClass(processingState2);
                    }
                    if ($validatorBox.hasClass(isHideClass)) {
                        $validatorBox.removeClass(isHideClass);
                    }
                    $tooltipBox.attr("data-original-title", "Validating " + displayName)
                        .attr("title", "Validating " + displayName);
                    // confirming processing state.
                    if (maxTryLimit !== null && maxTryLimit !== undefined && sentRequestCount > maxTryLimit) {
                        return;
                    }
                    $.ajax({
                        type: "POST",
                        dataType: "JSON",
                        url: validationUrl,
                        data: formData,
                        success: function(response) {
                            sentRequestCount = sentRequestCount + 1;
                            // Remove the processing state
                            if ($validatorBox.hasClass(processingState1)) {
                                $validatorBox.removeClass(processingState1);
                            }

                            if ($validatorBox.hasClass(processingState2)) {
                                $validatorBox.removeClass(processingState2);
                            }
                            if ($validatorBox.hasClass(isHideClass)) {
                                $validatorBox.removeClass(isHideClass);
                            }
                            // Remove the processing state
                            if (response == true) {
                                if ($validatorBox.hasClass(incorrectState)) {
                                    $validatorBox.removeClass(incorrectState);
                                }
                                if ($validatorBox.hasClass(colorRed)) {
                                    $validatorBox.removeClass(colorRed);
                                }
                                $validatorBox.addClass(colorGreen)
                                    .addClass(correctState)
                                    .attr("title", correctStateTitle);

                                $tooltipBox.attr("title", correctStateTitle)
                                    .attr("data-original-title", correctStateTitle);
                                if (isDisable) {
                                    $userTextbox.prop("isDisable", true);
                                }

                                $userTextbox.addClass("bold")
                                    .addClass("green")
                                    .next()
                                    .focus();

                                $userTextbox.removeAttr(invalidAttrName);
                            } else {
                                if ($validatorBox.hasClass(colorGreen)) {
                                    $validatorBox.removeClass(colorGreen);
                                }
                                if ($validatorBox.hasClass(correctState)) {
                                    $validatorBox.removeClass(correctState);
                                }
                                $userTextbox.prop("isDisable", false)
                                    .addClass("bold")
                                    .addClass("red");

                                $validatorBox.addClass(colorRed)
                                    .addClass(incorrectState)
                                    .attr("title", incorrectStateTitle);

                                $tooltipBox.attr("title", incorrectStateTitle)
                                    .attr("data-original-title", incorrectStateTitle);
                                if (isAlwaysFocusUntilValid === true) {
                                    $userTextbox.focus();
                                }
                                $userTextbox.attr(invalidAttrName, "true");
                            }
                            $(".tooltip-show").tooltip();

                        },
                        error: function(xhr, status, error) {
                            // Remove the processing state
                            if ($validatorBox.hasClass(processingState1)) {
                                $validatorBox.removeClass(processingState1);
                            }

                            if ($validatorBox.hasClass(processingState2)) {
                                $validatorBox.removeClass(processingState2);
                            }
                            if ($validatorBox.hasClass(isHideClass)) {
                                $validatorBox.removeClass(isHideClass);
                            }
                            // Remove the processing state
                            if ($validatorBox.hasClass(correctState)) {
                                $validatorBox.removeClass(correctState);
                            }

                            if ($validatorBox.hasClass(colorGreen)) {
                                $validatorBox.removeClass(colorGreen);
                            }
                            $userTextbox.prop("isDisable", false)
                                .addClass("bold")
                                .addClass("red");

                            $validatorBox.addClass(colorRed)
                                .addClass(incorrectState)
                                .attr("title", error);

                            $tooltipBox.attr("title", status)
                                .attr("data-original-title", error);

                            $(".tooltip-show").tooltip();
                            $userTextbox.attr(invalidAttrName, "true");
                        }
                    }); // ajax end
                });
            }; // if else end
        },


        fillRegisterFieldsOnDemo: function() {
            var i = 0;
            var controls = $(".form-group");
            var $fields = controls.find("input[type=text]");
            $.each($fields, function() {
                this.value = 1111111111111;
            });

            $fields = controls.find("input[type=password]");
            $.each($fields, function() {
                this.value = "asdf1234@";
            });


            $fields = controls.find("input[type=number]");
            $.each($fields, function() {
                this.value = i++;
            });

            $fields = controls.find("textarea");
            $.each($fields, function() {
                this.value = "1111111111111";
            });

            $fields = controls.find("input[type=email]");
            $.each($fields, function() {
                this.value = "auk.junk@live.com";
            });

            $fields = controls.find("input[type=checkbox]");
            $.each($fields, function() {
                this.prop("checked", true);
            });

        },
        //'.make-it-tab'
        bootstrapTabsMordernize: function(tabSelector) {
            var bootstrapTabs = $(tabSelector);
            if (bootstrapTabs.length > 0) {
                var tabHidden = $(".tab-content input[type='hidden'][name='tab']");

                if (tabHidden.length > 0) {
                    var tabHiddenValue = tabHidden.val();
                    if (!_.isEmpty(tabHiddenValue)) {
                        //tab name exist
                        bootstrapTabs.find("li>a[href='" + tabHiddenValue + "']").tab("show");
                    } else {
                        //no tab name exist.. select default one.
                        bootstrapTabs.find("li > a:first").tab("show");
                    }
                }

                bootstrapTabs.click(function(e) {
                    //e.preventDefault();                    
                    e.preventDefault();
                    $(this).tab("show");

                });

                $("ul" + tabSelector + ".nav-tabs > li > a").on("shown.bs.tab", function(e) {
                    var valueOfActive = $(e.target).attr("href");
                    // = $(tabSelector + " li.active>a").attr('href');
                    tabHidden.val(valueOfActive);
                    //window.location.hash = id;
                });
            }
        },
        ratingMordernize: function() {
            var ratingItems = $(".rating-5");

            if (ratingItems.length > 0) {
                ratingItems.rating({
                    showClear: false
                });
            }
            ratingItems = $(".rating-10");

            if (ratingItems.length > 0) {
                ratingItems.rating({
                    showClear: false,
                    starCaptionClasses: {
                        0.5: "label label-danger",
                        1: "label label-danger",
                        1.5: "label label-danger",
                        2: "label label-danger",
                        2.5: "label label-danger",
                        2: "label label-warning",
                        2.5: "label label-warning",
                        3: "label label-warning",
                        3.5: "label label-warning",
                        4: "label label-warning",
                        4.5: "label label-warning",
                        5: "label label-warning",
                        5.5: "label label-info",
                        6: "label label-info",
                        6.5: "label label-info",
                        7: "label label-info",
                        7.5: "label label-primary",
                        8: "label label-primary",
                        8.5: "label label-success",
                        9: "label label-success",
                        9.5: "label label-success",
                        10: "label label-success"
                    }
                });
            }
        },

        uxFriendlySlide: function(jQueryformSelector, keepOthersVisible, dontSubmit) {
            /// <summary>
            ///     hides except for the first div with value 0. Add attributes to divs
            ///     [data-dev-slide='number-zero-based'][data-dev-visited='false'] and
            ///     encapsulate inputs. Each click clicked on submit it will verify the inputs if verified next hide ones will be shown
            ///     it will be continuous process until hit the last.
            ///     Always use lower case false
            ///     [data-dev-slide='number-zero-based'][data-dev-visited='false']
            /// </summary>
            /// <param name="jQueryformSelector">jQuery selector for the form</param>
            /// <param name="keepOthersVisible">Should add new hide ones or previous ones hides and load new ones(divs)</param>
            /// <param name="dontSubmit">When none left , do we submit? True: don't submit</param>
            var slideObjects = $(jQueryformSelector + " [data-dev-slide][data-dev-visited='false']");
            var executedOnce = false;
            var binders = "input[type='text']:visible," +
                "input[type='password']:visible," +
                "input[type='email']:visible," +
                "input[type='numeric']:visible," +
                "select:visible";
            var order = 0;
            var totalSliderLength = slideObjects.length;
            var previousSlideNumber = 0;
            if (totalSliderLength > 0) {
                // exist slides.
                slideObjects.hide();
                previousSlideNumber = order;
                slideObjects.filter("[data-dev-slide='" + (order++) + "'][data-dev-visited='false']").show();
                $(jQueryformSelector).submit(function(e) {
                    e.preventDefault();

                    var nextOne = slideObjects.filter("[data-dev-slide='" + order + "'][data-dev-visited='false']");
                    // if (nextOne.length == 0) {
                    //    for (order += 1; nextOne.length == 0 && totalSliderLength >= order; order++) {
                    //        nextOne = slideObjects.filter("[data-dev-slide='" + order + "'][data-dev-visited='false']");
                    //    }
                    // }
                    var previousOne;
                    var inputBoxes;
                    if (nextOne.length > 0) {
                        previousOne = slideObjects.filter("[data-dev-slide='" + (order - 1) + "']"); // console.log(previousOne);
                        inputBoxes = previousOne.find("input, textarea"); // still exist , prevent submission
                        if (inputBoxes.length > 0 && $.devOrg.checkValidInputs(inputBoxes)) {
                            if (!keepOthersVisible) {
                                previousOne.hide("slow");
                            }
                            //console.log(inputBoxes);
                            //console.log(binders);
                            if (!nextOne.prop("data-dev-visited")) {
                                nextOne.attr("data-dev-visited", "true");
                                nextOne.show("slow");
                                //console.log(nextOne);
                                order++;
                            }
                        } else {
                            //console.log("no inboxes");
                        }

                    } else {
                        // nothing left.
                        // sttil check the validation.
                        previousOne = slideObjects.filter("[data-dev-slide='" + (order - 1) + "']"); // console.log(previousOne);
                        inputBoxes = previousOne.find("input");
                        if (inputBoxes.length > 0 && $.devOrg.checkValidInputs(inputBoxes)) {
                            if (!dontSubmit) {
                                this.submit();
                            }
                        }
                    }


                });

                // var notVisited = slideObjects.filter("[data-dev-visited='false']");
            }
        },
        // Send inputs array, if any of those false , returns false.
        checkValidInputs: function(jBinders) {
            var $currentInput = null;
            var length = jBinders.length;
            var label = "<label class='label label-danger small-font-size'>Please rate first.</label>";
            if (length > 0) {
                for (var i = 0; i < length; i++) {
                    $currentInput = $(jBinders[i]);

                    if ($currentInput.hasClass("common-rating")) {
                        var $ratingContainer = $currentInput.closest("div.rating-container");
                        var $wholeContainer = $ratingContainer.closest("div.star-rating");

                        if ($currentInput.val() === "0") {
                            $ratingContainer.css({
                                'text-shadow': "2px 2px red"
                            });
                            if (!$wholeContainer.attr("data-warned")) {
                                $wholeContainer.append(label);
                                $wholeContainer.attr("data-warned", "true");
                            }
                            return false;
                        } else {
                            $ratingContainer.css({
                                'text-shadow': "none"
                            });

                            if ($wholeContainer.attr("data-warned")) {
                                $wholeContainer.find("label").remove();
                                $wholeContainer.attr("data-warned", "false");
                            }
                        }
                    }
                    if (!$currentInput.valid()) {
                        return false;
                    }
                }
            }
            return true;
        }
    };
});