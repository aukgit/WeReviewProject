/// <reference path="../jquery-2.1.3.js" />
/// <reference path="../jquery-2.1.3.intellisense.js" />

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
