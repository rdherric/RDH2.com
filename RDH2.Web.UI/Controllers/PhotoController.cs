using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RDH2.Web.DataModel.Model;
using RDH2.Web.UI.Infrastructure;
using RDH2.Web.UI.Models;

namespace RDH2.Web.UI.Controllers
{
    public class PhotoController : Controller
    {
        #region Constants
        public const String BasePath = "~/Content/Images/Upload";
        private const Int32 _thumbnailLongSide = 150;
        private const Int32 _largeLongSide = 800;
        #endregion


        #region Member Variables
        private Repository<Gallery> _galleries = null;
        private Repository<Photo> _photos = null;
        #endregion


        #region Constructor
        /// <summary>
        /// Default Constructor for the PhotoController class.
        /// </summary>
        public PhotoController()
        {
            //Save the input in the member variables
            this._galleries = new Repository<Gallery>();
            this._photos = new Repository<Photo>();
        }
        #endregion


        #region MVC View Methods
        /// <summary>
        /// GetDialogHtml retrieves the HTML that will show a 
        /// Photo to the User through a jQuery AJAX call.
        /// </summary>
        /// <param name="photoID">The ID of the Photo to show to the User</param>
        /// <returns>ActionResult of Photo data</returns>
        public ActionResult GetDialogHtml(Int32 photoID)
        {
            //Declare a variable to return
            PartialViewResult rtn = null;

            //Get the Photo from the repository
            Photo photo = this._photos
                .GetBy(p => p.ID == photoID)
                .FirstOrDefault();

            //If the Photo came back successfully, send the View
            //to the User
            if (photo != null)
            {
                //Create a View Model
                PhotoViewModel model = new PhotoViewModel
                {
                    Title = photo.Title,
                    Path = this.Url.Content(Path.Combine(PhotoController.BasePath, photo.Gallery.FullPath, photo.LargePath)),
                    Size = photo.GetLargeSize(this.Server.MapPath(Path.Combine(PhotoController.BasePath, photo.Gallery.FullPath))),
                    FitImage = false
                };

                //Setup the View
                rtn = this.PartialView("DialogPartial", model);
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// GetUpdateHtml retrieves the HTML that will show the 
        /// Update controls on the page through a jQuery AJAX call.
        /// </summary>
        /// <param name="photoID">The ID of the photo to show to the User</param>
        /// <returns>ActionResult of Photo data</returns>
        public ActionResult GetUpdateHtml(Int32 photoID)
        {
            //Declare a variable to return 
            PartialViewResult rtn = null;

            //Get the Photo from the Repository
            Photo photo = this._photos
                .GetBy(p => p.ID == photoID)
                .FirstOrDefault();

            //If the photo came back successfully, send the View
            //to the User
            if (photo != null)
            {
                rtn = this.PartialView("UpdatePartial", photo);
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// Show shows a Photo in the MVC Mobile view.
        /// </summary>
        /// <param name="photoID">The ID of the Photo to show</param>
        /// <returns>ActionResult of Photo data</returns>
        public ActionResult Show(Int32 photoID)
        {
            //Declare a variable to return
            ActionResult rtn = null;

            //Get the Photo from the repository
            Photo photo = this._photos
                .GetBy(p => p.ID == photoID)
                .FirstOrDefault();

            //If the Photo came back successfully, send the View
            //to the User
            if (photo != null)
            {
                //Create a View Model
                PhotoViewModel model = new PhotoViewModel
                {
                    Title = photo.Title,
                    Path = this.Url.Content(Path.Combine(PhotoController.BasePath, photo.Gallery.FullPath, photo.LargePath)),
                    Size = photo.GetLargeSize(this.Server.MapPath(Path.Combine(PhotoController.BasePath, photo.Gallery.FullPath))),
                    FitImage = true
                };

                //Setup the View
                rtn = this.View("DialogPartial", model);
            }

            //Return the result
            return rtn;
        }
        #endregion


        #region MVC Edit Methods
        /// <summary>
        /// Save saves the posted Photos to the database.
        /// </summary>
        /// <param name="account">The Account that is used to create the Photos</param>
        /// <param name="galleryID">The ID of the Gallery that holds the Photos</param>
        /// <param name="photos">The Photos that are to be saved</param>
        /// <returns>ActionResult of saved Gallery</returns>
        public ActionResult Save(Account account, Int32 galleryID, IEnumerable<HttpPostedFileBase> photos)
        {
            //If the Account is good, save the Photos
            if (account != null)
            {
                //Get the Gallery from the Repository
                Gallery gallery = this._galleries
                    .GetBy(g => g.ID == galleryID)
                    .FirstOrDefault();

                //Generate the Path to the Gallery
                String path = this.Server.MapPath(Path.Combine(PhotoController.BasePath, gallery.FullPath));

                //Iterate through the uploads and save them to disk
                foreach (HttpPostedFileBase file in photos)
                {
                    //Try to make an Image with the data
                    Image img = Image.FromStream(file.InputStream);

                    //Resize the Image into thumbnail and large
                    Image thumbnail = img.Resize(PhotoController._thumbnailLongSide);
                    Image large = img.Resize(PhotoController._largeLongSide);

                    //Get the file names for the Images
                    String baseFileName = Path.GetFileNameWithoutExtension(file.FileName);
                    String thumbFileName = Path.ChangeExtension(baseFileName + "_thumb", "jpg");
                    String largeFileName = Path.ChangeExtension(baseFileName, "jpg");
                    String thumbFilePath = Path.Combine(path, thumbFileName);
                    String largeFilePath = Path.Combine(path, largeFileName);

                    //Save the Images to the directory of the Gallery
                    thumbnail.Save(thumbFilePath, ImageFormat.Jpeg);
                    large.Save(largeFilePath, ImageFormat.Jpeg);

                    //Finally create a new Photo object and add it to 
                    //the Gallery
                    Photo photo = new Photo
                    {
                        CreatedBy = account,
                        Gallery = gallery,
                        LargePath = largeFileName,
                        ThumbnailPath = thumbFileName
                    };

                    //Add the Photo to the database
                    this._photos.Add(photo);
                }
            }

            //Send back a result to show the page again
            return this.RedirectToAction("Edit", "Gallery", new { id = galleryID });
        }


        /// <summary>
        /// Update updates a Photo in the database.
        /// </summary>
        /// <param name="account">The Account used to update the Photo</param>
        /// <param name="photo">The Photo that is to be updated</param>
        /// <returns>JsonResult of photo update data</returns>
        public JsonResult Update(Account account, Photo photo)
        {
            //Declare a variable to return
            JsonResult rtn = null;

            //If the Account is valid, save the Photo
            if (account != null)
            {
                //Get the Photo from the Repository
                Photo toUpdate = this._photos
                    .GetBy(p => p.ID == photo.ID)
                    .FirstOrDefault();

                //If the Updateable Photo comes back, set the values in it 
                //and update
                if (toUpdate != null)
                {
                    //Set the values
                    toUpdate.Modified = DateTime.Now;
                    toUpdate.Title = photo.Title;

                    //Update the Photo
                    this._photos.Update(toUpdate);

                    //Set the return value
                    rtn = this.Json(new { success = true });
                }
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// Delete deletes a Photo from the database.
        /// </summary>
        /// <param name="account">The Account of the User deleting the Photo</param>
        /// <param name="photoID">The ID of the Photo to delete</param>
        /// <returns>JsonResult of Photo data</returns>
        public JsonResult Delete(Account account, Int32 photoID)
        {
            //Declare a variable to return
            JsonResult rtn = null;

            //Make sure that the user is logged in
            if (account != null)
            {
                //Get the Photo from the Repository
                Photo photo = this._photos
                    .GetBy(p => p.ID == photoID)
                    .FirstOrDefault();

                //If the Photo is not null, delete the information
                if (photo != null)
                {
                    //Get the full path to the Files
                    String path = this.Server.MapPath(Path.Combine(PhotoController.BasePath, photo.Gallery.FullPath));

                    //Delete the files that have to do with the Photo
                    System.IO.File.Delete(Path.Combine(path, photo.ThumbnailPath));
                    System.IO.File.Delete(Path.Combine(path, photo.LargePath));

                    //Delete the Object from the database
                    this._photos.Delete(photo);

                    //Set the return value
                    rtn = this.Json(new { success = true });
                }
            }

            //Return the result
            return rtn;
        }
        #endregion
    }
}
