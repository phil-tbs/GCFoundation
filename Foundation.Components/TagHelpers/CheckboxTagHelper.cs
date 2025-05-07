using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    /// <summary>
    /// A tag helper for rendering a custom checkbox input component with a label in the application.
    /// </summary>
    [HtmlTargetElement("gcds-checkbox")]
    public class CheckboxTagHelper : BaseFormComponentTagHelper
    {
        /// <summary>
        /// The ID of the checkbox element.
        /// </summary>
        public required string CheckboxId { get; set; }

        /// <summary>
        /// The label for the checkbox element.
        /// </summary>
        public required string Label { get; set; }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "checkbox-id", CheckboxId);
            AddAttributeIfNotNull(output, "label", Label);


            base.Process(context, output);
        }
    }
}
