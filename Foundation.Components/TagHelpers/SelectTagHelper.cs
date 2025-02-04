using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-select")]
    public class SelectTagHelper : BaseFromComponentTagHelper
    {
        public required string Label { get; set; }

        public required string SelectId { get; set; }

        public string? DefaultValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "label", Label);
            AddAttributeIfNotNull(output, "select-id", SelectId);
            AddAttributeIfNotNull(output, "default-value", DefaultValue);
            base.Process(context, output);
        }
    }
}
