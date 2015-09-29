/// <reference path="developers-organism.component.js" />
/// <reference path="byId.js" />
/// <reference path="../jquery-2.1.4.intellisense.js" />
/**
 * Written by Alim Ul Karim
 * Developers Organism
 * Written  : 14 Nov 2014
 * Modified : 28 Sep 2015
 */

/// <summary>
/// Were view app plug-in written by Alim Ul Karim
/// </summary>
$.WeReviewApp = {
    ///appForm represents both app-edit and app-posting form
    ///appFormSelector : form.app-editing-page
    $appFormWrapper: [],
    $appForm: [],//$("form.app-editing-page"), // means both editing and posting
    // appFormEdit selector : form.app-edit
    $appFormEdit: [], //$("form.app-edit"),
    // app form post  :form.app-post
    $appFormPost: [], //$("form.app-post"),
    // all input inside form app-port
    $allInputs: [],// $("form.app-post input"),
    ajaxDraftPostUrl: "/App/SaveDraft",
    $appPageUploaderNotifier: $.byId("notify-global-info-second"),
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
    ///consist of # : "#app-details-page"
    appDetailsPageParentId: "#app-details-page",
    friendlyUrlRegularExpression: "[^A-Za-z0-9_\.~]+",

    getFriendlyUrlSlug: function (str) {
        /// <summary>
        /// Returns friendly url slug from given string
        /// Hello World -> hello-world
        /// </summary>
        /// <param name="str"></param>
        //"[^A-Za-z0-9_\.~]+"
        if (_.isEmpty(str) === false) {
            var regexString = $.WeReviewApp.friendlyUrlRegularExpression;
            str = str.trim();
            var regExp = new RegExp(regexString, "gi");
            return str.replace(regExp, "-");
        } else {
            return "";
        }
    },
    /**
     * single input IFRAME code HTML  to Square
     */
    fixIframeTag: function ($jQueryInputText) {
        //<iframe width="560" height="315" src="//www.youtube.com/embed/ob-P2a6Mrjs" frameborder="0" allowfullscreen></iframe>
        var currentText = $jQueryInputText.val();
        //currentText = currentText.toLowerCase();
        var reg = new RegExp("<iframe", "gi");
        currentText = currentText.replace(reg, "[iframe");
        reg = new RegExp("</iframe>", "gi");
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
        var reg = new RegExp("\\[iframe", "gi");
        currentText = currentText.replace(reg, "<iframe");
        reg = new RegExp("\\[/iframe\\]", "gi");
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
        var self = $.WeReviewApp;

        function raiseUploaderInvalidMessage(failedBoolean) {
            if (failedBoolean) {
                self.$appPageUploaderNotifier.text("Please upload all necessary files to proceed next.");
            } else {
                self.$appPageUploaderNotifier.text("");
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

        if (self.$appFormPost.length > 0) {
            e.preventDefault();

            // only check uploader when posting time

            // first check if form is valid or not.
            var visibleInputsExceptFile = self.$allInputs.filter("[type!=file]:visible");
            var len = visibleInputsExceptFile.length;
            if (!self.isAppTitleValid()) {
                self.$globalTopErrorLabel.text("Please fill out the title correctly. It's very important for your app.");
                self.$globalTopErrorLabel.show();
                return;
            }

            for (var i = 0; i < len; i++) {
                var $singleInput = $(visibleInputsExceptFile[i]);
                if (!$singleInput.valid()) {
                    self.$globalTopErrorLabel.text("Please fill out the required fields.");
                    return;
                }
            }

            self.$globalTopErrorLabel.text("");

            // first check if the uploaders are visible or not
            var $uploaderRows = self.$appForm.find(self.selectorForUploaderRows);

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
                    if (self.$howtoUseUploaderInfoLabel.is(":hidden")) {
                        self.$howtoUseUploaderInfoLabel.show();
                    }
                    raiseUploaderInvalidMessage(ifAnyUploadfails);

                    $uploaderRow.show("slow");
                    return;
                }
            }


            // checking uploadeers if valid
            var $uploaders = self.$appForm.find("#collection-uploaders");
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

            if (!ifAnyUploadfails && self.isAppTitleValid()) {
                //everything is successful
                self.appInputChangesExist = false;
                self.fixAllInputIframeDataOrHtmlToSquare();
                // all conditions fulfilled so submit the form
                this.submit();
            }

        }
        //function preventDefaultInside(evt, formCanbeSent) {
        //    if (!ifAnyUploadfails && formCanbeSent) {
        //        // all uploads has been done.
        //        self.appInputChangesExist = false; // no change exist on the unbin method ... direct submit.
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
        var self = $.WeReviewApp;
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
        var self = $.WeReviewApp;
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
        var self = $.WeReviewApp;
        //currentText = currentText.toLowerCase();
        var heightRegEx = self.getAttributeRemoveRegularExpressionFor("Height");
        var widthRegEx = self.getAttributeRemoveRegularExpressionFor("Width");

        var reg = new RegExp(heightRegEx, "gi");
        currentText = currentText.replace(reg, "");
        reg = new RegExp(widthRegEx, "gi");
        currentText = currentText.replace(reg, "");
        $jQueryInputText.val(currentText);
    },

    fixYouTubeVideoPropertise: function () {
        var inputSelectors = "input.url-input";
        var self = $.WeReviewApp;
        var inputFields = self.$appForm.find(inputSelectors);
        if (inputFields.length > 0) {
            for (var i = 0; i < inputFields.length; i++) {
                var $eachInputfield = $(inputFields[i]);
                self.removeHeightWidthAttributes($eachInputfield);
            }
        }
    },

    /// it doesn't include fixing html inputs
    /// return as ajax response, add methods like success or fail to do something with it.
    ajaxDraftSaveApp: function (e) {
        var self = $.WeReviewApp,
            formData = self.$appForm.serializeArray();

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
        var self = $.WeReviewApp;
        if (self.appInputChangesExist) {

            if (self.$appFormPost.length > 0) {
                // app posting page
                // send as ajax post
                if (self.sendingDraftNumber <= self.numberOfDraftPossible) {
                    // fix all html inputs
                    self.fixAllInputIframeDataOrHtmlToSquare();

                    self.ajaxDraftSaveApp();
                }

            }// app posting page if else end.


            return "Are you sure you wanted to leave? Your app will be saved as a draft if you leave (you can have up to " + self.numberOfDraftPossible + " draft posts).";
        }
    },
    /**
     * When draft button is clicked from app-posting page.
     */
    appFormDraftBtnClicked: function () {
        var self = $.WeReviewApp;
        self.$appForm.find("#draft-btn").click(function (e) {
            e.preventDefault();
            self.appInputChangesExist = false;
            // fix html input type to relevant square brackets
            // fix all html inputs
            self.fixAllInputIframeDataOrHtmlToSquare();

            //send ajax request to draft save.
            self.ajaxDraftSaveApp()
            .done(function (data) {
                // if successful then move to redirect page.
                window.location.href = self.afterDraftPostRedirectPageUrl;
            })
            .fail(function (jqXHR, textStatus) {
                self.$globalTopErrorLabel.text(self.draftSavingFailedErrorMsg);
            });
        });
    },
    /*
     * This method is related to display contents 
     * when **only** app-editing page is ready (not submitting)
     * For submitting $.WeReviewApp.appEditingSubmitEvent method is called
     */
    appEditingPageOnReady: function () {
        var self = $.WeReviewApp;
        var $formInputs = self.$appForm.find("select,input[name!=YoutubeEmbedLink]");
        //console.log($formInputs);

        //$.devOrg.validateInputFromServer("#AppName", "/Validator/GetValidUrlEditing", "AppName", false, false, 3, true, " is invalid means that one app is already exist within this exact platform or category. You may change those to get a valid title and url.", null, $formInputs, self.maxTryInputSubmit);

        //stop form submitting the form if any file upload is not done.
        // before app editing submit method
        self.$appForm.submit(self.appEditingSubmitEvent);
        // fix square brackets to html brackets
        self.invertAllInputIframeDataOrSquareToHtml();
    },

    appNameOnBlur: function () {
        /// <summary>
        /// What happens when appname field is blured
        /// Getting tags from app title
        /// </summary>
        /// <returns type=""></returns>
        var $appNameInput = $.byId("AppName"),
            $tagsInput = $.byId("Tags"),
            previousText = null;
        //var $formInputs = self.$appForm.find("select,input[name!=YoutubeEmbedLink]");

        $appNameInput.blur(function () {
           var currentMethod = setTimeout(function () {
                var value = $appNameInput.val(),
                    tagsArray = value.split(" "),
                    tagsExistingText = $tagsInput.val(),
                    existingTagsArray = tagsExistingText.split(",");

                if (previousText !== value) {
                    previousText = value;
                    if (!_.isEmpty(value)) {
                        tagsArray.push(value.trim());
                    }
                    if (existingTagsArray.length > 0) {
                        for (var i = 0; i < existingTagsArray.length; i++) {
                            var element = existingTagsArray[i];
                            if (!_.isEmpty(element)) {
                                tagsArray.push(element.trim());
                            }
                        }
                    }
                    var uniqueArray = _.without(_.uniq(tagsArray), "");
                    //console.log(tagsArray);
                    //console.log(uniqueArray);
                    var tags = uniqueArray.join(",");
                    $tagsInput.val(tags);
                }
               clearTimeout(currentMethod);
           }, 500);

        });
    },

    /*
     * This method is related to display contents 
     * when **only** app-posting page is ready (not submitting)
     * for submitting $.WeReviewApp.appformPostEvent method is called.
     */
    appPostingPageOnReady: function () {
        var self = $.WeReviewApp;
        $.devOrg.uxFriendlySlide("form.app-post", true, true);



        ///hiding the uploader on the app loader page for every time before posting a new app.
        self.$appForm.find(self.selectorForUploaderRows).hide();

        // stop form submitting the form if any file upload is not done.
        self.$appForm.submit(self.appformPostEvent);

        self.appFormDraftBtnClicked();

    },
    /**
     * App edit or post before action.
     * Determination point of app edit or post.
     */
    routeAppEditingFormAndInitialize: function (e) {
        /// <summary>
        /// Route app editing form and initialize all other methods.
        /// </summary>
        /// <param name="e"></param>
        var self = $.WeReviewApp;
        // routing
        if (self.$appForm.length > 0) {
            self.$howtoUseUploaderInfoLabel.hide(); //hide uploader info label.

            //bind with blur event of AppName
            self.appNameOnBlur();

            if (self.$appFormPost.length > 0) {
                // app posting
                self.appPostingPageOnReady();

                // Only sends to draft if in the app posting page.
                $(window).bind("beforeunload", self.beforeUnloadEvent);
            } else if (self.$appFormEdit.length > 0) {
                // app editing
                self.appEditingPageOnReady();
            }

            // .app-editing-page class represent both editing and posting

            // Validate app-name
            $.devOrg.validateTextInputBasedOnRegEx(
                "#AppName",
                "^([A-zZ.]+\\s*)+(\\d*)\\s*([aA-zZ.]+\\s*)+(\\d*)",
                "Sorry your app name is not valid. Valid name example eg. Plant Vs. Zombies v2.");

            $.devOrg.reSetupjQueryValidate(self.$appForm);


            self.$appForm.find("input,textarea").change(function () {
                /// app anything change marked as edited
                self.appInputChangesExist = true;
            });

            self.$appForm.find("select").selectpicker(); // make selectpicker

            // enter to go next
            $.devOrg.enterToNextTextBoxWithoutTags(self.$appForm, true, false); // means both editing and posting

            var triggerAppNameValidateTimeOut,
                appTitleValidate = function () {
                    triggerAppNameValidateTimeOut = setTimeout(function () {
                        $("#AppName").trigger("blur");
                        clearTimeout(triggerAppNameValidateTimeOut);
                    }, 300);
                };
            // triggering appname blur when change any of these.
            // Because all are related to URL generate.
            $(".selectpicker,select").change(appTitleValidate);
            // to validate the app-name, triggering blur on app-name field
            $("#PlatformVersion").blur(appTitleValidate);
            
        }
    },


    /**
     * Processing review submit/save button click or submission process.
     */
    reviewFormSubmit: function (evt, $form) {
        evt.preventDefault(); //stop from submitting.
        var self = $.WeReviewApp;
        //console.log("ase");
        var $submittingSpinner = null;
        var $inputs, currformData = 0;
        $submittingSpinner = $.byId("submitting-review-spinner");
        var $failedIcon = $.byId("submitting-review-failed-icon");
        // indicates if it is in the review posting page or in editing page\
        // $lastDiv.length == 0 indicates it's in review edit mode
        var $lastDiv = $form.find("div[data-last-slide=true]:visible");
        var url = $form.attr("action");
        var isInReviewPostingMode = (url === self.reviewFormSubmitUrl && $lastDiv.length > 0);
        // indicates in review edit mode
        var isFormSubmitUrlIsSameAsReviewSubmitUrl = url !== self.reviewFormSubmitUrl;

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
        var self = $.WeReviewApp;
        var $reviewSpinner = $(self.reviewSpinnerSelector).hide();

        if ($reviewSpinner.length > 0) {
            $("#WriteReviewButton").click(function () {
                var $container = $(self.reviewFormContainerSelectorInAppPage);
                var text = $container.text().trim();
                if (text.length === 0) {
                    $container.hide();
                    $reviewSpinner.fadeIn("slow");
                    // load write form
                    var reqVerifyFieldsArray = $("#review-request-fields").find("input").serializeArray();
                    //console.log(reqVerifyFields);
                    $.ajax({
                        type: "POST",
                        dataType: "html",
                        url: self.writeReviewFormUrl,
                        data: reqVerifyFieldsArray,
                        success: function (response) {
                            var selectForm = self.reviewFormContainerSelectorInAppPage + " form";
                            var $submittingSpinner = null;
                            //var $response = $(response);
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
                                    self.reviewFormSubmit(evt, $form);
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
        var $likeBtns = $.byId("app-details-page").find("a[data-review-like-btn=true]");
        // Views/Reviews/ReviewsDisplay.cshtml contains that id
        var likeUrl = "/Reviews/Like";
        var dislikeUrl = "/Reviews/DisLike";
        // what happens when like or dislike is clicked
        // ajax request send
        var $spinners = null;

        var btnClicked = function ($button, e, url, serializedInputs) {
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
            var $disLikeBtns = $.byId("app-details-page").find("a[data-review-dislike-btn=true]");
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
            $appBoxes = $div.find(".appsbox[data-sequence]");
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
                var $appBoxesHidden = $appBoxes.filter("[data-hide=true]");
                for (var i = 0; i < length; i++) {
                    $appBox = $($appBoxesHidden[i]);
                    $appBox.show("slow");
                    $appBox.attr("data-hide", "false");
                }
                $loadMoreBtn.hide("slow");
            });
        }

        var $reviewLoadMoreBtn = $("#review-load-more-btn");
        //var reviewShows = 4;
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

    adminArea: function () {
        /// <summary>
        /// All codes related to admin area
        /// </summary>
        var $adminArea = $.byId("admin-area");
        var i = 0;
        if ($adminArea.length > 0) {
            var controllers = [
                {
                    execute: function category() {
                        var $categoryPage = $.byId("app-category-editing-page");
                        if ($categoryPage.length > 0) {
                            var $slug = $.byId("Slug");
                            var $category = $.byId("CategoryName");
                            $category.keyup(function () {
                                var slugString = $.WeReviewApp.getFriendlyUrlSlug($category.val());
                                $slug.val(slugString);
                            });
                        }
                    }
                }
            ];

            for (i = 0; i < controllers.length; i++) {
                controllers[i].execute();
            }
        }
    },
    initializeAppForms: function () {
        var self = $.WeReviewApp;
        self.$appFormWrapper = $.byId("app-form");
        var $fromWrapper = self.$appFormWrapper;
        if ($fromWrapper.length > 0) {
            var dataType = $fromWrapper.attr("data-type");
            self.$appForm = $fromWrapper.find(".app-editing-page");
            if (dataType === "post") {
                self.$appFormPost = self.$appForm;
                self.$allInputs = self.$appForm.find("input");
            } else if (dataType === "edit") {
                self.$appFormEdit = self.$appForm;
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
        /// <summary>
        /// Runs every other method.
        /// </summary>
        var self = $.WeReviewApp;
        self.initializeAppForms();
        self.routeAppEditingFormAndInitialize();
        //don't require anymore , it has been called from the fron-end.js script
        //self.frontEndJavaScript();

        //data-last-slide="true"
        self.askForReviewForm();

        self.suggestedOrReviewLoadmoreBtnLeft();

        self.reviewLikeDisLikeClicked();
        $.byId("developers-organism").addClass("hide");
        // fix date inputs
        self.fixDateInputs();

        // run other pages
        self.adminArea();

    }
};
$(function () {
    // this will call all the other events
    $.WeReviewApp.executeActions();
});
