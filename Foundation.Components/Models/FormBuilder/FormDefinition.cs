using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Models.FormBuilder
{
    /// <summary>
    /// Represents the definition of a form, including its unique identifier, title, and sections.
    /// </summary>
    public class FormDefinition
    {
        /// <summary>
        /// Gets or sets the unique identifier for the form.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Gets or sets the display title of the form.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the form action URL or endpoint.
        /// </summary>
        public required string Action { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method used to submit the form (e.g., "post" or "get").
        /// </summary>
        public required string Methode { get; set; } = "post";

        /// <summary>
        /// Gets or sets the text displayed on the form's submit button.
        /// </summary>
        public required string SubmithButtonText { get; set; } = "Submit";

        /// <summary>
        /// Gets or sets the collection of sections that make up the form.
        /// </summary>
        public IEnumerable<FormSection> Sections { get; set; } = Enumerable.Empty<FormSection>();
    }
}
