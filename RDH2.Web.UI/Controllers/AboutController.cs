using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RDH2.Web.UI.Controllers
{
    /// <summary>
    /// AboutController acts as the Controller between the 
    /// About page and the User.
    /// </summary>
    public class AboutController : Controller
    {
        /// <summary>
        /// Index shows the base About page.
        /// </summary>
        /// <returns>ActionResult of View data</returns>
        public ActionResult Index()
        {
            //Return the Index View
            return View();
        }

    }
}
