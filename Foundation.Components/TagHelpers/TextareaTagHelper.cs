using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-textarea")]
    public class TextareaTagHelper: BaseFormComponentTagHelper
    {
        public required string Label { get; set; }
        public required string TextareaId { get; set; }

        public int CharacterCount { get; set; }

        public bool HideLabel { get; set; }

        public int Rows { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "label", Label);
            AddAttributeIfNotNull(output, "textarea-id", TextareaId);
            AddAttributeIfNotNull(output, "character-count", CharacterCount);
            AddAttributeIfNotNull(output, "hide-label", HideLabel);
            AddAttributeIfNotNull(output, "rows", Rows);

            base.Process(context, output);
        }
    }
}
