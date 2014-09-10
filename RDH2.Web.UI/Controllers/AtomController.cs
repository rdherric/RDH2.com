using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RDH2.Web.DataModel.Model;
using RDH2.Web.UI.Infrastructure;

namespace RDH2.Web.UI.Controllers
{
    /// <summary>
    /// RssController handles the creation of the RSS feed and 
    /// returns it to the User.
    /// </summary>
    public class AtomController : Controller
    {
        #region Constants
        public const Int32 ActivityCount = 25;
        #endregion


        #region Member Variables
        private readonly Repository<Activity> _activities = null;
        #endregion


        #region Constructor
        /// <summary>
        /// Default constructor for the AtomController class.
        /// </summary>
        public AtomController()
        {
            //Save the input in the member variable
            this._activities = new Repository<Activity>();
        }
        #endregion


        #region MVC RSS Methods
        /// <summary>
        /// Index is the main method that the Atom feed uses.
        /// </summary>
        /// <returns>ActionResult of Atom data</returns>
        public ContentResult Index()
        {
            //Get the Activities from the Repository
            List<Activity> activities = this._activities
                .GetAll()
                .Take(AtomController.ActivityCount)
                .OrderByDescending(a => a.ActivityDate)
                .ToList();

            //Return the result
            return this.Content(AtomConvertor.ToFeed(activities, this.Url), "application/atom+xml", System.Text.Encoding.Default);
        }
        #endregion
    }
}
