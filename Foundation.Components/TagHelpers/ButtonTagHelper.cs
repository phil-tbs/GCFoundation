using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-button")]
    public class ButtonTagHelper : BaseTagHelper
    {
        public string? ButtonId { get; set; }

        public string? ButtonRole { get; set; }

        public bool Disable { get; set; }

        public string? Name { get; set; }

        public ButtonSizeType Size { get; set; } = ButtonSizeType.regular;

        public ButtonType Type { get; set; } = ButtonType.button;

        public string? Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            output.TagName = "gcds-button";

            AddAttributeIfNotNull(output, "button-id", ButtonId);
            AddAttributeIfNotNull(output, "button-role", ButtonRole);
            AddAttributeIfNotNull(output, "button-role", Disable);
            AddAttributeIfNotNull(output, "name", Name);
            AddAttributeIfNotNull(output, "size", Size);
            AddAttributeIfNotNull(output, "type", Type);
            AddAttributeIfNotNull(output, "value", Value);


            base.Process(context, output);
        }
    }
}
