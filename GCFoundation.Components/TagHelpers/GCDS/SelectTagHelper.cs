using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GCFoundation.Components.TagHelpers.GCDS
{
    /// <summary>
    /// TagHelper for rendering a GC Design System compliant select (dropdown) component.
    /// </summary>
    [HtmlTargetElement("gcds-select")]
    public class SelectTagHelper : BaseFormComponentTagHelper
    {
        /// <summary>
        /// Gets or sets the label text for the select component.
        /// </summary>
        public required string Label { get; set; }

        /// <summary>
        /// Gets or sets the unique ID for the select component.
        /// </summary>
        public required string SelectId { get; set; }

        /// <summary>
        /// Gets or sets the default selected value in the dropdown.
        /// </summary>
        public string? DefaultValue { get; set; }

        /// <inheritdoc />
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "label", Label);
            AddAttributeIfNotNull(output, "select-id", SelectId);
            AddAttributeIfNotNull(output, "default-value", DefaultValue);
            base.Process(context, output);
        }
    }
}
