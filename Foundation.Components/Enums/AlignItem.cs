using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Enums
{
    /// <summary>
    /// Defines the alignment options for individual items along the cross axis in a flex container.
    /// These values control how each item is aligned within the container, relative to other items.
    /// </summary>
    public enum AlignItem
    {
        /// <summary>
        /// Aligns the item to the baseline of the container.
        /// </summary>
        baseline,

        /// <summary>
        /// Aligns the item to the center of the container along the cross axis.
        /// </summary>
        center,

        /// <summary>
        /// Aligns the item to the end (bottom in a vertical container or right in a horizontal container) of the container.
        /// </summary>
        end,

        /// <summary>
        /// Aligns the item to the start (top in a vertical container or left in a horizontal container) of the container.
        /// </summary>
        start,

        /// <summary>
        /// Stretches the item to fill the container along the cross axis.
        /// </summary>
        stretch
    }
}
