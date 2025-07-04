using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GCFoundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// TagHelper for rendering a GC-style page header with title, description, and optional background image.
    /// </summary>
    [HtmlTargetElement("fdcp-page-heading")]
    public class FDCPPageHeaderTagHelper : TagHelper
    {
        /// <summary>
        /// The main heading text to display in the page header.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// The description text displayed below the title.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The URL of the background image for the page header.
        /// </summary>
        public string? Src { get; set; }

        /// <summary>
        /// Processes the tag helper and renders the page header markup.
        /// </summary>
        /// <param name="context">The context for the tag helper.</param>
        /// <param name="output">The output for the tag helper.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            var classValue = "fdcp-page-header-container";

            if (!string.IsNullOrWhiteSpace(Src))
            {
                classValue += " fdcp-page-header--has-bg";
                output.Attributes.SetAttribute("data-bg-src", Src);
            }

            output.Attributes.SetAttribute("class", classValue);

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("<div class='text-container'>");

            stringBuilder.Append(CultureInfo.InvariantCulture, $"<gcds-heading tag='h1'>{Title}</gcds-heading>");

            if (!string.IsNullOrWhiteSpace(Description))
            {
                stringBuilder.Append(CultureInfo.InvariantCulture, $"<gcds-text>{Description}</gcds-text>");
            }

            stringBuilder.Append("</div>");

            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }
}
