using System;
using System.Linq;

namespace RDH2.Web.DataModel.Model
{
    /// <summary>
    /// Video represents a video in the data store.
    /// </summary>
    public class Video
    {
        #region Member Variables
        private Int32 _id = -1;
        private Gallery _gallery = null;
        private String _title = String.Empty;
        private String _videoID = String.Empty;
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
        /// Gallery gets and sets the Gallery that this
        /// Video is in.
        /// </summary>
        public Gallery Gallery
        {
            get { return this._gallery; }
            set { this._gallery = value; }
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
        /// LargePath gets and sets the large path for the Photo.
        /// </summary>
        public String VideoID
        {
            get { return this._videoID; }
            set { this._videoID = value; }
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


        #region Relationship Properties
        /// <summary>
        /// CreatedBy gets and sets the user that created this
        /// video.
        /// </summary>
        public Account CreatedBy { get; set; }
        #endregion
    }
}
