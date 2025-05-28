namespace Foundation.Components.Models
{
    /// <summary>
    /// Represents an option for a search filter.
    /// </summary>
    public class SearchFilterOption
    {
        /// <summary>
        /// The name of the search filter option sent to the server.
        /// This should not contain any special characters.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The title displayed to the user.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// The number of results that contain this value.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Indicates whether the filter is active or selected.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
