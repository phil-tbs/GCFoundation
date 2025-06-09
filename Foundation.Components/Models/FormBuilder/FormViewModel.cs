using System.ComponentModel.DataAnnotations;
using Foundation.Components.Validation;

namespace Foundation.Components.Models.FormBuilder
{
    /// <summary>
    /// View model for form data, including the form definition and user input values.
    /// </summary>
    public class FormViewModel
    {
        /// <summary>
        /// Gets or sets the form definition, including structure and metadata.
        /// </summary>
        [ValidateFormDependencies]
        public required FormDefinition Form { get; set; }

        /// <summary>
        /// Gets or sets the dictionary containing form field values keyed by question ID.
        /// </summary>
        public Dictionary<string, object?> FormData { get; set; } = new();
    }
} 