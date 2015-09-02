/*
 * Written by Alim Ul Karim
 * Developers Organism
 * https://www.facebook.com/DevelopersOrganism
 * mailto:info@developers-organism.com
*/


var transactionStatusHide = function () {
    var $transactionStatus = $(".transaction-status");
    if ($transactionStatus.length > 0) {
        $transactionStatus.delay(1500).fadeOut(2500);
    }
}

$.app = $.app || {};

$.app.run = function () {

    this.init = function() {
        this.general();
        this.serverValidationActivate();
    }
    this.general = function() {
        var $tooltipItems = $('.tooltip-show');
        if ($tooltipItems.length > 0) {
            $tooltipItems.tooltip({ container: 'body' });
        }
        var $seoHideItems = $(".seo-hide");
        if ($seoHideItems.length > 0) {
            $seoHideItems.hide();
        }
    }

    this.serverValidationActivate = function() {
        var $processForm = $.byId("server-validation-form");
        $processForm.serverValidate({
            crossDomain: false,
            multipleRequests: true,
            checkValidationBeforeSendingRequest: true,
            dontSendSameRequestTwice: false,
            disableInputOnValidation: false,
            focusPersistIfNotValid: false,
            hideOnValidation: false,
            messages: {
                requesting: "Requesting data..."
            },
            selectors: {
                divContainer: ".form-row",
                validatorContainer: ".validator-container",
                validator: ".validator",
                additionalFields: [
                    "[name=__RequestVerificationToken]"
                ]
            },
            attributes: {
                url: "data-url",
                isValidate: "data-is-validate",
                submitMethod: "data-submit-method"
            },
            icons: {
                // if user wanted to change the icon
                invalid: "validation-icon-invalid fa fa-times",
                valid: "validation-icon-valid fa fa-check",
                spinner: "validation-icon-spinner fa fa-refresh fa-spin-custom",
                error: "validation-icon-error fa fa-exclamation-circle"
            },
            iconsIdPrefixes: {
                invalid: "invalid-mark-",
                valid: "valid-mark-",
                spinner: "validation-spinner-",
                error: "validation-error-"
            }
        });
    }
    this.init();
}

$(function () {
    //var devBackBtns = $("a.dev-btn-back");
    //if (devBackBtns.length > 0) {
    //    devBackBtns.click(function (e) {
    //        e.preventDefault();
    //        history.back();
    //    });
    //} 

    $.app.run.init();
});
