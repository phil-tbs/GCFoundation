using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GCFoundation.Components.TagHelpers.GCDS
{
    /// <summary>
    /// Gets or sets the name of the radio group. This name is used to group the radio buttons and associate them together.
    /// </summary>
    [HtmlTargetElement("gcds-radios")]
    public class RadiosTagHelper : BaseFormComponentTagHelper
    {
        /// <summary>
        /// The label for the checkbox element.
        /// </summary>
        public required string Legend { get; set; }

        /// <summary>
        /// Gets or sets the options for the radio buttons, provided as a JSON string.
        /// </summary>
        public required string Options { get; set; }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));
            output.TagName = "gcds-radios";

            AddAttributeIfNotNull(output, "Legend", Legend);
            AddAttributeIfNotNull(output, "value", Value);
            AddAttributeIfNotNull(output, "hint", Hint);

            AddAttributeIfNotNull(output, "options", Options);
            AddAttributeIfNotNull(output, "name", Name);

            //base.Process(context, output);
        }
    }
}
