using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDH2.Web.DataModel.Model
{
    /// <summary>
    /// PostCount represents a summary of counts for the Post
    /// object in the data store.
    /// </summary>
    public class PostCount
    {
        /// <summary>
        /// Bucket gets and sets the container for the count
        /// of Posts in the data store.
        /// </summary>
        public DateTime Bucket { get; set; }


        /// <summary>
        /// Count gets and sets the number of Posts within
        /// the Bucket container.
        /// </summary>
        public Int32 Count { get; set; }
    }
}
