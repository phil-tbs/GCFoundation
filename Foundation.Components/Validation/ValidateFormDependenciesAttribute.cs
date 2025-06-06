using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Foundation.Components.Models.FormBuilder;

namespace Foundation.Components.Validation
{
    /// <summary>
    /// Validation attribute to validate form dependencies
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class ValidateFormDependenciesAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
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
            var validationResults = validator.Validate();

            // If there are any validation errors, combine them into a single result
            if (validationResults.Any())
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