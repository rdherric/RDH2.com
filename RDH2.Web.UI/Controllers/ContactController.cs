using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RDH2.Web.UI.Infrastructure;
using RDH2.Web.UI.Models;

namespace RDH2.Web.UI.Controllers
{
    /// <summary>
    /// ContactController handles the RDH2 Contact page.
    /// </summary>
    public class ContactController : Controller
    {
        #region Member Variables
        private EmailSender _emailSender = null;
        #endregion


        #region Constructor
        /// <summary>
        /// Default constructor for the ContactController class.
        /// </summary>
        public ContactController()
        {
            //Create a new EmailSender
            this._emailSender = new EmailSender();
        }
        #endregion


        #region MVC View Methods
        /// <summary>
        /// Index shows the Contact page.
        /// </summary>
        /// <returns>ActionResult of Index data</returns>
        public ActionResult Index()
        {
            //Return the result 
            return this.View(new ContactFormModel());
        }
        #endregion


        #region MVC Post Methods
        /// <summary>
        /// SendMail sends the user-specified information to RDH2.
        /// </summary>
        /// <param name="formModel">The Model to send the email</param>
        /// <returns>JsonResult of success data</returns>
        [ValidateInput(false)]
        public JsonResult SendMail(ContactFormModel formModel)
        {
            //Declare a variable to return -- default to an 
            //invalid data object
            JsonResult rtn = null;

            //If the Model is valid, email off the contact form
            if (this.ModelState.IsValid == true)
            {
                //Send off the email with the EmailSender
                this._emailSender.SendMail(
                    formModel.ToEmailAddress, formModel.FromEmailAddress,
                    "RDH2.COM -- " + formModel.Subject, HttpUtility.HtmlDecode(formModel.Body));

                //Set the JSON return to show success
                rtn = this.Json(new { success = true, home = Url.Action("Index", "Home") });
            }

            //Return the result
            return rtn;
        }
        #endregion
    }
}
