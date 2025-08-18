using GCFoundation.Components.Controllers;
using GCFoundation.Components.Models.FormBuilder;
using GCFoundation.Web.Models;
using GCFoundation.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GCFoundation.Web.Controllers
{
    /// <summary>
    /// Controller that handles requests related to reusable UI components.
    /// </summary>
    [Route("components")]
    public class ComponentsController : GCFoundationBaseController
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
        /// Handles the POST request to test form validation.
        /// </summary>
        /// <param name="model">The form data submitted by the user.</param>
        /// <returns>
        /// The same view with the model and validation results.
        /// </returns>
        [HttpPost("testingForm")]
        [ValidateAntiForgeryToken]
        public IActionResult FormValidationTest(FormTestViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Add your logic here if needed when the model is valid
            }

            return View("Forms", model);
        }
        

        /// <summary>
        /// Displays the global resources configuration demo page.
        /// </summary>
        /// <returns>
        /// The global resources configuration view.
        /// </returns>
        [HttpGet("global-resources")]
        public IActionResult GlobalResources()
        {
            SetPageTitle("Global Resources Configuration");
            return View();
        }

        /// <summary>
        /// Displays the Badge component demo page.
        /// </summary>
        /// <returns>
        /// The Badge component view.
        /// </returns>
        [HttpGet("badge")]
        public IActionResult Badge()
        {
            SetPageTitle($"{Menu.Menu_Components}: Badge");
            return View();
        }

        /// <summary>
        /// Displays the Card component demo page.
        /// </summary>
        /// <returns>
        /// The Card component view.
        /// </returns>
        [HttpGet("card")]
        public IActionResult Card()
        {
            SetPageTitle($"{Menu.Menu_Components}: Card");
            return View();
        }

        /// <summary>
        /// Displays the Modal component demo page.
        /// </summary>
        /// <returns>
        /// The Modal component view.
        /// </returns>
        [HttpGet("modal")]
        public IActionResult Modal()
        {
            SetPageTitle($"{Menu.Menu_Components}: Modal");
            return View();
        }

        /// <summary>
        /// Displays the Stepper component demo page.
        /// </summary>
        /// <returns>
        /// The Stepper component view.
        /// </returns>
        [HttpGet("stepper")]
        public IActionResult Stepper()
        {
            SetPageTitle($"{Menu.Menu_Components}: Stepper");
            return View();
        }

        /// <summary>
        /// Displays the PageHeading component demo page.
        /// </summary>
        /// <returns>
        /// The PageHeading component view.
        /// </returns>
        [HttpGet("page-heading")]
        public IActionResult PageHeading()
        {
            SetPageTitle($"{Menu.Menu_Components}: Page Heading");
            return View();
        }

        /// <summary>
        /// Displays the Table component demo page.
        /// </summary>
        /// <returns>
        /// The Table component view.
        /// </returns>
        [HttpGet("table")]
        public IActionResult Table()
        {
            SetPageTitle($"{Menu.Menu_Components}: Table");
            return View();
        }

        /// <summary>
        /// Displays the User Login Partial component demo page.
        /// </summary>
        /// <returns>
        /// The User Login Partial component view.
        /// </returns>
        [HttpGet("user-login")]
        public IActionResult UserLogin()
        {
            SetPageTitle($"{Menu.Menu_Components}: User Login Partial");
            ViewData["LoginPartialViewName"] = "_ExampleUserLogin";
            return View();
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
                    new FormSection
                    {
                        Title = "Personal Information",
                        Hint = "Please provide your basic information",
                        Questions = new List<FormQuestion>
                        {
                            new FormQuestion
                            {
                                Id = "fullName",
                                Label = "Full Name",
                                Type = QuestionType.Text,
                                IsRequired = true,
                                Hint = "Enter your full legal name"
                            },
                            new FormQuestion
                            {
                                Id = "email",
                                Label = "Email Address",
                                Type = QuestionType.Email,
                                IsRequired = true,
                                Hint = "We'll use this for communication"
                            }
                        }
                    },
                    new FormSection
                    {
                        Title = "Location Information",
                        Hint = "Tell us where you're located",
                        Questions = new List<FormQuestion>
                        {
                            // Country selection with cascading dependencies
                            new FormQuestion
                            {
                                Id = "country",
                                Label = "Country of Residence",
                                Type = QuestionType.Dropdown,
                                IsRequired = true,
                                Options = new List<QuestionOption>
                                {
                                    new() { Id = "ca", Value = "CA", Label = "Canada" },
                                    new() { Id = "us", Value = "US", Label = "United States" },
                                    new() { Id = "other", Value = "OTHER", Label = "Other" }
                                }
                            },
                            // Province field - shows when Canada is selected
                            new FormQuestion
                            {
                                Id = "province",
                                Label = "Province",
                                Type = QuestionType.Dropdown,
                                Dependencies = new List<QuestionDependency>
                                {
                                    new()
                                    {
                                        SourceQuestionId = "country",
                                        TargetQuestionId = "province",
                                        Action = DependencyAction.Show,
                                        TriggerValue = "CA"
                                    },
                                    new()
                                    {
                                        SourceQuestionId = "country",
                                        TargetQuestionId = "province",
                                        Action = DependencyAction.Require,
                                        TriggerValue = "CA"
                                    }
                                },
                                Options = new List<QuestionOption>
                                {
                                    new() { Id = "on", Value = "ON", Label = "Ontario" },
                                    new() { Id = "qc", Value = "QC", Label = "Quebec" },
                                    new() { Id = "bc", Value = "BC", Label = "British Columbia" }
                                }
                            },
                            // State field - shows when US is selected
                            new FormQuestion
                            {
                                Id = "state",
                                Label = "State",
                                Type = QuestionType.Dropdown,
                                Dependencies = new List<QuestionDependency>
                                {
                                    new()
                                    {
                                        SourceQuestionId = "country",
                                        TargetQuestionId = "state",
                                        Action = DependencyAction.Show,
                                        TriggerValue = "US"
                                    },
                                    new()
                                    {
                                        SourceQuestionId = "country",
                                        TargetQuestionId = "state",
                                        Action = DependencyAction.Require,
                                        TriggerValue = "US"
                                    }
                                },
                                Options = new List<QuestionOption>
                                {
                                    new() { Id = "ny", Value = "NY", Label = "New York" },
                                    new() { Id = "ca", Value = "CA", Label = "California" },
                                    new() { Id = "tx", Value = "TX", Label = "Texas" }
                                }
                            },
                            // Other Country field - shows when Other is selected
                            new FormQuestion
                            {
                                Id = "otherCountry",
                                Label = "Specify Country",
                                Type = QuestionType.Text,
                                Dependencies = new List<QuestionDependency>
                                {
                                    new()
                                    {
                                        SourceQuestionId = "country",
                                        TargetQuestionId = "otherCountry",
                                        Action = DependencyAction.Show,
                                        TriggerValue = "OTHER"
                                    },
                                    new()
                                    {
                                        SourceQuestionId = "country",
                                        TargetQuestionId = "otherCountry",
                                        Action = DependencyAction.Require,
                                        TriggerValue = "OTHER"
                                    }
                                }
                            }
                        }
                    },
                    new FormSection
                    {
                        Title = "Service Selection",
                        Hint = "Choose your service preferences",
                        Questions = new List<FormQuestion>
                        {
                            // Service Type with multiple dependent fields
                            new FormQuestion
                            {
                                Id = "serviceType",
                                Label = "Service Type",
                                Type = QuestionType.Radio,
                                IsRequired = true,
                                Options = new List<QuestionOption>
                                {
                                    new() { Id = "basic", Value = "BASIC", Label = "Basic Service" },
                                    new() { Id = "premium", Value = "PREMIUM", Label = "Premium Service" },
                                    new() { Id = "custom", Value = "CUSTOM", Label = "Custom Service" }
                                }
                            },
                            // Premium features - shown and required for premium service
                            new FormQuestion
                            {
                                Id = "premiumFeatures",
                                Label = "Premium Features",
                                Type = QuestionType.Checkbox,
                                Hint = "Select the premium features you want",
                                Dependencies = new List<QuestionDependency>
                                {
                                    new()
                                    {
                                        SourceQuestionId = "serviceType",
                                        TargetQuestionId = "premiumFeatures",
                                        Action = DependencyAction.Show,
                                        TriggerValue = "PREMIUM"
                                    },
                                    new()
                                    {
                                        SourceQuestionId = "serviceType",
                                        TargetQuestionId = "premiumFeatures",
                                        Action = DependencyAction.Require,
                                        TriggerValue = "PREMIUM"
                                    }
                                },
                                Options = new List<QuestionOption>
                                {
                                    new() { Id = "feature1", Value = "24_7_SUPPORT", Label = "24/7 Support" },
                                    new() { Id = "feature2", Value = "PRIORITY", Label = "Priority Service" },
                                    new() { Id = "feature3", Value = "ADVANCED", Label = "Advanced Features" }
                                }
                            },
                            // Custom requirements - shown and enabled for custom service
                            new FormQuestion
                            {
                                Id = "customRequirements",
                                Label = "Custom Requirements",
                                Type = QuestionType.TextArea,
                                Hint = "Describe your custom service needs",
                                Dependencies = new List<QuestionDependency>
                                {
                                    new()
                                    {
                                        SourceQuestionId = "serviceType",
                                        TargetQuestionId = "customRequirements",
                                        Action = DependencyAction.Show,
                                        TriggerValue = "CUSTOM"
                                    },
                                    new()
                                    {
                                        SourceQuestionId = "serviceType",
                                        TargetQuestionId = "customRequirements",
                                        Action = DependencyAction.Require,
                                        TriggerValue = "CUSTOM"
                                    }
                                }
                            },
                            // Budget range - disabled for basic service
                            new FormQuestion
                            {
                                Id = "budgetRange",
                                Label = "Budget Range",
                                Type = QuestionType.Dropdown,
                                Dependencies = new List<QuestionDependency>
                                {
                                    new()
                                    {
                                        SourceQuestionId = "serviceType",
                                        TargetQuestionId = "budgetRange",
                                        Action = DependencyAction.Disable,
                                        TriggerValue = "BASIC"
                                    }
                                },
                                Options = new List<QuestionOption>
                                {
                                    new() { Id = "budget1", Value = "UNDER_1000", Label = "Under $1,000" },
                                    new() { Id = "budget2", Value = "1000_5000", Label = "$1,000 - $5,000" },
                                    new() { Id = "budget3", Value = "OVER_5000", Label = "Over $5,000" }
                                }
                            }
                        }
                    },
                    new FormSection
                    {
                        Title = "Additional Information",
                        Questions = new List<FormQuestion>
                        {
                            // Contact preference with dependent phone field
                            new FormQuestion
                            {
                                Id = "contactPreference",
                                Label = "Preferred Contact Method",
                                Type = QuestionType.Radio,
                                IsRequired = true,
                                Options = new List<QuestionOption>
                                {
                                    new() { Id = "email", Value = "EMAIL", Label = "Email" },
                                    new() { Id = "phone", Value = "PHONE", Label = "Phone" }
                                }
                            },
                            // Phone number - required when phone is selected
                            new FormQuestion
                            {
                                Id = "phoneNumber",
                                Label = "Phone Number",
                                Type = QuestionType.Text,
                                Dependencies = new List<QuestionDependency>
                                {
                                    new()
                                    {
                                        SourceQuestionId = "contactPreference",
                                        TargetQuestionId = "phoneNumber",
                                        Action = DependencyAction.Show,
                                        TriggerValue = "PHONE"
                                    },
                                    new()
                                    {
                                        SourceQuestionId = "contactPreference",
                                        TargetQuestionId = "phoneNumber",
                                        Action = DependencyAction.Require,
                                        TriggerValue = "PHONE"
                                    }
                                }
                            },
                            // Terms acceptance
                            new FormQuestion
                            {
                                Id = "termsAccepted",
                                Label = "Terms and Conditions",
                                Type = QuestionType.Checkbox,
                                IsRequired = true,
                                Options = new List<QuestionOption>
                                {
                                    new() { Id = "terms", Value = "true", Label = "I accept the terms and conditions" }
                                }
                            }
                        }
                    }
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
