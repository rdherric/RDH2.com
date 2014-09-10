using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using RDH2.Web.DataModel.Model;
using RDH2.Web.UI.Infrastructure;
using RDH2.Web.UI.Models;

namespace RDH2.Web.UI.Controllers
{
    /// <summary>
    /// GalleryController acts as the Controller for the 
    /// media galleries in the data store.
    /// </summary>
    public class GalleryController : Controller
    {
        #region Constants
        private const Int32 _recentActivityCount = 5;
        #endregion


        #region Member Variables
        private readonly Repository<Gallery> _galleries = null;
        private readonly Repository<Video> _videos = null;
        #endregion


        #region Constructor
        /// <summary>
        /// Default constructor for the GalleryController.
        /// </summary>
        public GalleryController()
        {
            this._galleries = new Repository<Gallery>();
            this._videos = new Repository<Video>();
        }
        #endregion


        #region MVC View Methods
        /// <summary>
        /// Index returns the full set of Galleries in the 
        /// data store.
        /// </summary>
        /// <returns>ActionResult of Gallery data</returns>
        public ActionResult Index()
        {
            //Return the result
            return this.View(GetGalleryViewModel(new Gallery()));
        }


        /// <summary>
        /// Show shows the selected Gallery to the User.
        /// </summary>
        /// <param name="input">The selected Gallery to show</param>
        /// <returns>ActionResult with View data</returns>
        public ActionResult Show(String input)
        {
            //Declare a variable to return
            ActionResult rtn = null;

            //Get the Gallery based on the input
            Gallery gallery = this._galleries
                .GetBy(g => g.FullPath == input)
                .FirstOrDefault();

            //Show the Gallery if it is found.  If the Gallery should show
            //Videos, show those.  If there is no Gallery to show, go back 
            //to the Index.
            if (gallery != null)
            {
                rtn = this.View("Index", this.GetGalleryViewModel(gallery));
            }
            else
            {
                return this.View("Index", GetGalleryViewModel(new Gallery()));
            }

            //Return the result
            return rtn;
        }
        #endregion


