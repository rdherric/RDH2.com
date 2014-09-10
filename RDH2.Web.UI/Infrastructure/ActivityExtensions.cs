using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using RDH2.Web.DataModel.Model;
using RDH2.Web.UI.Controllers;

namespace RDH2.Web.UI.Infrastructure
{
    /// <summary>
    /// ActivityExtensions extends the Activity class
    /// so that logic is encapsulated only here.
    /// </summary>
    public static class ActivityExtensions
    {
        #region Constants
        private const String _post = "Post";
        private const String _gallery = "Gallery";
        private const String _action = "Show";
        private const String _postFmt = "New Post: {0}";
        private const String _galleryFmt = "Gallery Updated: {0}";
        private const String _photoFmt = "<img src=\"{0}{1}\" />";
        #endregion


        /// <summary>
        /// FormatTitle formats the Title of an Activity for use
        /// by the Home page.
        /// </summary>
        /// <param name="activity">The Activity to format the Title</param>
        /// <returns>String of formatted Title data</returns>
        public static String FormatTitle(this Activity activity)
        {
            //If the Title is a Post, just return the Title.
            //Otherwise, return the Gallery or Video title 
            //formatted into the constant.
            String format = String.Empty;
            switch (activity.ActivityType)
            {
                case 0:
                    format = ActivityExtensions._postFmt;
                    break;

                case ActivityType.Gallery:
                    format = ActivityExtensions._galleryFmt;
                    break;

                default:
                    format = ActivityExtensions._postFmt;
                    break;
            }

            //Return the result
            return String.Format(format, activity.Title);
        }


        /// <summary>
        /// ToUrl formats the information in the Activity for
        /// use by the Atom and Home Controllers.
        /// </summary>
        /// <param name="activity">The Activity to format to a URL</param>
        /// <returns>String URL of Activity data</returns>
        public static String ToUrl(this Activity activity, UrlHelper urlHelper)
        {
            //Declare a variable to return
            String rtn = String.Empty;

            //Return the URL based on the ActivityType
            if (activity.ActivityType == ActivityType.Post)
            {
                rtn = urlHelper.Action(
                    ActivityExtensions._action, ActivityExtensions._post, new { input = activity.ReducedTitle });
            }
            else if (activity.ActivityType == ActivityType.Gallery)
            {
                rtn = urlHelper.Action(
                    ActivityExtensions._action, ActivityExtensions._gallery, new { input = activity.ReducedTitle });
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// ToPhotoUrls changes the Gallery that is referenced into
        /// a set of URL data.
        /// </summary>
        /// <param name="activity">The Activity to format to a set of Photo URLs</param>
        /// <param name="urlHelper">The UrlHelper to create the URL</param>
        /// <param name="baseUrl">The base URL of the Image</param>
        /// <returns>String of Photo URL data</returns>
        public static String ToPhotoUrls(this Activity activity, UrlHelper urlHelper, String baseUrl)
        {
            //Declare a variable to return
            StringBuilder rtn = new StringBuilder();

            //If this is a Gallery activity type, add all of the 
            //Photo URLs to the string.  Otherwise, add the base
            //YouTube image to the string.
            if (activity.ActivityType == ActivityType.Gallery)
            {
                //Get the Gallery in a circuitous manner
                Gallery gallery = activity.CreatedBy.Galleries
                    .Where(g => g.ID == Convert.ToInt32(activity.ID.Substring(1)))
                    .FirstOrDefault();

                //If the Gallery comes back, use it
                if (gallery != null)
                {
                    //If there are photos, show the images
                    if (gallery.Photos != null && gallery.Photos.Count > 0)
                    {
                        //Add the Photos label
                        rtn.Append("photos<br/>");

                        //Iterate through the Gallery Photos
                        foreach (Photo p in gallery.Photos)
                        {
                            rtn.AppendLine(String.Format(ActivityExtensions._photoFmt, baseUrl,
                                urlHelper.Content(Path.Combine(PhotoController.BasePath, gallery.FullPath, p.ThumbnailPath))));
                        }
                    }

                    //If there were Photos, add a break
                    if (rtn.Length > 0)
                    {
                        rtn.Append("<br/>");
                    }

                    //If there are videos, show the images
                    if (gallery.Videos != null && gallery.Videos.Count > 0)
                    {
                        //Add the Photos label
                        rtn.Append("videos<br/>");

                        //Iterate through the Gallery Video
                        foreach (Video v in gallery.Videos)
                        {
                            rtn.AppendLine(String.Format(ActivityExtensions._photoFmt,
                                String.Format(VideoController.VideoImageURLFmt, v.VideoID), ""));
                        }
                    }
                }
            }

            //Return the result
            return rtn.ToString();
        }
    }
}