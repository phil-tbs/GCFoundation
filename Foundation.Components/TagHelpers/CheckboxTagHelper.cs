using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-checkbox")]
    public class CheckboxTagHelper : BaseFormComponentTagHelper
    {
        /// <summary>
        /// Checkbox id
        /// </summary>
        public required string CheckboxId { get; set; }

        /// <summary>
        /// Label
        /// </summary>
        public required string Label { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "checkbox-id", CheckboxId);
            AddAttributeIfNotNull(output, "label", Label);
            

            base.Process(context, output);
        }
    }
}
