namespace GCFoundation.Components.Models
{
    /// <summary>
    /// Represents an individual radio button option for use in a radio group.
    /// </summary>
    public class RadioOption
    {
        /// <summary>
        /// Gets or sets the unique identifier for the radio option.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Gets or sets the display label for the radio option.
        /// </summary>
        public required string Label { get; set; }

        /// <summary>
        /// Gets or sets the value associated with the radio option.
        /// </summary>
        public required string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether selecting this option is required.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the radio option is disabled.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the radio option is pre-selected.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// Gets or sets an optional hint or description for the radio option.
        /// </summary>
        public string? Hint { get; set; }
    }
}
