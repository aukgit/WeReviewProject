;
$.byId = function(elementIdString) {
    /// <summary>
    ///     Get your element by id, there is no need to use #.
    ///     However if there is a hash then it will be removed.
    /// </summary>
    /// <param name="findElementById">Your element id, there is no need to use #</param>
    /// <returns>jQuery object , check length property to understand if any exist</returns>
    if (elementIdString !== undefined && elementIdString !== null && elementIdString !== "" && typeof elementIdString === "string") {
        var elementsById;
        if (elementIdString.charAt(0) !== "#") {
            elementsById = document.getElementById(elementIdString);
            return $(elementsById);
        } else {
            var newId = elementIdString.slice(1, elementIdString.length);
            elementsById = document.getElementById(newId);
            return $(elementsById);
        }
    }
    return $(null);
};