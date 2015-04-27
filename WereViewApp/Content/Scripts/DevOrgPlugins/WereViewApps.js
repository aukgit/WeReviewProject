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


$(function () {
    /// <summary>
    /// Were view app plug-in written by Alim Ul Karim
    /// </summary>
    $.WeReviewApp = {
        ///appForm represents both app-edit and app-posting form
        $appForm: $("form.app-editing-page:first"), // means both editing and posting
        $appFormEdit: $("form.app-edit:first"),
        $appFormPost: $("form.app-post:first"),
        $allInputs: $("form.app-post:first input"),
        ajaxDraftPostUrl: "/App/SaveDraft",
        $appPageUploaderNotifier: $("label.notify-global-info"),
        homePageUrl: "/",
        selectorForUploaderRows: "#collection-uploaders .form-row-uploader",
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
        getAttributeRemoveRegularExpressionFor:function(attributeName) {
            return "("+ attributeName +".*=.*[\"\"'])([a-zA-Z0-9:;\.\s\(\)\-\,]*)([\"\"'])";
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
        fixYouTubeVideoPropertise:function() {
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


                $.WeReviewApp.$appForm.find("input,textarea").change(function() {
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

            if ((url === $.WeReviewApp.reviewFormSubmitUrl && $lastDiv.length > 0) || url !== $.WeReviewApp.reviewFormSubmitUrl) {
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
                                var $form = $response.filter("form");

                                if ($form.length > 0) {

                                    $submittingSpinner = $response.find("#submitting-review-spinner");
                                    $submittingSpinner.hide();

                                    $container.find("form").submit($.WeReviewApp.reviewFormSubmit);
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

            if ($likeBtns.length > 0) {
                $likeBtns.click(btnClicked);
            }

            if ($disLikeBtns.length > 0) {
                $disLikeBtns.click(btnClicked);
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