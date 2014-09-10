using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDH2.Web.UI.Infrastructure
{
    /// <summary>
    /// StringExtensions contains methods for string 
    /// manipulation.
    /// </summary>
    public static class StringExtensions
    {
        public static String Reduce(this String input)
        {
            //Declare a StringBuilder to hold the result
            StringBuilder sb = new StringBuilder();

            //Iterate through the characters and setup the 
            //reduced String
            foreach (Char c in input)
            {
                //If the Char is a space, set the value to a hyphen.
                //Otherwise, check for letter or digit.
                if (c == ' ')
                {
                    sb.Append('-');
                }
                else if (Char.IsLetterOrDigit(c) == true)
                {
                    sb.Append(c.ToString().ToLower());
                }
            }

            //Return the result
            return sb.ToString();
        }
    }
}