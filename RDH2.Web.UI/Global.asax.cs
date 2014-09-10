using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using RDH2.Web.DataModel.Context;
using RDH2.Web.DataModel.Model;
using RDH2.Web.UI.Infrastructure;

namespace RDH2.Web.UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application_Start is called when the Application starts.
        /// </summary>
        protected void Application_Start()
        {
            //Register all of the Areas
            AreaRegistration.RegisterAllAreas();

            //Register the AccountModelBinder
            ModelBinders.Binders.Add(typeof(Account), new AccountModelBinder());

            //Register the configurations
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }


        /// <summary>
        /// Application_BeginRequest is called when there is a new
        /// Request made of the Application.
        /// </summary>
        /// <param name="sender">The Application that has a new Request</param>
        /// <param name="e">The EventArgs sent by the System</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Setup a new ModelContext
            ModelContext.Current = new ModelContext();
        }


        /// <summary>
        /// Application_EndRequest is called when there is a Request 
        /// made of the Application that is ending.
        /// </summary>
        /// <param name="sender">The Application that has a new Request</param>
        /// <param name="e">The EventArgs sent by the System</param>
        protected void Application_EndRequest()
        {
            //Dispose of the ModelContext
            ModelContext context = ModelContext.Current;
            context.Dispose();
        }
    }
}