using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.DataModel.Configuration
{
    /// <summary>
    /// VideoConfiguration configures the Video class
    /// for use by the Framework.
    /// </summary>
    internal class VideoConfiguration : EntityTypeConfiguration<Video>
    {
        #region Constants
        public const String TableName = "video";
        #endregion


        /// <summary>
        /// Default constructor for the VideoConfiguration class.
        /// </summary>
        public VideoConfiguration()
        {
            //Setup the Video class
            this.ToTable(VideoConfiguration.TableName);
            this.HasKey(v => v.ID);
            this.Property(v => v.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(v => v.Title).HasMaxLength(1024);
            this.Property(v => v.VideoID).HasMaxLength(1024);
            this.Property(v => v.Created).IsRequired();
            this.Property(v => v.Modified).IsRequired();
            this.HasRequired(v => v.Gallery).WithMany(g => g.Videos).Map(x => x.MapKey(GalleryConfiguration.GalleryIDFK));
            this.HasRequired(v => v.CreatedBy).WithMany(a => a.Videos).Map(x => x.MapKey(AccountConfiguration.CreatedByFK));
        }
    }
}
