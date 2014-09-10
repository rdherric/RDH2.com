using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDH2.Web.DataModel.Model
{
    /// <summary>
    /// Account is used to represent a user who has created
    /// content in the data store.
    /// </summary>
    public class Account
    {
        #region Member Variables
        private Int32 _id = -1;
        private DateTime _created = DateTime.Now;
        private DateTime _modified = DateTime.Now;
        #endregion


        #region Public Properties
        /// <summary>
        /// ID gets and sets the ID of the User Account
        /// </summary>
        public Int32 ID
        {
            get { return this._id; }
            set { this._id = value; }
        }


        /// <summary>
        /// FirstName gets and sets the First Name of the User
        /// </summary>
        public String FirstName { get; set; }


        /// <summary>
        /// LastName gets and sets the Last Name of the User
        /// </summary>
        public String LastName { get; set; }


        /// <summary>
        /// Login gets and sets the Login name of the User.
        /// </summary>
        public String Login { get; set; }


        /// <summary>
        /// Password gets and sets the Password of the User.
        /// </summary>
        public String Password { get; set; }


        /// <summary>
        /// Created gets and sets the date / time at which the
        /// Account was created.
        /// </summary>
        public DateTime Created
        {
            get { return this._created; }
            set { this._created = value; }
        }


        /// <summary>
        /// Modified gets and sets the date / time at which 
        /// the Account was modified.
        /// </summary>
        public DateTime Modified
        {
            get { return this._modified; }
            set { this._modified = value; }
        }
        #endregion


        #region Public Relationship Properties
        /// <summary>
        /// Posts gets and sets the Collection of 
        /// News posts that the User has made.
        /// </summary>
        public virtual ICollection<Post> Posts { get; set; }


        /// <summary>
        /// Galleries gets and sets the Collection of 
        /// Photo Galleries that the User has made.
        /// </summary>
        public virtual ICollection<Gallery> Galleries { get; set; }


        /// <summary>
        /// Photos gets and sets the Collection of Photos
        /// that the User has uploaded.
        /// </summary>
        public virtual ICollection<Photo> Photos { get; set; }


        /// <summary>
        /// Videos gets and sets the Collection of Videos
        /// that the User has uploaded.
        /// </summary>
        public virtual ICollection<Video> Videos { get; set; }
        #endregion
    }
}
