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

        public TopMenuAlignment Alignment { get; set; } = TopMenuAlignment.right;
        public Language Lang { get; set; } = Language.en;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "label", Label);
            AddAttributeIfNotNull(output, "alignment", Alignment);
            AddAttributeIfNotNull(output, "lan", Lang);

            base.Process(context, output);
        }
    }
}
