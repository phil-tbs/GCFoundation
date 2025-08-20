using GCFoundation.Components.Enums;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;
using System.Text;

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
        /// Sets the size of the page header. Default, or Large.
        /// </summary>
        public PageHeaderSize Size { get; set; } = PageHeaderSize.Default;

        /// <summary>
        /// The URL of the background image for the page header.
        /// </summary>
        public string? Src { get; set; }

        /// <summary>
        /// Adds a light background and a border around the text container to emphasize the content.
        /// </summary>
        public bool TextEmphasis { get; set; } = false;

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

            switch (Size)
            {
                case PageHeaderSize.Large:
                    classValue += " fdcp-page-header-large";
                    break;
                case PageHeaderSize.Default:
                default:
                    break;
            }

            output.Attributes.SetAttribute("class", classValue);

            var textContainerClass = "text-container";
            if (TextEmphasis)
                textContainerClass += " text-container-well";

            var textContainer = new StringBuilder();
            textContainer.Append(CultureInfo.InvariantCulture, $"<div class='{textContainerClass}'>");
            textContainer.Append(CultureInfo.InvariantCulture, $"<gcds-heading tag='h1'>{Title}</gcds-heading>");

            if (!string.IsNullOrWhiteSpace(Description))
            {
                textContainer.Append(CultureInfo.InvariantCulture, $"<gcds-text>{Description}</gcds-text>");
            }

            textContainer.Append("</div>");

            output.Content.SetHtmlContent(textContainer.ToString());
        }
    }
}
