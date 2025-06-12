namespace Foundation.Components.Models.FormBuilder
{
    /// <summary>
    /// Represents a question within a form, including its metadata, type, options, and dependencies.
    /// </summary>
    public class FormQuestion
    {
        /// <summary>
        /// Gets or sets the unique identifier for the question.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Gets or sets the display label for the question.
        /// </summary>
        public required string Label { get; set; }

        /// <summary>
        /// Gets or sets an optional hint or description for the question.
        /// </summary>
        public string? Hint { get; set; }

        /// <summary>
        /// Gets or sets the type of the question (e.g., Text, Email, Radio).
        /// </summary>
        public QuestionType Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the question is required.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the error message to display when validation fails.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the question is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets the size or length constraint for the question input, if applicable.
        /// </summary>
        public int? Size { get; set; }

        /// <summary>
        /// Gets or sets the list of selectable options for the question, if applicable.
        /// </summary>
        public IEnumerable<QuestionOption>? Options { get; set; }

        /// <summary>
        /// Gets or sets the list of dependencies that control the visibility or behavior of this question.
        /// </summary>
        public IEnumerable<QuestionDependency>? Dependencies { get; set; }

        /// <summary>
        /// Gets or sets the format for the question input, if applicable (e.g., "full" for date).
        /// </summary>
        public string? Format { get; set; }

        /// <summary>
        /// Gets or sets the current or default value for the question.
        /// </summary>
        public object? Value { get; set; }

        /// <summary>
        /// Gets or sets the list of validation rules for this question.
        /// </summary>
        public IEnumerable<ValidationRule>? ValidationRules { get; set; }

        /// <summary>
        /// Gets or sets whether to validate on blur (client-side).
        /// </summary>
        public bool ValidateOnBlur { get; set; } = true;

        /// <summary>
        /// Gets or sets the legend size for fieldsets (h2-h6).
        /// Used to visually place the legend among the headings on the page.
        /// </summary>
        public string? LegendSize { get; set; }

        /// <summary>
        /// Gets or sets whether the radio buttons or checkboxes should be displayed inline.
        /// </summary>
        public bool IsInline { get; set; }

        /// <summary>
        /// Gets or sets whether this is a single tickbox (small checkbox) rather than a group.
        /// Only applicable for Checkbox type questions with a single option.
        /// </summary>
        public bool IsTickbox { get; set; }
    }

    /// <summary>
    /// Specifies the type of a form question.
    /// </summary>
    public enum QuestionType
    {
        /// <summary>
        /// A single-line text input.
        /// </summary>
        Text,
        /// <summary>
        /// An email address input.
        /// </summary>
        Email,
        /// <summary>
        /// A password input.
        /// </summary>
        Password,
        /// <summary>
        /// A URL input.
        /// </summary>
        Url,
        /// <summary>
        /// A multi-line text area input.
        /// </summary>
        TextArea,
        /// <summary>
        /// A numeric input.
        /// </summary>
        Number,
        /// <summary>
        /// A date input.
        /// </summary>
        Date,
        /// <summary>
        /// A radio button group.
        /// </summary>
        Radio,
        /// <summary>
        /// A checkbox input.
        /// </summary>
        Checkbox,
        /// <summary>
        /// A dropdown/select input.
        /// </summary>
        Dropdown,
        /// <summary>
        /// A file upload input.
        /// </summary>
        FileUpload
    }
}
