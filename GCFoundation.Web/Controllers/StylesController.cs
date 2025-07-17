using GCFoundation.Components.Controllers;
using GCFoundation.Web.Resources;
using Microsoft.AspNetCore.Mvc;

namespace GCFoundation.Web.Controllers
{
    /// <summary>
    /// Controller that handles requests related to styling utilities and documentation.
    /// </summary>
    [Route("styles")]
    public class StylesController : GCFoundationBaseController
    {
        private readonly ILogger<StylesController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StylesController"/> class.
        /// </summary>
        /// <param name="logger">The logger used for logging actions and events in this controller.</param>
        public StylesController(ILogger<StylesController> logger)
            : base(logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Displays the main styles overview page with utilities documentation.
        /// </summary>
        /// <returns>
        /// The styles index view containing documentation for all available utility classes.
        /// </returns>
        [HttpGet("")]
        public IActionResult Index()
        {
            SetPageTitle(Menu.Menu_Styles);
            return View();
        }
    }
} 