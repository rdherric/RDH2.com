//Declare a variable to determine if the window is open
var dlgOpen = false;

$(document).ready(function () {
    //Setup the dialog
    var $dialog = $("<div></div>").kendoWindow(
        {
            modal: true,
            title: "View Photo",
            open: function () { dlgOpen = true; },
        }).data("kendoWindow");

    //Use the dialog in the IMG click
    $(".gallery-photo").click(function () {
        ShowPhoto(this, $dialog);
    });

    //Use the dialog in the Video click
    $(".gallery-video").click(function () {
        ShowVideo(this, $dialog);
    });

    //Setup the slideshow link
    $("#slideshow").click(function () {
        //Get the list of photos
        var photos = $(".gallery-photo");

        //Show the first photo
        ShowPhoto(photos[0], $dialog);

        //Show the rest of the photos
        var index = 1;
        $.doTimeout("slideshow", 3000, function () {
            ShowPhoto(photos.eq(index++), $dialog);
            if (index <= photos.length) {
                return true;
            }
            else {
                $dialog.close();
                return false;
            }
        });
    });

    //Setup the TreeView to save and remove expanded values
    var treeview = $("#AllGalleries").data("kendoTreeView");
    treeview.bind("expand", function (e) {
        //Save the value in the cookie
        $.cookie($(e.node).attr("data-id"), true, { path: "/" });
    });

    treeview.bind("collapse", function (e) {
        //Save the value in the cookie
        $.cookie($(e.node).attr("data-id"), false, { path: "/" });
    });
});

function ShowPhoto(photo, dlg) {
    var photoID = $(photo).attr("name");
    $.ajax(
        {
            url: "/Photo/GetDialogHtml",
            type: "get",
            data: { photoID: photoID },
            success: function (data) {
                dlg.title("View Photo");
                dlg.setOptions(
                    {
                        close: function () {
                            dlgOpen = false;
                            $.doTimeout("slideshow");
                    }
                });
                dlg.content(data).center();
                if (dlgOpen == false) {
                    dlg.open();
                }
            }
        });
}

function ShowVideo(video, dlg) {
    var videoID = $(video).attr("name");
    $.ajax(
        {
            url: "/Video/GetDialogHtml",
            type: "get",
            data: { videoID: videoID },
            success: function (data) {
                dlg.title("View Video");
                dlg.setOptions(
                    {
                        close: function () {
                            dlgOpen = false;
                            onVideoWindowClose();
                        }
                    });
                dlg.content(data).center();
                if (dlgOpen == false) {
                    dlg.open();
                }
            }
        });
}
