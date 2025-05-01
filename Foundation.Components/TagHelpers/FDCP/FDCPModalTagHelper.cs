using System.Text;
using Foundation.Components.Enum;
using Foundation.Components.Resources;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// Renders a Bootstrap 5 modal component.
    /// Use <fdcp-modal> in your Razor views to generate a modal dialog.
    /// </summary>
    [HtmlTargetElement("fdcp-modal")]
    public class FDCPModalTagHelper : TagHelper
    {
        /// <summary>
        /// The ID of the modal element. Must be unique on the page.
        /// </summary>
        public string Id { get; set; } = "modal";

        /// <summary>
        /// The title displayed in the modal header.
        /// </summary>
        public string Title { get; set; } = "Modal Title";

        /// <summary>
        /// Centers the modal vertically in the viewport.
        /// </summary>
        public bool Centered { get; set; } = true;

        /// <summary>
        /// Makes the modal body content scrollable if the content overflows.
        /// </summary>
        public bool Scrollable { get; set; } = false;

        /// <summary>
        /// Sets the size of the modal. Default, Small, or Large.
        /// </summary>
        public ModalSize Size { get; set; } = ModalSize.Default;

        /// <summary>
        /// Determines if a close ("×") button is shown in the modal header.
        /// </summary>
        public bool ShowCloseButton { get; set; } = true;

        public bool IsStaticBackdrop { get; set; } = false;

        /// <summary>
        /// Processes the <bootstrap-modal> tag and renders a Bootstrap 5 modal HTML structure.
        /// </summary>
        /// <param name="context">The context of the TagHelper execution.</param>
        /// <param name="output">The output to write the rendered HTML to.</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "modal fade");
            output.Attributes.SetAttribute("id", Id);
            output.Attributes.SetAttribute("tabindex", "-1");
            output.Attributes.SetAttribute("aria-labelledby", $"{Id}Label");
            output.Attributes.SetAttribute("aria-hidden", "true");
            if (IsStaticBackdrop)
            {
                output.Attributes.SetAttribute("data-bs-backdrop", "static");
            }

            var dialogClasses = "modal-dialog";
            if (Centered) dialogClasses += " modal-dialog-centered";
            if (Scrollable) dialogClasses += " modal-dialog-scrollable";
            if (Size == ModalSize.Small) dialogClasses += " modal-sm";
            else if (Size == ModalSize.Large) dialogClasses += " modal-lg";

            var childContent = await output.GetChildContentAsync();
            var html = childContent.GetContent();

            var sb = new StringBuilder();
            sb.AppendLine($"<div class='{dialogClasses}' role='document'>");
            sb.AppendLine("  <div class='modal-content'>");

            sb.AppendLine("    <div class='modal-header'>");
            sb.AppendLine($"      <h5 class='modal-title' id='{Id}Label'>{Title}</h5>");
            if (ShowCloseButton)
            {
                sb.AppendLine($"      <button type='button' class='btn-close' data-bs-dismiss='modal' aria-label='{Modal.Modal_Close}'></button>");
            }
            sb.AppendLine("    </div>");

            sb.AppendLine(html);

            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            output.Content.SetHtmlContent(sb.ToString());

            await Task.CompletedTask;
        }
    }
}
