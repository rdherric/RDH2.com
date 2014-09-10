using System.Web.Mvc;

namespace RDH2.Web.UI
{
    public class FilterConfig
    {
        /// <summary>
        /// RegisterGlobalFilters does the job of registering the 
        /// global filters with the Collection.
        /// </summary>
        /// <param name="filters">The Collection of GlobalFilters</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //Register the filters
            filters.Add(new HandleErrorAttribute());
        }
    }
}