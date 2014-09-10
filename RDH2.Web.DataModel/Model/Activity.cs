using System;
using System.Linq;

namespace RDH2.Web.DataModel.Model
{
    /// <summary>
    /// Activity represents a gallery update or post in 
    /// the data store.
    /// </summary>
    public class Activity
    {
        #region Public Properties
        /// <summary>
        /// ID gets and sets the ID of the Activity that occurred.
        /// </summary>
        public String ID { get; set; }

        
        /// <summary>
        /// Title gets and sets the Title to display to the User
        /// when the Activity is shown.
        /// </summary>
        public String Title { get; set; }


        /// <summary>
        /// ReducedTitle gets and sets the reduced Title to use in 
        /// the HyperLink when the Activity is shown.
        /// </summary>
        public String ReducedTitle { get; set; }


        /// <summary>
        /// ActivityDate gets and sets the date that the Activity 
        /// occurred when it is shown.
        /// </summary>
        public DateTime ActivityDate { get; set; }


        /// <summary>
        /// ActivityType gets and sets what type of Activity occurred
        /// to set the title appropriately when it is shown.
        /// </summary>
        public Int32 ActivityType { get; set; }


        /// <summary>
        /// Summary gets and sets the Summary of the Activity 
        /// based on the type -- part of the post for a Post and
        /// a generic string for a Gallery.
        /// </summary>
        public String Summary { get; set; }
        #endregion


        #region Public Relationship Properties
        /// <summary>
        /// CreatedBy gets and sets the User that has created 
        /// the content.
        /// </summary>
        public virtual Account CreatedBy { get; set; }
        #endregion
    }
}
