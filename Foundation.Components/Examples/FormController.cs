using System.Threading.Tasks;
using Foundation.Components.Models.FormBuilder;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Components.Examples
{
    /// <summary>
    /// Controller for handling form submissions and related actions.
    /// </summary>
    public class FormController : Controller
    {
        /// <summary>
        /// Handles the submission of a form.
        /// Validates the form data and processes it if valid.
        /// </summary>
        /// <param name="viewModel">The view model containing form definition and user input.</param>
        /// <returns>
        /// A redirect to the success page if the form is valid; otherwise, returns the form view with validation errors.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Submit([FromForm] FormViewModel viewModel)
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
                return View("Form", viewModel);
            }

            // Process the valid form data
            await ProcessFormData(viewModel).ConfigureAwait(false);

            // Redirect to success page
            return RedirectToAction("Success");
        }

        /// <summary>
        /// Processes the form data after successful validation.
        /// </summary>
        /// <param name="viewModel">The view model containing form data to process.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private static async Task ProcessFormData(FormViewModel viewModel)
        {
            // Your form processing logic here
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
} 