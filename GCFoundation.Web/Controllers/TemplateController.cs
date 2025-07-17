using GCFoundation.Components.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GCFoundation.Web.Controllers
{
    /// <summary>
    /// Controller responsible for serving the template demonstration or sample view.
    /// </summary>
    [Route("template")]
    public class TemplateController : GCFoundationBaseController
    {
        private readonly ILogger<TemplateController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateController"/> class.
        /// </summary>
        /// <param name="logger">The logger used for logging actions and events in this controller.</param>
        public TemplateController(ILogger<TemplateController> logger)
            : base(logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Displays the default template view.
        /// </summary>
        /// <returns>The template view result.</returns>
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
