using Foundation.Components.Controllers;
using Foundation.Components.Models.FormBuilder;
using Foundation.Web.Models;
using Foundation.Web.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    /// <summary>
    /// Controller that handles requests related to reusable UI components.
    /// </summary>
    [Route("components")]
    public class ComponentsController : FoundationBaseController
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
        /// <returns>
        /// The components index view.
        /// </returns>
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
        /// <returns>
        /// The view for the specified component.
        /// </returns>
        [HttpGet("component")]
        public IActionResult GetComponent(string componentName)
        {
            SetPageTitle($"{Menu.Menu_Components}: {componentName}");
            return View(componentName);
        }

        /// <summary>
        /// Displays the GC Design System components page.
        /// </summary>
        /// <returns>
        /// The GC Design System view.
        /// </returns>
        [HttpGet("gcds")]
        public IActionResult Gcds()
        {
            SetPageTitle(Menu.Menu_Components_GCDesign);
            return View();
        }

        /// <summary>
        /// Displays the form used to test validation behavior.
        /// </summary>
        /// <returns>
        /// The form validation test view with an empty model.
        /// </returns>
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
        /// <returns>
        /// The same view with the model and validation results.
        /// </returns>
        [HttpPost]
        public IActionResult FormValidationTest(FormTestViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Add your logic here if needed when the model is valid
            }

            return View("Forms", model);
        }

        /// <summary>
        /// Demonstrates a comprehensive example of a dynamic form with various question types and dependencies.
        /// This example showcases all possible dependency actions and their interactions.
        /// </summary>
        /// <returns>
        /// A view containing a form with various input types and complex dependencies.
        /// </returns>
        [HttpGet("TestFormBuilder")]
        public IActionResult ExampleFormBuilder()
        {
            var form = new FormDefinition
            {
                Id = "demo-form",
                Title = "Dynamic Form Demo",
                Action = Url.Action("SubmitFormBuilder", "Components") ?? "",
                Methode = "post",
                SubmithButtonText = "Submit Form",
                Sections = new List<FormSection>
                {
                    // ... (form sections omitted for brevity)
                }
            };

            var viewModel = new FormViewModel
            {
                Form = form,
            };

            return View("FormBuilder", viewModel);
        }

        /// <summary>
        /// Handles the submission of the dynamic form builder example.
        /// Validates the form data and processes it if valid.
        /// </summary>
        /// <param name="viewModel">The view model containing form definition and user input.</param>
        /// <returns>
        /// Redirects to the example form builder view with a success message if valid; otherwise, returns the form view with validation errors.
        /// </returns>
        [HttpPost("SubmitFormBuilder")]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitFormBuilder([FromForm] FormViewModel viewModel)
        {
            ArgumentNullException.ThrowIfNull(viewModel, nameof(viewModel));
            // Add the form data to the validation context
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(viewModel.Form)
            {
                Items = { ["FormData"] = viewModel.FormData }
            };

            // Validate the model including dependencies
            if (!TryValidateModel(viewModel, nameof(FormViewModel)))
            {
                // If validation fails, return to the form with error messages
                return View("FormBuilder", viewModel);
            }

            // Process the valid form data
            // TODO: Add your form processing logic here

            // Redirect to success page or show success message
            TempData["SuccessMessage"] = "Form submitted successfully!";
            return RedirectToAction("ExampleFormBuilder");
        }
    }
}
