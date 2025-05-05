using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-fieldset")]
    public class FieldsetTagHelper: BaseTagHelper
    {
        public required string FieldsetId { get; set; }

        public required string Legend { get; set; }

        public bool Disabled { get; set; }

        public string? ErrorMessage { get; set; }

        public string? Hint { get; set; }

        public Language Lan { get; set; } = Language.en;

        public bool Required { get; set; }

        public string? ValidateOn { get; set; } = "blur";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "fieldset-id", FieldsetId);
            AddAttributeIfNotNull(output, "legend", Legend);
            AddAttributeIfNotNull(output, "disabled", Disabled);
            AddAttributeIfNotNull(output, "error-message", ErrorMessage);
            AddAttributeIfNotNull(output, "hint", Hint);
            AddAttributeIfNotNull(output, "lan", Lan);
            AddAttributeIfNotNull(output, "required", Required);
            AddAttributeIfNotNull(output, "validate-on", ValidateOn);
            base.Process(context, output);
        }
    }
}
