; $.app = $.app || {};
$.app.initializeMethods = {
    /**
     * runs all the methods after initialize method.
     */
    initialize: function () {
        var self = $.app.initializeMethods;

        var keys = Object.keys(self);
        for (var i = 0; i < keys.length; i++) {
            var key = keys[i];
            if (key !== "initialize") {
                // execute all other than "initialize" method
                var functionsOrMethods = self[key];
                $.executeFunction(functionsOrMethods);
            }
        }
    },
    toolTipShow : function () {
        var $tooltipItems = $('.tooltip-show');
        if ($tooltipItems.length > 0) {
            $tooltipItems.tooltip({ container: 'body' });
        }

    },
    seoHide : function () {
        var $seoHideItems = $(".seo-hide");
        if ($seoHideItems.length > 0) {
            $seoHideItems.hide();
        }
    },
    transactionStatusEnable : function () {
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
    },
    loadWow: function () {
        var options = {
            scaleColor: false,
            trackColor: 'rgba(266,144,0,0.0)',
            barColor: '#ff7200',
            lineWidth: 2,
            lineCap: 'butt',
            size: 253
        };
        $().jetmenu();
        window.addEventListener('DOMContentLoaded', function () {
            var charts = [];
            [].forEach.call(document.querySelectorAll('.chart'), function (el) {
                charts.push(new EasyPieChart(el, options));
            });
        });

        var wow = new WOW({
            boxClass: 'wow',      // animated element css class (default is wow)
            animateClass: 'animated', // animation css class (default is animated)
            offset: 100,          // distance to the element when triggering the animation (default is 0)
            mobile: false        // trigger animations on mobile devices (true is default)
        });
        wow.init();
    }
};