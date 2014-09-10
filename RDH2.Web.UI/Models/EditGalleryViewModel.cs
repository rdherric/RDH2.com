using System;
using System.Collections.Generic;
using System.Linq;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.UI.Models
{
    /// <summary>
    /// EditGalleryViewModel is the model used to edit
    /// a Gallery.
    /// </summary>
    public class EditGalleryViewModel
    {
        /// <summary>
        /// Gallery gets and sets the Gallery that is to
        /// be edited.
        /// </summary>
        public Gallery Gallery { get; set; }


        /// <summary>
        /// ParentID gets and sets the ID of the Parent that the
        /// User chooses.
        /// </summary>
        public Int32 ParentID { get; set; }


        /// <summary>
        /// AllGalleries gets and sets the List of all 
        /// Galleries in the application.
        /// </summary>
        public List<Gallery> AllGalleries { get; set; }
    }
}