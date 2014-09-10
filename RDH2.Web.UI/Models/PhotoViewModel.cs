using System;
using System.Drawing;
using System.Linq;

namespace RDH2.Web.UI.Models
{
    /// <summary>
    /// PhotoViewModel acts as the Model for the ViewPhotoPartial
    /// View.
    /// </summary>
    public class PhotoViewModel
    {
        /// <summary>
        /// Title gets and sets the Title of the Photo
        /// </summary>
        public String Title { get; set; }


        /// <summary>
        /// Path gets and sets the large path for the Photo.
        /// </summary>
        public String Path { get; set; }


        /// <summary>
        /// Size gets and sets the Size that is used to size
        /// the Photo on the client side.
        /// </summary>
        public Size Size { get; set; }


        /// <summary>
        /// FitImage gets and sets whether to fit the image to the
        /// Page after the image has been loaded.
        /// </summary>
        public Boolean FitImage { get; set; }
    }
}