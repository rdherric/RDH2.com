using Kendo.Mvc.UI;

namespace RDH2.Web.UI.Controllers
{
    /// <summary>
    /// ImageBrowserController allows the access to the
    /// Image directory for upload of images.
    /// </summary>
    public class ImageBrowserController : EditorImageBrowserController
    {
        public override string ContentPath
        {
            get { return "/Content/Images/Upload"; }
        }
    }
}
