using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.UI.Infrastructure
{
    public class AccountModelBinder : IModelBinder
    {
        #region Constants
        private const String _loginKey = "_RDH2_login";
        #endregion


        #region IModelBinder Implementation
        /// <summary>
        /// BindModel binds the Account object to the model
        /// Context that is passed in.
        /// </summary>
        /// <param name="controllerContext">The Context in which the Controller is running</param>
        /// <param name="bindingContext">The Context of the model binding</param>
        /// <returns>Account object if the user is logged in, null otherwise</returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //Return the result from Session
            return AccountModelBinder.Current;
        }
        #endregion


        #region Static Methods to get or set Session Variable
        /// <summary>
        /// SetAccount sets the Account input in the Session 
        /// variable so that it can be used elsewhere.
        /// </summary>
        /// <param name="account">The Account to set in the Session</param>
        public static void SetAccount(Account account)
        {
            //If there is a valid HttpContext, use it
            if (HttpContext.Current != null)
            {
                //Set a cookie that says that the user logged in if the
                //Account is valid.  Otherwise, clear the cookie.
                if (account != null)
                {
                    //Set the Account in the Session if possible
                    HttpContext.Current.Session[AccountModelBinder._loginKey] = account.Login;

                    //Setup Forms Authentication
                    FormsAuthentication.SetAuthCookie(account.Login, false);
                }
                else
                {
                    //Sign out of Forms Authentication
                    FormsAuthentication.SignOut();
                }
            }
        }


        /// <summary>
        /// Current gets the current Account object from the Session.
        /// </summary>
        /// <returns>Account object if found, null otherwise</returns>
        public static Account Current
        {
            get
            {
                //Declare a variable to return
                Account rtn = null;

                //If the Session is available, get the Account out of it
                if (HttpContext.Current != null)
                {
                    //Get a repository of Accounts
                    Repository<Account> accounts = new Repository<Account>();

                    //Get the string Login from Session
                    String login = (HttpContext.Current.Session[AccountModelBinder._loginKey] != null ?
                        HttpContext.Current.Session[AccountModelBinder._loginKey].ToString() : 
                        String.Empty);

                    //Get the Account based on the Login in the Session
                    rtn = accounts
                        .GetBy(x => x.Login == login)
                        .FirstOrDefault();
                }

                //Return the result
                return rtn;
            }
        }
        #endregion
    }
}