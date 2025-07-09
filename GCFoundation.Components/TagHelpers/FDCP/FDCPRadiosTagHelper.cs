using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Reflection;
using System.Text.Json;

namespace GCFoundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// Renders a custom radio button component wrapped in a `gcds-fieldset` element.
    /// Use &lt;fdcp-radio&gt; in your Razor views to generate a radio button group.
    /// </summary>
    [HtmlTargetElement("fdcp-radios", Attributes = "for, items")]
    public class FDCPRadiosTagHelper : FDCPBaseFormComponentTagHelper
    {
        /// <summary>
        /// The list of items to be rendered as checkboxes.
        /// Each item should have a text (label) and value (for the checkbox).
        /// </summary>
        [HtmlAttributeName("items")]
        public IEnumerable<SelectListItem> Items { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// Gets or sets whether the checkbox group is required.
        /// </summary>
        [HtmlAttributeName("required")]
        public bool IsRequired { get; set; }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            if (For == null)
            {
                throw new InvalidOperationException("For is NULL in FDCPCheckboxes.");
            }

            string fieldName = For.Name;
            PropertyInfo? propertyInfo = PropertyInfo;

            if (propertyInfo == null)
            {
                throw new InvalidOperationException("Missing properties");
            }

            string legend = GetLocalizedLabel(propertyInfo);
            string hint = GetLocalizedHint(propertyInfo);

            // Retrieve selected values (if any)
            var selectedValues = For.Model as List<string> ?? new List<string>();

            output.TagName = "gcds-radios";
            output.TagMode = TagMode.StartTagAndEndTag;

            // Convert SelectListItems to the required options format
            var options = Items.Select(item => new
            {
                id = $"{fieldName}_{item.Value}",
                label = item.Text,
                value = item.Value,
                @checked = selectedValues.Contains(item.Value),
            });

            AddAttributeIfNotNull(output, "name", fieldName);
            AddAttributeIfNotNull(output, "legend", legend);
            AddAttributeIfNotNull(output, "hint", hint);
            AddAttributeIfNotNull(output, "options", JsonSerializer.Serialize(options));

            if (IsRequired)
            {
                output.Attributes.SetAttribute("required", "");
            }

            // Clear the content since we're using the options attribute
            output.Content.SetHtmlContent(string.Empty);
        }
    }
}
