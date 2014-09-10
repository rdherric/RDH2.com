using System;
using System.Linq;
using System.Web.Mvc;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.UI.Controllers
{
    /// <summary>
    /// VideoController acts as the Controller for the Videos
    /// stored in the data store.
    /// </summary>
    public class VideoController : Controller
    {
        #region Constants
        public const String VideoImageURLFmt = "http://i.ytimg.com/vi/{0}/default.jpg";
        public const String VideoIDQuery = "videoID";
        #endregion


        #region Member Variables
        private Repository<Gallery> _galleries = null;
        private Repository<Video> _videos = null;
        #endregion


        #region Constructor
        /// <summary>
        /// Default Constructor for the VideoController object.
        /// </summary>
        public VideoController()
        {
            //Setup the Repositories
            this._videos = new Repository<Video>();
            this._galleries = new Repository<Gallery>();
        }
        #endregion


        #region MVC View Methods
        /// <summary>
        /// GetDialogHtml gets the Partial of the Video that
        /// will be shown to the User.
        /// </summary>
        /// <param name="videoID">The ID of the Video to show</param>
        /// <returns>Result of Video HTML</returns>
        public ActionResult GetDialogHtml(Int32 videoID)
        {
            //Declare a variable to return
            PartialViewResult rtn = null;

            //Get the Photo from the repository
            Video video = this._videos
                .GetBy(v => v.ID == videoID)
                .FirstOrDefault();

            //If the Photo came back successfully, send the View
            //to the User
            if (video != null)
            {
                //Setup the View
                rtn = this.PartialView("DialogPartial", video);
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// Show shows the Video in the MVC Mobile view.
        /// </summary>
        /// <param name="videoID">The ID of the Video to show</param>
        /// <returns>ActionResult of Video HTML</returns>
        public ActionResult Show(Int32 videoID)
        {
            //Declare a variable to return
            ActionResult rtn = null;

            //Get the Photo from the repository
            Video video = this._videos
                .GetBy(v => v.ID == videoID)
                .FirstOrDefault();

            //If the Photo came back successfully, send the View
            //to the User
            if (video != null)
            {
                //Setup the View
                rtn = this.View("DialogPartial", video);
            }

            //Return the result
            return rtn;
        }
        #endregion


        #region MVC Edit Methods
        /// <summary>
        /// Update saves a Video to the database with the selected
        /// Gallery ID.
        /// </summary>
        /// <param name="account">The Account of the logged in User</param>
        /// <param name="video">The Video to be updated</param>
        /// <param name="galleryID">The ID of the Gallery to set the Video</param>
        /// <returns>JsonResult of Video saving information</returns>
        public JsonResult Update(Account account, Video video, Int32 galleryID)
        {
            //Declare a variable to return
            JsonResult rtn = null;

            //If the Account is valid, save the Video
            if (account != null)
            {
                //Get the Gallery from the Repository
                Gallery gallery = this._galleries
                    .GetBy(g => g.ID == galleryID)
                    .FirstOrDefault();

                //Get the Video from the Repository
                Video toUpdate = this._videos
                    .GetBy(v => v.ID == video.ID)
                    .FirstOrDefault();

                //If the Updateable Video comes back, set the values in it 
                //and update
                if (toUpdate != null)
                {
                    //Set the values
                    toUpdate.Modified = DateTime.Now;
                    toUpdate.Gallery = gallery;

                    //Update the Photo
                    this._videos.Update(toUpdate);

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
