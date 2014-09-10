using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.DataModel.Configuration
{
    /// <summary>
    /// AccountConfiguration configures the Account data object
    /// for use by the data store.
    /// </summary>
    internal class AccountConfiguration : EntityTypeConfiguration<Account>
    {
        #region Constants
        public const String TableName = "account";
        public const String CreatedByFK = "createdby";
        #endregion


        /// <summary>
        /// Default constructor for the AccountConfiguration object.
        /// </summary>
        public AccountConfiguration()
        {
            //Configure the Account class
            this.ToTable(AccountConfiguration.TableName);
            this.HasKey(a => a.ID);
            this.Property(a => a.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(a => a.Created).IsRequired();
            this.Property(a => a.Modified).IsRequired();
            this.HasMany(a => a.Posts).WithRequired(p => p.CreatedBy).Map(m => m.MapKey(AccountConfiguration.CreatedByFK));
            this.HasMany(a => a.Galleries).WithRequired(g => g.CreatedBy).Map(m => m.MapKey(AccountConfiguration.CreatedByFK));
            this.HasMany(a => a.Photos).WithRequired(p => p.CreatedBy);
        }
    }
}
