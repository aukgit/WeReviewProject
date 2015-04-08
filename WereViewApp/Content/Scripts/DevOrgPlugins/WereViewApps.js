/// <reference path="CustomJS.js" />
/// <reference path="CommonJsEveryPage.js" />
/// <reference path="../bootstrap.js" />
/// <reference path="../jquery-2.1.1.js" />
/// <reference path="../jquery-2.1.1.intellisense.js" />
/// <reference path="../jquery.validate.js" />
/// <reference path="../moment.js" />
/// <reference path="../validation.js" />
/// <reference path="../respond.js" />
/// <reference path="../bootstrap-datepicker.js" />
/// <reference path="../bootstrap-datetimepicker.js" />
/// <reference path="../bootstrap-select.js" />
/// <reference path="../bootstrap-timepicker.js" />
/// <reference path="DevOrgComponent.js" />
/// <reference path="../underscore.js" />
/// <reference path="../../Content/Scripts/star-rating.js" />
/// <reference path="../../Content/Scripts/bootstrap-table-filter.js" />
/// <reference path="../../Content/Scripts/Scripts/jquery.elastic.source.js" />
/// <reference path="I:\WereViewApp\WereViewProject\WereViewApp\Content/Scripts/Upload/devOrgUploadConfig.js" />


$(function () {
    /// <summary>
    /// Were view app plug-in written by Alim Ul Karim
    /// </summary>
    $.WereViewApp = {

        appForm: $("form.app-editing-page"), // means both editing and posting
        appFormEdit: $("form.app-edit"),
        appFormPost: $("form.app-post"),
        $allInputs: $("form.app-post input"),
        ajaxDraftPostUrl: "/App/SaveDraft",
        $appPageUploaderNotifier: $("label.notify-global-info"),
        homePageUrl: "/",
        jquerySelectorForUploaderRows: "#collection-uploaders .form-row-uploader",
        afterDraftPostRedirectPageUrl: "/",
        appInputChangesExist: false,
        $globalTopErrorLabel: $("#notify-global-info-top"),
        $howtoUseUploaderInfoLabel: $("#how-to-use-uploader-info"),
        isTesting: false,
        draftSavingFailedErrorMsg: "Sorry couldn't save the draft , possible reason maybe connection lost or your draft buffer is exceeded.",
        numberOfDraftPossible: 10,
        invalidAttrName: "data-invalid",
        galleryImageUploaderId: 0,
        maxTryInputSubmit: 250,
        sendingDraftNumber: 0,
        writeReviewFormUrl: "/Reviews/GetReviewForm",
        reviewSpinnerSelector: "#review-requesting-spinner",
        reviewFormContainerSelectorInAppPage: "div#write-review-form-container",
        reviewFormSubmitUrl: "/Reviews/Write",


        fixIframeTag: function ($jQueryInputText) {
            //<iframe width="560" height="315" src="//www.youtube.com/embed/ob-P2a6Mrjs" frameborder="0" allowfullscreen></iframe>
            var currentText = $jQueryInputText.val();
            //currentText = currentText.toLowerCase();
            var reg = new RegExp("<iframe", 'gi');
            currentText = currentText.replace(reg, "[iframe");
            reg = new RegExp("</iframe>", 'gi');
            currentText = currentText.replace(reg, "[/iframe]");
            currentText = currentText.replace(">", "]");
            $jQueryInputText.val(currentText);
        },

        iframeSquareToActualTag: function ($jQueryInputText) {
            //[iframe width="560" height="315" src="//www.youtube.com/embed/ob-P2a6Mrjs" frameborder="0" allowfullscreen></iframe]
            var currentText = $jQueryInputText.val();
            //currentText = currentText.toLowerCase();
            var reg = new RegExp("\\[iframe", 'gi');
            currentText = currentText.replace(reg, "<iframe");
            reg = new RegExp("\\[/iframe\\]", 'gi');
            currentText = currentText.replace(reg, "</iframe>");
            currentText = currentText.replace("]", ">");
            $jQueryInputText.val(currentText);
        },

        /// App post submitting event from both edit or posting page.
        appformPostEvent: function (e) {
            /// <summary>
            /// posting a new app event
            /// </summary>
            /// <param name="e">
            /// 
            /// </param>
            var ifAnyUploadfails = false;
            if ($.WereViewApp.appFormPost.length > 0) {
                e.preventDefault();

                // only check uploader when posting time

                // first check if form is valid or not.
                var visibleInputsExceptFile = $.WereViewApp.$allInputs.filter("[type!=file]:visible");
                var len = visibleInputsExceptFile.length;
                if (!$.WereViewApp.isAppTitleValid()) {
                    $.WereViewApp.$globalTopErrorLabel.text("Please fill out the title correctly. It's very important for your app.");
                    $.WereViewApp.$globalTopErrorLabel.show();
                    return;
                }

                for (var i = 0; i < len; i++) {
                    var $singleInput = $(visibleInputsExceptFile[i]);
                    if (!$singleInput.valid()) {
                        $.WereViewApp.$globalTopErrorLabel.text("Please fill out the required fields.");
                        return;
                    }
                }

                $.WereViewApp.$globalTopErrorLabel.text("");

                // first check if the uploaders are visible or not
                var $uploaderRows = $.WereViewApp.appForm.find($.WereViewApp.jquerySelectorForUploaderRows);

                //visibility check

                for (i = 0; i < $uploaderRows.length; i++) {
                    var $uploaderRow = $($uploaderRows[i]);
                    if ($uploaderRow.is(":visible")) {
                        var $currentUploader = $uploaderRow.find("input[type='file']");
                        ifAnyUploadfails = isInvalidateUploader($currentUploader);
                        if (ifAnyUploadfails) {
                            raiseUploaderInvalidMessage(ifAnyUploadfails);
                            return;
                        }
                    }
                    if ($uploaderRow.is(":hidden")) {
                        if ($.WereViewApp.$howtoUseUploaderInfoLabel.is(":hidden")) {
                            $.WereViewApp.$howtoUseUploaderInfoLabel.show();
                        }
                        raiseUploaderInvalidMessage(ifAnyUploadfails);

                        $uploaderRow.show("slow");
                        return;
                    }
                }


                // checking uploadeers if valid
                var $uploaders = $.WereViewApp.appForm.find("#collection-uploaders");
                if ($uploaders.length > 0) {
                    // only validate uploads if any uploader exist.
                    var countUploaders = $uploaders.length;

                    var $uploaders2 = $uploaders.find("input[type='file']");
                    for (i = 0; i < countUploaders; i++) {
                        var $uploader = $($uploaders2[i]);
                        ifAnyUploadfails = isInvalidateUploader($uploader);
                    }
                    raiseUploaderInvalidMessage(ifAnyUploadfails);
                }

                if (!ifAnyUploadfails && $.WereViewApp.isAppTitleValid()) {
                    //everything is successful
                    $.WereViewApp.appInputChangesExist = false;
                    $.WereViewApp.fixAllInputIframeDataOrHtmlToSquare();

                    this.submit();
                }

            }
            //function preventDefaultInside(evt, formCanbeSent) {
            //    if (!ifAnyUploadfails && formCanbeSent) {
            //        // all uploads has been done.
            //        $.WereViewApp.appInputChangesExist = false; // no change exist on the unbin method ... direct submit.
            //    }
            //}
            function isInvalidateUploader($uploaderx) {
                var idAttr = $uploaderx.attr("data-id"); //always use jquery to get attr
                var loadedValues = $.devOrgUP.getCountOfHowManyFilesUploaded(idAttr);

                if (loadedValues === 0) {
                    return true;
                } else {
                    return false;
                }
            }
            function raiseUploaderInvalidMessage(failedBoolean) {
                if (failedBoolean) {
                    $.WereViewApp.$appPageUploaderNotifier.text("Please upload all necessary files to proceed next.");
                } else {
                    $.WereViewApp.$appPageUploaderNotifier.text("");
                }
            }
        },

        isAppTitleValid: function () {
            var $appName = $("#AppName");
            var hasInvalidAttr = $appName.attr($.WereViewApp.invalidAttrName);

            if (hasInvalidAttr) {
                return false;
            } else {
                return true;
            }
        },

        appEditingSubmitEvent: function (e) {
            e.preventDefault();
            if ($.WereViewApp.isAppTitleValid()) {
                var count = $.devOrgUP.getCountOfHowManyFilesUploaded($.WereViewApp.galleryImageUploaderId);
                if (count > 0) {
                    //fix square brackets
                    $.WereViewApp.fixAllInputIframeDataOrHtmlToSquare();
                    // remove msg
                    $.WereViewApp.appInputChangesExist = false;
                    //submit
                    this.submit();
                } else {
                    $.WereViewApp.$appPageUploaderNotifier.text("Please upload all necessary files to proceed next.");
                }
            }
        },

        fixAllInputIframeDataOrHtmlToSquare: function () {
            var inputSelectors = "input.url-input"
            var inputFields = $.WereViewApp.appForm.find(inputSelectors);
            if (inputFields.length > 0) {
                for (var i = 0; i < inputFields.length; i++) {
                    var $eachInputfield = $(inputFields[i]);
                    $.WereViewApp.fixIframeTag($eachInputfield);
                }
            }
        },
        invertAllInputIframeDataOrSquareToHtml: function () {
            var inputSelectors = "input.url-input"
            var inputFields = $.WereViewApp.appForm.find(inputSelectors);
            if (inputFields.length > 0) {
                for (var i = 0; i < inputFields.length; i++) {
                    var $eachInputfield = $(inputFields[i]);
                    $.WereViewApp.iframeSquareToActualTag($eachInputfield);
                }
            }
        },

        /// it doesn't include fixing html inputs
        /// return as ajax response, add methods like success or fail to do something with it.
        ajaxDraftSaveApp: function (e) {
            var formData;
            formData = $.WereViewApp.appForm.serializeArray();

            // ajax post to save draft app
            return $.ajax({
                type: "POST",
                dataType: "JSON",
                url: $.WereViewApp.ajaxDraftPostUrl,
                data: formData
            }); // ajax end
        },

        beforeUnloadEvent: function () {
            /// <summary>
            /// Only sends to draft if in the app posting page.
            /// </summary>
            /// <returns type=""></returns>
            if ($.WereViewApp.appInputChangesExist) {

                if ($.WereViewApp.appFormPost.length > 0) {
                    // app posting page
                    // send as ajax post
                    if ($.WereViewApp.sendingDraftNumber <= $.WereViewApp.numberOfDraftPossible) {
                        // fix all html inputs
                        $.WereViewApp.fixAllInputIframeDataOrHtmlToSquare();

                        $.WereViewApp.ajaxDraftSaveApp();
                    }

                }// app posting page if else end.


                return "Are you sure you wanted to leave? Your app will be saved as a draft if you leave (you can have up to " + $.WereViewApp.numberOfDraftPossible + " draft posts).";
            }
        },



        appFormDraftBtnClicked: function () {
            $.WereViewApp.appForm.find("#draft-btn").click(function (e) {
                e.preventDefault();
                $.WereViewApp.appInputChangesExist = false;
                // fix html input type to relevant square brackets
                // fix all html inputs
                $.WereViewApp.fixAllInputIframeDataOrHtmlToSquare();

                //send ajax request to draft save.
                $.WereViewApp.ajaxDraftSaveApp()
                .done(function (data) {
                    // if successful then move to redirect page.
                    window.location.href = $.WereViewApp.afterDraftPostRedirectPageUrl;
                })
                .fail(function (jqXHR, textStatus) {
                    $.WereViewApp.$globalTopErrorLabel.text($.WereViewApp.draftSavingFailedErrorMsg);
                });
            });
        },

        appEditingPageOnReady: function () {
            var $formInputs = $.WereViewApp.appForm.find("select,input[name!=YoutubeEmbedLink]");
            //console.log($formInputs);

            $.devOrg.validateInputFromServer("#AppName", "/Validator/GetValidUrlEditing", "AppName", false, false, 3, true, " is invalid means that one app is already exist within this exact platform or category. You may change those to get a valid title and url.", null, $formInputs, $.WereViewApp.maxTryInputSubmit);
            $.WereViewApp.appForm.submit($.WereViewApp.appEditingSubmitEvent);

            $.WereViewApp.invertAllInputIframeDataOrSquareToHtml();
        },

        appPostingPageOnReady: function () {
            $.devOrg.uxFriendlySlide("form.app-post", true, true);
            var $formInputs = $.WereViewApp.appForm.find("select,input[name!=YoutubeEmbedLink]");
            console.log($formInputs);
            $.devOrg.validateInputFromServer("#AppName", "/Validator/GetValidUrl", "AppName", false, false, 3, true, " is invalid means that one app is already exist within this exact platform or category. You may change those to get a valid title and url.", null, $formInputs, $.WereViewApp.maxTryInputSubmit);

            ///hiding the uploader on the app loader page for every time before posting a new app.
            $.WereViewApp.appForm.find($.WereViewApp.jquerySelectorForUploaderRows).hide();

            // stop form submitting the form if any file upload is not done.
            $.WereViewApp.appForm.submit($.WereViewApp.appformPostEvent);

            $.WereViewApp.appFormDraftBtnClicked();

        },

        generalAppFormEditingOrPostingPageOnReady: function (e) {

            if ($.WereViewApp.appForm.length > 0) {
                $.WereViewApp.$howtoUseUploaderInfoLabel.hide(); //hide uploader info label.

                if ($.WereViewApp.appFormPost.length > 0) {
                    // app posting
                    $.WereViewApp.appPostingPageOnReady();
                } else if ($.WereViewApp.appFormEdit.length > 0) {
                    // app editing
                    $.WereViewApp.appEditingPageOnReady();
                }

                $.devOrg.validateTextInputBasedOnRegEx("#AppName", "^([A-zZ.]+\\s*)+(\\d*)\\s*([aA-zZ.]+\\s*)+(\\d*)", "Sorry your app name is not valid. Valid name example eg. Plant Vs. Zombies v2.");

                $.devOrg.reSetupjQueryValidate("form");


                $.WereViewApp.appForm.find("input,textarea").change(function () {
                    $.WereViewApp.appInputChangesExist = true;
                })

                $.WereViewApp.appForm.find("select").selectpicker();

                // enter to go next
                $.devOrg.enterToNextTextBox("form.app-editing-page", true); // means both editing and posting

                // Only sends to draft if in the app posting page.
                $(window).bind('beforeunload', $.WereViewApp.beforeUnloadEvent);

                // triggering appname blur when change any of these. Because all are related to URL generate.
                $(".selectpicker,select").change(function () {
                    $("#AppName").trigger("blur");

                });

                $("#PlatformVersion").blur(function () {
                    $("#AppName").trigger("blur");
                    //console.log("dev");
                });



            }
        },

        frontEndJavaScript: function () {
            var image = null;
            $('.pricebox').number(true, 0);

            $('#slider-thumbs a').nivoLightbox({ effect: 'fade', keyboardNav: true });



            //$("#input-21f").rating();

            var $ratingItems = $(".rating-5");

            if ($ratingItems.length > 0) {
                $ratingItems.rating({
                    showClear: false
                });
            }
            $ratingItems = $(".rating-10");

            if ($ratingItems.length > 0) {
                $ratingItems.rating({
                    showClear: false,
                    starCaptionClasses: {
                        0.5: 'label label-danger',
                        1: 'label label-danger',
                        1.5: 'label label-danger',
                        2: 'label label-danger',
                        2.5: 'label label-danger',
                        2: 'label label-warning',
                        2.5: 'label label-warning',
                        3: 'label label-warning',
                        3.5: 'label label-warning',
                        4: 'label label-warning',
                        4.5: 'label label-warning',
                        5: 'label label-warning',
                        5.5: 'label label-info',
                        6: 'label label-info',
                        6.5: 'label label-info',
                        7: 'label label-info',
                        7.5: 'label label-primary',
                        8: 'label label-primary',
                        8.5: 'label label-success',
                        9: 'label label-success',
                        9.5: 'label label-success',
                        10: 'label label-success'
                    }
                });
            }
            var nameOfTheCarosul = "#myCarousel";
            var $appPageGallery = $(nameOfTheCarosul);
            if ($appPageGallery.length > 0) {
                $appPageGallery
                    .carousel({ interval: 5000 })
                    .bind('slid.bs.carousel', function (e) {
                        var to_slide = $('.carousel-item.active').attr('data-slide-no');
                        //console.log("slide");
                        //console.log("active item " + to_slide);
                        $('.myCarousel-target.active').removeClass('active');
                        $('.carousel-indicators [data-slide-to=' + to_slide + ']').addClass('active');
                    });
                $('.myCarousel-target').on('click', function () {
                    $(nameOfTheCarosul).carousel(parseInt($(this).attr('data-slide-to')));
                    //console.log("ase");
                    $('.myCarousel-target.active').removeClass('active');
                    $(this).addClass('active');

                });



                //var $images = $appPageGallery.find("div.item");
                //var length = $images.length;
                //var $image = null;
                //var slideNo = null;

                //for (var i = 0; i < length; i++) {
                //    $image = $($images[i]);
                //    $image.click(function () {
                //        var $imgInside = $(this);
                //        console.log($imgInside);
                //        slideNo = $imgInside.attr("data-slide-number");
                //        console.log(slideNo);
                //        $("#slider-thumbs a#carousel-selector-" + slideNo).trigger('click');
                //    }).css({
                //        'cursor': 'pointer'
                //    });

                //}

                //jQuery(document).bind('keyup', function (e) {

                //    if (e.keyCode == 39) {
                //        jQuery('a.carousel-control.right').trigger('click');
                //    }

                //    else if (e.keyCode == 37) {
                //        jQuery('a.carousel-control.left').trigger('click');
                //    }

                //});
            }


            function showHide(shID) {
                if (document.getElementById(shID)) {
                    if (document.getElementById(shID + '-show').style.display != 'none') {
                        // already hidden
                        document.getElementById(shID + '-show').style.display = 'none';
                        document.getElementById(shID).style.display = 'inline';
                        $("span#elipse-dot").hide();
                    } else {
                        document.getElementById(shID + '-show').style.display = 'inline';
                        document.getElementById(shID).style.display = 'none';
                        $("span#elipse-dot").show();
                    }
                }
            }
            function showHideForReview(i) {

                if (document.getElementById('show_review' + i).style.display != 'none') {
                    document.getElementById('show_review' + i).style.display = 'none';
                    document.getElementById('less_review' + i).style.display = 'inline';
                }
                else {
                    document.getElementById('show_review' + i).style.display = 'inline';
                    document.getElementById('less_review' + i).style.display = 'none';
                }
            }

            function showAppsInfoMore(i) {

                if (document.getElementById('show_hide_id_more' + i).style.display != 'none') {
                    document.getElementById('show_hide_id_more' + i).style.display = 'none';
                    document.getElementById('show_hide_id_less' + i).style.display = 'inline';
                }
                else {
                    document.getElementById('show_hide_id_more' + i).style.display = 'inline';
                    document.getElementById('show_hide_id_less' + i).style.display = 'none';
                }
            }

            $("#app-description-see-more-btn,#app-description-see-less-btn").click(function () {
                showHide("example");
            }).css({
                'cursor': 'pointer'
            });;
            var $showBtns = $("div.blogitemWrap a.showLink");
            var $hideBtns = $("div.blogitemWrap a.hideLink");
            $showBtns.click(doProcessMoreNLessBtns).css({
                'cursor': 'pointer'
            });
            $hideBtns.click(doProcessMoreNLessBtns).css({
                'cursor': 'pointer'

            });


            function doProcessMoreNLessBtns(e) {
                e.preventDefault();
                var $this = $(this);
                var id = $this.attr("data-sequence");
                showHideForReview(id);
            }

        },

        reviewFormSubmit: function (evt) {
            evt.preventDefault();
            //console.log("ase");
            $form = $(this);
            var $submittingSpinner = null;
            var length = 0;
            var $inputs, $input, currformData = 0;
            $submittingSpinner = $form.find("#submitting-review-spinner");
            var $failedIcon = $form.find("#submitting-review-failed-icon");

            var $lastDiv = $form.find("div[data-last-slide=true]:visible");
            var url = $form.attr("action");

            if ((url === $.WereViewApp.reviewFormSubmitUrl && $lastDiv.length > 0) || url !== $.WereViewApp.reviewFormSubmitUrl) {
                $inputs = $lastDiv.find("input");
                var $comment = $("#Comments");
                var commentValue = $comment.val();

                if ($.devOrg.checkValidInputs($inputs) && !_.isEmpty(commentValue)) {
                    // now we can submit, all are valid.
                    $submittingSpinner.fadeIn("slow");
                    currformData = $form.serializeArray();
                    console.log(currformData);
                    $.ajax({
                        type: "POST",
                        dataType: "json",
                        url: url,
                        data: currformData,
                        success: function (response) {
                            console.log(response);
                            var isDone = response.isDone;
                            var msg = response.msg;
                            if (isDone) {
                                location.reload(true);
                            } else {
                                $failedIcon.fadeIn("slow");
                            }
                            $submittingSpinner.fadeOut("slow");

                        },
                        error: function (xhr, status, error) {

                        }
                    }); // ajax end
                }
            }
        },

        askForReviewForm: function () {
            var $reviewSpinner = $($.WereViewApp.reviewSpinnerSelector).hide();

            if ($reviewSpinner.length > 0) {
                $("#WriteReviewButton").click(function () {
                    var $container = $($.WereViewApp.reviewFormContainerSelectorInAppPage);
                    var text = $container.text().trim();
                    if (text.length === 0) {
                        $container.hide();
                        $reviewSpinner.fadeIn("slow");
                        // load write form
                        var reqVerifyFields = $("#review-request-fields input").serializeArray();
                        //console.log(reqVerifyFields);
                        $.ajax({
                            type: "POST",
                            dataType: "html",
                            url: $.WereViewApp.writeReviewFormUrl,
                            data: reqVerifyFields,
                            success: function (response) {
                                var selectForm = $.WereViewApp.reviewFormContainerSelectorInAppPage + " form";
                                var $submittingSpinner = null;
                                var $response = $(response);
                                $container.html(response);

                                var $failedIcon = $("#submitting-review-failed-icon");
                                $failedIcon.hide();

                                $container.show("slow");
                                var $form = $response.filter("form");

                                if ($form.length > 0) {

                                    $submittingSpinner = $response.find("#submitting-review-spinner");
                                    $submittingSpinner.hide();

                                    $container.find("form").submit($.WereViewApp.reviewFormSubmit);
                                    //$container.find("button.btn.btn-success").click(function () {
                                    //    console.log("at place");
                                    //});

                                    $.devOrg.uxFriendlySlide(
                                        selectForm,
                                        true,
                                        true);


                                }
                                //console.log(response);
                                $reviewSpinner.fadeOut("slow");
                            },
                            error: function (xhr, status, error) {

                            }
                        }); // ajax end
                    } else {
                        $container.toggle("slow");
                    }

                }).css({
                    'cursor': 'pointer'
                });
            }
        },
        reviewLikeDisLikeClicked: function () {
            var $likeBtns = $("div.blogitemWrap a[data-review-like-btn=true]");
            var $disLikeBtns = $("div.blogitemWrap a[data-review-dislike-btn=true]");
            if ($likeBtns.length > 0) {
                $likeBtns.click(btnClicked);
            }

            if ($disLikeBtns.length > 0) {
                $disLikeBtns.click(btnClicked);
            }
            function btnClicked(e) {
                e.preventDefault();
                var $this = $(this);
                var url = $this.attr("href");
                $.ajax({
                    type: "GET",
                    url: url,
                    success: function (response) {

                    }
                }); // ajax end
                var sequence = $this.attr("data-sequence");
                var isLikeBtn = $this.attr("data-review-like-btn");

                var $otherA = null;
                if (isLikeBtn) {
                    $otherA = $("#review-thumbs-down-click-" + sequence);
                } else {
                    $otherA = $("#review-thumbs-up-click-" + sequence);
                }
                $otherA.find("i").removeClass("active");
                $this.find("i").toggleClass("active");
            }
        },
        suggestedOrReviewLoadmoreBtnLeft: function () {
            var $loadMoreBtn = $("#suggested-load-more-btn");
            var length, $appBox = 0;
            var showAfterCount = 5;
            if ($loadMoreBtn.length > 0) {

                var $div = $("#suggested-apps-list-div");
                var $appBoxes = $div.find("div.appsbox[data-sequence]");
                length = $appBoxes.length;

                for (var i = 0; i < length; i++) {
                    if (i >= showAfterCount) {
                        $appBox = $($appBoxes[i])
                        $appBox.hide();
                        $appBox.attr("data-hide", "true");

                    }
                }

                if ($loadMoreBtn.is(":hidden") && length > showAfterCount) {
                    $loadMoreBtn.show("slow");
                } else if ($loadMoreBtn.is(":visible") && length < showAfterCount) {
                    $loadMoreBtn.hide();
                }

                $loadMoreBtn.click(function () {
                    $appBoxes = $div.find("div.appsbox[data-sequence][data-hide=true]");
                    for (var i = 0; i < length; i++) {
                        $appBox = $($appBoxes[i])
                        $appBox.show("slow");
                        $appBox.attr("data-hide", "false");
                    }
                    $loadMoreBtn.hide("slow");
                });
            }

            var $reviewLoadMoreBtn = $("#review-load-more-btn");
            var reviewShows = 4;
            if ($reviewLoadMoreBtn.length > 0) {
                var $div = $("#review-collection");
                var $appBoxes = $div.find("div.blogitembox[data-sequence]");
                length = $appBoxes.length;

                for (var i = 0; i < length; i++) {
                    if (i >= showAfterCount) {
                        $appBox = $($appBoxes[i])
                        $appBox.hide();
                        $appBox.attr("data-hide", "true");
                    }
                }

                if ($reviewLoadMoreBtn.is(":hidden") && length > showAfterCount) {
                    $reviewLoadMoreBtn.show("slow");
                } else if ($reviewLoadMoreBtn.is(":visible") && length < showAfterCount) {
                    $reviewLoadMoreBtn.hide();
                }

                $reviewLoadMoreBtn.click(function () {
                    $appBoxes = $div.find("div.blogitembox[data-sequence][data-hide=true]");
                    for (var i = 0; i < length; i++) {
                        $appBox = $($appBoxes[i])
                        $appBox.show("slow");
                        $appBox.attr("data-hide", "false");
                    }
                    $reviewLoadMoreBtn.hide("slow");
                });
            }
        },

        fixDateInputs: function () {
            var $dateInputs = $("form input.date-input");
            var $dateInput = null;
            var length = $dateInputs.length;
            var text = null;
            if (length > 0) {
                for (var i = 0; i < length; i++) {
                    $dateInput = $($dateInputs[i]);
                    text = $dateInput.val();
                    if (!_.isEmpty(text)) {
                        text = text.replace(/\-/ig, "/");
                        $dateInput.val(text);
                    }
                }
            }
        },


        /* 
        * hides all uploader at first : $.WereViewApp.appForm.find("#collection-uploaders uploader-auto").hide();
        * modify $.WereViewApp.appInputChangesExist based on user input
        * enter to next line bind : $.devOrg.enterToNextTextBox("form.app-editing-page", true);
        * bind with form submit-> which binds to $.WereViewApp.appformPostEvent
        * draftbtnClicked : $.WereViewApp.appFormDraftBtnClicked();
        * binds with beforeunload which binds with $.WereViewApp.beforeUnloadEvent
        */
        executeActions: function () {
            $.WereViewApp.generalAppFormEditingOrPostingPageOnReady();

            $.WereViewApp.frontEndJavaScript();

            //data-last-slide="true"
            $.WereViewApp.askForReviewForm();

            $.WereViewApp.suggestedOrReviewLoadmoreBtnLeft();

            $.WereViewApp.reviewLikeDisLikeClicked();
            $("#developers-organism").addClass("hide");
            // fix date inputs
            $.WereViewApp.fixDateInputs();
        }
    };

    // this will call all the other events
    $.WereViewApp.executeActions();
});