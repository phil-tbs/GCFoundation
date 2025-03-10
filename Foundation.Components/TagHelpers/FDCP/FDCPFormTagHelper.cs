using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Foundation.Components.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.FDCP
{
    [HtmlTargetElement("fdcp-form", Attributes = "for, method, action")]
    public class FDCPFormTagHelper : TagHelper
    {
        [HtmlAttributeName("for")]
        public BaseViewModel Model { get; set; } = default!;

        [HtmlAttributeName("method")]
        public string Method { get; set; } = "post";

        [HtmlAttributeName("action")]
        public string Action { get; set; } = default!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Model == null)
            {
                throw new ArgumentNullException(nameof(Model), "The model cannot be null.");
            }

            var childContent = output.Content.IsModified ? output.Content.GetContent() :
            (await output.GetChildContentAsync()).GetContent();

            // Start the <form> tag
            output.TagName = "form";
            output.Attributes.SetAttribute("method", Method);
            if (!string.IsNullOrEmpty(Action))
            {
                output.Attributes.SetAttribute("action", Action);
            }


            var errorSummaryTag = new TagBuilder("gcds-error-summary");
            errorSummaryTag.Attributes.Add("lang", CultureInfo.CurrentCulture.Name);

            // Add error summary if model has errors
            if (!Model.IsValid)
            {
                var errorLinks = Model.Errors.ToDictionary(
                    kvp => $"#{kvp.Key}", // Convert field names to anchor links
                    kvp => string.Join(" ", kvp.Value) // Join multiple errors per field
                );

                var errorJson = JsonSerializer.Serialize(errorLinks);
                errorSummaryTag.Attributes.Add("error-links", errorJson);
            }

            output.Content.AppendHtml(errorSummaryTag);

            output.Content.AppendHtml(childContent);
        }
    }
}
