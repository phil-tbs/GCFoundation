﻿namespace GCFoundation.Components.Models.FormBuilder
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

        /// <summary>
        /// Gets or sets the list of dependencies that control the visibility or behavior of this option.
        /// This is particularly useful for checkbox options that need individual dependency rules.
        /// </summary>
        public IEnumerable<QuestionDependency>? Dependencies { get; set; }

        /// <summary>
        /// Gets or sets whether this option is disabled.
        /// When disabled, the option will be greyed out and non-interactive.
        /// </summary>
        public bool IsDisabled { get; set; }
    }
}
