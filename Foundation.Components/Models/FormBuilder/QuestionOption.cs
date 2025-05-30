using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Models.FormBuilder
{
    /// <summary>
    /// Represents an option for a form question, such as a choice in a dropdown or radio button group.
    /// </summary>
    public class QuestionOption
    {
        /// <summary>
        /// Gets or sets the unique identifier for the option.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Gets or sets the value associated with the option.
        /// </summary>
        public required string Value { get; set; }

        /// <summary>
        /// Gets or sets the display label for the option.
        /// </summary>
        public required string Label { get; set; }

        /// <summary>
        /// Gets or sets an optional hint or description for the option.
        /// </summary>
        public string? Hint { get; set; }
    }
}
