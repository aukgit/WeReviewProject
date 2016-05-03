/* Superslides - v0.6.2 - 2013-07-10
* https://github.com/nicinabox/superslides
* Copyright (c) 2013 Nic Aitch; Licensed MIT */
//(function (i, t) { var n, e = "superslides"; n = function (n, e) { this.options = t.extend({ play: !1, animation_speed: 600, animation_easing: "swing", animation: "slide", inherit_width_from: i, inherit_height_from: i, pagination: !0, hashchange: !1, scrollable: !0, elements: { preserve: ".preserve", nav: ".slides-navigation", container: ".slides-container", pagination: ".slides-pagination" } }, e); var s = this, o = t("<div>", { "class": "slides-control" }), a = 1; this.$el = t(n), this.$container = this.$el.find(this.options.elements.container); var r = function () { return a = s._findMultiplier(), s.$el.on("click", s.options.elements.nav + " a", function (i) { i.preventDefault(), s.stop(), t(this).hasClass("next") ? s.animate("next", function () { s.start() }) : s.animate("prev", function () { s.start() }) }), t(document).on("keyup", function (i) { 37 === i.keyCode && s.animate("prev"), 39 === i.keyCode && s.animate("next") }), t(i).on("resize", function () { setTimeout(function () { var i = s.$container.children(); s.width = s._findWidth(), s.height = s._findHeight(), i.css({ width: s.width, left: s.width }), s.css.containers(), s.css.images() }, 10) }), t(i).on("hashchange", function () { var i, t = s._parseHash(); i = t && !isNaN(t) ? s._upcomingSlide(t - 1) : s._upcomingSlide(t), i >= 0 && i !== s.current && s.animate(i) }), s.pagination._events(), s.start(), s }, h = { containers: function () { s.init ? (s.$el.css({ height: s.height }), s.$control.css({ width: s.width * a, left: -s.width }), s.$container.css({})) : (t("body").css({ margin: 0 }), s.$el.css({ position: "relative", overflow: "hidden", width: "100%", height: s.height }), s.$control.css({ position: "relative", transform: "translate3d(0)", height: "100%", width: s.width * a, left: -s.width }), s.$container.css({ display: "none", margin: "0", padding: "0", listStyle: "none", position: "relative", height: "100%" })), 1 === s.size() && s.$el.find(s.options.elements.nav).hide() }, images: function () { var i = s.$container.find("img").not(s.options.elements.preserve); i.removeAttr("width").removeAttr("height").css({ "-webkit-backface-visibility": "hidden", "-ms-interpolation-mode": "bicubic", position: "absolute", left: "0", top: "0", "z-index": "-1", "max-width": "none" }), i.each(function () { var i = s.image._aspectRatio(this), n = this; if (t.data(this, "processed")) s.image._scale(n, i), s.image._center(n, i); else { var e = new Image; e.onload = function () { s.image._scale(n, i), s.image._center(n, i), t.data(n, "processed", !0) }, e.src = this.src } }) }, children: function () { var i = s.$container.children(); i.is("img") && (i.each(function () { if (t(this).is("img")) { t(this).wrap("<div>"); var i = t(this).attr("id"); t(this).removeAttr("id"), t(this).parent().attr("id", i) } }), i = s.$container.children()), s.init || i.css({ display: "none", left: 2 * s.width }), i.css({ position: "absolute", overflow: "hidden", height: "100%", width: s.width, top: 0, zIndex: 0 }) } }, c = { slide: function (i, t) { var n = s.$container.children(), e = n.eq(i.upcoming_slide); e.css({ left: i.upcoming_position, display: "block" }), s.$control.animate({ left: i.offset }, s.options.animation_speed, s.options.animation_easing, function () { s.size() > 1 && (s.$control.css({ left: -s.width }), n.eq(i.upcoming_slide).css({ left: s.width, zIndex: 2 }), i.outgoing_slide >= 0 && n.eq(i.outgoing_slide).css({ left: s.width, display: "none", zIndex: 0 })), t() }) }, fade: function (i, t) { var n = this, e = n.$container.children(), s = e.eq(i.outgoing_slide), o = e.eq(i.upcoming_slide); o.css({ left: this.width, opacity: 1, display: "block" }), i.outgoing_slide >= 0 ? s.animate({ opacity: 0 }, n.options.animation_speed, n.options.animation_easing, function () { n.size() > 1 && (e.eq(i.upcoming_slide).css({ zIndex: 2 }), i.outgoing_slide >= 0 && e.eq(i.outgoing_slide).css({ opacity: 1, display: "none", zIndex: 0 })), t() }) : (o.css({ zIndex: 2 }), t()) } }; c = t.extend(c, t.fn.superslides.fx); var d = { _centerY: function (i) { var n = t(i); n.css({ top: (s.height - n.height()) / 2 }) }, _centerX: function (i) { var n = t(i); n.css({ left: (s.width - n.width()) / 2 }) }, _center: function (i) { s.image._centerX(i), s.image._centerY(i) }, _aspectRatio: function (i) { if (!i.naturalHeight && !i.naturalWidth) { var t = new Image; t.src = i.src, i.naturalHeight = t.height, i.naturalWidth = t.width } return i.naturalHeight / i.naturalWidth }, _scale: function (i, n) { n = n || s.image._aspectRatio(i); var e = s.height / s.width, o = t(i); e > n ? o.css({ height: s.height, width: s.height / n }) : o.css({ height: s.width * n, width: s.width }) } }, l = { _setCurrent: function (i) { if (s.$pagination) { var t = s.$pagination.children(); t.removeClass("current"), t.eq(i).addClass("current") } }, _addItem: function (i) { var n = i + 1, e = n, o = s.$container.children().eq(i), a = o.attr("id"); a && (e = a); var r = t("<a>", { href: "#" + e, text: e }); r.appendTo(s.$pagination) }, _setup: function () { if (s.options.pagination && 1 !== s.size()) { var i = t("<nav>", { "class": s.options.elements.pagination.replace(/^\./, "") }); s.$pagination = i.appendTo(s.$el); for (var n = 0; s.size() > n; n++) s.pagination._addItem(n) } }, _events: function () { s.$el.on("click", s.options.elements.pagination + " a", function (i) { i.preventDefault(); var t = s._parseHash(this.hash), n = s._upcomingSlide(t - 1); n !== s.current && s.animate(n, function () { s.start() }) }) } }; return this.css = h, this.image = d, this.pagination = l, this.fx = c, this.animation = this.fx[this.options.animation], this.$control = this.$container.wrap(o).parent(".slides-control"), s._findPositions(), s.width = s._findWidth(), s.height = s._findHeight(), this.css.children(), this.css.containers(), this.css.images(), this.pagination._setup(), r() }, n.prototype = { _findWidth: function () { return t(this.options.inherit_width_from).width() }, _findHeight: function () { return t(this.options.inherit_height_from).height() }, _findMultiplier: function () { return 1 === this.size() ? 1 : 3 }, _upcomingSlide: function (i) { if (/next/.test(i)) return this._nextInDom(); if (/prev/.test(i)) return this._prevInDom(); if (/\d/.test(i)) return +i; if (i && /\w/.test(i)) { var t = this._findSlideById(i); return t >= 0 ? t : 0 } return 0 }, _findSlideById: function (i) { return this.$container.find("#" + i).index() }, _findPositions: function (i, t) { t = t || this, void 0 === i && (i = -1), t.current = i, t.next = t._nextInDom(), t.prev = t._prevInDom() }, _nextInDom: function () { var i = this.current + 1; return i === this.size() && (i = 0), i }, _prevInDom: function () { var i = this.current - 1; return 0 > i && (i = this.size() - 1), i }, _parseHash: function (t) { return t = t || i.location.hash, t = t.replace(/^#/, ""), t && !isNaN(+t) && (t = +t), t }, size: function () { return this.$container.children().length }, destroy: function () { return this.$el.removeData() }, update: function () { this.css.children(), this.css.containers(), this.css.images(), this.pagination._addItem(this.size()), this._findPositions(this.current), this.$el.trigger("updated.slides") }, stop: function () { clearInterval(this.play_id), delete this.play_id, this.$el.trigger("stopped.slides") }, start: function () { var n = this; n.options.hashchange ? t(i).trigger("hashchange") : this.animate(), this.options.play && (this.play_id && this.stop(), this.play_id = setInterval(function () { n.animate() }, this.options.play)), this.$el.trigger("started.slides") }, animate: function (t, n) { var e = this, s = {}; if (!(this.animating || (this.animating = !0, void 0 === t && (t = "next"), s.upcoming_slide = this._upcomingSlide(t), s.upcoming_slide >= this.size()))) { if (s.outgoing_slide = this.current, s.upcoming_position = 2 * this.width, s.offset = -s.upcoming_position, ("prev" === t || s.outgoing_slide > t) && (s.upcoming_position = 0, s.offset = 0), e.size() > 1 && e.pagination._setCurrent(s.upcoming_slide), e.options.hashchange) { var o = s.upcoming_slide + 1, a = e.$container.children(":eq(" + s.upcoming_slide + ")").attr("id"); i.location.hash = a ? a : o } e.$el.trigger("animating.slides", [s]), e.animation(s, function () { e._findPositions(s.upcoming_slide, e), "function" == typeof n && n(), e.animating = !1, e.$el.trigger("animated.slides"), e.init || (e.$el.trigger("init.slides"), e.init = !0, e.$container.fadeIn("fast")) }) } } }, t.fn[e] = function (i, s) { var o = []; return this.each(function () { var a, r, h; return a = t(this), r = a.data(e), h = "object" == typeof i && i, r || (o = a.data(e, r = new n(this, h))), "string" == typeof i && (o = r[i], "function" == typeof o) ? o = o.call(r, s) : void 0 }), o }, t.fn[e].fx = {} })(this, jQuery);



