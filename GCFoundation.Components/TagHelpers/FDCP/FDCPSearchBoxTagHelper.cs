using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GCFoundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// A TagHelper that renders a styled search box for FDCP filtering scenarios.
    /// </summary>
    /// <remarks>
    /// Use the <c>&lt;fdcp-search-box&gt;</c> tag in Razor Pages to render a search input with custom label and placeholder.
    /// </remarks>
    [HtmlTargetElement("fdcp-search-box")]
    public class FDCPSearchBoxTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the placeholder text for the search input.
        /// </summary>
        public required string Placeholder { get; set; }

        /// <summary>
        /// Gets or sets the label text for the search input (used for accessibility).
        /// </summary>
        public required string Label { get; set; }

        /// <summary>
        /// Gets or sets the name attribute for the search input.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the search box.
        /// </summary>
        public required string SearchBoxId { get; set; }

        /// <summary>
        /// Gets or sets the value of the search input.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Processes the tag helper and renders the search box markup.
        /// </summary>
        /// <param name="context">The context for the tag helper.</param>
        /// <param name="output">The output for the tag helper.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "fdcp-filtered-search");

            StringBuilder sr = new StringBuilder();

            sr.Append("<div class='fdcp-search-box-wrapper'>");

            sr.Append(CultureInfo.InvariantCulture, $"<label class='sr-only'>{Label}</label>");
            sr.Append(CultureInfo.InvariantCulture, $"<input type='search' {(string.IsNullOrEmpty(Value) ? "" : $"value='{Value}'")} placeholder='{Placeholder}'>");
            sr.Append("</div>");

            output.Content.SetHtmlContent(sr.ToString());
        }
    }
}
