/// <reference path="jquery-1.10.2.js" />
/// <reference path="jquery-1.10.2.intellisense.js" />
/// <reference path="jquery.fileupload.js" />
/// <reference path="jquery.iframe-transport.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery.ui.widget.js" />
/// <reference path="modernizr-2.6.2.js" />
/// <reference path="bootstrap.js" />
/// <reference path="bootstrap-progressbar.js" />

$(function () {
    'use strict';

    // Change this to the location of your server-side upload handler:
    ///url = null , will determinate url from form.
    //$.devOrg.jQueryUploadConfigure = function (url) {

    //Written by Alim Ul Karim

    $.devOrg.jQueryUploadConfigureManual = function () {
        //var urlExist = false;
        //var kRep = 0;
        var failedEvent = "fileuploadfail";
        var progressEvent = "fileuploadprogressall";
        var doneEvent = "fileuploaddone";
        var filesAreAddedForUploadEvent = "fileuploadadd";
        var filesAlwaysProcessEvent = "fileuploadprocessalways";


        var $uploaderDiv = $(".uploader");
        if ($uploaderDiv.length === 0) {
            return;
        }
        var $uploadExist = $uploaderDiv.find("input[type='file'].dev-fileupload");
        var multipleUploaders = null;
        var singleUploaderUrl = null;
        var singleUploader = null;
        var form, forms, formUrl;
        var kRep;
        if ($uploadExist.length > 0) {
            var forms = $("form");
            if (forms.length > 0) {
                for (kRep = 0; kRep < forms.length; kRep++) {
                    form = $(forms[kRep]);
                    formUrl = form.attr("action");
                    multipleUploaders = form.find("input[type='file'].dev-fileupload");
                    for (var i = 0; i < multipleUploaders.length; i++) {
                        singleUploader = multipleUploaders[i];
                        singleUploaderUrl = $(singleUploader).attr('data-url');
                        if (singleUploaderUrl === null || singleUploaderUrl === undefined || singleUploaderUrl.length === 0) {
                            // single input doesn't have the data-url attribute so set it from form.
                            $(singleUploader).attr("data-url", formUrl);
                            console.log("form url :" + formUrl + " in " + $(singleUploader).attr('data-id'));
                        }
                    }
                }
            }

            var uploadButton = $('<button/>')
            .addClass('btn btn-primary')
            .prop('disabled', true)
            .text('Processing...')
            .on('click', function () {
                var $this = $(this),
                    data = $this.data();
                $this
                    .off('click')
                    .text('Abort')
                    .on('click', function () {
                        $this.remove();
                        data.abort();
                    });
                data.submit().always(function () {
                    $this.remove();
                });
            });

            $uploaderDiv.find(".error-warning-labels").hide();
            $uploaderDiv.find(".progress").hide()
                .progressbar({
                    warningMarker: 60,
                    dangerMarker: 80,
                    maximum: 100,
                    step: 1
                });
            $uploaderDiv.find('input.dev-fileupload').fileupload({
                url: $(this).attr('data-url'),
                dataType: 'json',
                autoUpload: false,
                acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
                maxFileSize: 1024000, // 1MB                
                done: function (e, data) {
                    var id = $(this).attr('data-id');
                    //console.log(data.files);
                    //var progressor = $('.uploader .progress-' + id);

                    //if (progressor.is(":visible")) {
                    //    progressor.hide();
                    //}
                    //console.log($('.uploader .progress-' + id));
                    var dataNotifiedLabel = $(".file-uploaded-notify-label-" + id);
                    var uploaderInputbox = $('input.fileupload[data-id=' + id + ']');
                    var uploadedNotification = null;

                    $.each(data.files, function (index, file) {
                        //console.log(file.name);
                        var count = parseInt(dataNotifiedLabel.attr('data-loaded'));
                        count += 1;

                        //$('ul.files-list-' + id).append('<li>' + file.name + '</li>');
                        var title = dataNotifiedLabel.attr('title');
                        if (title === null || title === undefined || title.length === 0) {
                            // first time
                            dataNotifiedLabel.attr('title', file.name);
                        } else {
                            dataNotifiedLabel.attr('title', title + "," + file.name);
                        }
                        uploadedNotification = count + " files uploaded.";
                        uploaderInputbox.attr("title", uploadedNotification)
                        dataNotifiedLabel.attr('data-loaded', count);
                        dataNotifiedLabel.text(uploadedNotification);
                    });
                },
                progressall: function (e, data) {
                    var id = $(this).attr('data-id');
                    //if (data.total === data.loaded) {
                    //    return;
                    //}
                    var progress = parseInt(data.loaded / data.total * 100, 10);

                    var progressor = null;
                    if (progressor === null) {
                        progressor = $uploaderDiv.find('.progress-' + id).show('slow');
                    }
                    if (progressor.not(":visible")) {
                        progressor.show('slow');
                    }

                    //$('.progress-' + id + ' .progress-bar').css(
                    //    'width',
                    //    progress + '%'
                    //);
                    progressor.progressbar('setPosition', progress);
                    var progressLabelForValue = $uploaderDiv.find("progress-label-value-" + id);
                    if (progressLabelForValue.length > 0) {
                        progressLabelForValue.text(progress + "%");
                    }

                    if (data.total === data.loaded) {
                        //success               
                        if (progressor.is(":visible")) {
                            progressor.hide('slow');
                        }
                        if (progressLabelForValue.length > 0) {
                            progressLabelForValue.text("");
                        }
                    }
                }
            }).on(failedEvent, function (e, data) {
                var id = $(this).attr('data-id');
                var warningLabel = null;
                if (warningLabel === null) {
                    warningLabel = $uploaderDiv.find(".error-warning-label-" + id);
                }
                if (warningLabel !== null) {
                    warningLabel.show("slow");
                }
                var unorderedList = $uploaderDiv.find('ul.files-list-' + id);
                $.each(data.files, function (index, file) {
                    //console.log("Failed : " + file.name);
                    unorderedList.append('<li class="label label-danger block">' + file.name + '</li>');

                });
            }).on(filesAreAddedForUploadEvent, function (e, data) {
                var id = $(this).attr('data-id');
                data.context = $('<span/>').appendTo('.files-display-' + id);
                $.each(data.files, function (index, file) {
                    var node = $('<p/>')
                            .append($('<span/>').text(file.name));
                    if (!index) {
                        node
                            .append('<br>')
                            .append(uploadButton.clone(true).data(data));
                    }
                    node.appendTo(data.context);
                });
            }).on(filesAlwaysProcessEvent, function (e, data) {
                var index = data.index,
                    file = data.files[index],
                    node = $(data.context.children()[index]);
                if (file.preview) {
                    node
                        //.prepend('<br>')
                        .prepend(file.preview);
                }
                if (file.error) {
                    node
                        //.append('<br>')
                        .append($('<span class="text-danger"/>').text(file.error));
                }
                if (index + 1 === data.files.length) {
                    data.context.find('button')
                        .text('Upload')
                        .prop('disabled', !!data.files.error);
                }
            }).prop('disabled', !$.support.fileInput)
                .parent().addClass($.support.fileInput ? undefined : 'disabled');
        };

    }


    $.devOrg.uploadDeleteBtnClicked = function (e, $deleteBtn, $editBtn, id, sequence, $imageRow, $uploaderLabel, $uploaderInput) {
        //console.log(id);
        //console.log($uploaderLabel);
        //console.log($uploaderInput);
        //console.log($deleteBtn);
        var url = $deleteBtn.attr("href");

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
                    var count = parseInt($uploaderLabel.attr("data-loaded"));
                    count = count - 1;
                    if (count <= 0) {
                        count = 0;
                        $editBtn.hide();
                    }

                    var uploadedNotification = count + " files uploaded.";
                    $uploaderLabel.attr('data-loaded', count);
                    $uploaderLabel.text(uploadedNotification);
                }
            },
            error: function (xhr, status, error) {

            }
        }); // ajax end

    }


    $.devOrg.uploadEditBtnClicked = function (e, $editBtn, id, $uploaderLabel, $uploaderInput, $uploaderDv) {
        e.preventDefault();
        var url = $editBtn.attr("href");
        var $editList = $(".edit-items-list-" + id);
        var formData = $editBtn.closest("form").serializeArray();
        var $spinner = $uploaderDv.find("span.CustomValidation[data-id='" + id + "']");
        //alert("id " + id + " url :" + url);
        $spinner.removeClass("hide").show();
        $.ajax({
            type: "POST",
            dataType: "html",
            url: url,
            data: formData,
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

                    $.devOrg.uploadDeleteBtnClicked(e, $deleteButton, $editBtn, id, sequence, $imageRow, $uploaderLabel, $uploaderInput);
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


    };

    $.devOrg.uploaderEditBtnClickEvntBindingNHide = function ($uploaderDiv, uploaderInputClass) {
        /// <summary>
        /// It's not the actual event but it's the method which binds edit button with click event.
        /// Hides unnecessary edit buttons
        /// </summary>
        /// <param name="$uploaderDiv"></param>
        /// <param name="uploaderInputClass"></param>
        var $editClassBtns = $uploaderDiv.find("a[data-btn=edit].edit-btn");
        if ($editClassBtns.length > 0) {
            $editClassBtns.on('click', function (e) {
                e.preventDefault();
                var $editBtn = $(this);
                var id = $editBtn.attr("data-id");
                var $label = $uploaderDiv.find("label.file-uploaded-notify-label-" + id);
                var $uploaderInput = $uploaderDiv.find('input' + uploaderInputClass + '[data-id=' + id + ']');
                $.devOrg.uploadEditBtnClicked(e, $editBtn, id, $label, $uploaderInput, $uploaderDiv);
            });

            var length = $editClassBtns.length;
            for (i = 0; i < length; i++) {
                var $singleEditBtn = $(editClassBtns[i]);
                var isShow = $singleEditBtn.attr("data-default-show");
                if (!isShow) {
                    $editBtn.hide();
                }
            }
        }
    }

    $.devOrg.uploaderFixingDataUrlOnInvalidUrls = function (uploaderInputClass) {
        var $form = null, $forms = null, formUrl = null;
        var $multipleUploaders = null, $singleUploader = null, singleUploaderUrl = null;
        var kRep = 0;
        var length = 0;
        var $forms = $("$form");
        if ($forms.length > 0) {
            for (kRep = 0; kRep < $forms.length; kRep++) {
                $form = $($forms[kRep]);
                formUrl = $form.attr("action");
                $multipleUploaders = $form.find("input[type='file']" + uploaderInputClass);
                length= $multipleUploaders.length;

                for (var i = 0; i < length; i++) {
                    $singleUploader = $(multipleUploaders[i]);
                    singleUploaderUrl = $singleUploader.attr('data-url');
                    if (singleUploaderUrl === null || singleUploaderUrl === undefined || singleUploaderUrl.length === 0) {
                        // single input doesn't have the data-url attribute so set it from $form.
                        $singleUploader.attr("data-url", formUrl);
                        //console.log("$form url :" + formUrl + " in " + $($singleUploader).attr('data-id'));
                    }
                }
            }
        }
    }

    $.devOrg.jQueryUploadConfigureAuto = function () {
        //var urlExist = false;
        //var kRep = 0;
        var failedEvent = "fileuploadfail";
        var progressEvent = "fileuploadprogressall";
        var doneEvent = "fileuploaddone";
        var filesAreAddedForUploadEvent = "fileuploadadd";
        var filesAlwaysProcessEvent = "fileuploadprocessalways";
        var uploaderInputClass = ".dev-fileupload";
        var length = 0;

        var $uploaderDiv = $(".uploader-auto");
        if ($uploaderDiv.length === 0) {
            return;
        }
        var $uploadExist = $uploaderDiv.find("input[type='file']" + uploaderInputClass);
       
  
        if ($uploadExist.length > 0) {
            
            $.devOrg.uploaderFixingDataUrlOnInvalidUrls(uploaderInputClass);

            $.devOrg.editBtnClickEvntBindingNHide($uploaderDiv, uploaderInputClass);

            $uploaderDiv.find(".error-warning-labels").hide();

            $uploaderDiv.find('input' + uploaderInputClass).fileupload({
                url: $(this).attr('data-url'),
                dataType: 'json',
                autoUpload: true,
                //formData: $(this).closest("form").serializeArray(),
                acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
                maxFileSize: 1024000, // 1MB                
                done: function (e, data) {
                    var id = $(this).attr('data-id');
                    
                    
                    var uploaderInputbox = $uploaderDiv.find('input' + uploaderInputClass + '[data-id=' + id + ']');
                    var uploadedNotification = null;


                    $.each(data.files, function (index, file) {
                        //console.log(file.name);
                        var count = parseInt(dataNotifiedLabel.attr('data-loaded'));
                        count += 1;

                        //$('ul.files-list-' + id).append('<li>' + file.name + '</li>');
                        var title = dataNotifiedLabel.attr('title');
                        if (uploaderInputbox.attr("multiple")) {
                            var editBtn = $uploaderDiv.find(".edit-" + id);
                            if (editBtn.length > 0) {
                                if (editBtn.not(":visible")) {
                                    editBtn.show("slow");
                                }
                            }
                        } else {
                            // not multiple
                            count = 1;
                            title = null;
                        }
                        if (title === null || title === undefined || title.length === 0) {
                            // first time
                            dataNotifiedLabel.attr('title', file.name);
                        } else {
                            dataNotifiedLabel.attr('title', title + "," + file.name);
                        }
                        uploadedNotification = count + " files uploaded.";
                        uploaderInputbox.attr("title", uploadedNotification)
                        dataNotifiedLabel.attr('data-loaded', count);
                        dataNotifiedLabel.text(uploadedNotification);
                    });
                },
                progressall: function (e, data) {
                    var id = $(this).attr('data-id');
                    //if (data.total === data.loaded) {
                    //    return;
                    //}
                    var progress = parseInt(data.loaded / data.total * 100, 10);

                    var progressor = null;
                    if (progressor === null) {
                        progressor = $uploaderDiv.find('.progress-' + id).show('slow');
                    }
                    if (progressor.not(":visible")) {
                        progressor.show('slow');
                    }

                    //$('.progress-' + id + ' .progress-bar').css(
                    //    'width',
                    //    progress + '%'
                    //);
                    progressor.progressbar('setPosition', progress);
                    var progressLabelForValue = $uploaderDiv.find(".progress-label-value-" + id);
                    if (progressLabelForValue.length > 0) {
                        progressLabelForValue.text(progress + "%");
                    }

                    if (data.total === data.loaded) {
                        //success               
                        if (progressor.is(":visible")) {
                            progressor.hide('slow');
                        }
                        if (progressLabelForValue.length > 0) {
                            progressLabelForValue.text("");
                        }
                    }
                }
            }).on('fileuploadsubmit', function (e, data) {

                data.formData = $(this).closest("form").find("input:not('.url-input')").serializeArray();


            }).on(failedEvent, function (e, data) {
                var id = $(this).attr('data-id');
                var warningLabel = null;
                if (warningLabel === null) {
                    warningLabel = $uploaderDiv.find(".error-warning-label-" + id);
                }
                if (warningLabel !== null) {
                    warningLabel.show("slow");
                }
                var unorderedList = $uploaderDiv.find('ul.files-list-' + id);
                $.each(data.files, function (index, file) {
                    //console.log("Failed : " + file.name);
                    unorderedList.append('<li class="label label-danger block">' + file.name + '</li>');

                });
            }).prop('disabled', !$.support.fileInput)
                .parent().addClass($.support.fileInput ? undefined : 'disabled');
        };
    }

    $.devOrg.jQueryUploadConfigureManual();
    $.devOrg.jQueryUploadConfigureAuto();
});
