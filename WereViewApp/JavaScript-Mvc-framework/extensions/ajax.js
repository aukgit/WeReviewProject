;$.app = $.app || {};
/**
 * Ajax library to make any form submit to ajax call.
 */
;$.app.ajax = {
    attr: {
        successMessage: "message-on-success",
        failMessage: "message-on-fail",
        replaceContainerSelector: "replace-container-selector",
        onlyMakeRequestIf: "only-make-request-if",
        showSpinner: "shown-spinner",
        blockUISelector: "block-UI-selector"
    },
    events: {
        beforeSend: "before-sending-call",
        afterReceive: "after-receive-call",
        onSuccess: "on-success-call",
        onfailed: "on-fail-call",
        always: "always-call",
    }
};