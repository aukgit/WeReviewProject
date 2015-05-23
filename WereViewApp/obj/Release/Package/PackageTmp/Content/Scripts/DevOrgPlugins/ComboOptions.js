/*
 * Written by Alim Ul Karim
 * Developers Organism
 * https://www.facebook.com/DevelopersOrganism
 * mailto:info@developers-organism.com
*/
function FillComboDepenedOnOther(dependingSelectBoxID, fillSelectBoxID, locationToRetrive, optionalSelectedIdorName) {
    $(dependingSelectBoxID).bind("change", function () {
        // every time the value of the dropdown 1 is changed, do this:
        // console.log('ase' + dependingSelectBoxID);
        var paramId = $(dependingSelectBoxID).val();

        $.getJSON(locationToRetrive + '/' + paramId, function (data) {
            $(fillSelectBoxID).empty(); // clear old values if exist

            var options = "";
            for (i = 0; i < data.length; i++) { // build options
                if ((optionalSelectedIdorName == data[i].id || data[i].text == optionalSelectedIdorName) && (optionalSelectedIdorName != null || optionalSelectedIdorName != "")) {
                    options += ("<option value='" + data[i].id + "' Selected='selected'>" + data[i].text + "</option>");
                } else {
                    options += ("<option value='" + data[i].id + "'>" + data[i].text + "</option>");
                }
            }
            $(fillSelectBoxID).append(options);
        });
    });
}


function FillComboAjaxBased(SelectBoxID, locationToRetrive, optionalSelectedIdorName) {
    $.getJSON(locationToRetrive + '/', function (data) {
        $(SelectBoxID).empty(); // clear old values if exist
        var options = "";
        for (i = 0; i < data.length; i++) { // build options
            if ((optionalSelectedIdorName == data[i].id || data[i].text == optionalSelectedIdorName) && (optionalSelectedIdorName != null || optionalSelectedIdorName != "")) {
                options += ("<option value='" + data[i].id + "' Selected='selected'>" + data[i].text + "</option>");
            } else {
                options += ("<option value='" + data[i].id + "'>" + data[i].text + "</option>");
            }
        }
        $(SelectBoxID).append(options);
    });
}


/*
Check me (checked)
.attr('checked'): checked
.prop('checked'): true
.is(':checked'): true

Check me (not checked)
.attr('checked'): undefined
.prop('checked'): false
.is(':checked'): false

<button id="theBtn" value="taDa" disabled="disabled">Hi There</button>
We can use 
Copy code
$('#theBtn').attr('disabled')    // to return the initial value of this attribute as a string "disabled"
// and
$('#theBtn').prop('disabled')    // to return the current state of this property (true or false)
$('#theBtn').prop('disabled',true)    // to change the property - using a correct data-type
*/
function AddPropertyValue(SelectBoxID, locationToRetrive, propertyName) {
    $.getJSON(locationToRetrive + '/' + SelectBoxID.value, function (data) {
        $(SelectBoxID).prop(propertyName, data);
    });
}

function ShowInformationBaseOnComboInAnotherBoxBind(SelectBoxID, locationToRetrive, BoxID) {
    $(SelectBoxID).change(function () {
        $.getJSON(locationToRetrive + '/' + this.value, function (data) {
            $(BoxID).text(data);
        });
    }).triggerHandler('change');
}

function ShowInformationBaseOnComboInAnotherBox(SelectBoxID, locationToRetrive, BoxID) {
    $.getJSON(locationToRetrive + '/' + SelectBoxID.value, function (data) {
        $(BoxID).text(data);
    }).triggerHandler('change');

}

