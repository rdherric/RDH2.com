(function($) {
    $.fn.fitImage = function () {
        //Get the image as a jQuery object
        var img = $(this);

        //Get the sides of the image
        var imgHeight = img.height();
        var imgWidth = img.width();

        //Set the size -- always want to fit width since
        //height cannot be fit 
        img.css("max-width", imgWidth + "px");
        img.width("100%");
        img.height(Math.round((img.width() / imgWidth) * imgHeight));

        //Return the object
		return this;
	};
})(jQuery);