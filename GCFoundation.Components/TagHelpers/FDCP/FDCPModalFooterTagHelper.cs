using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GCFoundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// A tag helper for rendering the footer of a modal dialog.
    /// This tag helper generates a <c>&lt;div&gt;</c> element with the class "fdcp-modal__footer".
    /// The content inside the modal footer is determined by the child content within the tag in the Razor view.
    /// </summary>
    [HtmlTargetElement("fdcp-modal-footer")]
    public class FDCPModalFooterTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the alignment of the modal footer content.
        /// Defaults to <see cref="ModalFooterAlign.Right"/>.
        /// </summary>
        /// <value>
        /// The alignment of the footer content. Can be Left, Center, or Right.
        /// </value>
        public ModalFooterAlign Align { get; set; } = ModalFooterAlign.Right;

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

#pragma warning disable CA2208 // Instantiate argument exceptions correctly
            string alignClass = Align switch
            {
                ModalFooterAlign.Left => "left",
                ModalFooterAlign.Center => "center",
                ModalFooterAlign.Right => "right",
                _ => throw new ArgumentOutOfRangeException(nameof(Align), Align, null) 
            };
#pragma warning restore CA2208 // Instantiate argument exceptions correctly

            output.Attributes.Add("class", $"fdcp-modal__footer {alignClass}");

            // Razor context: ConfigureAwait(false) is not safe here
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
            var childContent = await output.GetChildContentAsync();
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
            var html = childContent.GetContent();

            output.Content.SetHtmlContent(html);
        }
    }

    /// <summary>
    /// Specifies the alignment options for the modal footer content.
    /// </summary>
    public enum ModalFooterAlign
    {
        /// <summary>
        /// Aligns the footer content to the left.
        /// </summary>
        Left,

        /// <summary>
        /// Aligns the footer content to the center.
        /// </summary>
        Center,

        /// <summary>
        /// Aligns the footer content to the right.
        /// </summary>
        Right
    }
}
