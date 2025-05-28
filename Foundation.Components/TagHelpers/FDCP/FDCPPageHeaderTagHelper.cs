using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.FDCP
{
    [HtmlTargetElement("fdcp-page-heading")]
    public class FDCPPageHeaderTagHelper: TagHelper
    {

        public required string Title { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? Src { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            var classValue = "fdcp-page-header-container";

            if (!string.IsNullOrWhiteSpace(Src))
            {
                classValue += " fdcp-page-header--has-bg";
                output.Attributes.SetAttribute("data-bg-src", Src);
            }

            output.Attributes.SetAttribute("class", classValue);

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("<div class='text-container'>");

            stringBuilder.Append(CultureInfo.InvariantCulture,$"<gcds-heading tag='h1'>{Title}</gcds-heading>");

            if (!string.IsNullOrWhiteSpace(Description))
            {
                stringBuilder.Append(CultureInfo.InvariantCulture, $"<gcds-text>{Description}</gcds-text>");
            }

            stringBuilder.Append("</div>");

            output.Content.SetHtmlContent(stringBuilder.ToString());

        }
    }
}
