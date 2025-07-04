namespace GCFoundation.Web.Models
{
    /// <summary>
    /// Represents a founding opportunity with details such as section, title, description, and status.
    /// </summary>
    public class FoundingOpportunity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the founding opportunity.
        /// </summary>
        public required Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the section to which the founding opportunity belongs.
        /// </summary>
        public required string Section { get; set; }

        /// <summary>
        /// Gets or sets the title of the founding opportunity.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the founding opportunity.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the allocation amount for the founding opportunity.
        /// </summary>
        public double Allocation { get; set; }

        /// <summary>
        /// Gets or sets the status of the founding opportunity.
        /// </summary>
        public FoundingOpportunityStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the date when the status was last updated.
        /// </summary>
        public DateOnly StatusDate { get; set; }

        /// <summary>
        /// Gets or sets the tags associated with the founding opportunity.
        /// </summary>
        public IEnumerable<string>? Tags { get; set; }
    }

    /// <summary>
    /// Represents the status of a founding opportunity.
    /// </summary>
    public enum FoundingOpportunityStatus
    {
        /// <summary>
        /// Indicates that the founding opportunity is open.
        /// </summary>
        Open,

        /// <summary>
        /// Indicates that the founding opportunity is closed.
        /// </summary>
        Close
    }
}
