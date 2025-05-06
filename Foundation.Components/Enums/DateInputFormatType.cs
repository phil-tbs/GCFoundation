using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Enums
{
    /// <summary>
    /// Defines the available formats for date input fields.
    /// This enum specifies whether the date should be displayed in a full or compact format.
    /// </summary>
    public enum DateInputFormatType
    {
        /// <summary>
        /// The full date format, which expects/outputs a value formatted as YYYY-MM-DD.
        /// This format includes the year, month, and day.
        /// </summary>
        full,

        /// <summary>
        /// The compact date format, which expects/outputs a value formatted as YYYY-MM.
        /// This format includes only the year and month.
        /// </summary>
        compact
    }
}
