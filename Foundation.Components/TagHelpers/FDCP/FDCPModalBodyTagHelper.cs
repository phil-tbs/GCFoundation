using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.FDCP
{
    [HtmlTargetElement("fdcp-modal-body")]
    public class FDCPModalBodyTagHelper : TagHelper
    {

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "modal-body");

            // Razor context: ConfigureAwait(false) is not safe here
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
            var childContent = await output.GetChildContentAsync();
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
            var html = childContent.GetContent();

            output.Content.SetHtmlContent(html);

        }
    }
}
