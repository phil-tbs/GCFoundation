using System.ComponentModel.DataAnnotations;

namespace GCFoundation.Components.Models
{
    /// <summary>
    /// Represents a base model class that contains properties for validation, error handling, and metadata.
    /// It provides functionality for performing data annotation validation and collecting error messages.
    /// </summary>
    public class BaseViewModel
    {
        /// <summary>
        /// Gets or sets the success message for the model.
        /// </summary>
        public string SuccessMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets the dictionary of validation error messages, keyed by the field name.
        /// </summary>
        public Dictionary<string, List<string>> Errors { get; private set; } = new();

        /// <summary>
        /// Gets the metadata associated with the model, stored as key-value pairs.
        /// </summary>
        public Dictionary<string, string> Metadata { get; } = new();

        /// <summary>
        /// Validates the model using data annotations and collects any error messages into the Errors dictionary.
        /// This method clears previous errors before performing validation.
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
        /// <param name="field">The name of the field for which the error is being added.</param>
        /// <param name="errorMessage">The error message to be added for the field.</param>
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
        /// Checks if the model is valid by verifying if there are any validation errors.
        /// </summary>
        public bool IsValid => Errors.Count == 0;

        /// <summary>
        /// Clears all error messages from the Errors dictionary.
        /// </summary>
        public void ClearErrors()
        {
            Errors.Clear();
        }
    }
}
