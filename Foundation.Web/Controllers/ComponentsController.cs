using Foundation.Web.Models;
using Foundation.Web.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    /// <summary>
    /// Controller that handles requests related to reusable UI components.
    /// </summary>
    [Route("components")]
    public class ComponentsController : BaseController
    {

        private readonly ILogger<ComponentsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentsController"/> class.
        /// </summary>
        /// <param name="logger">The logger used for logging actions and events in this controller.</param>

        public ComponentsController(ILogger<ComponentsController> logger)
            : base(logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Displays the main components overview page.
        /// </summary>
        /// <returns>The components index view.</returns>
        [HttpGet("")]
        public IActionResult Index()
        {
            SetPageTitle(Menu.Menu_Components);
            return View();
        }

        /// <summary>
        /// Displays a specific component view based on the component name.
        /// </summary>
        /// <param name="componentName">The name of the component to load.</param>
        /// <returns>The view for the specified component.</returns>
        [HttpGet("component")]
        public IActionResult GetComponent(string componentName)
        {
            SetPageTitle($"{Menu.Menu_Components}: {componentName}");
            return View(componentName);
        }

        /// <summary>
        /// Displays the GC Design System components page.
        /// </summary>
        /// <returns>The GC Design System view.</returns>
        [HttpGet("gcds")]
        public IActionResult Gcds()
        {
            SetPageTitle(Menu.Menu_Components_GCDesign);
            return View();
        }

        /// <summary>
        /// Displays the form used to test validation behavior.
        /// </summary>
        /// <returns>The form validation test view with an empty model.</returns>
        [HttpGet("testingForm")]
        public IActionResult FormValidationTest()
        {
            SetPageTitle("Form");
            FormTestViewModel model = new();

            return View("Forms", model);
        }

        /// <summary>
        /// Handles the POST request to test form validation.
        /// </summary>
        /// <param name="model">The form data submitted by the user.</param>
        /// <returns>The same view with the model and validation results.</returns>
        [HttpPost]
        public IActionResult FormValidationTest(FormTestViewModel model)
        {

            if (ModelState.IsValid)
            {

            }

            return View("Forms", model);
        }
    }
}
