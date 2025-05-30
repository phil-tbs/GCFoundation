using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Models.FormBuilder
{
    /// <summary>
    /// Represents a section within a form, containing a title, an optional hint, and a collection of questions.
    /// </summary>
    public class FormSection
    {
        /// <summary>
        /// Gets or sets the title of the form section.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets an optional hint or description for the section.
        /// </summary>
        public string? Hint { get; set; }

        /// <summary>
        /// Gets or sets the list of questions included in this section.
        /// </summary>
        public IEnumerable<FormQuestion> Questions { get; set; } = Enumerable.Empty<FormQuestion>();
    }
}
