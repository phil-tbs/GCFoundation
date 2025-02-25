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
    [HtmlTargetElement("fdcp-checkboxes", Attributes = "for, items")]
    public class FDCPCheckboxes : FDCPBaseFormComponentTagHelper
    {
        [HtmlAttributeName("items")]
        public IEnumerable<SelectListItem> Items { get; set; } = new List<SelectListItem>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (For == null)
            {
                throw new InvalidOperationException("For is NULL in FDCPCheckboxes.");
            }

            string fieldName = For.Name;
            string label = GetLocalizedLabel(For.Metadata.ContainerType.GetProperty(For.Metadata.PropertyName));
            string hint = GetLocalizedHint(For.Metadata.ContainerType.GetProperty(For.Metadata.PropertyName));

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

                sb.Append($@"<gcds-checkbox checkbox-id=""{fieldName}_{item.Value}""
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
