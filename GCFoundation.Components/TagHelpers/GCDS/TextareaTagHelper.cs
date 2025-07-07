using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GCFoundation.Components.TagHelpers.GCDS
{
    /// <summary>
    /// Represents a tag helper for rendering a textarea input element with customizable properties like label, row count, and character count.
    /// </summary>
    [HtmlTargetElement("gcds-textarea")]
    public class TextareaTagHelper : BaseFormComponentTagHelper
    {
        /// <summary>
        /// Gets or sets the label for the textarea input element. This field is required.
        /// </summary>
        public required string Label { get; set; }

        /// <summary>
        /// Gets or sets the ID for the textarea element. This field is required.
        /// </summary>
        public required string TextareaId { get; set; }

        /// <summary>
        /// Gets or sets the character count for the textarea. This value determines the maximum number of characters.
        /// </summary>
        public int CharacterCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to hide the label for the textarea input element. Default is <c>false</c>.
        /// </summary>
        public bool HideLabel { get; set; }

        /// <summary>
        /// Gets or sets the number of rows for the textarea element. This controls the visible height of the textarea.
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// Processes the tag helper by adding the relevant attributes to the output based on the properties.
        /// </summary>
        /// <param name="context">The context of the tag helper.</param>
        /// <param name="output">The output to which the attributes will be added.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "label", Label);
            AddAttributeIfNotNull(output, "textarea-id", TextareaId);
            AddAttributeIfNotNull(output, "character-count", CharacterCount);
            AddAttributeIfNotNull(output, "hide-label", HideLabel);
            AddAttributeIfNotNull(output, "rows", Rows);

            base.Process(context, output);
        }
    }
}
