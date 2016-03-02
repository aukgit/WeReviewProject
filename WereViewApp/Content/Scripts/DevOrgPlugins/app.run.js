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

    this.init = function () {
        this.toolTipShow();
        this.seoHide();
        this.transactionStatusHide();
    }
    this.toolTipShow = function () {
        var $tooltipItems = $('.tooltip-show');
        if ($tooltipItems.length > 0) {
            $tooltipItems.tooltip({ container: 'body' });
        }

    }
    this.seoHide = function () {
        var $seoHideItems = $(".seo-hide");
        if ($seoHideItems.length > 0) {
            $seoHideItems.hide();
        }
    }

    this.transactionStatusHide = function () {
        var $transaction = $(".transaction-status");

        setTimeout(function () {
            $transaction.attr("data-shown", "true");
            $transaction.hide();
        }, 3500);
    }
    this.init();
}

$(function () {
    $.app.run();
});
