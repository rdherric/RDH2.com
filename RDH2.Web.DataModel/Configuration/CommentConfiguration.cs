using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.DataModel.Configuration
{
    /// <summary>
    /// CommentConfiguration is used to configure the DbContext
    /// for the Comment data store class.
    /// </summary>
    internal class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        #region Member Variables
        public const String TableName = "comment";
        #endregion


        /// <summary>
        /// Default constructor for the CommentConfiguration object.
        /// </summary>
        public CommentConfiguration()
        {
            //Setup the configuration
            this.ToTable(CommentConfiguration.TableName);
            this.HasKey(c => c.ID);
            this.Property(c => c.Name).IsRequired().HasMaxLength(50);
            this.Property(c => c.EmailAddress).HasMaxLength(50);
            this.Property(c => c.Body).IsRequired().HasMaxLength(1024);
        }
    }
}
