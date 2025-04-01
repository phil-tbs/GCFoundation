using System.Globalization;
using System.Text.Json;
using Foundation.Components.Models;
using Foundation.Components.Utilities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.FDCP
{
    [HtmlTargetElement("fdcp-error-summary", Attributes = "for")]
    public class FDCPErrorSummaryTagHelper: TagHelper
    {
        [HtmlAttributeName("for")]
        public BaseViewModel Model { get; set; } = default!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(Model == null)
            {
                output.SuppressOutput();
                return;
            }

            var errorJson = JsonSerializer.Serialize(Model.Errors.ToDictionary(
                    kvp => $"#{kvp.Key}",
                    kvp => string.Join(" ", kvp.Value)
                ));
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "gcds-error-summary";
            output.Attributes.SetAttribute("lang", LanguageUtility.GetCurrentApplicationLanguage());

            if (!Model.IsValid)
            {
                output.Attributes.SetAttribute("error-links", errorJson);
            }
            output.Attributes.SetAttribute("listen", true);

        }
    }
}
