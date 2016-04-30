/// <reference path="../jQuery/jquery-2.1.3.js" />
/// <reference path="../jQuery/jquery-2.1.3.intellisense.js" />
/// <reference path="../jQuery/jquery.number.js" />
/// <reference path="../jQuery/jquery.unobtrusive-ajax.js" />
/// <reference path="../jQuery/jquery.validate-vsdoc.js" />
/// <reference path="../jQuery/jquery.validate.js" />
/// <reference path="../jQuery/jquery.validate.unobtrusive.js" />
/// <reference path="../jQuery/moment.js" />
/// <reference path="../jQuery/underscore.js" />
/// <reference path="../jQuery/validation.js" />
/// <reference path="../jQuery/Upload/jquery.fileupload.js" />
/// <reference path="../Bootstrap/bootstrap.js" />
/// <reference path="../Bootstrap/bootstrap-select.js" />
/// <reference path="../Bootstrap/bootstrap-table-export.js" />
/// <reference path="../Bootstrap/bootstrap-table-filter.js" />
/// <reference path="../Bootstrap/bootstrap-datetimepicker.js" />
/// <reference path="../Bootstrap/bootstrap-datepicker.js" />
/// <reference path="../Bootstrap/common-tasks-run-every-page.js" />
/// <reference path="../Bootstrap/modernizr-2.8.3.js" />
/// <reference path="../Bootstrap/respond.js" />
/// <reference path="../Bootstrap/star-rating.js" />
/*
* Version 1.0
* Written by Alim Ul Karim
* Developers Organism
* https://www.facebook.com/DevelopersOrganism
* mailto:info@developers-organism.com
*/
$.devOrg.dynamicSelect = {
    isDependableAttribute: "data-dependable",
    dependablePropertyNameAttribute: "data-dependable-prop-name",
    propertyNameAttribute: "data-prop",
    dynamicSelectClass: "dynamic-select-load",
    urlAttribute: "data-url",
    dataValueAttribute: "data-value",
    isDynamicSelectElementAttribute: "data-dev-dynamic-select",
    additionalCssAttribute: "data-additional-css",
    liveSearchAttribute: "data-live-search",
    isHtmlAttribute: "dev-isHtml",
    isStyledAttribute: "dev-isStyled",
    $dynamicSelectContainerDiv: $("div.dynamic-select-container[data-dynamic-select-container=true]"),
    $allDynamicImmidiaeSelectDivs: null, // will be defined from initialize function
    $dependancySelectsHasNotProcessed: [], //only populated if a dependency combo can't find parent.

    initialize: function (additionalSelector) {
        /// <summary>
        /// select div and push info based on html properties
        /// class="dynamic-select-load"
        ///      data-prop="NameOfThePropertyInDatabase"
        ///      data-dev-dynamic-select="true"
        ///      data-url="where the json is"
        ///      data-value="value for the select"
        ///      data-dependable-prop-name=""
        ///      data-dependable="true/false"
        ///      data-load-auto="true/false"
        /// </summary>
        "use strict";
        if (additionalSelector === undefined) {
            additionalSelector = "";
        }

        var selector = "div." + $.devOrg.dynamicSelect.dynamicSelectClass + "[" + $.devOrg.dynamicSelect.isDynamicSelectElementAttribute + "=true]" + additionalSelector;
        var $dynamicDiv = $(selector);
        // don't use $.devOrg.dynamicSelect type of caching because it is not updated when items are appened ** alim ul karim
        $.devOrg.dynamicSelect.$allDynamicImmidiaeSelectDivs = $dynamicDiv;
        var length = $dynamicDiv.length;


        for (var i = 0; i < length; i++) {
            var $div = $($dynamicDiv[i]);
            var isDependable = $div.attr($.devOrg.dynamicSelect.isDependableAttribute);

            var url = $div.attr(this.urlAttribute);
            if (!_.isEmpty(url) && isDependable === 'false') {
                $.devOrg.dynamicSelect.getJsonProcessSelectDynamicOptions($div, url);
                // dependency will be handled in side the parent when json is reviced in the parent
            }
        }
    },
    isHtmlRequest: function ($div) {
        return $div.attr($.devOrg.dynamicSelect.isHtmlAttribute);
    },
    fixUrlWithSlash: function (url) {
        /// <summary>
        /// if url doesn't contain slash at end
        /// slash will be added
        /// 
        /// </summary>
        /// <param name="url">site.com/ or site.com will return site.com/</param>
        if (!_.isEmpty(url)) {
            var len = url.length;
            var lastChar = url[len - 1];
            if (lastChar !== "/") {
                url += "/";
            }
        }
        return url;
    },
    filterDependableDivByPropName: function (depenablePropName) {
        var findChildSelector = "[" + $.devOrg.dynamicSelect.dependablePropertyNameAttribute + "=" + depenablePropName + "]";
        return $.devOrg.dynamicSelect.$allDynamicImmidiaeSelectDivs.filter(findChildSelector);
    },
    filterDivByPropName: function (propName) {
        var findChildSelector = "[" + $.devOrg.dynamicSelect.propertyNameAttribute + "=" + propName + "]";
        return $.devOrg.dynamicSelect.$allDynamicImmidiaeSelectDivs.filter(findChildSelector);
    },
    getUrlFromDynamicSelectDiv: function ($div) {
        var url = $div.attr($.devOrg.dynamicSelect.urlAttribute);
        url = $.devOrg.dynamicSelect.fixUrlWithSlash(url);
        return url;
    },
    selectFirstItemInSelectAndGetValue: function ($currentSelect) {
        var parentValue = $currentSelect.val();
        if (parentValue === null) {
            $currentSelect.val($currentSelect.find("option:first").val());
            parentValue = $currentSelect.val();
        }
        return parentValue;
    },
    getJsonProcessSelectDynamicOptions: function ($div, url) {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="$div"></param>
        /// <param name="url">given url to get the json</param>
        "use strict";
        var isHtml = $.devOrg.dynamicSelect.isHtmlRequest($div);
        var requestType = "JSON";
        if (isHtml) {
            requestType = "HTML";
        }

        var value = $div.attr($.devOrg.dynamicSelect.dataValueAttribute);
        var liveSearch = $div.attr($.devOrg.dynamicSelect.liveSearchAttribute);
        var additionCss = $div.attr($.devOrg.dynamicSelect.additionalCssAttribute);
        var propName = $div.attr($.devOrg.dynamicSelect.propertyNameAttribute);
        var addAttr = "data-style='" + additionCss + "'" +
                      "data-live-search='" + liveSearch + "'";
        var selectBoxStart = "<select name='" + propName + "' " + addAttr + " class=' form-control' >";
        var selectBoxEnd = "</select>";
        var selectOfParentDiv = "div.form-row-" + propName + ":first";
        var $containerDiv = $(selectOfParentDiv);
        var isStyled = $div.attr($.devOrg.dynamicSelect.isStyledAttribute);

        $.ajax({
            type: "GET",
            dataType: requestType, //json or html
            url: url,
            success: function (response) {
                //console.log(url + " . Data:");
                //console.log(jsonData);
                $containerDiv.hide();
                if (response.length > 0) {
                    //$div.hide();
                    //successfully got  the json
                    var compactSelectHtml = "";
                    if (isHtml === 'false') {
                        response = JSON.parse(response);
                        //json type
                        var options = new Array(response.length + 5);
                        for (var i = 0; i < response.length; i++) { // build options
                            if (!_.isEmpty(value) && (value === response[i].id || response[i].display === value)) {
                                options[i] = ("<option value='" + response[i].id + "' Selected='selected'>" + response[i].display + "</option>");
                            } else {
                                options[i] = ("<option value='" + response[i].id + "'>" + response[i].display + "</option>");
                            }
                        }
                        compactSelectHtml = selectBoxStart + options.join("") + selectBoxEnd;
                        $div.prepend(compactSelectHtml);
                    } else {
                        // html
                        $div.html(response);
                    }

                    //$div.show("slow");
                    $containerDiv.show('slow');
                    // find any of the dependency if exist
                    var $parentSelect = $div.find("select:first");
                    var isItemsExist = $parentSelect.find("option:first").length === 1;
                    $.devOrg.dynamicSelect.selectFirstItemInSelectAndGetValue($parentSelect);

                    if (isStyled === 'true') {
                        $parentSelect.selectpicker();
                    }

                    var $childDiv = $.devOrg.dynamicSelect.filterDependableDivByPropName(propName);
                    var childUrl = $.devOrg.dynamicSelect.getUrlFromDynamicSelectDiv($childDiv);

                    if ($parentSelect.length === 1 && isItemsExist && $childDiv.length === 1) {
                        $parentSelect.change(function () {
                            var $currentSelect = $(this);
                            var parentValue = $currentSelect.val();
                            var tempUrl = childUrl + parentValue;
                            $childDiv.html("");
                            $.devOrg
                                .dynamicSelect
                                .getJsonProcessSelectDynamicOptions(
                                    $childDiv,
                                    tempUrl);
                        }).trigger('change');
                    }

                }
            },
            error: function (xhr, status, error) {
                console.log("Error: Can't retrieved the data from given url : " + url + ". Error : " + error);

            }
        });
    }

}
