using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-top-nav")]
    public class TopNavigationTagHelper : BaseTagHelper
    {
        public required string Label { get; set; }

        public TopMenuAlignmentEnum Alignment { get; set; } = TopMenuAlignmentEnum.right;
        public LanguageEnum Lang { get; set; } = LanguageEnum.en;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "label", Label);
            AddAttributeIfNotNull(output, "alignment", Alignment);
            AddAttributeIfNotNull(output, "lan", Lang);

            base.Process(context, output);
        }
    }
}
