using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Enum
{
    /// <summary>
    /// Defines the possible resizable options for Tabulator columns. Determines whether the column is resizable, and if so, whether it can be resized by the header or the cell.
    /// </summary>
    public enum TabulatorResizableOption
    {
        /// <summary>
        /// The column is resizable.
        /// </summary>
        True,

        /// <summary>
        /// The column is not resizable.
        /// </summary>
        False,

        /// <summary>
        /// The column is resizable by the header.
        /// </summary>
        Header,

        /// <summary>
        /// The column is resizable by the cell.
        /// </summary>
        Cell
    }
}
