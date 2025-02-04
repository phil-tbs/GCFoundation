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
    [HtmlTargetElement("gcds-radio-group")]
    public class RadioGroupTagHelper: BaseTagHelper
    {
        public required string Name { get; set; }
        public required List<RadioOption> Options { get; set; }
        public LanguageEnum Lang { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "name", Name);
            AddAttributeIfNotNull(output, "lang", Lang);


            string optionJson = JsonSerializer.Serialize(
                Options,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower }
            );
            output.Attributes.SetAttribute("options", optionJson);


            base.Process(context, output);
        }
    }
}
