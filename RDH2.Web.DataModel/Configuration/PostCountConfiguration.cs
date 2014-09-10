using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.DataModel.Configuration
{
    /// <summary>
    /// PostCountConfiguration configures the PostCount class
    /// for retrieval from the data store.
    /// </summary>
    internal class PostCountConfiguration : EntityTypeConfiguration<PostCount>
    {
        #region Constants
        public const String ComputedTableName = "postcountcomputed";
        #endregion


        /// <summary>
        /// Default constructor for the PostCountConfiguration object.
        /// </summary>
        public PostCountConfiguration()
        {
            //Setup the PostCount class
            this.ToTable(PostCountConfiguration.ComputedTableName);
            this.HasKey(pc => pc.Bucket);
        }
    }
}
