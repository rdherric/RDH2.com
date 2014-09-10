using System;
using System.Net.Mail;

namespace RDH2.Web.UI.Infrastructure
{
    /// <summary>
    /// EmailSender sends off an email using the RDH2.COM
    /// email server.
    /// </summary>
    public class EmailSender
    {
        #region Member Variables
        private const String _rdh2Address = "rdherric@rdh2.com";
        #endregion


        /// <summary>
        /// SendMail sends off an email to the specified Email address.
        /// </summary>
        /// <param name="toAddress">The Address to which to send the Email</param>
        /// <param name="fromAddress">The Address from which to send the Email</param>
        /// <param name="subject">The Subject of the Email</param>
        /// <param name="body">The Body of the Email</param>
        /// <returns>Boolean true if successful, false otherwise</returns>
        public Boolean SendMail(String toAddress, String fromAddress, String subject, String body)
        {
            //Create the new MailMessage
            MailMessage msg = new MailMessage(fromAddress, EmailSender._rdh2Address)
            { 
                Body = body, 
                IsBodyHtml = true, 
                Subject = subject 
            };

            //Create the new SmtpClient to send the mail
            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    client.Send(msg);
                }
            }
            catch { }

            return true;
        }
    }
}