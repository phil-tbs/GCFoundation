using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.GCDS
{
    /// <summary>
    /// A tag helper for rendering a single checkbox using the gcds-checkboxes component.
    /// </summary>
    [HtmlTargetElement("gcds-checkboxes")]
    public class CheckboxesTagHelper : BaseFormComponentTagHelper
    {

        /// <summary>
        /// The label for the checkbox element.
        /// </summary>
        public required string Legend { get; set; }

        /// <summary>
        /// Gets or sets the options for the checkboxes, provided as a JSON string.
        /// </summary>
        public required string Options { get; set; }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));
            output.TagName = "gcds-checkboxes";

            AddAttributeIfNotNull(output, "Legend", Legend);
            AddAttributeIfNotNull(output, "value", Value);
            AddAttributeIfNotNull(output, "hint", Hint);

            AddAttributeIfNotNull(output, "options", Options);
            AddAttributeIfNotNull(output, "name", Name);

            //base.Process(context, output);
        }
    }
}
