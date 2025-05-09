using Foundation.Components.Enums;
using Foundation.Components.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Foundation.Components.Controllers
{
    /// <summary>
    /// Provides base functionality for controllers within the application.
    /// This class includes common methods for setting page notifications, menu views, and page titles.
    /// </summary>
    public abstract class FoundationBaseController : Controller
    {

        private readonly ILogger<FoundationBaseController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoundationBaseController"/> class.
        /// </summary>
        protected FoundationBaseController(ILogger<FoundationBaseController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Sets the name of the partial view to be used for the page menu.
        /// </summary>
        /// <param name="viewMenu">The name of the partial view for the menu.</param>
        protected void SetViewMenu(string viewMenu)
        {
            ViewData["MenuPartialViewName"] = viewMenu;
        }

        /// <summary>
        /// Sets a page-level notification to be displayed at the top of the page.
        /// </summary>
        /// <param name="notification">An object containing the notification title, message, and alert type.</param>
        protected void SetPageNotification(PageNotification notification)
        {
            ViewData["PageNotification"] = notification;
        }

        /// <summary>
        /// Sets a success-type notification to be displayed at the top of the page.
        /// </summary>
        /// <param name="title">The title of the success message.</param>
        /// <param name="message">The message content of the success notification.</param>
        protected void SetPageSuccessNotification(string title, string message)
        {
            ViewData["PageNotification"] = new PageNotification
            {
                Title = title,
                Message = message,
                AlertType = AlertType.Success
            };
        }

        /// <summary>
        /// Sets the HTML page title to appear in the browser's title bar or tab.
        /// </summary>
        /// <param name="title">The title of the page.</param>
        protected void SetPageTitle(string title)
        {
            ViewData["Title"] = title;
        }
    }
}
