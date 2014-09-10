using System;
using System.Collections.Generic;
using System.Linq;

namespace RDH2.Web.DataModel.Model
{
    /// <summary>
    /// Photo represents a picture in the data store.
    /// </summary>
    public class Photo
    {
        #region Member Variables
        private Int32 _id = -1;
        private String _title = String.Empty;
        private String _thumbnailPath = String.Empty;
        private String _largePath = String.Empty;
        private DateTime _created = DateTime.Now;
        private DateTime _modified = DateTime.Now;
        #endregion


        #region Public Properties
        /// <summary>
        /// ID gets and sets the ID of the Photo.
        /// </summary>
        public Int32 ID
        {
            get { return this._id; }
            set { this._id = value; }
        }


        /// <summary>
        /// Title gets and sets the optional title on the Photo.
        /// </summary>
        public String Title
        {
            get { return this._title; }
            set { this._title = value; }
        }


        /// <summary>
        /// ThumbnailPath gets and sets the thumbnail path for
        /// the Photo.
        /// </summary>
        public String ThumbnailPath
        {
            get { return this._thumbnailPath; }
            set { this._thumbnailPath = value; }
        }


        /// <summary>
        /// LargePath gets and sets the large path for the Photo.
        /// </summary>
        public String LargePath
        {
            get { return this._largePath; }
            set { this._largePath = value; }
        }


        /// <summary>
        /// Created gets and sets the date and time the Photo
        /// was created.
        /// </summary>
        public DateTime Created
        {
            get { return this._created; }
            set { this._created = value; }
        }


        /// <summary>
        /// Modified gets and sets the date and time the Photo
        /// was last modified.
        /// </summary>
        public DateTime Modified
        {
            get { return this._modified; }
            set { this._modified = value; }
        }
        #endregion


        #region Public Relationship Properties
        /// <summary>
        /// Gallery gets and sets the Gallery in which this Photo
        /// is contained.
        /// </summary>
        public virtual Gallery Gallery { get; set; }


        /// <summary>
        /// CreatedBy gets and sets the Account of the user
        /// that created this Photo.
        /// </summary>
        public virtual Account CreatedBy { get; set; }
        #endregion
    }
}
