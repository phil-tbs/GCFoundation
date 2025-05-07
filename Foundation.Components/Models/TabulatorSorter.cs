namespace Foundation.Components.Models
{
    /// <summary>
    /// Represents a sorter for sorting Tabulator data, allowing the specification of the field to sort by and the direction of sorting.
    /// </summary>
    public class TabulatorSorter
    {
        /// <summary>
        /// Gets or sets the field by which to sort the data. This is the name of the column in Tabulator.
        /// </summary>
        public string Field { get; set; } = "";

        /// <summary>
        /// Gets or sets the direction of sorting. This can be "asc" for ascending or "desc" for descending. Default is "asc".
        /// </summary>
        public string Dir { get; set; } = "asc"; // or "desc"
    }
}
