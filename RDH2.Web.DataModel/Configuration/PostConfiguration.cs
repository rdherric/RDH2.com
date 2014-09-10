using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.DataModel.Configuration
{
    /// <summary>
    /// PostConfiguration is the configuration class for the 
    /// Post data object.
    /// </summary>
    internal class PostConfiguration : EntityTypeConfiguration<Post>
    {
        #region Constants
        public const String TableName = "post";
        #endregion


        /// <summary>
        /// Default constructor for the PostConfiguration object.
        /// </summary>
        public PostConfiguration()
        {
            //Setup the Post class
            this.ToTable(PostConfiguration.TableName);
            this.HasKey(p => p.ID);
            this.Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(p => p.Title).IsRequired().HasMaxLength(50);
            this.Property(p => p.ReducedTitle).IsRequired().HasMaxLength(50);
            this.Property(p => p.Body).IsRequired().IsMaxLength();
            this.Property(p => p.Tags).IsOptional().IsMaxLength();
            this.Property(p => p.Created).IsRequired();
            this.Property(p => p.Modified).IsRequired();
            this.Property(p => p.Published).IsRequired();
            this.HasRequired(p => p.CreatedBy).WithMany(a => a.Posts).Map(x => x.MapKey(AccountConfiguration.CreatedByFK));
            this.HasMany(p => p.Comments).WithRequired(c => c.Post);
        }
    }
}
