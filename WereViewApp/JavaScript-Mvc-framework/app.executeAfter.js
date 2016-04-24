/// <reference path="byId.js" />
/// <reference path="D:\Working (SSD)\GitHub\WereViewProject\WereViewApp\Content/Scripts/toastr.js" />
/// <reference path="D:\Working (SSD)\GitHub\WereViewProject\WereViewApp\Content/Scripts/jquery-2.1.4.js" />
/// <reference path="D:\Working (SSD)\GitHub\WereViewProject\WereViewApp\Content/Scripts/jquery.blockUI.js" />
/// <reference path="D:\Working (SSD)\GitHub\WereViewProject\WereViewApp\Content/Scripts/jquery-2.1.4.intellisense.js" />
/// <reference path="app.global.js" />
; $.app = $.app || {};
/**
 * method which runs after all the initialization is done.
 */
$.app.executeAfter = {
    /**
     * method which runs after all the initialization is done.
     * @returns {} 
     */
    documentSpinnerHide : function() {
        $.app.global.documentFullSpinnerHide();
    }
};