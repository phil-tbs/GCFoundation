using GCFoundation.Components.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GCFoundation.Web.Controllers
{
    /// <summary>
    /// Controller responsible for handling installation-related pages.
    /// </summary>
    [Route("installtion")]
    public class InstallationController : FoundationBaseController
    {
        private readonly ILogger<InstallationController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallationController"/> class.
        /// </summary>
        /// <param name="logger">The logger used for logging actions and events in this controller.</param>
        public InstallationController(ILogger<InstallationController> logger)
            : base(logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Displays the main installation page.
        /// </summary>
        /// <returns>The default view for the installation page.</returns>
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
