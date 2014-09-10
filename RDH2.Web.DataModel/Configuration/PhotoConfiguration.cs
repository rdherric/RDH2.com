using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.DataModel.Configuration
{
    /// <summary>
    /// PhotoConfiguration is used to configure the Photo
    /// class to the data store.
    /// </summary>
    internal class PhotoConfiguration : EntityTypeConfiguration<Photo>
    {
        #region Constants
        public const String TableName = "photo";
        #endregion

        
        /// <summary>
        /// Default constructor for the PhotoConfiguration object.
        /// </summary>
        public PhotoConfiguration()
        {
            //Setup the Photo class
            this.ToTable(PhotoConfiguration.TableName);
            this.HasKey(p => p.ID);
            this.Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(p => p.Title).HasMaxLength(50);
            this.Property(p => p.LargePath).IsRequired().IsMaxLength();
            this.Property(p => p.ThumbnailPath).IsRequired().IsMaxLength();
            this.Property(p => p.Created).IsRequired();
            this.Property(p => p.Modified).IsRequired();
            this.HasRequired(p => p.Gallery).WithMany(g => g.Photos).Map(m => m.MapKey(GalleryConfiguration.GalleryIDFK));
            this.HasRequired(p => p.CreatedBy).WithMany(a => a.Photos).Map(x => x.MapKey(AccountConfiguration.CreatedByFK));
        }
    }
}
