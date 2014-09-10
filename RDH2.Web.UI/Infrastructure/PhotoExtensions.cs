using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.UI.Infrastructure
{
    /// <summary>
    /// PhotoExtensions adds extension methods to the Photo
    /// data model object
    /// </summary>
    public static class PhotoExtensions
    {
        /// <summary>
        /// GetSize gets the size of the Image that represents
        /// the photo at the LargePath.
        /// </summary>
        /// <param name="photo">The Photo data model object</param>
        /// <param name="basePath">The base path on the Server to the Photo</param>
        /// <returns>Size of Image information</returns>
        public static Size GetLargeSize(this Photo photo, String basePath)
        {
            //Declare a variable to return 
            Size rtn = new Size();

            //Try to the get the Size
            try
            {
                //Get the Image
                Image img = Image.FromFile(Path.Combine(basePath, photo.LargePath));

                //Set the return
                rtn = img.Size;
            }
            catch { }

            //Return the result
            return rtn;
        }
    }
}