$.app.service.user = {
    isLoggedIn: function () {
        $.app.service.redirect.toLogin();
        return $.getHiddenValue("is-logged") === "True";
    },
    
};