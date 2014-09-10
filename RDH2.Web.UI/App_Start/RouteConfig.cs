using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace RDH2.Web.UI
{
    public class RouteConfig
    {
        /// <summary>
        /// RegisterRoutes does the job of registering the routes with 
        /// the Collection.
        /// </summary>
        /// <param name="routes">The Collection of Routes</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            //Ignore the AXD files and favicon
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");

            //Create an Array of Route Definitions
            var routeDefinitions = new[]
               {
                    new { url = "Gallery/Create", controller = "Gallery", action = "Create" },
                    new { url = "Gallery/Edit/{id}", controller = "Gallery", action = "Edit" },
                    new { url = "Gallery/EditVideos", controller = "Gallery", action = "EditVideos" },
                    new { url = "Gallery/Save", controller = "Gallery", action = "Save" },
                    new { url = "Gallery/Delete", controller = "Gallery", action = "Delete" },
                    new { url = "Gallery/GetPhotoHtml", controller = "Gallery", action = "GetPhotoHtml" },
                    new { url = "Gallery", controller = "Gallery", action = "Index" },
                    new { url = "Gallery/{*input}", controller = "Gallery", action = "Show" },
                    new { url = "Photo/Show/{photoID}", controller = "Photo", action = "Show" },
                    new { url = "Video/Show/{videoID}", controller = "Video", action = "Show" },
                    new { url = "Post/Create", controller = "Post", action = "Create" },
                    new { url = "Post/Edit/{id}", controller = "Post", action = "Edit" },
                    new { url = "Post/Save", controller = "Post", action = "Save" },
                    new { url = "Post/Delete", controller = "Post", action = "Delete" },
                    new { url = "Post/{input}", controller = "Post", action = "Show" },
                    new { url = "Post", controller = "Post", action = "Index" },
                    new { url = "ImageBrowser/Create", controller = "ImageBrowser", action = "Create" },
                    new { url = "ImageBrowser/Destroy", controller = "ImageBrowser", action = "Destroy" },
                    new { url = "ImageBrowser/Read", controller = "ImageBrowser", action = "Read" },
                    new { url = "ImageBrowser/Thumbnail", controller = "ImageBrowser", action = "Thumbnail" },
                    new { url = "ImageBrowser/Upload", controller = "ImageBrowser", action = "Upload" },
                    new { url = "About", controller = "About", action = "Index" },
                    new { url = "Contact", controller = "Contact", action = "Index" },
                    new { url = "Atom", controller = "Atom", action = "Index" },
                    new { url = "{input}", controller = "Post", action = "Show" }
               };

            //Iterate through the route definitions and register them
            foreach (var definition in routeDefinitions)
            {
                routes.MapRoute(
                    null,
                    definition.url,
                    new { controller = definition.controller, action = definition.action });
            }

            //Finally, map the Generic Route
            routes.MapRoute(
                null,
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}