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
    /// <summary>
    /// A tag helper for rendering a form. It binds to a model and adds the necessary form attributes (method, action). 
    /// Additionally, it generates an error summary if the model contains validation errors.
    /// </summary>
    [HtmlTargetElement("fdcp-form", Attributes = "for, method, action")]
    public class FDCPFormTagHelper : TagHelper
    {
        /// <summary>
        /// The model that the form is bound to. This model should contain any validation errors
        /// that will be displayed in the error summary if present.
        /// </summary>
        [HtmlAttributeName("for")]
        public BaseViewModel Model { get; set; } = default!;

        /// <summary>
        /// The HTTP method used for the form submission (e.g., GET, POST). Defaults to "post".
        /// </summary>
        [HtmlAttributeName("method")]
        public string Method { get; set; } = "post";

        /// <summary>
        /// The action URL for the form submission.
        /// </summary>
        [HtmlAttributeName("action")]
        public string Action { get; set; } = default!;

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            if (Model == null)
            {
                throw new InvalidOperationException("The model cannot be null.");
            }

#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
            var childContent = output.Content.IsModified ? output.Content.GetContent() :
            (await output.GetChildContentAsync()).GetContent();
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task

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
