$(document).ready(function () {
    //Setup the form validation
    var validator = $("#contact-form").kendoValidator().data("kendoValidator");

    //Validate the form
    $("#contact-submit").click(function () {
        if (validator.validate()) {
            $.ajax(
                {
                    url: "/Contact/SendMail",
                    type: "post",
                    data: { FromEmailAddress: $("#emailAddress").val(), Subject: $("#subject").val(), Body: $("#body").val() }
                })
            .done(function (data) {
                if (data.success == true)
                    ShowMessage("Email was successfully sent.", data.home);
                else
                    ShowMessage("Email could not be sent.", "");
            })
            .fail(function () {
                ShowMessage("Email could not be sent.", "");
            });
        }
    });
});

function ShowMessage(msg, url) {
    //Setup the dialog
    var $dialog = $("<div></div>").kendoWindow(
        {
            modal: true,
            title: "Contact Us",
            close: function () { if (url != "") window.location.replace(url); }
        }).data("kendoWindow");

    $dialog.content(msg).center().open();
}