$(document).ready(function () {
    //Setup the photo uploader
    $("#photos").kendoUpload();

    //Setup the form validation
    var validator = $("#gallery-form").kendoValidator().data("kendoValidator");

    //Attach the Delete Gallery button
    $("#delete-gallery-button").click(function () {
        if (confirm("Are you sure you want to delete this Gallery?") == true) {
            alert("Deleting Gallery!");
        }
    });

    //Attach the Save Gallery Button
    $("#save-gallery-button").click(function () {
        if (validator.validate()) {
            $.ajax(
                {
                    url: "/Gallery/Save",
                    type: "post",
                    data: {
                        ID: $("#Gallery_ID").val(),
                        Name: $("#galleryName").val(),
                        Expanded: $("#Gallery_Expanded").prop("checked"),
                        IsRecent: $("#Gallery_IsRecent").prop("checked"),
                        parentID: $("#ParentID").val()
                    }
                })
            .done(function (data) {
                if (data.success == true) {
                    ShowSuccessMessage("Successfully saved Gallery!");
                    $("#ID").val(data.id);
                }
                else if (data.success == false) {
                    ShowErrorMessage(data.message);
                }
                else {
                    ShowErrorMessage("Could not save Gallery -- Unknown Error");
                }
            })
            .fail(function (data) {
                    ShowErrorMessage("Could not save Gallery -- Connection Error.");
            });
        }
    });

    //Attach the Delete photo buttons
    $(".update-photo button").click(function (event) {
        //Get a reference to the button
        var button = event.target;

        if (confirm("Are you sure that you want to delete this photo?\nID: " +
            getPhotoProperty(button, "#Photo_ID") + "\nTitle: " +
            getPhotoProperty(button, "#Photo_Title")) == true) {

            //Delete the photo from the database
            $.ajax(
                {
                    url: "/Photo/Delete",
                    type: "post",
                    data: { photoID: getPhotoProperty(button, "#Photo_ID") }
                })
            .done(function (data) {
                if (data.success == true) {
                    $(button).parent().parent().remove();
                }
            })
            .fail(function () { alert("Could not delete Photo."); });
        }
    });

    //Setup the Processing dialog
    var $processingDlg = $("<div></div>").kendoWindow(
            {
                modal: true,
                title: "Processing Request",
            }).data("kendoWindow");

    var content = "Processing [total] Photos...<br />" +
                  "Succeeded: 0<br />" +
                  "Failed: 0";

    //Attach the Update All Photos button
    $("#update-photos").click(function () {
        //Setup the numbers in the dialog
        var succeeded = 0;
        var failed = 0;
        var total = $(".update-photo button").length;

        //Setup the total in the dialog
        content = content.replace("[total]", total);
        $processingDlg.content(content);

        //Show the Processing dialog
        $processingDlg.center().open();

        //Register the ajaxStop event
        $("#update-photos").one("ajaxStop", function () {
            $processingDlg.close();
        });

        //Update the photos in the database
        $(".update-photo button").each(function (index, button) {
            $.ajax(
                {
                    url: "/Photo/Update",
                    type: "post",
                    data: { id: getPhotoProperty(button, "#Photo_ID"), title: getPhotoProperty(button, "#Photo_Title") }
                })
            .done(function (data) {
                if (data.success == true) {
                    content = content.replace("Succeeded: " + succeeded, "Succeeded: " + ++succeeded);
                    $processingDlg.content(content);
                }
                else if (data.success == false) {
                    content = content.replace("Failed: " + failed, "Failed: " + ++failed);
                    $processingDlg.content(content);
                }
            })
            .fail(function () { alert("Could not update Photos."); });
        });
    });
});

function getPhotoProperty(button, selector) {
    return $(button).parent().find(selector)[0].value;
}

function onPhotoSuccess(e) {
    if (e.operation == "upload") {
        for (var i = 0; i < e.response.photoIDs.length; i++) {
            $.ajax(
                {
                    url: "/Photo/GetUpdateHtml",
                    type: "get",
                    data: { photoID: e.response.photoIDs[i] }
                })
            .done(function (data) {
                $("#update-photo-holder").append(data);
            });
        }
    }
}
