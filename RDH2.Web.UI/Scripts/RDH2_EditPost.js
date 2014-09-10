$(document).ready(function () {
    //Setup the form validation
    var validator = $("#post-form").kendoValidator().data("kendoValidator");

    //Attach the Delete button
    $("#delete-button").click(function () {
        if (confirm("Are you sure you want to delete this Post?") == true) {
            $.ajax(
                {
                    url: "/Post/Delete",
                    type: "post",
                    data: { ID: $("#ID").val() }
                })
            .done(function (data) {
                if (data.success == true) {
                    window.location.replace(data.url);
                }
                else if (data.success == false) {
                    ShowErrorMessage("Could not delete Post -- Unknown Error.");
                }
                else {
                    ShowErrorMessage("Could not delete Post -- Unknown Error");
                }
            })
            .fail(function () {
                ShowErrorMessage("Could not delete Post -- Connection Error.");
            });
        }
    });

    //Attach the Draft button
    $("#draft-button").click(function () {
        if (validator.validate()) {
            $.ajax(
                {
                    url: "/Post/Save",
                    type: "post",
                    data: GetPostData(false)
                })
            .done(function (data) {
                if (data.success == true) {
                    ShowSuccessMessage("Successfully saved Post to Draft!");
                    $("#ID").val(data.id);
                }
                else if (data.success == false) {
                    ShowErrorMessage(data.message);
                }
                else {
                    ShowErrorMessage("Could not save Post to Draft -- Unknown Error");
                }
            })
            .fail(function (data) {
                ShowErrorMessage("Could not save Post to Draft -- Connection Error.");
            });
        }
    });

    //Attach the Publish button
    $("#publish-button").click(function () {
        if (validator.validate()) {
            $.ajax(
                {
                    url: "/Post/Save",
                    type: "post",
                    data: GetPostData(true)
                })
            .done(function (data) {
                if (data.success == true) {
                    window.location.replace(data.url);
                }
                else if (data.success == false) {
                    ShowErrorMessage(data.message);
                }
                else {
                    ShowErrorMessage("Could not Publish Post -- Unknown Error");
                }
            })
            .fail(function () {
                ShowErrorMessage("Could not publish Post -- Connection Error.");
            });
        }
    });
});

function GetPostData(publish) {
    return { ID: $("#ID").val(), Title: $("#Title").val(), Body: $("#PostBody").data("kendoEditor").value(), Tags: $("#Tags").val(), Published: publish }
}