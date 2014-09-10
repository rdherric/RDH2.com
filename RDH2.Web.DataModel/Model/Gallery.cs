using System;
using System.Collections.Generic;
using System.Linq;

namespace RDH2.Web.DataModel.Model
{
    /// <summary>
    /// Gallery is the class used to represent a collection
    /// of media -- photos and videos -- in the data store.
    /// </summary>
    public class Gallery
    {
        #region Member Variables
        private Int32 _id = -1;
        private String _name = String.Empty;
        private String _reducedName = String.Empty;
        private DateTime _created = DateTime.Now;
        private DateTime _modified = DateTime.Now;
        private DateTime _lastModified = DateTime.Now;
        private Boolean _expanded = true;
        private Boolean _isRecent = true;
        #endregion


        #region Public Properties
        /// <summary>
        /// ID gets and sets the ID of the Gallery.
        /// </summary>
        public Int32 ID
        {
            get { return this._id; }
            set { this._id = value; }
        }


        /// <summary>
        /// Name gets and sets the name of the Gallery.
        /// </summary>
        public String Name
        {
            get { return this._name; }
            set { this._name = value; }
        }


        /// <summary>
        /// ReducedName gets and sets the reduced name of
        /// the Gallery.
        /// </summary>
        public String ReducedName
        {
            get { return this._reducedName; }
            set { this._reducedName = value; }
        }


        /// <summary>
        /// FullName gets the full human-readable path of the
        /// Photo Gallery.
        /// </summary>
        public String FullName { get; set; }


        /// <summary>
        /// FullPath gets the full MVC URL path of the Gallery.
        /// </summary>
        public String FullPath { get; set; }


        /// <summary>
        /// Created gets and sets the date and time that this 
        /// Gallery was created.
        /// </summary>
        public DateTime Created
        {
            get { return this._created; }
            set { this._created = value; }
        }


        /// <summary>
        /// Modified gets and sets the date and time that this 
        /// Gallery was last modified.
        /// </summary>
        public DateTime Modified
        {
            get { return this._modified; }
            set { this._modified = value; }
        }


        /// <summary>
        /// LastModified gets and sets the last date that
        /// the Gallery or its children were modified.
        /// </summary>
        public DateTime LastModified
        {
            get { return this._lastModified; }
            set { this._lastModified = value; }
        }


        /// <summary>
        /// Expanded gets and sets whether the Gallery should
        /// be expanded in the TreeView.
        /// </summary>
        public Boolean Expanded
        {
            get { return this._expanded; }
            set { this._expanded = value; }
        }

        
        /// <summary>
        /// IsRecent gets and sets whether the Gallery should
        /// be shown in Recent Activity.
        /// </summary>
        public Boolean IsRecent
        {
            get { return this._isRecent; }
            set { this._isRecent = value; }
        }
        #endregion


        #region Public Relationship Properties
        /// <summary>
        /// Parent returns the Parent Gallery of this
        /// Gallery.
        /// </summary>
        public virtual Gallery Parent { get; set; }


        /// <summary>
        /// Galleries gets and sets the child Galleries of 
        /// this Photo Gallery.
        /// </summary>
        public virtual ICollection<Gallery> Galleries { get; set; }


        /// <summary>
        /// Photos gets the Collection of Photo objects 
        /// that are contained by this Gallery.
        /// </summary>
        public virtual ICollection<Photo> Photos { get; set; }


        /// <summary>
        /// Videos gets the Collection of Video objects 
        /// that are contained by this Gallery.
        /// </summary>
        public virtual ICollection<Video> Videos { get; set; }


        /// <summary>
        /// CreatedBy gets and sets the Account of the user that 
        /// created this Gallery.
        /// </summary>
        public virtual Account CreatedBy { get; set; }
        #endregion
    }
}
