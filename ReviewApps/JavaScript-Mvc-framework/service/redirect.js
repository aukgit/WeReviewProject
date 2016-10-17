$.app.service.redirect = {

    toLogin: function () {
        var loginUrl = $.app.urls.getGeneralUrlSchema(false, ["login"]).login;
        //console.log(loginUrl);
        $.app.service.redirect.to(loginUrl);
    },

    to: function (url) {
        /// <summary>
        /// url to a location.
        /// </summary>
        /// <param name="url" type="type"></param>
        if (url[0] === "/") {
            // relative path
            var host = window.location.hostname,
                protocol = window.location.protocol,
                port = window.location.port,
                path = url;
            if (!$.isEmpty(port)) {
                port = ":" + port;
            }
            url = protocol + "//" + host + port + path;
            //console.log(url);
            window.location = url;
        } else {
            // absolute path.
            window.location = url;
        }
    }
};