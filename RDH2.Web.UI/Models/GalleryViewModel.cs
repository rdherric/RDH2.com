using System;
using System.Collections.Generic;
using System.Linq;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.UI.Models
{
    /// <summary>
    /// GalleryViewModel acts as the Model for the 
    /// Gallery Partial View.
    /// </summary>
    public class GalleryViewModel
    {
        #region Public Properties
        /// <summary>
        /// RecentActivity gets and sets the Galleries that have
        /// had recent activity.
        /// </summary>
        public List<Gallery> RecentActivity { get; set; }


        /// <summary>
        /// AllGalleries gets and sets the complete set of Galleries
        /// in the Application.
        /// </summary>
        public List<Gallery> AllGalleries { get; set; }


        /// <summary>
        /// SelectedGallery gets and sets the Gallery that has been
        /// selected by the User.
        /// </summary>
        public Gallery SelectedGallery { get; set; }


        /// <summary>
        /// VideoID gets and sets the ID of the Video to 
        /// show from the Videos Page.
        /// </summary>
        public String VideoID { get; set; }
        #endregion
    }
}