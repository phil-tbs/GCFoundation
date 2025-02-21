using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("fd-select", Attributes = "for, items")]
    public class FoundationSelectTagHelper : TagHelper
    {
        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; } = default!;

        [HtmlAttributeName("items")]
        public IEnumerable<SelectListItem> Items { get; set; } = new List<SelectListItem>();

        [ViewContext]
        public ViewContext ViewContext { get; set; } = default!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if (For == null)
            {
                output.SuppressOutput();
                return;
            }

            PropertyInfo property = For.Metadata.ContainerType.GetProperty(For.Metadata.PropertyName);
            if (property == null)
            {
                output.SuppressOutput();
                return;
            }

            string label = GetLocalizedLabel(property);
            string hint = GetLocalizedHint(property);

            output.TagName = "select";
            output.Attributes.SetAttribute("name", For.Name);
            output.Attributes.SetAttribute("id", For.Name);
            output.Attributes.SetAttribute("class", "gcds-select");
            output.Attributes.SetAttribute("","");

            // Generate options dynamically
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("<option value=''>" + For.Metadata.DisplayName + "</option>"); // Default placeholder

            foreach (var item in Items)
            {
                var selected = For.Model?.ToString() == item.Value ? " selected" : "";
                sb.AppendLine($"<option value='{item.Value}'{selected}>{item.Text}</option>");
            }

            output.Content.SetHtmlContent(sb.ToString());
        }

        private string GetLocalizedLabel(PropertyInfo property)
        {
            var displayAttr = property.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.GetName() ?? property.Name;
        }

        private string GetLocalizedHint(PropertyInfo property)
        {
            var displayAttr = property.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.GetDescription() ?? string.Empty;
        }
    }
}
