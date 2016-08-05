$.app.service.redirect = {
    
    toLogin : function() {
        var loginUrl = $.app.urls.getGeneralUrlSchema(false, ["login"]);
        console.log(loginUrl);
    }
};