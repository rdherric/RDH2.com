$(document).ready(function () {
    //Setup the Processing dialog
    var $processingDlg = $("<div></div>").kendoWindow(
            {
                modal: true,
                title: "Processing Request",
            }).data("kendoWindow");

    var content = "Processing [total] Videos...<br />" +
                  "Succeeded: 0<br />" +
                  "Failed: 0";

    //Attach the Update All Videos button
    $("#update-videos").click(function () {
        //Setup the numbers in the dialog
        var succeeded = 0;
        var failed = 0;
        var total = $(".update-single-video").length;

        //Setup the total in the dialog
        content = content.replace("[total]", total);
        $processingDlg.content(content);

        //Show the Processing dialog
        $processingDlg.center().open();

        //Register the ajaxStop event
        $("#update-videos").one("ajaxStop", function () {
            $processingDlg.close();
        });

        //Update the photos in the database
        $(".update-single-video").each(function (index, table) {
            $.ajax(
                {
                    url: "/Video/Update",
                    type: "post",
                    data: { id: $(table).find("#Video_ID").val(), galleryID: $(table).find("#GalleryID").val() }
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
            .fail(function () { alert("Could not update Videos."); });
        });
    });
});