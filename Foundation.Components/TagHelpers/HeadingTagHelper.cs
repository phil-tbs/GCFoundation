using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enums;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    /// <summary>
    /// A tag helper for generating heading elements with customizable properties.
    /// </summary>
    [HtmlTargetElement("gcds-heading")]
    public class HeadingTagHelper : BaseTagHelper
    {
        /// <summary>
        /// The HTML heading tag to be used (e.g., h1, h2, etc.).
        /// Default is <see cref="HeadingTag.h2"/>.
        /// </summary>
        public required HeadingTag Tag { get; set; } = HeadingTag.h2;

        /// <summary>
        /// Whether to apply a character limit for the heading.
        /// Default is <c>true</c>.
        /// </summary>
        public bool CharacterLimit { get; set; } = true;

        /// <summary>
        /// The margin-bottom CSS property value for the heading.
        /// </summary>
        public string? MarginBottom { get; set; }

        /// <summary>
        /// The margin-top CSS property value for the heading.
        /// </summary>
        public string? MarginTop { get; set; }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "tag", Tag);
            AddAttributeIfNotNull(output, "character-limit", CharacterLimit);
            AddAttributeIfNotNull(output, "margin-bottom", Tag);
            AddAttributeIfNotNull(output, "margin-top", MarginTop);

            base.Process(context, output);
        }

    }
}
