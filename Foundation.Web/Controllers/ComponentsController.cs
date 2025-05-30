using Foundation.Components.Controllers;
using Foundation.Components.Models.FormBuilder;
using Foundation.Web.Models;
using Foundation.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

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


        [HttpGet("TestFormBuilder")]
        public IActionResult ExampleFormBuilder()
        {
            var test = Url.Action("SubmitFormBuilder", "Components");
            var form = new FormDefinition
            {
                Id = "demo-form",
                Title = "Demo Form",
                Action = Url.Action("SubmitFormBuilder", "Components"),
                Methode = "post",
                SubmithButtonText = "submit",
                Sections = new List<FormSection>
                {
                    new FormSection
                    {
                        Title = "All Question Types",
                        Questions = new List<FormQuestion>
                        {
                            new FormQuestion
                            {
                                Id = "text",
                                Label = "Text",
                                Type = QuestionType.Text,
                                IsRequired = true
                            },
                            new FormQuestion
                            {
                                Id = "email",
                                Label = "Email",
                                Type = QuestionType.Email,
                                IsRequired = true
                            },
                            new FormQuestion
                            {
                                Id = "password",
                                Label = "Password",
                                Type = QuestionType.Password,
                                IsRequired = true
                            },
                            new FormQuestion
                            {
                                Id = "url",
                                Label = "Website URL",
                                Type = QuestionType.Url,
                                IsRequired = false
                            },
                            new FormQuestion
                            {
                                Id = "textarea",
                                Label = "Biography",
                                Type = QuestionType.TextArea,
                                IsRequired = false
                            },
                            new FormQuestion
                            {
                                Id = "number",
                                Label = "Age",
                                Type = QuestionType.Number,
                                IsRequired = false
                            },
                            new FormQuestion
                            {
                                Id = "date",
                                Label = "Date of Birth",
                                Type = QuestionType.Date,
                                IsRequired = false
                            },
                            new FormQuestion
                            {
                                Id = "radio",
                                Label = "Favorite Color",
                                Type = QuestionType.Radio,
                                IsRequired = true,
                                Options = new List<QuestionOption>
                                {
                                    new QuestionOption { Id = "red", Value = "red", Label = "Red" },
                                    new QuestionOption { Id = "blue", Value = "blue", Label = "Blue" },
                                    new QuestionOption { Id = "green", Value = "green", Label = "Green" }
                                }
                            },
                            new FormQuestion
                            {
                                Id = "checkbox",
                                Label = "Select Hobbies",
                                Type = QuestionType.Checkbox,
                                Options = new List<QuestionOption>
                                {
                                    new QuestionOption { Id = "reading", Value = "reading", Label = "Reading" },
                                    new QuestionOption { Id = "sports", Value = "sports", Label = "Sports" },
                                    new QuestionOption { Id = "music", Value = "music", Label = "Music" }
                                }
                            },
                            new FormQuestion
                            {
                                Id = "dropdown",
                                Label = "Country",
                                Type = QuestionType.Dropdown,
                                Options = new List<QuestionOption>
                                {
                                    new QuestionOption { Id = "canada", Value = "canada", Label = "Canada" },
                                    new QuestionOption { Id = "usa", Value = "usa", Label = "USA" },
                                    new QuestionOption { Id = "mexico", Value = "mexico", Label = "Mexico" }
                                },
                                Dependencies = new List<QuestionDependency>
                                {
                                    new QuestionDependency { Action = DependencyAction.Require, TriggerValue = "canada", TargetQuestionId = "town"}
                                }
                            },
                            new FormQuestion
                            {
                                Id = "town",
                                Label = "Canadien Town",
                                Type = QuestionType.Text,
                                IsRequired = false
                            },
                            new FormQuestion
                            {
                                Id = "fileupload",
                                Label = "Upload Resume",
                                Type = QuestionType.FileUpload,
                                IsRequired = false
                            }
                        }
                    }
                }
            };

            ViewBag.Form = form;
            return View("FormBuilder");
        }


        [HttpPost("SubmitFormBuilder")]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitFormBuilder(IFormCollection form)
        {

            //var formDefinition = _formService.GetFormDefinition("form-id"); // Already localized
            //var errors = new List<FormError>();

            //foreach (var section in formDefinition.Sections)
            //{
            //    foreach (var question in section.Questions)
            //    {
            //        var value = form[question.Id];

            //        if (question.IsRequired && string.IsNullOrWhiteSpace(value))
            //        {
            //            errors.Add(new FormError
            //            {
            //                QuestionId = question.Id,
            //                Message = new LocalizedString
            //                {
            //                    En = "This field is required.",
            //                    Fr = "Ce champ est requis."
            //                }
            //            });
            //        }

            //        // Add dependency validation here if needed
            //    }
            //}

            //if (errors.Any())
            //{
            //    ViewBag.Errors = errors;
            //    ViewBag.Form = formDefinition;
            //    return View("FormView");
            //}

            // Process form data
            return RedirectToAction("Success");

        }
    }
}
