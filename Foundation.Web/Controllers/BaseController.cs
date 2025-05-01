using Foundation.Components.Models;
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
        /// You can set a notification at the top of the page
        /// </summary>
        /// <param name="notification">Object containing information about the notification</param>
        protected void SetPageNotification(PageNotification notification)
        {
            ViewData["PageNotification"] = notification;
        }

        /// <summary>
        /// You can set success a page notification at the top of the page
        /// </summary>
        /// <param name="title">Title of the notification</param>
        /// <param name="message">Content of the notification</param>
        protected void SetPageSuccessNotification(string title, string message)
        {
            ViewData["PageNotification"] = new PageNotification
            {
                Title       = title,
                Message     = message,
                AlertType   = Components.Enum.AlertTypeEnum.Success
            };
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
