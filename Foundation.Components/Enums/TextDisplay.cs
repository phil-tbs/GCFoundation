using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Enums
{
    /// <summary>
    /// Defines the available display types for text components.
    /// </summary>
    public enum TextDisplay
    {
        /// <summary>
        /// Displays the text as a block-level element, starting on a new line.
        /// </summary>
        Block,

        /// <summary>
        /// Displays the text as a flex container.
        /// </summary>
        Flex,

        /// <summary>
        /// Displays the text as an inline element, allowing it to flow within the text.
        /// </summary>
        Inline,

        /// <summary>
        /// Displays the text as an inline-block element, allowing it to behave like both an inline and block element.
        /// </summary>
        InlineBlock,

        /// <summary>
        /// Displays the text as an inline flex container.
        /// </summary>
        InlineFlex,

        /// <summary>
        /// Hides the text element completely.
        /// </summary>
        None
    }
}
