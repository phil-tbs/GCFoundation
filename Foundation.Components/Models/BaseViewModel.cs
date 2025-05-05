using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Models
{
    public class BaseViewModel
    {
        public string SuccessMessage { get; set; } = string.Empty;

        public Dictionary<string, List<string>> Errors { get; private set; } = new();

        public Dictionary<string, string> Metadata { get; } = new();

        /// <summary>
        /// Checks Data Annotation validation and collects error messages.
        /// </summary>
        public void Validate()
        {
            ClearErrors();

            var validationContext = new ValidationContext(this);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(this, validationContext, validationResults, true))
            {
                foreach (var validationResult in validationResults)
                {
                    foreach (var member in validationResult.MemberNames)
                    {
                        AddError(member, validationResult.ErrorMessage ?? "Invalid value.");
                    }
                }
            }
        }

        /// <summary>
        /// Adds an error message for a specific field.
        /// </summary>
        public void AddError(string field, string errorMessage)
        {
            if (!Errors.TryGetValue(field, out var errorList))
            {
                errorList = new List<string>();
                Errors[field] = errorList;
            }
            errorList.Add(errorMessage);
        }

        /// <summary>
        /// Checks if the model has any validation errors.
        /// </summary>
        public bool IsValid => Errors.Count == 0;

        /// <summary>
        /// Clears all error messages.
        /// </summary>
        public void ClearErrors()
        {
            Errors.Clear();
        }
    }
}
