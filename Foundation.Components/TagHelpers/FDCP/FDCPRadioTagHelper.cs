using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.FDCP
{

    [HtmlTargetElement("fdcp-radio", Attributes = "for, items")]
    public class FDCPRadioTagHelper : FDCPBaseFormComponentTagHelper
    {

        [HtmlAttributeName("items")]
        public IEnumerable<SelectListItem> Items { get; set; } = new List<SelectListItem>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Call base class to handle validation messages if needed
            base.Process(context, output);
            if (For == null)
            {
                throw new InvalidOperationException("For is NULL in FDCPRadioTagHelper.");
            }

            string fieldName = For.Name;
            string fieldValue = For.Model?.ToString() ?? string.Empty; // Retrieve the selected value
            string label = GetLocalizedLabel(For.Metadata.ContainerType.GetProperty(For.Metadata.PropertyName));
            string hint = GetLocalizedHint(For.Metadata.ContainerType.GetProperty(For.Metadata.PropertyName));

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

            string optionsJson = JsonSerializer.Serialize(optionsList, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            var sb = new StringBuilder();
            sb.AppendLine($@"<gcds-radio-group name=""{fieldName}"" options='{optionsJson}'></gcds-radio-group>");

            output.Content.SetHtmlContent(sb.ToString());

            
        }
    }
}
