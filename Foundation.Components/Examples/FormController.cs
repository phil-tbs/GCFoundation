using Foundation.Components.Models.FormBuilder;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Components.Examples
{
    public class FormController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Submit([FromForm] FormViewModel viewModel)
        {
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
            await ProcessFormData(viewModel);

            // Redirect to success page
            return RedirectToAction("Success");
        }

        private async Task ProcessFormData(FormViewModel viewModel)
        {
            // Your form processing logic here
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}