///OptionalInSameLayer means in the same block displayed.
function FillHtmlBasedOnDependableCombo(DependableCombo, locationToRetriveJsonAsValue, DefComboValueAtFirst ,BoxToPutData, AddClassToElements, AddBeginingHTML, AddClosingTagHTML, OptionalInSameLayer, OptionalItemsSeperator, NoRecordsFoundMsg, HTMLRowAttr) {
    $(DependableCombo).change(function () {
        var valueToLoad = -1;
        if (this.value == null || this.value == undefined || this.value == "" || this.value == " ") {
            valueToLoad = DefComboValueAtFirst;
        } else {
            valueToLoad = this.value;
        }
        //console.log(valueToLoad);

        $.getJSON(locationToRetriveJsonAsValue + '/' + valueToLoad, function (data) {
            $(BoxToPutData).empty(); // clear old values if exist
            var SameLayer = false;

            if (OptionalInSameLayer != null && OptionalInSameLayer != undefined && OptionalInSameLayer == true) {
                SameLayer = true;
            }

            if (data == null || data == undefined || data.length == null) {
                if (HTMLRowAttr != null && HTMLRowAttr != undefined) {
                    generated += "<" + HTMLRowAttr + ">";
                }
                if (NoRecordsFoundMsg == null || NoRecordsFoundMsg == undefined) {
                    NoRecordsFoundMsg = "Sorry , records can't be display because of network problem or no records found."
                }
                generated += AddBeginingHTML;
                generated += "<span class = '" + AddClassToElements + " bold red' >" + NoRecordsFoundMsg + "</span>";
                generated += AddClosingTagHTML;
                if (HTMLRowAttr != null && HTMLRowAttr != undefined) {
                    generated += "</" + HTMLRowAttr + ">";
                }
                $(BoxToPutData).html(generated);
                return;
            }

            if (AddClassToElements == null || AddClassToElements == undefined) {
                AddClassToElements = "Dependable-Generation"
            }
            var generated = "";
            for (i = 0; i < data.length; i++) { // build options
                if (HTMLRowAttr != null && HTMLRowAttr != undefined) {
                    generated += "<" + HTMLRowAttr + ">";
                }
                if (AddBeginingHTML != null && AddBeginingHTML != undefined && AddClosingTagHTML != null && AddClosingTagHTML != undefined) {
                    generated += AddBeginingHTML;
                    generated += "<span class = '" + AddClassToElements + "' >" + data[i].textue + "</span>";
                    if (SameLayer) {
                        generated += OptionalItemsSeperator;
                        generated += "<span class = '" + AddClassToElements + "-Optional Optional-Case' >" + data[i].optional + "</span>";
                    }
                    generated += AddClosingTagHTML;

                    if ((data[i].optional != null && data[i].optional != undefined) && SameLayer == false) {
                        if (OptionalItemsSeperator != null && OptionalItemsSeperator != undefined) {
                            generated += AddBeginingHTML;
                            generated += OptionalItemsSeperator;
                            generated += AddClosingTagHTML;
                        }
                        generated += AddBeginingHTML;
                        generated += "<span class = '" + AddClassToElements + "-Optional' >" + data[i].optional + "</span>";
                        generated += AddClosingTagHTML;
                    }
                } else {
                    generated += "<span class = '" + AddClassToElements + "' >" + data[i].textue + "</span>";
                    if (SameLayer) {
                        generated += OptionalItemsSeperator;
                        generated += "<span class = '" + AddClassToElements + "-Optional Optional-Case' >" + data[i].optional + "</span>";
                    }
                    if ((data[i].optional != null && data[i].optional != undefined) && SameLayer == false) {
                        if (OptionalItemsSeperator != null && OptionalItemsSeperator != undefined) {
                            generated += AddBeginingHTML;
                            generated += OptionalItemsSeperator;
                            generated += AddClosingTagHTML;
                        }
                        generated += "<span class = '" + AddClassToElements + "-Optional' >" + data[i].optional + "</span>";
                    }
                }

                if (HTMLRowAttr != null && HTMLRowAttr != undefined) {
                    generated += "</" + HTMLRowAttr + ">";
                }
            }
            $(BoxToPutData).html(generated);

        });
    }).triggerHandler('change');
}


//example
function LoadCountryAndDistrict(DefCountry, DefDistrict) {
    var countryId = "#CountryID";
    var districtId = "#CountryDivisionID";
    var getCountries = "/Country/GetCountries/0";
    var getDistrics = "/Country/GetDistricts";

    if ($(countryId).length > 0) {
        FillComboAjaxBased(countryId, getCountries, DefCountry);
    }

    if ($(districtId).length > 0) {
        //get the default country districts
        FillComboAjaxBased(districtId, getDistrics + "/" + DefCountry, DefDistrict);
        //bind it to the change event.
        FillComboDepenedOnOther(countryId, districtId, getDistrics, DefDistrict);
    }
}