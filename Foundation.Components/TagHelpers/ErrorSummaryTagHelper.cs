using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Foundation.Components.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-error-summary")]
    public class ErrorSummaryTagHelper : BaseTagHelper
    {
        public List<ErrorLink>? ErrorLinks { get; set; }

        public string? Heading { get; set; }

        public LanguageEnum Lan { get; set; }

        public bool Listen { get; set; } = true;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if (ErrorLinks?.Any() == true)
            {
                string errorLinksJson = JsonSerializer.Serialize(
                    ErrorLinks.ToDictionary(link => link.Href, link => link.Message),
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                );
                output.Attributes.SetAttribute("error-links", errorLinksJson);
            }
            AddAttributeIfNotNull(output, "heading", Heading);
            AddAttributeIfNotNull(output, "lan", Lan);
            AddAttributeIfNotNull(output, "listen", Listen);
            base.Process(context, output);
        }

    }
}
