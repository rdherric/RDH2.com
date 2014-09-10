using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.DataModel.Configuration
{
    /// <summary>
    /// ActivityConfiguration performs the configuration
    /// of the Activity object for the data store.
    /// </summary>
    internal class ActivityConfiguration : EntityTypeConfiguration<Activity>
    {
        #region Constants
        public const String ComputedTableName = "activitycomputed";
        #endregion


        /// <summary>
        /// Default constructor for the ActivityConfiguration object.
        /// </summary>
        public ActivityConfiguration()
        {
            this.ToTable(ActivityConfiguration.ComputedTableName);
            this.HasRequired(a => a.CreatedBy).WithMany().Map(m => m.MapKey(AccountConfiguration.CreatedByFK));
        }
    }
}
