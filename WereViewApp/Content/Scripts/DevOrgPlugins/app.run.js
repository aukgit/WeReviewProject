/*
 * Written by Alim Ul Karim
 * Developers Organism
 * https://www.facebook.com/DevelopersOrganism
 * mailto:info@developers-organism.com
*/



$.app = $.app || {};

$.app.run = function () {

    this.initialize = function () {
        //var keys = Object.keys(this);
        //console.log(keys);
        //console.log(this.constructor);
        //console.log(this);
        //console.log(this.name);
        //this.toolTipShow();
        //this.seoHide();
        //this.transactionStatusEnable();
        var keys = Object.keys(this);
        //console.log(keys);
        for (var i = 0; i < keys.length; i++) {
            var methodName = keys[i],
                method = this[methodName];
            //console.log(methodName);
            if ((methodName === 'initialize' || methodName === 'run') === false && typeof method === "function") {
                // execute all other functions.
                method.apply();
            }
        }
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

    this.transactionStatusEnable = function () {
        var $transaction = $.byId("transaction-container"),
            hideTimeOut = parseInt($transaction.attr("data-hide-duration"));

        var hideStatus = function () {
            $transaction.attr("data-shown", "true");
            $transaction.hide(500);
        };
        var timer = setTimeout(hideStatus, hideTimeOut);

        var stopTimer = function () {
            clearTimeout(timer);
        }

        $transaction.click(function () {
            stopTimer();
            hideStatus();
        });
    }

    this.bootstrapTableEnable = function() {
        // make tables look nice with pagination
        var $tables = $(".bootstrap-table-do");
        if ($tables.length > 0) {
            $tables.bootstrapTable();
        }
    }
    this.initialize();
}

$(function () {
    $.app.run();
});
