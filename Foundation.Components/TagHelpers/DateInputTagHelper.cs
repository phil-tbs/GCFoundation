using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-date-input")]
    public class DateInputTagHelper : BaseFormComponentTagHelper
    {
        public required DateInputFormatTypeEnum Format { get; set; }

        public required string Legend { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "format", Format);
            AddAttributeIfNotNull(output, "legend", Legend);
            base.Process(context, output);
        }
    }
}
