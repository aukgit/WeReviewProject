/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../jquery-2.1.3.intellisense.js" />

$.faster = {
    browserName: null,
    browserVersion: 0,
    isBrowsersDetected: false,
    isOpera: null,
    isInternetExplorer: null,
    isSafari: null,
    isChrome: null,
    isFirefox: null,
    getById: function (findElementById) {
        "use strict";
        /// <summary>
        /// Get your element by id, there is no need to use #.
        /// However if there is a hash then it will be removed.
        /// </summary>
        /// <param name="findElementById">Your element id, there is no need to use #</param>
        /// <returns>jQuery object , check length property to understand if any exist</returns>
        if (findElementById !== undefined && findElementById !== null && findElementById !== "") {
            var elementsById;
            if (findElementById.charAt(0) !== "#") {
                elementsById = document.getElementById(findElementById);
                return $(elementsById);
            } else {
                var newId = findElementById.slice(1, findElementById.length);
                elementsById = document.getElementById(newId);
                return $(elementsById);
            }
        }
        return $(null);
    },
    //http://jsperf.com/jquery-vs-queryselectorall-selection
    isQuerySelectorSupported: function () {
        if ($.faster.isChrome && $.faster.browserVersion >= 35) {
            return true;
        } else if ($.faster.isFirefox && $.faster.browserVersion >= 8) {
            return true;
        } else if ($.faster.isInternetExplorer && $.faster.browserVersion >= 10) {
            return true;
        } else if ($.faster.isSafari && $.faster.browserVersion >= 5) {
            return true;
        } else if ($.faster.isOpera && $.faster.browserVersion >= 8) {
            return true;
        }
        return false;
    },
    printBrowser: function () {
        var name = $.faster.browserName + " " + $.faster.browserVersion;
        console.log(name);
        return name;
    },

    setBrowserDetectedFlags: function () {
        $.faster.isBrowsersDetected = true;
        $.faster.isChrome = false;
        $.faster.isOpera = false;
        $.faster.isSafari = false;
        $.faster.isFirefox = false;
        $.faster.isInternetExplorer = false;

        if ($.faster.browserName === "Chrome") {
            $.faster.isChrome = true;
        } else if ($.faster.browserName === "Firefox") {
            $.faster.isFirefox = true;
        } else if ($.faster.browserName === "IE") {
            $.faster.isInternetExplorer = true;
        } else if ($.faster.browserName === "Safari") {
            $.faster.isSafari = true;
        } else if ($.faster.browserName === "Opera") {
            $.faster.isOpera = true;
        }

        //console.log("Chrome : " + $.faster.isChrome);
        //console.log("Firefox : " + $.faster.isFirefox);
        //console.log("IE : " + $.faster.isInternetExplorer);
        //console.log("Safari : " + $.faster.isSafari);
        //console.log("Opera : " + $.faster.isOpera);

    },
    detectBrowser: function () {
        if ($.faster.isBrowsersDetected) {
            return $.faster.printBrowser();
        }
        var ua = navigator.userAgent, tem,
        M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
        if (/trident/i.test(M[1])) {
            tem = /\brv[ :]+(\d+)/g.exec(ua) || [];
            //internet explorer
            $.faster.browserName = "IE";
            $.faster.browserVersion = tem[1] || '';
            //return 'IE ' + (tem[1] || '');

            // all browser flags : isOpera, isChrome..
            $.faster.setBrowserDetectedFlags();
            return $.faster.printBrowser();
        }
        if (M[1] === 'Chrome') {
            tem = ua.match(/\b(OPR|Edge)\/(\d+)/);
            if (tem != null) {
                //opera
                // slice example 
                /**
                 * var fruits = ['Banana', 'Orange', 'Lemon', 'Apple', 'Mango'];
                 * var citrus = fruits.slice(1, 3);
                 * citrus contains ['Orange','Lemon']
                 */
                var simpleBrowserName = tem.slice(1); // get browser name [0] = browser and [1] = version
                $.faster.browserName = simpleBrowserName[0].replace('OPR', 'Opera');
                $.faster.browserVersion = simpleBrowserName[1];
                // all browser flags : isOpera, isChrome..
                $.faster.setBrowserDetectedFlags();

                return $.faster.printBrowser();
            }
        }
        M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
        if ((tem = ua.match(/version\/(\d+)/i)) != null) M.splice(1, 1, tem[1]);
        var simpleBrowserName2 = M.join(' ').split(" ");
        $.faster.browserName = simpleBrowserName2[0];
        $.faster.browserVersion = simpleBrowserName2[1];
        // all browser flags : isOpera, isChrome..
        $.faster.setBrowserDetectedFlags();
        return $.faster.printBrowser();

    },
    queryAll: function (cssSelector) {
        "use strict";
        /// <summary>
        /// Find all elements and returns as jQuery element and
        /// carefully check which browser it supports and then 
        /// decide which one to choose.
        /// jQuery vs document.querySelector, querySelector is much more faster than jQuery selector. 
        /// In chrome 35 above has better performance so it checks
        /// https://jsperf.com/jquery-vs-document-queryselector
        /// </summary>
        /// <param name="cssSelector">Find all elements</param>
        /// <returns>jQuery object , check length property to understand if any exist</returns>
        if (cssSelector !== undefined && cssSelector !== null && cssSelector !== "") {
            if ($.faster.isQuerySelectorSupported) {
                var elements;
                elements = document.querySelectorAll(cssSelector);
                return $(elements);
            } else {
                return $(cssSelector);
            }
        }
        return $(null);
    },
    queryFirst: function (cssSelector) {
        "use strict";
        /// <summary>
        /// Find only first element and returns as jQuery element,
        /// it will automatically add :first with your selector
        /// carefully check which browser it supports and then 
        /// decide which one to choose.
        /// jQuery vs document.querySelector, querySelector is much more faster than jQuery selector. 
        /// In chrome 35 above has better performance than jQuery so it checks and usages based on that.
        /// https://jsperf.com/jquery-vs-document-queryselector
        /// </summary>
        /// <param name="cssSelector">Find first element and add ":first" with your selector</param>
        /// <returns>jQuery object , check length property to understand if any exist</returns>
        if (cssSelector !== undefined && cssSelector !== null && cssSelector !== "") {
            if ($.faster.isQuerySelectorSupported) {
                var elements;
                elements = document.querySelector(cssSelector);
                return $(elements);
            } else {
                return $(cssSelector + ":first");
            }
        }
        return $(null);
    }

}
$.faster.detectBrowser();


