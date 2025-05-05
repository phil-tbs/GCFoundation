using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-stepper")]
    public class StepperTagHelper: BaseTagHelper
    {
        public int CurrentStep { get; set; }

        public int TotalStep { get; set; }

        public Language Lang { get; set; } = Language.en;

        public StepperTag Tag { get; set; } = StepperTag.h2;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "current-step", CurrentStep);
            AddAttributeIfNotNull(output, "total-step", TotalStep);
            AddAttributeIfNotNull(output, "current-step", Lang);
            AddAttributeIfNotNull(output, "tag", Tag);
            base.Process(context, output);
        }
    }
}
