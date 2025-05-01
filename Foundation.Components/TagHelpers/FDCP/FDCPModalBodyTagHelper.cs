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
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "modal-body");

            var childContent = await output.GetChildContentAsync();
            var html = childContent.GetContent();

            output.Content.SetHtmlContent(html);

        }
    }
}
