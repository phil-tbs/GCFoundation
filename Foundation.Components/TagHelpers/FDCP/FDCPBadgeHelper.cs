using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.FDCP
{
    [HtmlTargetElement("fdcp-badge")]
    public class FDCPBadgeHelper: TagHelper
    {

        public string Text { get; set; }

        public bool IsRemovable { get; set; }

        public string TagId { get; set; }

        public FDCPBadgeStyle Style { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "span";

            var classes = new StringBuilder("fdcp-badge");
            classes.Append($" fdcp-badge-{Style.ToString().ToLowerInvariant()}");

            if (IsRemovable)
            {
                classes.Append(" fdcp-badge-removable");
            }

            output.Attributes.SetAttribute("class", classes.ToString());

            if (!string.IsNullOrEmpty(TagId))
            {
                output.Attributes.SetAttribute("id", TagId);
            }

            // Build inner HTML
            var contentBuilder = new StringBuilder();
            contentBuilder.Append(Text);

            if (IsRemovable)
            {
                contentBuilder.Append(" ");
                contentBuilder.Append("<button type='button' class='fdcp-badge-close' aria-label='Remove badge'>&times;</button>");
            }

            output.Content.SetHtmlContent(contentBuilder.ToString());

        }

    }

    public enum FDCPBadgeStyle
    {
        Success,
        Danger,
        Info,
        Warning,
        Primary,
        Secondary,
        Light,
        Dark
    }
}
