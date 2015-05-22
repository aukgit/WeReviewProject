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
/// <reference path="I:\WeReviewApp\WereViewProject\WeReviewApp\Content/Scripts/Upload/devOrgUploadConfig.js" />
/// <reference path="faster-jQuery.js" />


/**
 * Written by Alim Ul Karim
 * Developers Organism
 * Written  : 14 Nov 2014
 * Modified : 28 Apr 2015
 */

$(function () {
    /// <summary>
    /// Were view app plug-in written by Alim Ul Karim
    /// </summary>
    $.WeReviewApp = {
        ///appForm represents both app-edit and app-posting form
        $appForm: $.queryFirst("form.app-editing-page"), // means both editing and posting
        $appFormEdit: $.queryFirst("form.app-edit"),
        $appFormPost: $.queryFirst("form.app-post"),
        $allInputs: $("form.app-post input"),
        ajaxDraftPostUrl: "/App/SaveDraft",
        $appPageUploaderNotifier: $("label.notify-global-info"),
        homePageUrl: "/",
        selectorForUploaderRows: "#collection-uploaders .form-row-uploader",
        afterDraftPostRedirectPageUrl: "/",
        appInputChangesExist: false,
        $globalTopErrorLabel: $.byId("notify-global-info-top"),
        $howtoUseUploaderInfoLabel: $.byId("how-to-use-uploader-info"),
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
        ///consist of # : "#app-deails-page"
        appDetailsPageParentId: "#app-deails-page",
        /**
         * single input IFRAME code HTML  to Square
         */
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
        /**
         * single input IFRAME code Square  to HTML
         */
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

            function raiseUploaderInvalidMessage(failedBoolean) {
                if (failedBoolean) {
                    $.WeReviewApp.$appPageUploaderNotifier.text("Please upload all necessary files to proceed next.");
                } else {
                    $.WeReviewApp.$appPageUploaderNotifier.text("");
                }
            }

            function isInvalidateUploader($uploaderx) {
                var idAttr = $uploaderx.attr("data-id"); //always use jquery to get attr
                var loadedValues = $.devOrgUP.getCountOfHowManyFilesUploaded(idAttr);

                if (loadedValues === 0) {
                    return true;
                } else {
                    return false;
                }
            }

            if ($.WeReviewApp.$appFormPost.length > 0) {
                e.preventDefault();

                // only check uploader when posting time

                // first check if form is valid or not.
                var visibleInputsExceptFile = $.WeReviewApp.$allInputs.filter("[type!=file]:visible");
                var len = visibleInputsExceptFile.length;
                if (!$.WeReviewApp.isAppTitleValid()) {
                    $.WeReviewApp.$globalTopErrorLabel.text("Please fill out the title correctly. It's very important for your app.");
                    $.WeReviewApp.$globalTopErrorLabel.show();
                    return;
                }

                for (var i = 0; i < len; i++) {
                    var $singleInput = $(visibleInputsExceptFile[i]);
                    if (!$singleInput.valid()) {
                        $.WeReviewApp.$globalTopErrorLabel.text("Please fill out the required fields.");
                        return;
                    }
                }

                $.WeReviewApp.$globalTopErrorLabel.text("");

                // first check if the uploaders are visible or not
                var $uploaderRows = $.WeReviewApp.$appForm.find($.WeReviewApp.selectorForUploaderRows);

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
                        if ($.WeReviewApp.$howtoUseUploaderInfoLabel.is(":hidden")) {
                            $.WeReviewApp.$howtoUseUploaderInfoLabel.show();
                        }
                        raiseUploaderInvalidMessage(ifAnyUploadfails);

                        $uploaderRow.show("slow");
                        return;
                    }
                }


                // checking uploadeers if valid
                var $uploaders = $.WeReviewApp.$appForm.find("#collection-uploaders");
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

                if (!ifAnyUploadfails && $.WeReviewApp.isAppTitleValid()) {
                    //everything is successful
                    $.WeReviewApp.appInputChangesExist = false;
                    $.WeReviewApp.fixAllInputIframeDataOrHtmlToSquare();
                    // all conditions fulfilled so submit the form
                    this.submit();
                }

            }
            //function preventDefaultInside(evt, formCanbeSent) {
            //    if (!ifAnyUploadfails && formCanbeSent) {
            //        // all uploads has been done.
            //        $.WeReviewApp.appInputChangesExist = false; // no change exist on the unbin method ... direct submit.
            //    }
            //}
        },

        isAppTitleValid: function () {
            var $appName = $("#AppName");
            var hasInvalidAttr = $appName.attr($.WeReviewApp.invalidAttrName);

            if (hasInvalidAttr) {
                return false;
            } else {
                return true;
            }
        },
        // before app editing submit
        appEditingSubmitEvent: function (e) {
            e.preventDefault();
            if ($.WeReviewApp.isAppTitleValid()) {
                var count = $.devOrgUP.getCountOfHowManyFilesUploaded($.WeReviewApp.galleryImageUploaderId);
                if (count > 0) {
                    //fix square brackets
                    $.WeReviewApp.fixAllInputIframeDataOrHtmlToSquare();
                    // remove msg
                    $.WeReviewApp.appInputChangesExist = false;
                    //submit
                    this.submit();
                } else {
                    $.WeReviewApp.$appPageUploaderNotifier.text("Please upload all necessary files to proceed next.");
                }
            }
        },
        /**
         * Covert all url inputs IFRAME code HTML to Square
         */
        fixAllInputIframeDataOrHtmlToSquare: function () {
            var inputSelectors = "input.url-input";
            var inputFields = $.WeReviewApp.$appForm.find(inputSelectors);
            if (inputFields.length > 0) {
                for (var i = 0; i < inputFields.length; i++) {
                    var $eachInputfield = $(inputFields[i]);
                    $.WeReviewApp.fixIframeTag($eachInputfield);
                }
            }
        },
        /**
         * Covert all url inputs IFRAME code Square to HTML
         */
        invertAllInputIframeDataOrSquareToHtml: function () {
            var inputSelectors = "input.url-input";
            var inputFields = $.WeReviewApp.$appForm.find(inputSelectors);
            if (inputFields.length > 0) {
                for (var i = 0; i < inputFields.length; i++) {
                    var $eachInputfield = $(inputFields[i]);
                    $.WeReviewApp.iframeSquareToActualTag($eachInputfield);
                }
            }
        },
        /**
         * get a string regular expression based on parameter
         */
        getAttributeRemoveRegularExpressionFor: function (attributeName) {
            return "(" + attributeName + ".*=.*[\"\"'])([a-zA-Z0-9:;\.\s\(\)\-\,]*)([\"\"'])";
        },
        removeHeightWidthAttributes: function ($jQueryInputText) {
            var currentText = $jQueryInputText.val();
            //currentText = currentText.toLowerCase();
            var heightRegEx = $.WeReviewApp.getAttributeRemoveRegularExpressionFor("Height");
            var widthRegEx = $.WeReviewApp.getAttributeRemoveRegularExpressionFor("Width");

            var reg = new RegExp(heightRegEx, 'gi');
            currentText = currentText.replace(reg, "");
            reg = new RegExp(widthRegEx, 'gi');
            currentText = currentText.replace(reg, "");
            $jQueryInputText.val(currentText);
        },
        fixYouTubeVideoPropertise: function () {
            var inputSelectors = "input.url-input";
            var inputFields = $.WeReviewApp.$appForm.find(inputSelectors);
            if (inputFields.length > 0) {
                for (var i = 0; i < inputFields.length; i++) {
                    var $eachInputfield = $(inputFields[i]);
                    $.WeReviewApp.removeHeightWidthAttributes($eachInputfield);
                }
            }
        },

        /// it doesn't include fixing html inputs
        /// return as ajax response, add methods like success or fail to do something with it.
        ajaxDraftSaveApp: function (e) {
            var formData;
            formData = $.WeReviewApp.$appForm.serializeArray();

            // ajax post to save draft app
            return $.ajax({
                type: "POST",
                dataType: "JSON",
                url: $.WeReviewApp.ajaxDraftPostUrl,
                data: formData
            }); // ajax end
        },

        beforeUnloadEvent: function () {
            /// <summary>
            /// Only sends to draft if in the app posting page.
            /// </summary>
            /// <returns type=""></returns>
            if ($.WeReviewApp.appInputChangesExist) {

                if ($.WeReviewApp.$appFormPost.length > 0) {
                    // app posting page
                    // send as ajax post
                    if ($.WeReviewApp.sendingDraftNumber <= $.WeReviewApp.numberOfDraftPossible) {
                        // fix all html inputs
                        $.WeReviewApp.fixAllInputIframeDataOrHtmlToSquare();

                        $.WeReviewApp.ajaxDraftSaveApp();
                    }

                }// app posting page if else end.


                return "Are you sure you wanted to leave? Your app will be saved as a draft if you leave (you can have up to " + $.WeReviewApp.numberOfDraftPossible + " draft posts).";
            }
        },
        /**
         * When draft button is clicked from app-posting page.
         */
        appFormDraftBtnClicked: function () {
            $.WeReviewApp.$appForm.find("#draft-btn").click(function (e) {
                e.preventDefault();
                $.WeReviewApp.appInputChangesExist = false;
                // fix html input type to relevant square brackets
                // fix all html inputs
                $.WeReviewApp.fixAllInputIframeDataOrHtmlToSquare();

                //send ajax request to draft save.
                $.WeReviewApp.ajaxDraftSaveApp()
                .done(function (data) {
                    // if successful then move to redirect page.
                    window.location.href = $.WeReviewApp.afterDraftPostRedirectPageUrl;
                })
                .fail(function (jqXHR, textStatus) {
                    $.WeReviewApp.$globalTopErrorLabel.text($.WeReviewApp.draftSavingFailedErrorMsg);
                });
            });
        },
        /*
         * This method is related to display contents 
         * when **only** app-editing page is ready (not submitting)
         * For submitting $.WeReviewApp.appEditingSubmitEvent method is called
         */
        appEditingPageOnReady: function () {
            var $formInputs = $.WeReviewApp.$appForm.find("select,input[name!=YoutubeEmbedLink]");
            //console.log($formInputs);

            $.devOrg.validateInputFromServer("#AppName", "/Validator/GetValidUrlEditing", "AppName", false, false, 3, true, " is invalid means that one app is already exist within this exact platform or category. You may change those to get a valid title and url.", null, $formInputs, $.WeReviewApp.maxTryInputSubmit);

            //stop form submitting the form if any file upload is not done.
            // before app editing submit method
            $.WeReviewApp.$appForm.submit($.WeReviewApp.appEditingSubmitEvent);
            // fix square brackets to html brackets
            $.WeReviewApp.invertAllInputIframeDataOrSquareToHtml();
        },
        /*
         * This method is related to display contents 
         * when **only** app-posting page is ready (not submitting)
         * for submitting $.WeReviewApp.appformPostEvent method is called.
         */
        appPostingPageOnReady: function () {
            $.devOrg.uxFriendlySlide("form.app-post", true, true);
            var $formInputs = $.WeReviewApp.$appForm.find("select,input[name!=YoutubeEmbedLink]");
            //console.log($formInputs);
            $.devOrg.validateInputFromServer("#AppName", "/Validator/GetValidUrl", "AppName", false, false, 3, true, " is invalid means that one app is already exist within this exact platform or category. You may change those to get a valid title and url.", null, $formInputs, $.WeReviewApp.maxTryInputSubmit);

            ///hiding the uploader on the app loader page for every time before posting a new app.
            $.WeReviewApp.$appForm.find($.WeReviewApp.selectorForUploaderRows).hide();

            // stop form submitting the form if any file upload is not done.
            $.WeReviewApp.$appForm.submit($.WeReviewApp.appformPostEvent);

            $.WeReviewApp.appFormDraftBtnClicked();

        },
        /**
         * App edit or post before action.
         * Determination point of app edit or post.
         */
        generalAppFormEditingOrPostingPageOnReady: function (e) {

            if ($.WeReviewApp.$appForm.length > 0) {
                $.WeReviewApp.$howtoUseUploaderInfoLabel.hide(); //hide uploader info label.

                if ($.WeReviewApp.$appFormPost.length > 0) {
                    // app posting
                    $.WeReviewApp.appPostingPageOnReady();

                    // Only sends to draft if in the app posting page.
                    $(window).bind('beforeunload', $.WeReviewApp.beforeUnloadEvent);
                } else if ($.WeReviewApp.$appFormEdit.length > 0) {
                    // app editing
                    $.WeReviewApp.appEditingPageOnReady();
                }

                // .app-editing-page class represent both editing and posting

                // Validate app-name
                $.devOrg.validateTextInputBasedOnRegEx("#AppName", "^([A-zZ.]+\\s*)+(\\d*)\\s*([aA-zZ.]+\\s*)+(\\d*)", "Sorry your app name is not valid. Valid name example eg. Plant Vs. Zombies v2.");

                $.devOrg.reSetupjQueryValidate("form");


                $.WeReviewApp.$appForm.find("input,textarea").change(function () {
                    $.WeReviewApp.appInputChangesExist = true;
                });

                $.WeReviewApp.$appForm.find("select").selectpicker();

                // enter to go next
                $.devOrg.enterToNextTextBox("form.app-editing-page", true); // means both editing and posting


                // triggering appname blur when change any of these.
                // Because all are related to URL generate.
                $(".selectpicker,select").change(function () {
                    $("#AppName").trigger("blur");

                });
                // to validate the app-name, triggering blur on app-name field
                $("#PlatformVersion").blur(function () {
                    $("#AppName").trigger("blur");
                    //console.log("dev");
                });
            }
        },


        /**
         * Processing review submit/save button click or submission process.
         */
        reviewFormSubmit: function (evt, $form) {
            evt.preventDefault(); //stop from submitting.

            //console.log("ase");
            var $submittingSpinner = null;
            var $inputs, currformData = 0;
            $submittingSpinner = $form.find("#submitting-review-spinner");
            var $failedIcon = $form.find("#submitting-review-failed-icon");
            // indicates if it is in the review posting page or in editing page\
            // $lastDiv.length == 0 indicates it's in review edit mode
            var $lastDiv = $form.find("div[data-last-slide=true]:visible");
            var url = $form.attr("action");
            var isInReviewPostingMode = (url === $.WeReviewApp.reviewFormSubmitUrl && $lastDiv.length > 0);
            // indicates in review edit mode
            var isFormSubmitUrlIsSameAsReviewSubmitUrl = url !== $.WeReviewApp.reviewFormSubmitUrl;

            if (isInReviewPostingMode || isFormSubmitUrlIsSameAsReviewSubmitUrl) {
                $inputs = $lastDiv.find("input");
                var $comment = $("#Comments");
                var commentValue = $comment.val();

                if ($.devOrg.checkValidInputs($inputs) && !_.isEmpty(commentValue)) {
                    // now we can submit, all inputs are valid.
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
                                // reload the page, because we can't change the review from here.
                                location.reload(true);
                                //$container.fadeOut("slow");
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
        /**
         * After clicking on "Write Review" in app-details page.
         * Everything stars from here.
         */
        askForReviewForm: function () {
            var $reviewSpinner = $($.WeReviewApp.reviewSpinnerSelector).hide();

            if ($reviewSpinner.length > 0) {
                $("#WriteReviewButton").click(function () {
                    var $container = $($.WeReviewApp.reviewFormContainerSelectorInAppPage);
                    var text = $container.text().trim();
                    if (text.length === 0) {
                        $container.hide();
                        $reviewSpinner.fadeIn("slow");
                        // load write form
                        var reqVerifyFieldsArray = $("#review-request-fields input").serializeArray();
                        //console.log(reqVerifyFields);
                        $.ajax({
                            type: "POST",
                            dataType: "html",
                            url: $.WeReviewApp.writeReviewFormUrl,
                            data: reqVerifyFieldsArray,
                            success: function (response) {
                                var selectForm = $.WeReviewApp.reviewFormContainerSelectorInAppPage + " form";
                                var $submittingSpinner = null;
                                var $response = $(response);
                                $container.html(response);

                                var $failedIcon = $("#submitting-review-failed-icon");
                                $failedIcon.hide();

                                $container.show("slow");

                                //var $form = $response.filter("form");
                                var $form = $container.find("form:first");

                                if ($form.length > 0) {
                                    $submittingSpinner = $("#submitting-review-spinner");
                                    $submittingSpinner.hide();

                                    //stop submitting and go through the processes and pages
                                    $.devOrg.uxFriendlySlide(
                                        selectForm,
                                        true,
                                        true //don't submit
                                        );

                                    // stop submitting , process review submit button actions
                                    // anonymous function would be faster 
                                    // however it would be dis-organized and since
                                    // it's only be used few times so it's okay.
                                    // note : $form.submit() doesn't work ! don't know why?
                                    //        it doesn't work because (may be) it is not in the page html.
                                    $form.submit(function (evt) {
                                        evt.preventDefault();
                                        //var $sendingForm = $(this);
                                        $.WeReviewApp.reviewFormSubmit(evt, $form);
                                    });

                                    //$container.find("button.btn.btn-success").click(function () {
                                    //    console.log("at place");
                                    //});


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
                });
            }
        },
        /**
         * App review : like-dislike functionality
         */
        reviewLikeDisLikeClicked: function () {
            var $likeBtns = $("#app-deails-page a[data-review-like-btn=true]");
            // Views/Reviews/ReviewsDisplay.cshtml contains that id
            var likeUrl = "/Reviews/Like";
            var dislikeUrl = "/Reviews/DisLike";
            // what happens when like or dislike is clicked
            // ajax request send
            var $spinners = null;

            function btnClicked($button, e, url, serializedInputs) {
                e.preventDefault();
                var reviewId = $button.attr("data-review-id");
                var data = serializedInputs + "&reviewId=" + reviewId;
                var sequence = $button.attr("data-sequence");
                var $spinnerForthisLike = $spinners.filter("#spinner-" + sequence);
                var isLikeBtn = $button.attr("data-review-like-btn");
                var $otherA = null;
                //console.log($button);

                //console.log(reviewId);

                if (isLikeBtn) {
                    $otherA = $.byId("review-thumbs-down-click-" + sequence);
                } else {
                    $otherA = $.byId("review-thumbs-up-click-" + sequence);
                }
                $button.hide();
                $spinnerForthisLike.show(); // show spinner until load
                console.log($spinnerForthisLike);

                function errorExecute(jqXHR, textStatus, errorThrown) {
                    $spinnerForthisLike.hide();
                    var $clone = $spinnerForthisLike.clone();
                    var $span = $clone.find("span");
                    var failedMessage = "like/dislike request failed , please refresh page. Reason : " + errorThrown;

                    $span.attr("class", "fa fa-times")
                        .attr("title", failedMessage);
                    $clone.attr("data-original-title", failedMessage)
                        .attr("title", failedMessage)
                        .show();
                    console.log(failedMessage);
                    $spinnerForthisLike.after($clone);
                }

                $.ajax({
                    type: "POST",
                    url: url,
                    data: data,
                    success: function (response) {
                        response = $.parseJSON(response);
                        $spinnerForthisLike.hide();
                        $button.show();
                        $otherA.find("i").removeClass("active");
                        if (response.isDone) {
                            $button.find("i").toggleClass("active");
                        } else if (!response.isDone) {
                            $button.find("i").removeClass("active");
                            //errorExecute(null, "Can't get the right response.", null);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        errorExecute(jqXHR, textStatus, errorThrown);
                    }
                }); // ajax end
            }
            if ($likeBtns.length > 0) {
                var $disLikeBtns = $("#app-deails-page a[data-review-dislike-btn=true]");
                var serializedData = $.byId("review-like-dislike-form-submit").serialize();
                $spinners = $(".spinner-for-like").hide(); //like btns
                $likeBtns.click(function (evt) {
                    var $button = $(this);
                    btnClicked($button, evt, likeUrl, serializedData);
                });
                //dislike btns
                $disLikeBtns.click(function (evt) {
                    var $button = $(this);
                    btnClicked($button, evt, dislikeUrl, serializedData);
                });
            }
        },

        suggestedOrReviewLoadmoreBtnLeft: function () {
            var $loadMoreBtn = $("#suggested-load-more-btn");
            var length, $appBox = 0;
            var showAfterCount = 5;
            var $appBoxes;
            var $div;
            var i;
            if ($loadMoreBtn.length > 0) {
                $div = $("#suggested-apps-list-div");
                $appBoxes = $div.find("div.appsbox[data-sequence]");
                length = $appBoxes.length;

                for (i = 0; i < length; i++) {
                    if (i >= showAfterCount) {
                        $appBox = $($appBoxes[i]);
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
                        $appBox = $($appBoxes[i]);
                        $appBox.show("slow");
                        $appBox.attr("data-hide", "false");
                    }
                    $loadMoreBtn.hide("slow");
                });
            }

            var $reviewLoadMoreBtn = $("#review-load-more-btn");
            var reviewShows = 4;
            if ($reviewLoadMoreBtn.length > 0) {
                $div = $("#review-collection");
                $appBoxes = $div.find("div.blogitembox[data-sequence]");
                length = $appBoxes.length;

                for (i = 0; i < length; i++) {
                    if (i >= showAfterCount) {
                        $appBox = $($appBoxes[i]);
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
                        $appBox = $($appBoxes[i]);
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
        * hides all uploader at first : $.WeReviewApp.$appForm.find("#collection-uploaders uploader-auto").hide();
        * modify $.WeReviewApp.appInputChangesExist based on user input
        * enter to next line bind : $.devOrg.enterToNextTextBox("form.app-editing-page", true);
        * bind with form submit-> which binds to $.WeReviewApp.appformPostEvent
        * draftbtnClicked : $.WeReviewApp.appFormDraftBtnClicked();
        * binds with beforeunload which binds with $.WeReviewApp.beforeUnloadEvent
        */
        executeActions: function () {
            $.WeReviewApp.generalAppFormEditingOrPostingPageOnReady();
            //don't require anymore , it has been called from the fron-end.js script
            //$.WeReviewApp.frontEndJavaScript();

            //data-last-slide="true"
            $.WeReviewApp.askForReviewForm();

            $.WeReviewApp.suggestedOrReviewLoadmoreBtnLeft();

            $.WeReviewApp.reviewLikeDisLikeClicked();
            $("#developers-organism").addClass("hide");
            // fix date inputs
            $.WeReviewApp.fixDateInputs();
        }
    };

    // this will call all the other events
    $.WeReviewApp.executeActions();
});