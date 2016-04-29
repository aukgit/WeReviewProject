; $.app = $.app || {};
; $.app.component = $.app.component || {};
; $.app.component.list = $.app.component.list || {};

/**
 * Loads this component if this string value present in the hidden field of "Component-Enable"
 * @returns {} 
 */
$.app.component.list["revolution-gallery"] = function() {
    var $frontPageGallyery = $(".tp-banner");
    if ($frontPageGallyery.length > 0) {
        $frontPageGallyery.show().revolution({
            dottedOverlay: "none",
            delay: 5000,
            startwidth: 960,
            startheight: 320,
            hideThumbs: 10,
            fullWidth: "off",
            navigationType: "bullet",
            navigationStyle: "preview2",
            forceFullWidth: "off"
        });
    }
};

$.app.component.list["isotop"] = function() {
    var $isotopContainer = $("ul.search-page-apps-list");
    if ($isotopContainer.length > 0) {
        var $filterIsotopItems = $('.filter li a');
        if ($filterIsotopItems.length > 0) {
            $filterIsotopItems.click(function() {
                $('.filter').find("li a").removeClass('active');
                $(this).addClass('active');
                var selector = $(this).attr('data-filter');

                $isotopContainer.isotope({
                    filter: selector
                });
                return false;
            });
        }
    }
};

$.app.component.list["owl-carousel"] = function () {
    var owlOptions = {
        responsiveClass: true,
        navigation: true,
        navigationText: [
            "<i class='fa fa-chevron-circle-left'></i>",
            "<i class='fa fa-chevron-circle-right'></i>"
        ],
        items: 1, //10 items above 1000px browser width
        //itemsDesktop: [1152, 6], //5 items between 1000px and 901px
        //itemsDesktopSmall: [900, 4], // betweem 900px and 601px
        //itemsTablet: [600, 3], //2 items between 600 and 0
        //itemsMobile: [450, 2],
        itemsCustom: [370, 1]
    };

    $(".app-suggested-list-items-mobile").owlCarousel(owlOptions);
    $(".featured-apps-list-items").owlCarousel(owlOptions);

    var $suggestionCarosel = $(".owl-list");
    if ($suggestionCarosel.length > 0) {
        $suggestionCarosel.owlCarousel({
            navigation: true,
            navigationText: [
                "<i class='fa fa-chevron-circle-left'></i>",
                "<i class='fa fa-chevron-circle-right'></i>"
            ],
            items: 7, //10 items above 1000px browser width
            itemsDesktop: [1152, 6], //5 items between 1000px and 901px
            itemsDesktopSmall: [966, 5], // betweem 900px and 601px
            itemsTabletSmall: [730, 4],
            itemsTablet: [600, 3], //2 items between 600 and 0
            //itemsCustom: [[0, 2], [435, 3], [450, 2], [600, 3], [730, 4], [900, 5],  [950, 6]], // [[740, 6], [1000, 8], [1200, 10], [1600, 16]]
            itemsMobile: [450, 2]
            //itemsScaleUp: false

        });
    }

    var $appsPreview = $("#apps-preview");
    if ($appsPreview.length > 0) {
        $appsPreview.owlCarousel({
            slideSpeed: 300,
            paginationSpeed: 400,
            singleItem: true,
            items: 1,
            itemsDesktop: false,
            itemsDesktopSmall: false,
            itemsTablet: false,
            itemsMobile: false,
            stopOnHover: true,
            navigation: true, // Show next and prev buttons
            pagination: false,
            autoHeight: true,
            navigationText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"]
        });
    }

};