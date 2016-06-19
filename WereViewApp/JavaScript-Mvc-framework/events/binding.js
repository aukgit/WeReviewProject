; $.app = $.app || {};
; $.app.events = $.app.events || {};
/**
 * Use "data-event-binding" attribute with js controller to execute specific events or write using csv or use * for finding all events if exist.
 */
; $.app.events.binding = {
    executeAll: function($controller) {
        var list = $.app.events.list, i,
            event;
        for (i = 0; i < list.length; i++) {
            event = list[i];
            $.executeFunctionWithArguments(event, [$controller]);
        }
    },
    execute: function ($controller, csvEvents) {
        var events = csvEvents.split(","),
            i,
            list = $.app.events.list,
            eventName,
            event;
        for (i = 0; i < events.length; i++) {
            eventName = events[i];
            event = list[eventName];
            $.executeFunctionWithArguments(event, [$controller]);
        }
    }
}