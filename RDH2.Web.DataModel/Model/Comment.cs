using System;
using System.Collections.Generic;
using System.Linq;

namespace RDH2.Web.DataModel.Model
{
    /// <summary>
    /// Comment is used to represent comments to a piece
    /// of content in the data store.
    /// </summary>
    public class Comment
    {
        #region Member Variables
        private Int32 _id = -1;
        private String _name = "Anonymous";
        private String _emailAddress = String.Empty;
        private String _body = String.Empty;
        #endregion


        #region Public Properties
        /// <summary>
        /// ID gets or sets the ID of the Comment.
        /// </summary>
        public Int32 ID
        {
            get { return this._id; }
            set { this._id = value; }
        }


        /// <summary>
        /// Name gets or sets the Name of the User making
        /// the Comment.
        /// </summary>
        public String Name
        {
            get { return this._name; }
            set { this._name = value; }
        }


        /// <summary>
        /// EmailAddress gets or sets the Email Address of the 
        /// User making the Comment.
        /// </summary>
        public String EmailAddress
        {
            get { return this._emailAddress; }
            set { this._emailAddress = value; }
        }


        /// <summary>
        /// Body gets or sets the Body of the Comment made 
        /// by the User.
        /// </summary>
        public String Body
        {
            get { return this._body; }
            set { this._body = value; }
        }
        #endregion


        #region Relationship Property
        /// <summary>
        /// Post gets or sets the Post to which this 
        /// Comment refers.
        /// </summary>
        public virtual Post Post { get; set; }
        #endregion
    }
}
