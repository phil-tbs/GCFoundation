using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-topic-menu")]
    public class TopicMenuTagHelper: BaseTagHelper
    {
        public bool Home { get; set; } = false;
        public LanguageEnum Lang { get; set; } = LanguageEnum.en;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "home", Home);
            AddAttributeIfNotNull(output, "lang", Lang);
            base.Process(context, output);
        }
    }
}
