using System.ComponentModel.DataAnnotations;
using Foundation.Components.Validation;

namespace Foundation.Components.Models.FormBuilder
{
    public class FormViewModel
    {
        [ValidateFormDependencies]
        public required FormDefinition Form { get; set; }

        // Dictionary to store form data
        public Dictionary<string, object?> FormData { get; set; } = new();
    }
} 