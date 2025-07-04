namespace GCFoundation.Components.Models
{
    /// <summary>
    /// Represents a filter for Tabulator data request, specifying the field, filter type, and value to filter by.
    /// </summary>
    public class TabulatorFilter
    {
        /// <summary>
        /// Gets or sets the field name to apply the filter on.
        /// </summary>
        public string Field { get; set; } = "";

        /// <summary>
        /// Gets or sets the type of filter to apply. Defaults to "like". Common values are "like", "=", "&lt;", "&gt;", etc.
        /// </summary>
        public string Type { get; set; } = "like";

        /// <summary>
        /// Gets or sets the value to filter by. This is optional and can be null.
        /// </summary>
        public string? Value { get; set; }
    }
}
