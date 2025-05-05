using System;
using System.Collections.Generic;
using System.Globalization;
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

        public HeadingTag TitleTag { get; set; } = HeadingTag.h2;

        public AlertType Type { get; set; } = AlertType.Info;
        public Language Lang { get; set; } = Language.en;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));
            output.TagName = "gcds-notice";

            AddAttributeIfNotNull(output, "notice-title", Title);

            AddAttributeIfNotNull(output, "notice-title-tag", TitleTag);
#pragma warning disable CA1308 // Normalize strings to uppercase
            AddAttributeIfNotNull(output, "type", Type.ToString().ToLowerInvariant());
#pragma warning restore CA1308 // Normalize strings to uppercase
#pragma warning disable CA1308 // Normalize strings to uppercase
            AddAttributeIfNotNull(output, "lange", Lang.ToString().ToLowerInvariant());
#pragma warning restore CA1308 // Normalize strings to uppercase

            base.Process(context, output);
        }
    }
}
