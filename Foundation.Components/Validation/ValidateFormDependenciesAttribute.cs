using System.ComponentModel.DataAnnotations;
using Foundation.Components.Models.FormBuilder;

namespace Foundation.Components.Validation
{
    /// <summary>
    /// Validation attribute to validate form dependencies.
    /// Ensures that the dependencies between form questions are satisfied based on the provided form data.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class ValidateFormDependenciesAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validates the form dependencies for the specified value and validation context.
        /// </summary>
        /// <param name="value">The value of the object to validate. Expected to be a <see cref="FormDefinition"/>.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// A <see cref="ValidationResult"/> indicating whether validation succeeded or failed.
        /// If validation fails, returns a <see cref="ValidationResult"/> with error messages and member names.
        /// </returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ArgumentNullException.ThrowIfNull(validationContext);

            if (value is not FormDefinition form)
            {
                return new ValidationResult("This attribute can only be used on FormDefinition properties.");
            }

            // Get the form data from the validation context
            var formData = validationContext.Items["FormData"] as Dictionary<string, object?>;
            if (formData == null)
            {
                return new ValidationResult("Form data not found in validation context.");
            }

            // Create validator and validate dependencies
            var validator = new FormDependencyValidator(form, formData);
            var validationResults = validator.Validate().ToList();

            // If there are any validation errors, combine them into a single result
            if (validationResults.Count > 0)
            {
                var errorMessages = validationResults
                    .Select(r => r.ErrorMessage)
                    .Where(m => m != null)
                    .ToList();

                return new ValidationResult(
                    string.Join(Environment.NewLine, errorMessages),
                    validationResults.SelectMany(r => r.MemberNames).Distinct()
                );
            }

            return ValidationResult.Success;
        }
    }
}