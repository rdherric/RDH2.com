using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RDH2.Web.DataModel.Model;
using RDH2.Web.UI.Infrastructure;

namespace RDH2.Web.UI.Controllers
{
    /// <summary>
    /// AccountController is the Controller between the User
    /// and logging in.
    /// </summary>
    public class AccountController : Controller
    {
        #region Member Variables
        private readonly Repository<Account> _accounts = null;
        #endregion


        #region Constructor
        /// <summary>
        /// Default constructor for the AccountController object.
        /// </summary>
        public AccountController()
        {
            //Save the input in the member variables
            this._accounts = new Repository<Account>();
        }
        #endregion


        #region MVC View Actions
        /// <summary>
        /// GetLogInHtml gets the Login Partial View
        /// for showing to the User.
        /// </summary>
        /// <returns>ActionResult with Login HTML</returns>
        public ActionResult GetLogInHtml()
        {
            return this.PartialView("LogInPartial");
        }
        #endregion


        #region MVC Post Actions
        /// <summary>
        /// LogIn does the work of trying to log into the 
        /// RDH2.COM application.
        /// </summary>
        /// <param name="userName">The user name supplied by the User</param>
        /// <param name="password">The password supplied by the User</param>
        /// <returns>JsonResult with success data</returns>
        public JsonResult LogIn(String userName, String password)
        {
            //Declare a variable to return
            JsonResult rtn = null;

            //Try to get the User from the Repository
            Account account = this._accounts
                .GetBy(a => a.Login == userName && a.Password == password)
                .FirstOrDefault();

            //If the Account was found, save the value in Session and 
            //return success to the User
            if (account != null)
            {
                //Set the value in Session
                AccountModelBinder.SetAccount(account);

                //Create a JSON result for the caller
                rtn = this.Json(new { success = true, url = this.Request.Url.ToString() });
            }

            //Return the result
            return rtn;
        }


        /// <summary>
        /// LogOut logs the User out of the Application.
        /// </summary>
        /// <param name="returnUrl">The URL to which to return</param>
        /// <returns>ActionResult of View data</returns>
        public ActionResult LogOut(String returnUrl)
        {
            //Set the Account to null to sign out
            AccountModelBinder.SetAccount(null);

            //Refresh the Page by redirecting
            return this.Redirect(returnUrl);
        }
        #endregion
    }
}
