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


        public required string Action { get; set; }

        public required string Methode { get; set; } = "post";

        public required string SubmithButtonText { get; set; } = "Submit";

        /// <summary>
        /// Gets or sets the collection of sections that make up the form.
        /// </summary>
        public IEnumerable<FormSection> Sections { get; set; } = Enumerable.Empty<FormSection>();
    }
}
