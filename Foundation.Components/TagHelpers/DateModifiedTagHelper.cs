using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    public class DateModifiedTagHelper : BaseTagHelper
    {
        public LanguageEnum Lan { get; set; } = LanguageEnum.en;

        public DateModifiedTypeEnum Type { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "lang", Lan);
            AddAttributeIfNotNull(output, "type", Type);
            base.Process(context, output);
        }
    }
}
