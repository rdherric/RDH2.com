using System;
using System.Collections.Generic;
using System.Linq;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.UI.Models
{
    /// <summary>
    /// PostIndexViewModel acts as the conduit between
    /// the Index page of the PostController and the User.
    /// </summary>
    public class PostViewModel
    {
        /// <summary>
        /// Tags gets and sets a list of tags that are used 
        /// by the User to get to content quickly.
        /// </summary>
        public List<String> Tags { get; set; }


        /// <summary>
        /// CurrentTag gets and sets the currently selected tag.
        /// </summary>
        public String CurrentTag { get; set; }


        /// <summary>
        /// Posts gets and sets the list of Posts that will be
        /// shown to the User.  If there is only one Post, the
        /// whole Post will be shown.
        /// </summary>
        public List<Post> Posts { get; set; }


        /// <summary>
        /// PostCounts gets and sets the list of PostCounts that 
        /// show the whole history of Posts.
        /// </summary>
        public List<PostCount> PostCounts { get; set; }
    }
}