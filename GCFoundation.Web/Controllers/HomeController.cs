using GCFoundation.Components.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GCFoundation.Web.Controllers
{
    /// <summary>
    /// Controller for handling the home page and related routes.
    /// </summary>
    [Route("home")]
    public class HomeController : FoundationBaseController
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger for logging actions in this controller.</param>
        public HomeController(ILogger<HomeController> logger)
            : base(logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Displays the home page.
        /// </summary>
        /// <returns>The default view for the home page.</returns>
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
