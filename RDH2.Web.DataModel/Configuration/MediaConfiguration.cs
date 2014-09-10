using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.DataModel.Configuration
{
    /// <summary>
    /// MediaConfiguration is used to configure the Media
    /// class to the data store.
    /// </summary>
    internal class MediaConfiguration : EntityTypeConfiguration<Media>
    {
        #region Constants
        public const String TableName = "media";
        #endregion

        
        /// <summary>
        /// Default constructor for the MediaConfiguration object.
        /// </summary>
        public MediaConfiguration()
        {
            //Setup the Media class
            
        }
    }
}
