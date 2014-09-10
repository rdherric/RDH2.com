using System;
using System.Collections.Generic;
using System.Linq;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.UI.Models
{
    /// <summary>
    /// EditVideoViewModel is used to move data to the EditVideos
    /// View so that the user can put Videos into Galleries.
    /// </summary>
    public class EditVideoViewModel
    {
        /// <summary>
        /// Video gets and sets the Video that is to
        /// be edited.
        /// </summary>
        public Video Video { get; set; }


        /// <summary>
        /// GalleryID gets and sets the ID of the Gallery that the
        /// User chooses.
        /// </summary>
        public Int32 GalleryID { get; set; }


        /// <summary>
        /// AllGalleries gets and sets the List of all 
        /// Galleries in the application.
        /// </summary>
        public List<Gallery> AllGalleries { get; set; }
    }
}