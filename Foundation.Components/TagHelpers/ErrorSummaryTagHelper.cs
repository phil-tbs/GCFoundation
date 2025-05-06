using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Foundation.Common.Utilities;
using Foundation.Components.Enum;
using Foundation.Components.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    /// <summary>
    /// A tag helper for rendering a summary of error messages with links to specific error details.
    /// </summary>
    [HtmlTargetElement("gcds-error-summary")]
    public class ErrorSummaryTagHelper : BaseTagHelper
    {
        /// <summary>
        /// A collection of error links, each containing an error message and a hyperlink to the specific error.
        /// </summary>
        public IEnumerable<ErrorLink>? ErrorLinks { get; set; }

        /// <summary>
        /// The heading to be displayed for the error summary section.
        /// </summary>
        public string? Heading { get; set; }

        /// <summary>
        /// A flag that indicates whether the error summary should listen for changes and update dynamically.
        /// Default value is true.
        /// </summary>
        public bool Listen { get; set; } = true;

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            if (ErrorLinks != null && ErrorLinks.Any() != false)
            {
                string errorLinksJson = JsonSerializer.Serialize(
                    ErrorLinks.ToDictionary(link => link.Href, link => link.Message),
                    JsonOptionsUtility.CamelCase
                );
                output.Attributes.SetAttribute("error-links", errorLinksJson);
            }
            AddAttributeIfNotNull(output, "heading", Heading);
            AddAttributeIfNotNull(output, "lan", Lang);
            AddAttributeIfNotNull(output, "listen", Listen);
            base.Process(context, output);
        }

    }
}
