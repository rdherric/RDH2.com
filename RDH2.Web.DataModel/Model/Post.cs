using System;
using System.Collections.Generic;
using System.Linq;

namespace RDH2.Web.DataModel.Model
{
    /// <summary>
    /// Post is the class that represents a news or blog
    /// post in the data store.
    /// </summary>
    public class Post
    {
        #region Member Variables
        private Int32 _id = -1;
        private String _title = String.Empty;
        private String _reducedTitle = String.Empty;
        private String _body = String.Empty;
        private String _tags = String.Empty;
        private DateTime _created = DateTime.Now;
        private DateTime _modified = DateTime.Now;
        private Boolean _published = true;
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
        /// Title gets and sets the Title of the Post
        /// </summary>
        public String Title
        {
            get { return this._title; }
            set { this._title = value; }
        }


        /// <summary>
        /// ReducedTitle gets and sets the Reduced Title of the Post.
        /// </summary>
        public String ReducedTitle
        {
            get { return this._reducedTitle; }
            set { this._reducedTitle = value; }
        }


        /// <summary>
        /// Body gets and sets the Body of the Post
        /// </summary>
        public String Body
        {
            get { return this._body; }
            set { this._body = value; }
        }


        /// <summary>
        /// Tags gets and sets the Tags set on the Post
        /// </summary>
        public String Tags
        {
            get { return this._tags; }
            set { this._tags = value; }
        }


        /// <summary>
        /// TagList gets the list of Tags split into a List.
        /// </summary>
        public List<String> TagList
        {
            get
            {
                //Declare a List to return
                List<String> rtn = new List<String>();

                //If the Tag has content, split it
                if (String.IsNullOrEmpty(this.Tags) == false)
                {
                    rtn.AddRange(this.Tags.ToLower().Split(' '));
                }

                //Return the result
                return rtn;
            }
        }


        /// <summary>
        /// Created gets and sets the date / time at which the
        /// Post was created.
        /// </summary>
        public DateTime Created
        {
            get { return this._created; }
            set { this._created = value; }
        }


        /// <summary>
        /// Modified gets and sets the date / time at which 
        /// the Post was modified.
        /// </summary>
        public DateTime Modified
        {
            get { return this._modified; }
            set { this._modified = value; }
        }


        /// <summary>
        /// Published gets and sets whether the Post has
        /// been published to the world.
        /// </summary>
        public Boolean Published
        {
            get { return this._published; }
            set { this._published = value; }
        }
        #endregion


        #region Public Relationship Properties
        /// <summary>
        /// CreatedBy gets and sets the Account that created 
        /// the NewsPost.
        /// </summary>
        public virtual Account CreatedBy { get; set; }


        /// <summary>
        /// Comments gets and sets the list of Comments
        /// for the Post.
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }
        #endregion
    }
}
