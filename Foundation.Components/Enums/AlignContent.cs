using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Enums
{
    /// <summary>
    /// Defines the alignment options for the content along the cross axis in a flex container.
    /// These values control how space is distributed between and around content within the container.
    /// </summary>
    public enum AlignContent
    {
        /// <summary>
        /// Aligns the content to the center of the container.
        /// </summary>
        center,

        /// <summary>
        /// Aligns the content to the end (bottom in a vertical container or right in a horizontal container) of the container.
        /// </summary>
        end,

        /// <summary>
        /// Distributes the content with space around each item, leaving equal space around the content.
        /// </summary>
        spaceArround,

        /// <summary>
        /// Distributes the content with equal space between each item, but no space at the ends.
        /// </summary>
        spaceBetween,

        /// <summary>
        /// Distributes the content with equal space between each item and the ends of the container.
        /// </summary>
        spaceEvenly,

        /// <summary>
        /// Aligns the content to the start (top in a vertical container or left in a horizontal container) of the container.
        /// </summary>
        start,

        /// <summary>
        /// Stretches the content to fill the container along the cross axis.
        /// </summary>
        stretch
    }
}
