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
    [HtmlTargetElement("gcds-footer")]
    public class FooterTagHelper : BaseTagHelper
    {
        public string? ContextualHeadling {  get; set; }

        public List<FooterLink>? ContextualLinks { get; set; }

        public FooterDisplayTypeEnum Display { get; set; } = FooterDisplayTypeEnum.full;

        public LanguageEnum Lan { get; set; } = LanguageEnum.en;

        public List<FooterLink>? SubLinks { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "contextual-heading", ContextualHeadling);

            if (ContextualLinks?.Any() == true)
            {
                string contextualLinksJson = JsonSerializer.Serialize(
                    ContextualLinks.ToDictionary(link => link.Label, link => link.Link),
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                );
                output.Attributes.SetAttribute("contextual-links", contextualLinksJson);
            }

            AddAttributeIfNotNull(output, "display", Display);

            AddAttributeIfNotNull(output, "lan", Lan);

            if (SubLinks?.Any() == true)
            {
                string subLinksJson = JsonSerializer.Serialize(
                    SubLinks.ToDictionary(link => link.Label, link => link.Link),
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                );
                output.Attributes.SetAttribute("contextual-links", subLinksJson);
            }

            base.Process(context, output);
        }
    }
}