(function ($) {
    jQuery.fn.jetmenu = function (options) {
        var settings = {
            indicator: true     			// indicator that indicates a submenu
        , speed: 100     			// submenu speed
        , delay: 0					// submenu show delay
        , hideClickOut: true     			// hide submenus when click outside menu
        , align: "left"				// menu alignment (left/right)
        , submenuTrigger: "hover"			// defines if submenu appears after hover/click
        }
        $.extend(settings, options);
        var menu = $(".jetmenu");
        var lastScreenWidth = windowWidth();
        var bigScreen = false;
        $(menu).prepend("<li class='showhide'><span class='title'></span><i class='fa fa-bars'></i></li>");
        if (settings.indicator == true) {
            $(menu).find("a").each(function () {
                if ($(this).siblings(".dropdown, .megamenu").length > 0) {
                    $(this).append("<span class='indicator'><i class='fa fa-angle-right'></i></span>");
                }
            });
        }

        function bindHover() {
            if (navigator.userAgent.match(/Mobi/i) || window.navigator.msMaxTouchPoints > 0 || settings.submenuTrigger == "click") {
                $(menu).find("a").on("click touchstart", function (e) {
                    e.stopPropagation();
                    e.preventDefault();
                    $(this).parent("li").siblings("li").find(".dropdown, .megamenu").stop(true, true).fadeOut(settings.speed);
                    if ($(this).siblings(".dropdown, .megamenu").css("display") == "none") {
                        $(this).siblings(".dropdown, .megamenu").stop(true, true).delay(settings.delay).fadeIn(settings.speed);
                        return false;
                    }
                    else {
                        $(this).siblings(".dropdown, .megamenu").stop(true, true).fadeOut(settings.speed);
                        $(this).siblings(".dropdown").find(".dropdown").stop(true, true).fadeOut(settings.speed);
                    }
                    window.location.href = $(this).attr("href");
                });

                $(menu).find("li").bind("mouseleave", function () {
                    $(this).children(".dropdown, .megamenu").stop(true, true).fadeOut(settings.speed);
                });

                if (settings.hideClickOut == true) {
                    $(document).bind("click.menu touchstart.menu", function (ev) {
                        if ($(ev.target).closest(menu).length == 0) {
                            $(menu).find(".dropdown, .megamenu").fadeOut(settings.speed);
                        }
                    });
                }
            }
            else {
                $(menu).find("li").bind("mouseenter", function () {
                    $(this).children(".dropdown, .megamenu").stop(true, true).delay(settings.delay).fadeIn(settings.speed);
                }).bind("mouseleave", function () {
                    $(this).children(".dropdown, .megamenu").stop(true, true).fadeOut(settings.speed);
                });
            }
        }

        function bindClick() {
            $(menu).find("li:not(.showhide)").each(function () {
                if ($(this).children(".dropdown, .megamenu").length > 0) {
                    $(this).children("a").bind("click", function (e) {
                        if ($(this).siblings(".dropdown, .megamenu").css("display") == "none") {
                            $(this).siblings(".dropdown, .megamenu").delay(settings.delay).slideDown(settings.speed).focus();
                            $(this).parent("li").siblings("li").find(".dropdown, .megamenu").slideUp(settings.speed);
                            return false;
                        }
                        else {
                            $(this).siblings(".dropdown, .megamenu").slideUp(settings.speed).focus();
                            firstItemClick = 1;
                        }
                    });
                }
            });
        }

        function showCollapse() {
            $(menu).children("li:not(.showhide)").hide(0);
            $(menu).children("li.showhide").show(0);
            $(menu).children("li.showhide").bind("click", function () {
                if ($(menu).children("li").is(":hidden")) {
                    $(menu).children("li").slideDown(settings.speed);
                }
                else {
                    $(menu).children("li:not(.showhide)").slideUp(settings.speed);
                    $(menu).children("li.showhide").show(0);
                    $(menu).find(".dropdown, .megamenu").hide(settings.speed);
                }
            });
        }

        function hideCollapse() {
            $(menu).children("li").show(0);
            $(menu).children("li.showhide").hide(0);
        }

        function rightAlignMenu() {
            $(menu).children("li").addClass("jsright");
            var items = $(menu).children("li");
            $(menu).children("li:not(.showhide)").detach();
            for (var i = items.length; i >= 1; i--) {
                $(menu).append(items[i]);
            }
            fixSubmenuRight();
        }

        function fixSubmenuRight() {
            $(menu).children("li").removeClass("last");
            var items = $(menu).children("li");
            for (var i = 1; i <= items.length; i++) {
                if ($(items[i]).children(".dropdown, .megamenu").length > 0) {
                    var lastItemsWidth = 0;
                    for (var y = 1; y <= i; y++) {
                        lastItemsWidth = lastItemsWidth + $(items[y]).outerWidth();
                    }
                    if ($(items[i]).children(".dropdown, .megamenu").outerWidth() > lastItemsWidth) {
                        $(items[i]).addClass("last");
                    }
                }
            }
        }

        function fixSubmenuLeft() {
            $(menu).children("li").removeClass("fix-sub");
            var items = $(menu).children("li");
            var menuWidth = $(menu).outerWidth();
            var itemsWidth = 0;
            for (var i = 1; i <= items.length; i++) {
                if ($(items[i]).children(".dropdown, .megamenu").length > 0) {
                    if ($(items[i]).position().left + $(items[i]).children(".dropdown, .megamenu").outerWidth() > menuWidth) {
                        $(items[i]).addClass("fix-sub");
                    }
                }
            }
        }

        function unbindEvents() {
            $(menu).find("li, a").unbind();
            $(document).unbind("click.menu touchstart.menu");
            $(menu).find(".dropdown, .megamenu").hide(0);
        }

        function windowWidth() {
            return window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
        }


        $(window).resize(function () {
            if (lastScreenWidth <= 768 && windowWidth() > 768) {
                unbindEvents();
                hideCollapse();
                bindHover();
                if (settings.align == "right" && bigScreen == false) {
                    rightAlignMenu();
                    bigScreen = true;
                }
            }
            if (lastScreenWidth > 768 && windowWidth() <= 768) {
                unbindEvents();
                showCollapse();
                bindClick();
                if (bigScreen == true) {
                    rightAlignMenu();
                    bigScreen = false;
                }
            }
            if (settings.align == "right") {
                if (lastScreenWidth > 768 && windowWidth() > 768)
                    fixSubmenuRight();
            }
            else {
                if (lastScreenWidth > 768 && windowWidth() > 768)
                    fixSubmenuLeft();
            }
            lastScreenWidth = windowWidth();
        });

        function screenSize() {
            if (windowWidth() <= 768) {
                showCollapse();
                bindClick();
                if (bigScreen == true) {
                    rightAlignMenu();
                    bigScreen = false;
                }
            }
            else {
                hideCollapse();
                bindHover();
                if (settings.align == "right") {
                    rightAlignMenu();
                    bigScreen = true;
                }
                else {
                    fixSubmenuLeft();
                }
            }
        }
        screenSize();

    }

}(jQuery));