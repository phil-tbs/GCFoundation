using GCFoundation.Components.Converters;
using GCFoundation.Components.Enums;
using System.Text.Json.Serialization;

namespace GCFoundation.Components.Models
{
    /// <summary>
    /// Represents a column definition for a Tabulator table.
    /// </summary>
    public class TabulatorColumn
    {
        /// <summary>The visible column header title.</summary>
        public string Title { get; set; } = "";

        /// <summary>The field name to bind from the data object (must match JSON property).</summary>
        public string Field { get; set; } = "";

        /// <summary>Determines if the column header is sortable. Default is true.</summary>
        public bool HeaderSort { get; set; } = true;

        /// <summary>Specifies a fixed width for the column (e.g., "100px", "10%").</summary>
        public string? Width { get; set; }

        /// <summary>Horizontal alignment: "left", "center", "right".</summary>
        public string? HozAlign { get; set; }

        /// <summary>Optional formatter (e.g., "tickCross", "html", "link").</summary>
        public string? Formatter { get; set; }

        /// <summary>CSS class to apply to the column's cells.</summary>
        public string? CssClass { get; set; }

        /// <summary>Whether the column can be resized by the user. 
        /// </summary>
        [JsonConverter(typeof(TabulatorResizableOptionConverter))]
        public TabulatorResizableOption Resizable { get; set; } = TabulatorResizableOption.False;

        /// <summary>If true, the column is frozen in place while scrolling horizontally.</summary>
        public bool? Frozen { get; set; }

        /// <summary>Tooltip text or boolean to enable/disable tooltips for this column.</summary>
        public string? Tooltip { get; set; }

        /// <summary>
        /// If we want to filter on that column
        /// </summary>
        public bool Filter { get; set; }

    }
}
