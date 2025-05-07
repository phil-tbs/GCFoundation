namespace Foundation.Components.Models
{
    /// <summary>
    /// Represents a link in the footer with its label and URL.
    /// </summary>
    public class FooterLink
    {
        /// <summary>
        /// Gets or sets the label of the footer link.
        /// </summary>
        public required string Label { get; set; }

        /// <summary>
        /// Gets or sets the URL for the footer link.
        /// </summary>
        public required string Link { get; set; }
    }
}