$.queryAll = function (cssSelector) {
    /// <summary>
    /// Find all elements and returns as jQuery element,
    /// Carefully check which browser it supports and then 
    /// decide which one to choose.
    /// document.querySelectorAll is much more faster than jQuery selector 
    /// In chrome 35 above has better performance so it checks
    /// </summary>
    /// <param name="cssSelector">Find all elements</param>
    /// <returns>jQuery object , check length property to understand if any exist</returns>
    return $.faster.queryAll(cssSelector);
}

$.queryFirst = function (cssSelector) {
    /// <summary>
    /// Find only first element and returns as jQuery element
    /// it will automatically add :first with your selector
    /// Carefully check which browser it supports and then 
    /// decide which one to choose.
    /// document.querySelector is much more faster than jQuery selector 
    /// In chrome 35 above has better performance so it checks
    /// https://jsperf.com/jquery-vs-document-queryselector
    /// </summary>
    /// <param name="cssSelector">Find first element and add ":first" with your selector</param>
    /// <returns>jQuery object , check length property to understand if any exist</returns>
    return $.faster.queryFirst(cssSelector);
}
$.byId = function (findElementById) {
    /// <summary>
    /// Get your element by id, there is no need to use #.
    /// However if there is a hash then it will be removed.
    /// </summary>
    /// <param name="findElementById">Your element id, there is no need to use #</param>
    /// <returns>jQuery object , check length property to understand if any exist</returns>
    if (findElementById !== undefined && findElementById !== null && findElementById !== "") {
        var elementsById;
        if (findElementById.charAt(0) !== "#") {
            elementsById = document.getElementById(findElementById);
            return $(elementsById);
        } else {
            var newId = findElementById.slice(1, findElementById.length);
            elementsById = document.getElementById(newId);
            return $(elementsById);
        }
    }
    return $(null);
}
