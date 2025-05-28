namespace Foundation.Components.Models
{
    /// <summary>
    /// Represents a filter category.
    /// </summary>
    public class SearchFilterCategory
    {
        /// <summary>
        /// The title of the category.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// A unique identifier for the filter category, used in HTML, CSS, and JavaScript actions.
        /// </summary>
        public required string SearchFilterCategoryId { get; set; }

        /// <summary>
        /// Indicates whether the category is open or collapsed in the UI.
        /// </summary>
        public bool IsOpen { get; set; } = true;

        /// <summary>
        /// A list of search filter options associated with the category.
        /// </summary>
        public IEnumerable<SearchFilterOption> Filters { get; set; } = Enumerable.Empty<SearchFilterOption>();
    }
}
