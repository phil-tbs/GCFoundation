using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Foundation.Common.Utilities;
using Foundation.Components.Enum;
using Foundation.Components.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-footer")]
    public class FooterTagHelper : BaseTagHelper
    {
        public string? ContextualHeadling {  get; set; }

        public IEnumerable<FooterLink>? ContextualLinks { get; set; }

        public FooterDisplayType Display { get; set; } = FooterDisplayType.full;

        public Language Lan { get; set; } = Language.en;

        public IEnumerable<FooterLink>? SubLinks { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "contextual-heading", ContextualHeadling);

            if (ContextualLinks != null && ContextualLinks.Any() != false)
            {
                string contextualLinksJson = JsonSerializer.Serialize(
                    ContextualLinks.ToDictionary(link => link.Label, link => link.Link),
                    JsonOptionsUtility.CamelCase
                );
                output.Attributes.SetAttribute("contextual-links", contextualLinksJson);
            }

            AddAttributeIfNotNull(output, "display", Display);

            AddAttributeIfNotNull(output, "lan", Lan);

            if (SubLinks != null && SubLinks.Any() != false)
            {
                string subLinksJson = JsonSerializer.Serialize(
                    SubLinks.ToDictionary(link => link.Label, link => link.Link),
                    JsonOptionsUtility.CamelCase
                );
                output.Attributes.SetAttribute("contextual-links", subLinksJson);
            }

            base.Process(context, output);
        }
    }
}
