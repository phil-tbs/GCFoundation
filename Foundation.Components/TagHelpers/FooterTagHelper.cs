using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Foundation.Common.Utilities;
using Foundation.Components.Enums;
using Foundation.Components.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    /// <summary>
    /// Represents a custom tag helper for rendering a footer section with contextual and sub-links.
    /// This tag helper generates a footer with support for customizable headings, links, and display options.
    /// </summary>
    [HtmlTargetElement("gcds-footer")]
    public class FooterTagHelper : BaseTagHelper
    {
        /// <summary>
        /// The optional heading text to display in the footer's contextual section.
        /// </summary>
        public string? ContextualHeadling {  get; set; }

        /// <summary>
        /// The optional heading text to display in the footer's contextual section.
        /// </summary>
        public IEnumerable<FooterLink>? ContextualLinks { get; set; }

        /// <summary>
        /// The display type of the footer. Determines how the footer is rendered (e.g., full, minimal).
        /// Default is <see cref="FooterDisplayType.full"/>.
        /// </summary>
        public FooterDisplayType Display { get; set; } = FooterDisplayType.full;

        /// <summary>
        /// The collection of sub-links to display in the footer.
        /// </summary>
        public IEnumerable<FooterLink>? SubLinks { get; set; }


        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "contextual-heading", ContextualHeadling);

            if (ContextualLinks != null && ContextualLinks.Any() != false)
            {
                string contextualLinksJson = JsonSerializer.Serialize(
                    ContextualLinks.ToDictionary(link => link.Label, link => link.Link),
                    JsonOptionsUtility.CamelCase
                );
                output.Attributes.SetAttribute("contextual-links", contextualLinksJson);
            }

            AddAttributeIfNotNull(output, "display", Display);

            AddAttributeIfNotNull(output, "lang", Lang);

            if (SubLinks != null && SubLinks.Any() != false)
            {
                string subLinksJson = JsonSerializer.Serialize(
                    SubLinks.ToDictionary(link => link.Label, link => link.Link),
                    JsonOptionsUtility.CamelCase
                );
                output.Attributes.SetAttribute("contextual-links", subLinksJson);
            }

            base.Process(context, output);
        }
    }
}
