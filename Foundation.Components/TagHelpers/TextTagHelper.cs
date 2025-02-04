using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-text")]
    public class TextTagHelper: BaseTagHelper
    {
        public bool CharacterLimit { get; set; } = true;

        public TextDisplayEnum Display { get; set; } = TextDisplayEnum.Block;

        public string? MarginBottom { get; set; } = "300";
        public string? MarginTop { get; set; } = "0";

        public TextSizeEnum Size { get; set; } = TextSizeEnum.Body;

        public TextRoleEnum TextRole { get; set; } = TextRoleEnum.Primary;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "character-limit", CharacterLimit);
            AddAttributeIfNotNullWithCaseConversion(output, "display", Display);
            AddAttributeIfNotNull(output, "display", Display);
            AddAttributeIfNotNull(output, "margin-bottom", MarginBottom);
            AddAttributeIfNotNull(output, "margin-top", MarginTop);
            AddAttributeIfNotNull(output, "size", Size);
            AddAttributeIfNotNull(output, "text-role", TextRole);
            base.Process(context, output);
        }
    }
}
