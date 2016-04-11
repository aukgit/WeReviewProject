;
$.app = $.app || {};
$.app.pagination = function($paginationContainer, pageCountUrl, selectedPage, pagesNumberToDisplay, additionalClass, paginationClass, onComplete) {
    /// <summary>
    ///     render pagination numbers
    /// </summary>
    /// <param name="$paginationContainer">The jQuery container where the pagination will be created.</param>
    /// <param name="pageCountUrl">Where to retrieve the page count.</param>
    /// <param name="selectedPage">Which page is currently selected.</param>
    /// <param name="pagesNumberToDisplay"></param>
    /// <param name="additionalClass">If any additional class need to be added with the pagination ul.</param>
    /// <param name="paginationClass">class will be added with the anchors by default it would be 'pagination-request'</param>
    /// <param name="onComplete">On complete this function will run.</param>
    var renderPagination = function(totalPageCount) {
        if ($.app.isDebugging) {
            console.log("Page count:");
            console.log(totalPageCount);
        }
        if (!paginationClass) {
            paginationClass = "pagination-request";
        }
        if (!additionalClass) {
            additionalClass = "";
        }
        var pagesCount = totalPageCount,
            $ul = $("<ul></ul>", {
                'class': "pagination " + additionalClass
            });
        var start = 2, end = pagesCount;
        var mid = Math.ceil(pagesNumberToDisplay / 2); // 5/2 = 2
        if (pagesCount > pagesNumberToDisplay) {
            end = selectedPage + mid;
            start = selectedPage - mid;
            if (start <= 0) {
                start = 0 - start;
                end += start;
                start = 2;
            }
            if (end >= pagesCount) {
                start = selectedPage - mid - (end - pagesCount);
                end = pagesCount;
            }
        }
        // first page link
        var differentPage = 1,
            differnetPageStringName = "First",
            isSelectedPage = selectedPage === differentPage;
        var $li = $("<li></li>", {
            'class': isSelectedPage === true ? "active" : null
        });
        var linkUrl = "#" + differentPage,
            anchorHtml = "<a class='" + paginationClass + "' href='" + linkUrl + "' data-page='" + differentPage + "'>" + differnetPageStringName + "</a>";
        $li.append(anchorHtml);
        $li.appendTo($ul);

        if (start !== 2) {
            differentPage = start - 1;
            differnetPageStringName = "...";
            isSelectedPage = selectedPage === differentPage;
            linkUrl = "#" + differentPage;
            $li = $("<li></li>", {
                'class': isSelectedPage === true ? "active" : null
            });
            anchorHtml = "<a class='" + paginationClass + "' href='" + linkUrl + "' data-page='" + differentPage + "'>" + differnetPageStringName + "</a>";
            $li.append(anchorHtml);
            $li.appendTo($ul);
        }

        for (var i = start; i <= end; i++) {
            isSelectedPage = selectedPage === i;
            linkUrl = "#" + i; //tableUrl + queryStringforPage;
            $li = $("<li></li>", {
                'class': isSelectedPage === true ? "active" : null
            });
            anchorHtml = "<a class='" + paginationClass + "' href='" + linkUrl + "' data-page='" + i + "'>" + i + "</a>";
            $li.append(anchorHtml);
            $li.appendTo($ul);
        }
        if (end + 1 < pagesCount) {
            // ... for end
            differentPage = end + 1 <= pagesCount ? end + 1 : pagesCount;
            differnetPageStringName = "...";
            isSelectedPage = selectedPage === differentPage;
            linkUrl = "#" + differentPage;
            $li = $("<li></li>", {
                'class': isSelectedPage === true ? "active" : null
            });
            anchorHtml = "<a class='" + paginationClass + "' href='" + linkUrl + "' data-page='" + differentPage + "'>" + differnetPageStringName + "</a>";
            $li.append(anchorHtml);
            $li.appendTo($ul);
        }
        differentPage = pagesCount;
        differnetPageStringName = "Last";
        isSelectedPage = selectedPage === differentPage;
        linkUrl = "#" + differentPage;
        $li = $("<li></li>");
        anchorHtml = "<a class='" + paginationClass + "' href='" + linkUrl + "' data-page='" + differentPage + "'>" + differnetPageStringName + "</a>";
        $li.append(anchorHtml);
        $li.appendTo($ul);
        $ul.appendTo($paginationContainer);
        if (typeof onComplete === "function") {
            onComplete.apply();
        }
    };
    jQuery.ajax({
        method: "POST", // by default "GET"
        url: pageCountUrl,
        dataType: "JSON" //, // "Text" , "HTML", "xml", "script" 
    }).done(function(response) {
        var totalCountOfPages = response;
        $.setHiddenValue("pages-exist", totalCountOfPages);
        renderPagination(totalCountOfPages);
    });

};