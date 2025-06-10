using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// Tag helper for rendering a single checkbox using the gcds-checkboxes component.
    /// </summary>
    [HtmlTargetElement("fdcp-checkbox")]
    public class FDCPCheckboxTagHelper : FDCPBaseFormComponentTagHelper
    {
        /// <summary>
        /// Gets or sets whether the checkbox is required.
        /// </summary>
        [HtmlAttributeName("required")]
        public bool IsRequired { get; set; }

        private class CheckboxOption
        {
            [JsonPropertyName("id")]
            public string Id { get; set; } = string.Empty;

            [JsonPropertyName("label")]
            public string Label { get; set; } = string.Empty;

            [JsonPropertyName("value")]
            public string Value { get; set; } = string.Empty;

            [JsonPropertyName("checked")]
            public bool Checked { get; set; }

            [JsonPropertyName("hint")]
            public string? Hint { get; set; }
        }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            if (For == null)
            {
                throw new InvalidOperationException("For is NULL in FDCPCheckbox.");
            }

            string fieldName = For.Name;
            PropertyInfo? propertyInfo = this.PropertyInfo;

            if (propertyInfo == null)
            {
                throw new InvalidOperationException("Missing properties");
            }

            string label = GetLocalizedLabel(propertyInfo);
            string hint = GetLocalizedHint(propertyInfo);

            // Get the current value
            var currentValue = For.Model as bool? ?? false;

            output.TagName = "gcds-checkboxes";
            output.TagMode = TagMode.StartTagAndEndTag;

            // Create the single checkbox option
            var option = new CheckboxOption
            {
                Id = fieldName,
                Label = label,
                Value = "true",
                @Checked = currentValue,
                Hint = hint
            };

            // Set the required attributes
            AddAttributeIfNotNull(output, "legend", label);
            AddAttributeIfNotNull(output, "name", fieldName);
            AddAttributeIfNotNull(output, "options", JsonSerializer.Serialize(new[] { option }, CamelCaseOptions));

            if (IsRequired)
            {
                output.Attributes.SetAttribute("required", "");
            }

            // Clear the content since we're using options attribute
            output.Content.SetHtmlContent(string.Empty);
        }
    }
}
