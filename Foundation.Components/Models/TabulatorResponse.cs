using System.Text.Json.Serialization;

namespace Foundation.Components.Models
{
    /// <summary>
    /// Represents a response structure for Tabulator tables using AJAX.
    /// Includes the total number of pages and the data for the current page.
    /// </summary>
    /// <typeparam name="T">The type of the data items in the table.</typeparam>
    public class TabulatorResponse<T>
    {
        /// <summary>
        /// Gets or sets the total number of pages available for pagination.
        /// </summary>
        [JsonPropertyName("last_page")]
        public int LastPage { get; set; }

        /// <summary>
        /// Gets the data items to be rendered in the table for the current page.
        /// </summary>
        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; init; } = Enumerable.Empty<T>();
    }
}
