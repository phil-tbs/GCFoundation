using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    public class BaseController : Controller
    {

        public BaseController() 
        {
            
        }


        protected void SetViewMenu(string viewMenu)
        {
            ViewData["MenuPartialViewName"] = viewMenu;
        }

        /// <summary>
        /// Will set the page title in the html head
        /// </summary>
        /// <param name="title">Title of the page</param>
        protected void SetPageTitle(string title)
        {
            ViewData["Title"] = title;
        }
    }
}
