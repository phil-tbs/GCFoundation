namespace GCFoundation.Components.Models
{
    /// <summary>
    /// Represents a request for filtering search results.
    /// </summary>
    public class SearchFilterRequest
    {
        /// <summary>
        /// Gets or sets the page number for the search results.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the search term used to filter results.
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int NumberByPage { get; set; }

        /// <summary>
        /// Gets or sets the categories used to filter search results.
        /// </summary>
        public IEnumerable<SearchFilterCategory> Categories { get; set; } = Enumerable.Empty<SearchFilterCategory>();
    }
}
