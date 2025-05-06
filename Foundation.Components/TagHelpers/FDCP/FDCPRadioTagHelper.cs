using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Foundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// Renders a custom radio button component wrapped in a `gcds-fieldset` element.
    /// Use &lt;fdcp-radio&gt; in your Razor views to generate a radio button group.
    /// </summary>
    [HtmlTargetElement("fdcp-radio", Attributes = "for, items")]
    public class FDCPRadioTagHelper : FDCPBaseFormComponentTagHelper
    {
        /// <summary>
        /// List of options to display in the radio button group.
        /// </summary>
        [HtmlAttributeName("items")]
        public IEnumerable<SelectListItem> Items { get; set; } = new List<SelectListItem>();

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            // Call base class to handle validation messages if needed
            base.Process(context, output);
            if (For == null)
            {
                throw new InvalidOperationException("For is NULL in FDCPRadioTagHelper.");
            }

            string fieldName = For.Name;
            string fieldValue = For.Model?.ToString() ?? string.Empty; // Retrieve the selected value

            PropertyInfo? propertyInfo = this.PropertyInfo;

            if (propertyInfo == null)
            {
                throw new InvalidOperationException("Missing proprities");
            }

            string label = GetLocalizedLabel(propertyInfo);
            string hint = GetLocalizedHint(propertyInfo);

            output.TagName = "gcds-fieldset"; // Wrap everything in gcds-fieldset
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("fieldset-id", fieldName);
            output.Attributes.SetAttribute("legend", label);
            output.Attributes.SetAttribute("hint", hint);

            var optionsList = Items.Select(item => new
            {
                label = item.Text,
                id = $"{fieldName}_{item.Value}",
                value = item.Value,
                hint = ""
            }).ToList();

            string optionsJson = JsonSerializer.Serialize(optionsList, CamelCaseOptions);

            var sb = new StringBuilder();
            sb.AppendLine(CultureInfo.InvariantCulture,$@"<gcds-radio-group name=""{fieldName}"" options='{optionsJson}'></gcds-radio-group>");

            output.Content.SetHtmlContent(sb.ToString());

            
        }
    }
}
