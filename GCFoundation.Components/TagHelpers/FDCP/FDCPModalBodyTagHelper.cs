using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GCFoundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// A tag helper for rendering the body of a modal dialog. 
    /// This tag helper generates a <c>&lt;div&gt;</c> element with the class "modal-body".
    /// The content inside the modal body is determined by the child content within the tag in the Razor view.
    /// </summary>
    [HtmlTargetElement("fdcp-modal-body")]
    public class FDCPModalBodyTagHelper : TagHelper
    {
        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "fdcp-modal__body");

            // Razor context: ConfigureAwait(false) is not safe here
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
            var childContent = await output.GetChildContentAsync();
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
            var html = childContent.GetContent();

            output.Content.SetHtmlContent(html);

        }
    }
}
