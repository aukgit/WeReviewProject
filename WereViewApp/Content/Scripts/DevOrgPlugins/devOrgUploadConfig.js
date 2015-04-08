/// <reference path="jquery-1.10.2.js" />
/// <reference path="jquery-1.10.2.intellisense.js" />
/// <reference path="jquery.fileupload.js" />
/// <reference path="jquery.iframe-transport.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery.ui.widget.js" />
/// <reference path="modernizr-2.6.2.js" />
/// <reference path="bootstrap.js" />
/// <reference path="bootstrap-progressbar.js" />
/// <reference path="I:\WereViewApp\WereViewProject\WereViewApp\Scripts/underscore.js" />

$(function () {
    'use strict';



    $.devOrgUP = {

        uploadedFilesNotificationLabelSelector: "label.file-uploaded-notify-label-",
        editBtnSpinnerSelector: "[data-edit-btn-spinner=true]",
        progressorSpinnerSelector: "[data-progressor-spinner=true]",
        wholeProgressorSelector: "[data-progressor-div=true].uploader-progress-info",

        preUploadedFilesMessage: "files exist.",
        uploadedFilesMessage: "uploaded successfully.",

        failedEvent: "fileuploadfail",
        progressEvent: "fileuploadprogressall",
        doneEvent: "fileuploaddone",
        submitEvent: "fileuploadsubmit",
        fileUploadAddedEvent: "fileuploadadd",

        filesAreAddedForUploadEvent: "fileuploadadd",
        filesAlwaysProcessEvent: "fileuploadprocessalways",
        uploaderInputClass: ".dev-fileupload",

        maxUploadParameter: "data-max-upload-count",
        uploadNumberParameter: "data-uploaded-count",
        uploadPreexistCountParameter: "data-upload-pre-exist-count",
        hasEditButtonParameter: "data-has-edit-btn",


        $forms: $("form"),
        formData: $("form input[type='file']").closest("form").serializeArray(),
        //$uploaderDivRow: $("form div[data-uploader-div-row=true].form-row-uploader"),
        $uploaderWorkingDiv: $("form div.uploader"),
        $allFileInputTypes: $("form div.uploader>span.fileinput-button>input[type='file']"),
        $allSpinners: $("form div.uploader a[data-spinner=spinner].spinner"),
        $allWholeProgressor: $("form div.uploader div[data-progressor-div=true].uploader-progress-info"),
        $allProgressorValueIdicator: $("form div.uploader div[data-progressor-div=true].uploader-progress-info>a[data-progressor-value=true]"),
        $allLabelsToIndicateUploadedFilesNumber: $("form div.uploader>label[data-label-file-uploaded=true]"),
        $allEditButtons: $("form div.uploader>a[data-btn=edit].edit-btn"),

        $allSuccessIcons: $("form div.uploader>a[data-success-icon=true]"),
        $allFailedIcons: $("form div.uploader>a[data-failed-icon=true]"),

        $allErrorsRelatedTags: $("form div.uploader>[data-error-related=true]"),
        $allULErrorsRelatedTags: $("form div.uploader>ul[data-error-related=true]"),

        initializeHide: function () {
            // only hide edit spinner
            $.devOrgUP.$allSpinners.filter(".edit-btn-spinner").hide();
            $.devOrgUP.$allWholeProgressor.hide();
            //$.devOrgUP.$allProgressorValueIdicator.hide();
            $.devOrgUP.$allEditButtons.hide();
            $.devOrgUP.$allSuccessIcons.hide();
            $.devOrgUP.$allFailedIcons.hide();
            $.devOrgUP.$allErrorsRelatedTags.hide();
        },
        getSuccessIcon: function (id) {
            /// <summary>
            /// returns a tag.
            /// </summary>
            /// <param name="id">number of the data-id attribute</param>
            return $.devOrgUP.$allSuccessIcons.filter("[data-id=" + id + "]");
        },
        showAllErrorDisplay: function (id) {
            var $errorSpecficDetailBoxes = $.devOrgUP.$allErrorsRelatedTags.filter("[data-id=" + id + "]");

            if ($errorSpecficDetailBoxes.is(":hidden")) {
                $errorSpecficDetailBoxes.fadeIn("slow");
            }
        },

        getErrorUL: function (id) {
            /// <summary>
            /// returns ul.
            /// </summary>
            /// <param name="id">number of the data-id attribute</param>
            return $.devOrgUP.$allULErrorsRelatedTags.filter("[data-id=" + id + "]");
        },

        addErrorInfoInUL: function ($ul, msg, title) {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="$ul"></param>
            /// <param name="msg"></param>
            /// <param name="title"></param>
            $ul.append('<li class="label label-danger block" title="' + title + '">' + msg + '</li>');
        },



        showSuccessIcon: function (id) {
            return $.devOrgUP.getSuccessIcon(id).show("slow");
        },
        hideSuccessIcon: function (id) {
            return $.devOrgUP.getSuccessIcon(id).hide();
        },


        getFailedIcon: function (id) {
            /// <summary>
            /// returns a tag.
            /// </summary>
            /// <param name="id">number of the data-id attribute</param>
            return $.devOrgUP.$allFailedIcons.filter("[data-id=" + id + "]");
        },

        setSuccessIconMsg: function (id, msg) {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="msg"></param>
            var $icon = $.devOrgUP.getSuccessIcon(id);
            $icon.attr("data-original-title", msg);
            return $icon.attr("title", msg).tooltip();
        },

        setFailedIconMsg: function (id, msg) {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <param name="msg"></param>
            var $icon = $.devOrgUP.getFailedIcon(id);
            $icon.attr("data-original-title", msg);
            return $icon.attr("title", msg).tooltip();
        },

        showFailedIcon: function (id) {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            return $.devOrgUP.getFailedIcon(id).show("slow");
        },

        hideFailedIcon: function (id) {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            return $.devOrgUP.getFailedIcon(id).hide();
        },

        getLabelToIndicateUploadedFiles: function (id) {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id"></param>
            /// <returns type=""></returns>
            return $.devOrgUP.$allLabelsToIndicateUploadedFilesNumber.filter("[data-id=" + id + "]");
        },

        getEditButton: function (id) {
            /// <summary>
            /// returns a tag.
            /// </summary>
            /// <param name="id">number of the data-id attribute</param>
            return $.devOrgUP.$allEditButtons.filter("[data-id=" + id + "]");
        },
        getEditSpinner: function (id) {
            /// <summary>
            /// returns a tag.
            /// </summary>
            /// <param name="id">number of the data-id attribute</param>
            /// <returns type=""></returns>
            return $.devOrgUP.$allSpinners.filter($.devOrgUP.editBtnSpinnerSelector + "[data-id=" + id + "]");
        },



        getInputFile: function (id) {
            /// <summary>
            /// returns input tag with file
            /// </summary>
            /// <param name="id">number of the data-id attribute</param>
            /// <returns type=""></returns>
            return $.devOrgUP.$allFileInputTypes.filter("[data-id=" + id + "]");
        },

        getUploaderSpinner: function (id) {
            /// <summary>
            /// returns the whole 'A' tag / progressor spinner for upload
            /// </summary>
            /// <param name="id">number of the data-id attribute</param>
            /// <returns type=""></returns>
            return $.devOrgUP.$allSpinners.filter($.devOrgUP.editBtnSpinnerSelector + "[data-id=" + id + "]");
        },

        getProgressorValudeIndicator: function (id) {
            /// <summary>
            /// returns A tag inside there will be an span for the value show.
            /// </summary>
            /// <param name="id">number of the data-id attribute</param>
            /// <returns type=""></returns>
            return $.devOrgUP.$allProgressorValueIdicator.filter("[data-id=" + id + "]");
        },
        getWholeProgressorDiv: function (id) {
            /// <summary>
            /// returns a div
            /// </summary>
            /// <param name="id">number of the data-id attribute</param>
            /// <returns type=""></returns>
            return $.devOrgUP.$allWholeProgressor.filter("[data-id=" + id + "]");
        },





        hideUploadProgressor: function (id) {
            /// <summary>
            /// hides the whole progressor div.
            /// </summary>
            /// <param name="id">number of the data-id attribute</param>
            /// <returns type=""></returns>
            var $wholeDiv = $.devOrgUP.getWholeProgressorDiv(id);
            if ($wholeDiv.is(":visible")) {
                $wholeDiv.hide();
            }
        },

        showUploadProgressor: function (id) {
            /// <summary>
            /// show the whole progressor div.
            /// </summary>
            /// <param name="id">number of the data-id attribute</param>
            /// <returns type=""></returns>
            $.devOrgUP.getWholeProgressorDiv(id).fadeIn("slow");


        },

        setProgressorValue: function (id, val) {
            /// <summary>
            /// sets the value of the progresssor, it doesn't do the visible thing.
            /// </summary>
            /// <param name="id">data-id</param>
            /// <param name="val">only give the number between 1-100</param>
            var $indicator = $.devOrgUP.getProgressorValudeIndicator(id);
            if ($indicator.length > 0) {
                $indicator.attr("title", val + "% done.");
                $indicator.attr("data-original-title", val + "% done.");
                return $indicator.find("span").text(val + "%");
            }
            return $indicator;
        },

        setTextInLabel: function ($label, val) {
            /// <summary>
            /// set text to the results display labels. It also contains the uploaded files number.
            /// </summary>
            /// <param name="id">data-id</param>
            /// <param name="val">Any text. Empty text hides the label.</param>
            /// <returns type="$label">Returns the label.</returns>
            if ($label.length > 0) {
                if (val === null || val === undefined || val === "" || val.length == 0) {
                    // empty
                    $label.hide();
                    $label.text("");
                } else {
                    if ($label.is(":hidden")) {
                        $label.show("slow");
                    }
                    $label.text(val);
                }
            }
            return $label;
        },
        setTextInLabelWithUploadNumber: function ($label, val, increaseTheNumberOfUpload) {
            /// <summary>
            /// set text to the results display labels. It also contains the uploaded files number.
            /// Will not update count more than max.
            /// </summary>
            /// <param name="id">data-id</param>
            /// <param name="val">Any text. Empty text hides the label.</param>
            /// <param name="increaseTheNumberOfUpload">increases the value of the upload value</param>
            /// <returns type="">
            /// return updated count value.
            /// </returns>

            var currentUploadCount = parseInt($label.attr($.devOrgUP.uploadNumberParameter));
            var maxUploadCount = parseInt($label.attr($.devOrgUP.maxUploadParameter));
            if (_.isNaN(maxUploadCount)) {
                maxUploadCount = 1;
            }
            if (_.isNull(increaseTheNumberOfUpload) || _.isNaN(increaseTheNumberOfUpload) || _.isUndefined(increaseTheNumberOfUpload)) {
                increaseTheNumberOfUpload = 0;
            }
            var updatedValue = currentUploadCount + increaseTheNumberOfUpload;
            if (updatedValue > maxUploadCount) {
                updatedValue = maxUploadCount;
            }
            if (updatedValue < 0) {
                updatedValue = 0;
            }
            $label.attr($.devOrgUP.uploadNumberParameter, updatedValue);
            $label.text(updatedValue + " " + val);
            return updatedValue;
        },

        setTextInLabelWithPreUploadNumber: function ($label, val, preuploadedCount) {
            /// <summary>
            /// set text to the results display labels. Sets the pre
            /// </summary>
            /// <param name="id">data-id</param>
            /// <param name="val">Any text. Empty text hides the label.</param>
            /// <param name="preuploadedCount">increases the value of the upload value</param>
            $label.attr($.devOrgUP.preuploadedCount, preuploadedCount);
            $label.text(preuploadedCount + " " + val);
        },

        showEditButton: function (id) {
            /// <summary>
            /// show the whole progressor div.
            /// </summary>
            /// <param name="id">number of the data-id attribute</param>
            /// <returns type=""></returns>
            return $.devOrgUP.getEditButton(id).show("slow");
        },

        hideEditButton: function (id) {
            /// <summary>
            /// show the whole progressor div.
            /// </summary>
            /// <param name="id">number of the data-id attribute</param>
            /// <returns type=""></returns>
            $.devOrgUP.getEditButton(id).hide();
        },

        showEditProgressor: function (id) {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id">number of the data-id attribute</param>
            /// <returns type=""></returns>
            return $.devOrgUP.getEditSpinner(id).fadeIn("slow");
        },

        hideEditProgressor: function (id) {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="id">number of the data-id attribute</param>
            /// <returns type=""></returns>
            return $.devOrgUP.getEditSpinner(id).hide();
        },



        uploadDeleteBtnClicked: function (e, $deleteBtn, $editBtn, id, sequence, $imageRow, $uploaderLabel, $uploaderInput) {
            var url = $deleteBtn.attr("href");
            var count = 0;
            $.ajax({
                type: "GET",
                dataType: "json",
                url: url,
                success: function (response) {
                    if (response) {
                        // removed
                        $imageRow.hide('slow', function () {
                            $imageRow.remove();
                        });
                        count = $.devOrgUP.setTextInLabelWithUploadNumber($uploaderLabel, $.devOrgUP.uploadedFilesMessage, -1);
                        if (count <= 0) {
                            $editBtn.hide();
                        }
                    }
                },
                error: function (xhr, status, error) {

                }
            }); // ajax end

        },


        uploadEditBtnClicked: function (e, $editBtn, id, $uploaderLabel, $uploaderInput) {
            e.preventDefault();
            var url = $editBtn.attr("href");

            var $spinner = $.devOrgUP.showEditProgressor(id);


            $.ajax({
                type: "POST",
                dataType: "html",
                url: url,
                data: $.devOrgUP.formData,
                success: function (response) {
                    // Remove the processing state     
                    //$editList.html(response);
                    var $response = $(response);
                    $response.modal();
                    //inside modal find delete btns and bind it with delete event.

                    var deleteBtns = $response.find("a[data-btn='delete']");
                    deleteBtns.on('click', function (ew) {
                        ew.preventDefault();
                        var $deleteButton = $(this);
                        var sequence = $deleteButton.attr("data-sequence");

                        var $imageRow = $response.find("div.row[data-id='" + id + "'][data-sequence='" + sequence + "']");

                        $.devOrgUP.uploadDeleteBtnClicked(e, $deleteButton, $editBtn, id, sequence, $imageRow, $uploaderLabel, $uploaderInput);
                    });


                    //$response.find("[data-btn='close']").on('click', modelCloseClicked)
                    $response.on('hidden.bs.modal', modelCloseClicked);
                    $spinner.hide();
                },
                error: function (xhr, status, error) {
                    // Remove the processing state
                    $spinner.hide();
                    //console.error("Error occurred when drafting app. Err Msg:" + error);
                }
            }); // ajax end

            function modelCloseClicked() {
                $("body").removeClass('modal-open');
                $('.modal-backdrop').remove();
                $('.modal').remove();
            }


        },

        uploaderEditBtnClickEvntBinder: function ($editBtn, $label, $uploaderInput, id) {
            /// <summary>
            /// It's not the actual event but it's the method which binds edit button with click event.
            /// Hides unnecessary edit buttons
            /// </summary>
            /// <param name="$editBtn">
            /// </param>
            /// <param name="$label">
            /// </param>
            /// <param name="$uploaderInput">
            /// </param>
            /// <param name="id">
            /// </param>

            if ($editBtn.length > 0) {
                $editBtn.on('click', function (e) {
                    e.preventDefault();
                    $.devOrgUP.uploadEditBtnClicked(e, $editBtn, id, $label, $uploaderInput);
                });
            }
        },

        uploaderFixingDataUrlOnInvalidUrls: function ($uploader) {
            var currentUrl = $uploader.attr("data-url");
            if (_.isEmpty(currentUrl)) {
                var $form = $uploader.closest("form");
                formUrl = $form.attr("action");
                $uploader.attr("data-url", formUrl);
            }
        },

        isPreuploadExist: function ($label) {
            var count = parseInt($label.attr($.devOrgUP.uploadPreexistCountParameter));
            if (count > 0) {
                return true;
            }
            return false;
        },
        getPreuploadValue: function ($label) {
            var count = parseInt($label.attr($.devOrgUP.uploadPreexistCountParameter));
            return count;
        },

        getCountOfHowManyFilesUploaded: function (id) {
            var $label = $.devOrgUP.getLabelToIndicateUploadedFiles(id);
            var count = parseInt($label.attr($.devOrgUP.uploadNumberParameter));
            return count;
        },
        showEditButtonBasedOnPreUploadNShowMessageOnLabel: function ($label, id) {
            /// <summary>
            /// Fix uploaded counts from preexist count.
            /// </summary>
            /// <param name="$label">
            /// 
            /// </param>
            /// <param name="id">
            /// 
            /// </param>
            /// <returns type="">
            /// return preupload count.
            /// </returns>
            var count = parseInt($label.attr($.devOrgUP.uploadPreexistCountParameter));
            var hasEditbutton;
            var $editBtn;
            if (count > 0) {
                //if any preload exist then

                // set pre load to load
                $label.attr($.devOrgUP.uploadNumberParameter, count);

                hasEditbutton = $label.attr($.devOrgUP.hasEditButtonParameter);

                //show edit buttn
                if (hasEditbutton) {
                    $editBtn = $.devOrgUP.getEditButton(id);
                    $editBtn.fadeIn("slow");
                }

                //var $label = $.devOrgUP.getLabelToIndicateUploadedFiles(id);

                //set msg
                $.devOrgUP.setTextInLabelWithPreUploadNumber($label, $.devOrgUP.preUploadedFilesMessage, count);
                return count;
            }
            return 0;
        },
        getId: function ($uploaderItem) {
            return $uploaderItem.attr("data-id");
        },
        initialize: function (acceptedFileSizeInMB, acceptFileTypeRegularExpressionString) {
            //var urlExist = false;
            //var kRep = 0;

            var uploadersLength = 0;
            var actualSize = acceptedFileSizeInMB * 1024000;
            var $executingUploader = null;
            var id = 0;
            var $uploaderDiv = $.devOrgUP.$uploaderWorkingDiv;
            var $label = null;
            var $editBtn = null;

            var $singleUploader = null;

            if ($uploaderDiv.length === 0) {
                return;
            }
            var $uploaders = $.devOrgUP.$allFileInputTypes.filter($.devOrgUP.uploaderInputClass);

            uploadersLength = $uploaders.length;
            if (uploadersLength > 0) {

                // hide all necessary objects
                $.devOrgUP.initializeHide();

                //$.devOrgUP.uploaderFixingDataUrlOnInvalidUrls();

                //$.devOrgUP.editBtnClickEvntBindingNHide();

                for (var i = 0; i < uploadersLength; i++) {
                    $singleUploader = $($uploaders[i]);


                    /// fix urls if not exist. put from form.
                    $.devOrgUP.uploaderFixingDataUrlOnInvalidUrls($singleUploader);

                    id = $singleUploader.attr("data-id");

                    // edit button and preload, show edit button if edit btn is true
                    $label = $.devOrgUP.getLabelToIndicateUploadedFiles(id);
                    $.devOrgUP.showEditButtonBasedOnPreUploadNShowMessageOnLabel($label, id);


                    // binding with edit button clicked.
                    $editBtn = $.devOrgUP.getEditButton(id);
                    $.devOrgUP.uploaderEditBtnClickEvntBinder($editBtn, $label, $singleUploader, id);


                }


                $uploaders.fileupload({
                    url: $(this).attr("data-url"),
                    dataType: 'json',
                    autoUpload: $(this).attr("data-is-auto"),
                    //singleFileUploads: true,
                    acceptFileTypes: new RegExp(acceptFileTypeRegularExpressionString, 'i'),
                    FileSize: actualSize, // in MB
                    progressall: function (e, data) {
                        // when in progress

                        //written here so that doesn't have to go back and fort.
                        var $this = $(this);
                        var idAttr = $this.attr("data-id");
                        var progress = parseInt(data.loaded / data.total * 100, 10);

                        $.devOrgUP.setProgressorValue(idAttr, progress);
                    }
                })
                .on($.devOrgUP.fileUploadAddedEvent, function (e, data) {
                    // file upload added event.
                    $.devOrgUP.onFileUploadAddedEvent(e, data, $(this));
                })
                // when on submit
                .on($.devOrgUP.submitEvent, function (e, data) {
                    $.devOrgUP.onSubmitEvent(e, data, $(this));
                })
                //.on($.devOrgUP.progressEvent, function (e, data) {                  

                //    //console.log(progress);
                //})
                // when done
                .on($.devOrgUP.doneEvent, function (e, data) {
                    $.devOrgUP.onUploadDoneEvent(e, data, $(this));
                })
                // when failed
                .on($.devOrgUP.failedEvent, function (e, data) {
                    $.devOrgUP.onUploadFailedErrorEvent(e, data, $(this));
                }).prop('disabled', !$.support.fileInput)
                    .parent().addClass($.support.fileInput ? undefined : 'disabled');
            }
        },
        onFileUploadAddedEvent: function (e, data, $this) {
            var id = $this.attr("data-id");
            var $label = $.devOrgUP.getLabelToIndicateUploadedFiles(id);
            $.devOrgUP.showUploadProgressor(id);
            $.devOrgUP.setTextInLabel($label, "Uploading...");
        },
        onSubmitEvent: function (e, data, $this) {
            //var id = $this.attr("data-id");
            data.formData = $.devOrgUP.formData;
        },
        onUploadDoneEvent: function (e, data, $this) {
            var id = $this.attr("data-id");
            var $label = $.devOrgUP.getLabelToIndicateUploadedFiles(id);
            //console.log("done" + id);
            var result = data.result;
            var isSingleFileUploader = false;
            var isUploaded = result.isUploaded;
            var uploadedFilesCount = result.uploadedFiles;
            var message = result.message;
            var fileNames = [];
            var $editBtn = null;
            if ($label.is(":hidden")) {
                $label.fadeIn("slow");
            }


            // check if single uploader.
            if (!$this.attr("multiple")) {
                isSingleFileUploader = true;
            }

            if (isSingleFileUploader) {
                if (isUploaded) {
                    uploadedFilesCount = 1;
                }
            } else {
                // if multiple then consider edit button to show up.
                
            }

            if ($this.attr($.devOrgUP.hasEditButtonParameter)) {
                // edit button, show edit button if exist.
                $editBtn = $.devOrgUP.getEditButton(id);
                if ($editBtn.is(":hidden")) {
                    $editBtn.show("slow");
                }
            }




            var length = data.files.length;
            for (var i = 0; i < length; i++) {
                var file = data.files[i];
                fileNames.push(file.name);
            }

            var filesNamesString = fileNames.join();

            var labelTitle = $label.attr("title");
            if (!_.isEmpty(labelTitle)) {
                labelTitle += ",";
            } else {
                labelTitle = "";
            }
            if (isUploaded) {

                // upload successful
                $.devOrgUP.setTextInLabelWithUploadNumber($label, $.devOrgUP.uploadedFilesMessage, uploadedFilesCount);
                //icons
                var failed = $.devOrgUP.hideFailedIcon(id);
                $.devOrgUP.showSuccessIcon(id);
                var success = $.devOrgUP.setSuccessIconMsg(id, filesNamesString);
                //console.log(success);

                labelTitle += filesNamesString;
                $label.attr("title", labelTitle);
                //console.log($label);

                // hide progressor
                $.devOrgUP.hideUploadProgressor(id);
            } else {
                $.devOrgUP.onUploadFailedErrorEvent(e, data, $this);
                $.devOrgUP.setFailedIconMsg(id, filesNamesString);
            }

        },
        onUploadFailedErrorEvent: function (e, data, $this) {
            var id = $this.attr("data-id");
            var $label = $.devOrgUP.getLabelToIndicateUploadedFiles(id);
            var length = data.files.length;
            var files = data.files;
            var file = "";


            // show error headers
            $.devOrgUP.showAllErrorDisplay(id);
            var $ul = $.devOrgUP.getErrorUL(id);
            if ($ul.length > 0) {
                for (var i = 0; i < length; i++) {
                    file = files[i];
                    $.devOrgUP.addErrorInfoInUL($ul, file.name, file.name + " upload failed.");
                }
            }
            $.devOrgUP.showFailedIcon(id);
            $.devOrgUP.hideSuccessIcon(id);
            $.devOrgUP.setFailedIconMsg(id, "Failed.");
            //reset the upload label.
            $.devOrgUP.setTextInLabelWithUploadNumber($label, $.devOrgUP.uploadedFilesMessage);
            // hide progressor
            $.devOrgUP.hideUploadProgressor(id);
        }
    }

    $.devOrgUP.initialize(1, "(\\.|\\/)(gif|jpe?g|png)$");

});
