using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GCFoundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// Renders a custom dropdown (select) component.
    /// Use &lt;fdcp-select&gt; in your Razor views to generate a dropdown list.
    /// </summary>
    [HtmlTargetElement("fdcp-select", Attributes = "for, items")]
    public class FDCPSelectTagHelper : FDCPBaseFormComponentTagHelper
    {
        /// <summary>
        /// The list of selectable options for the dropdown.
        /// </summary>
        [HtmlAttributeName("items")]
        public IEnumerable<SelectListItem> Items { get; set; } = new List<SelectListItem>();

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            // Call base class to handle label, hint, and errors
            base.Process(context, output);

            output.TagName = "gcds-select"; // Render as `<select>`
            output.Attributes.SetAttribute("name", For.Name);
            output.Attributes.SetAttribute("select-id", For.Name);
            output.Attributes.SetAttribute("class", "gcds-select");
            output.Attributes.SetAttribute("default-value", "Select option");

            var sb = new StringBuilder();

            // Generate the dropdown options dynamically
            foreach (var item in Items)
            {
                var selected = For.Model?.ToString() == item.Value ? " selected" : "";
                sb.AppendLine(CultureInfo.InvariantCulture, $"<option value='{item.Value}'{selected}>{item.Text}</option>");
            }

            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
