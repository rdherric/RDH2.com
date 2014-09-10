using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.UI.Controllers
{
    /// <summary>
    /// HomeController handles the Home page for the 
    /// RDH2.COM web site.
    /// </summary>
    public class HomeController : Controller
    {
        #region Constants
        private const Int32 _activityCount = 10;
        #endregion


        #region Member Variables
        private readonly Repository<Activity> _activities = null;
        #endregion


        #region Constructor
        /// <summary>
        /// Default Constructor for the HomeController class.
        /// </summary>
        public HomeController()
        {
            //Save the input in the member variables
            this._activities = new Repository<Activity>();
        }
        #endregion


        #region MVC Action Methods
        /// <summary>
        /// Index returns the default View for the Index page.
        /// </summary>
        /// <returns>ActionResult for the Home/Index View</returns>
        public ActionResult Index()
        {
            //Get the List of Activities from the Repository
            List<Activity> activityList = this._activities
                .GetAll()
                .Include(a => a.CreatedBy)
                .Take(HomeController._activityCount)
                .OrderByDescending(a => a.ActivityDate)
                .ToList();

            //Return the default View for Index
            return View(activityList);
        }
        #endregion
    }
}
