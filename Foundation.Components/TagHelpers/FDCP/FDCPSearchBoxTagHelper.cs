using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.FDCP
{
    [HtmlTargetElement("fdcp-search-box")]
    public class FDCPSearchBoxTagHelper: TagHelper
    {
        public required string Placeholder { get; set; }

        public required string Label { get; set; }

        public string Name { get; set; }

        public required string SearchBoxId { get; set; }

        public string? Value { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Attributes.SetAttribute("class","fdcp-filtered-search");

            StringBuilder sr = new StringBuilder();

            sr.Append("<div class='fdcp-search-box-wrapper'>");

            //sr.Append("<span class='search-icon'></span>");
            sr.Append(CultureInfo.InvariantCulture, $"<label class='sr-only'>{Label}</label>");
            sr.Append(CultureInfo.InvariantCulture, $"<input type='search' {(string.IsNullOrEmpty(Value)? "": $"value='{Value}'")} placeholder='{Placeholder}'>");
            sr.Append("</div>");

            output.Content.SetHtmlContent(sr.ToString());
        }

    }
}
