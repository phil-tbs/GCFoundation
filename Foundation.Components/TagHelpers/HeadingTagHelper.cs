using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-heading")]
    public class HeadingTagHelper : BaseTagHelper
    {
        public required HeadingTag Tag { get; set; } = HeadingTag.h2;

        public bool CharacterLimit { get; set; } = true;

        public string? MarginBottom { get; set; }

        public string? MarginTop { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "tag", Tag);
            AddAttributeIfNotNull(output, "character-limit", CharacterLimit);
            AddAttributeIfNotNull(output, "margin-bottom", Tag);
            AddAttributeIfNotNull(output, "margin-top", MarginTop);

            base.Process(context, output);
        }

    }
}
