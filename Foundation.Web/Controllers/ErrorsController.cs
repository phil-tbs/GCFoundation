using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    /// <summary>
    /// Controller responsible for handling error pages.
    /// </summary>
    public class ErrorsController : Controller
    {
        /// <summary>
        /// Displays the custom 404 Not Found error page.
        /// </summary>
        /// <returns>The NotFound view.</returns>
        [Route("Error/NotFound")]
        public IActionResult NotFoundError()
        {
            return View("NotFound");
        }

        /// <summary>
        /// Displays a global error page for unhandled exceptions.
        /// </summary>
        /// <returns>The GlobalError view.</returns>
        [Route("Error/Global")]
        public IActionResult GlobalError()
        {
            return View("GlobalError");
        }
    }
}
