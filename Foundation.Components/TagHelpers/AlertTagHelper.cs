using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-notice")]
    public class AlertTagHelper: BaseTagHelper
    {

        public required string Title { get; set; }

        public HeadingTagEnum TitleTag { get; set; } = HeadingTagEnum.h2;

        public AlertTypeEnum Type { get; set; } = AlertTypeEnum.Info;
        public LanguageEnum Lang { get; set; } = LanguageEnum.en;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "gcds-notice";

            AddAttributeIfNotNull(output, "notice-title", Title);

            AddAttributeIfNotNull(output, "notice-title-tag", TitleTag);
            AddAttributeIfNotNull(output, "type", Type.ToString().ToLower());
            AddAttributeIfNotNull(output, "lange", Lang.ToString().ToLower());

            base.Process(context, output);
        }
    }
}