        #region MVC Edit Methods
        /// <summary>
        /// Create allows a logged in User to be able to create
        /// a new Gallery.
        /// </summary>
        /// <param name="account">The Account of the logged in User</param>
        /// <returns>ActionResult of View data</returns>
        public ActionResult Create(Account account)
        {
            //Declare a variable to return
            ActionResult rtn = null;

            //Return the base View with a new Gallery if the
            //User is logged in.  Otherwise, show the Index.
            if (account != null)
            {
                rtn = this.View("EditGallery", this.CreateEditGalleryViewModel(new Gallery()));
            }
            else
            {
                rtn = this.Index();
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// Edit allows a logged in User to be able to Edit as
        /// existing Gallery.
        /// </summary>
        /// <param name="account">The Account of the logged in User</param>
        /// <param name="gallery">The ID of the Gallery to edit</param>
        /// <returns>ActionResult of View data</returns>
        public ActionResult Edit(Account account, Int32 id)
        {
            //Declare a variable to return
            ActionResult rtn = null;

            //Get the existing Gallery
            Gallery gallery = this._galleries
                .GetBy(g => g.ID == id)
                .Include(g => g.Parent)
                .FirstOrDefault();

            //If the Gallery came back and the User is logged in, allow
            //the edit.  Otherwise, go to the Index.
            if (gallery != null && account != null)
            {
                rtn = this.View("EditGallery", this.CreateEditGalleryViewModel(gallery));
            }
            else
            {
                rtn = this.Index();
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// EditVideos allows a user to edit the list of Videos to put
        /// them into Galleries.
        /// </summary>
        /// <param name="account">The Account of the logged in User</param>
        /// <returns>ActionResult of View data</returns>
        public ActionResult EditVideos(Account account)
        {
            //Declare a variable to return
            ActionResult rtn = null;

            //If the Account is valid, allow the editing.  Otherwise,
            //return to the Index
            if (account != null)
            {
                //Update the Video feed from YouTube
                VideoExtensions.UpdateVideos();

                //Show the View
                rtn = this.View("EditVideos", this.CreateEditVideoViewModels());
            }
            else
            {
                rtn = this.Index();
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// SaveGallery saves a Gallery to the data store.
        /// </summary>
        /// <param name="account">The Account of the logged in User</param>
        /// <param name="gallery">The Gallery to save to the data store</param>
        /// <returns>JSON of result data</returns>
        public JsonResult Save(Account account, Gallery gallery, Int32 parentID)
        {
            //Declare a variable to return
            JsonResult rtn = null;

            //If the Account is valid, try to save the data
            if (account != null)
            {
                //Set the Account in the Post
                gallery.CreatedBy = account;

                //Setup the reduced Name
                gallery.ReducedName = gallery.Name.Reduce();

                //Get the parent Gallery
                gallery.Parent = this._galleries
                    .GetBy(g => g.ID == parentID)
                    .FirstOrDefault();

                //Try to save the Post to the data store
                try
                {
                    if (gallery.ID == -1)
                    {
                        //Save the Gallery in the database
                        gallery = this._galleries.AddUnique(gallery, g => gallery.ReducedName == g.ReducedName);

                        //Generate the Path to the Gallery
                        String path = this.Server.MapPath(Path.Combine(PhotoController.BasePath, gallery.FullPath));

                        //If the path doesn't exist, create it
                        if (Directory.Exists(path) == false)
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                    else
                    {
                        //Get the Gallery from the Repository
                        Gallery toUpdate = this._galleries.GetBy(g => g.ID == gallery.ID).FirstOrDefault();

                        //If the gallery Name changed, change the name of the
                        //Directory that holds the media
                        if (toUpdate.Name != gallery.Name ||
                            (toUpdate.Parent != null && toUpdate.Parent.ID != parentID))
                        {
                            //Get the source directory
                            String source = this.Server.MapPath(Path.Combine(PhotoController.BasePath, toUpdate.FullPath));
                            String destination = this.Server.MapPath(Path.Combine(PhotoController.BasePath, 
                                (gallery.Parent == null ? "" : gallery.Parent.FullPath), gallery.ReducedName));

                            //Move the directory
                            Directory.Move(source, destination);
                        }

                        //Set the values on the Gallery to update
                        toUpdate.CreatedBy = account;
                        toUpdate.Modified = DateTime.Now;
                        toUpdate.Name = gallery.Name;
                        toUpdate.Parent = gallery.Parent;
                        toUpdate.ReducedName = gallery.ReducedName;
                        toUpdate.Expanded = gallery.Expanded;
                        toUpdate.IsRecent = gallery.IsRecent;

                        //Save the Gallery to the database
                        gallery = this._galleries.Update(toUpdate);
                    }
                }
                catch (Exception e)
                {
                    rtn = this.Json(new { success = false, message = e.Message });
                }

                //If the Gallery was saved properly, return a success message
                if (gallery.ID > 0)
                {
                    rtn = this.Json(new
                    {
                        success = true,
                        id = gallery.ID,
                        url = this.Url.Action("Show", new { input = gallery.ReducedName })
                    });
                }
            }

            //Return the result
            return rtn;
        }
        #endregion


        #region Helper Methods
        /// <summary>
        /// GetGalleryViewModel creates the GalleryViewModel that
        /// runs the Index and Show methods.
        /// </summary>
        /// <param name="gallery">The Gallery used to create the View Model</param>
        /// <returns>GalleryViewModel with data</returns>
        private GalleryViewModel GetGalleryViewModel(Gallery gallery)
        {
            //Get the List of Galleries with Recent Activity
            List<Gallery> recent = this._galleries
                .GetAll()
                .Where(g => g.IsRecent == true && g.ID > -1)
                .Take(GalleryController._recentActivityCount)
                .OrderByDescending(g => g.LastModified)
                .ToList();

            //Get the List of all Galleries from the Repository
            List<Gallery> all = this._galleries
                .GetAll()
                .Where(g => g.Parent == null && g.ID > -1)
                .OrderBy(g => g.Name)
                .ToList();

            //If the Gallery ID is -1, then this is the Index
            //page, so set the recent folder on the Galleries 
            //property
            if (gallery.ID == -1)
            {
                gallery.Galleries = recent;
            }

            //Create the View Model
            return new GalleryViewModel
            {
                RecentActivity = recent,
                AllGalleries = all,
                SelectedGallery = gallery,
                VideoID = this.Request.QueryString[VideoController.VideoIDQuery]
            };
        }


        /// <summary>
        /// CreateEditGalleryViewModel creates the EditGalleryViewModel
        /// required to edit a Gallery.
        /// </summary>
        /// <param name="gallery">The Gallery to edit</param>
        /// <returns>EditGalleryViewModel with data to edit the Gallery</returns>
        private EditGalleryViewModel CreateEditGalleryViewModel(Gallery gallery)
        {
            //Get the list of all Galleries ordered by FullPath
            List<Gallery> all = this._galleries
                .GetAll()
                .Where(g => g.ID > -1)
                .OrderBy(g => g.FullName)
                .ToList();

            //Put a null Gallery on the top of the List
            all.Insert(0, new Gallery { Name = "None", FullName = "-- No Parent --" });

            //Return the result
            return new EditGalleryViewModel
            {
                Gallery = gallery,
                ParentID = (gallery.Parent == null ? -1 : gallery.Parent.ID),
                AllGalleries = all
            };
        }


        /// <summary>
        /// CreateEditVideViewModels creates a List of view models 
        /// for the EditVideos view to consume.
        /// </summary>
        /// <returns>List of EditVideoViewModel objects</returns>
        private List<EditVideoViewModel> CreateEditVideoViewModels()
        {
            //Declare a variable to return
            List<EditVideoViewModel> rtn = new List<EditVideoViewModel>();

            //Get the list of all Galleries ordered by FullPath
            List<Gallery> allGalleries = this._galleries
                .GetAll()
                .Where(g => g.ID > -1)
                .OrderBy(g => g.FullName)
                .ToList();

            //Put a null Gallery on the top of the List
            allGalleries.Insert(0, new Gallery { Name = "None", FullName = "-- No Parent --" });

            //Get the list of all Videos
            List<Video> videos = this._videos
                .GetAll()
                .OrderByDescending(v => v.Created)
                .ToList();

            //Iterate through the Videos and add them to the List
            foreach (Video v in videos)
            {
                rtn.Add(new EditVideoViewModel
                {
                    Video = v,
                    GalleryID = (v.Gallery != null ? v.Gallery.ID : -1),
                    AllGalleries = allGalleries
                });
            }

            //Return the result
            return rtn;
        }
        #endregion
    }
}
