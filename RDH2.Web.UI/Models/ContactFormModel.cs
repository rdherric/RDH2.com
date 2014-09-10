using System;
using System.Collections.Generic;
using System.Linq;

namespace RDH2.Web.UI.Models
{
    public class ContactFormModel
    {
        /// <summary>
        /// The Email Address to which to send the email.
        /// </summary>
        public String ToEmailAddress { get; set; }


        /// <summary>
        /// The Email Address from which to send the email.
        /// </summary>
        public String FromEmailAddress { get; set; }


        /// <summary>
        /// The Subject of the email.
        /// </summary>
        public String Subject { get; set; }


        /// <summary>
        /// The Body of the email.
        /// </summary>
        public String Body { get; set; }
    }
}