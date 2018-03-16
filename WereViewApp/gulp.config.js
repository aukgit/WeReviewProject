module.exports = function () {
    var config = {
        htmlSource: [
            "/View/Home/Index.cshtml" /* The HTML file */
        ],
        js: [
            ".Content/script/published/front-end.js",
            ".Content/script/published/jqueryval.js",
            ".Content/script/published/upload.js"
            /* List of js files in the order as they appear in index.html*/
        ],
        minJs: "js-min.js", /* Minified JS file name */
        minJsDestination: ".Content/published/Scripts/", /* Minified JS file destination */
        css: [
            ".Content/styles/*.css",
            ".Content/styles/css/*css",
            ".Content/styles/css/Upload/*.css",
            ".Content/styles/css/Upload/Tagit/*.css"/* List of css files that need to be minified */
        ],
        minCss: "css-min.css", /* Minified CSS file name */
        minCssDestination: ".Content/published/Styles/" /* Minified CSS file destination */
    }
    return config;
}