using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.DataModel.Configuration
{
    /// <summary>
    /// GalleryConfiguration is used to configure the 
    /// Gallery class for use by the data store.
    /// </summary>
    internal class GalleryConfiguration : EntityTypeConfiguration<Gallery>
    {
        #region Constants
        public const String ComputedTableName = "gallerycomputed";
        public const String ParentGalleryFK = "parent";
        public const String GalleryIDFK = "galleryid";
        #endregion


        /// <summary>
        /// Default configuration for the GalleryConfiguration class.
        /// </summary>
        public GalleryConfiguration()
        {
            //Setup the basic properties on the class
            this.ToTable(GalleryConfiguration.ComputedTableName);
            this.HasKey(g => g.ID);
            this.Property(g => g.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(g => g.Created).IsRequired();
            this.Property(g => g.Modified).IsRequired();
            this.Property(g => g.Name).IsRequired().HasMaxLength(50);
            this.Property(g => g.ReducedName).IsRequired().HasMaxLength(50);
            this.HasOptional(g => g.Parent).WithMany(p => p.Galleries).Map(x => x.MapKey(GalleryConfiguration.ParentGalleryFK));
            this.HasMany(g => g.Photos).WithRequired(p => p.Gallery).Map(x => x.MapKey(GalleryConfiguration.GalleryIDFK));
            this.HasMany(g => g.Videos).WithRequired(v => v.Gallery).Map(x => x.MapKey(GalleryConfiguration.GalleryIDFK));
            this.HasRequired(g => g.CreatedBy).WithMany(a => a.Galleries).Map(x => x.MapKey(AccountConfiguration.CreatedByFK));
        }
    }
}
