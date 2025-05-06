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

namespace Foundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// A tag helper for rendering a group of checkboxes within a fieldset. It binds to a model property
    /// and renders a list of checkbox elements based on the provided items. It also handles 
    /// validation, localization of labels and hints, and setting the selected values based on the model.
    /// </summary>
    [HtmlTargetElement("fdcp-checkboxes", Attributes = "for, items")]
    public class FDCPCheckboxesTagHelper : FDCPBaseFormComponentTagHelper
    {
        /// <summary>
        /// The list of items to be rendered as checkboxes.
        /// Each item should have a text (label) and value (for the checkbox).
        /// </summary>
        [HtmlAttributeName("items")]
        public IEnumerable<SelectListItem> Items { get; set; } = new List<SelectListItem>();

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            base.Process(context, output);

            if (For == null)
            {
                throw new InvalidOperationException("For is NULL in FDCPCheckboxes.");
            }

            string fieldName = For.Name;
            PropertyInfo? propertyInfo = this.PropertyInfo;

            if (propertyInfo == null)
            {
                throw new InvalidOperationException("Missing proprities");
            }

            string label = GetLocalizedLabel(propertyInfo);
            string hint = GetLocalizedHint(propertyInfo);

            // Retrieve selected values (if any)
            var selectedValues = For.Model as List<string> ?? new List<string>();

            output.TagName = "gcds-fieldset";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("fieldset-id", fieldName);
            output.Attributes.SetAttribute("legend", label);
            output.Attributes.SetAttribute("hint", hint);

            var sb = new StringBuilder();
            foreach (var item in Items)
            {
                bool isChecked = selectedValues.Contains(item.Value);

                sb.Append(CultureInfo.InvariantCulture, $@"<gcds-checkbox checkbox-id=""{fieldName}_{item.Value}""
                  label=""{item.Text}""
                  name=""{fieldName}""
                  value=""{item.Value}""
                  {(isChecked ? "checked" : "")} 
                ></gcds-checkbox>");
            }

            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
