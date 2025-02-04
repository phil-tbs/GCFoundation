using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-input")]
    public class InputTagHelper : BaseFromComponentTagHelper
    {
        public required string InputId { get; set; }

        public required string Label { get; set; }

        public AutocompleteTypeEnum Autocomplete { get; set; } = AutocompleteTypeEnum.off;

        public bool HideLabel { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "input-id", InputId);
            AddAttributeIfNotNull(output, "label", Label);
            AddAttributeIfNotNull(output, "hide-label", HideLabel);
            AddAttributeIfNotNull(output, "autocomplete", Autocomplete);
            base.Process(context, output);
        }
    }
}
