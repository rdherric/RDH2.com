$(document).ready(function () {
    //Setup the dialog
    var $login = $("<div></div>").kendoWindow(
        {
            activate: function () { $("#userName").focus() },
            modal: true,
            title: "Log In to RDH2.COM",
        }).data("kendoWindow");

    //Use the dialog in the link click
    $("#login-link").click(function () {
        $.ajax(
                {
                    url: "/Account/GetLogInHtml",
                    type: "get",
                    success: function (data) {
                        $login.content(data);
                        $login.center().open();
                    },
                });

    });
});

function ShowSuccessMessage(msg) {
    $("#message").html(msg).removeClass().addClass("success");
}

function ShowErrorMessage(msg) {
    $("#message").html(msg).removeClass().addClass("error");
}