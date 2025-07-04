namespace GCFoundation.Components.Models
{
    /// <summary>
    /// Represents a request for data in Tabulator, including pagination, sorting, and filtering options.
    /// </summary>
    public class TabulatorRequest
    {
        /// <summary>
        /// Gets or sets the current page number for the data request. Defaults to 1.
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of records per page. Defaults to 10.
        /// </summary>
        public int Size { get; set; } = 10;

        /// <summary>
        /// Gets or sets the sorting options for the data request. Contains an array of <see cref="TabulatorSorter"/> objects.
        /// </summary>
        public IEnumerable<TabulatorSorter> Sort { get; set; } = Enumerable.Empty<TabulatorSorter>();

        /// <summary>
        /// Gets or sets the filtering options for the data request. Contains an array of <see cref="TabulatorFilter"/> objects.
        /// </summary>
        public IEnumerable<TabulatorFilter> Filter { get; set; } = Enumerable.Empty<TabulatorFilter>();
    }
}
