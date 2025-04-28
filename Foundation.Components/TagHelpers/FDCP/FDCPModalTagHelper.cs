using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
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

        /// <summary>
        /// Processes the <bootstrap-modal> tag and renders a Bootstrap 5 modal HTML structure.
        /// </summary>
        /// <param name="context">The context of the TagHelper execution.</param>
        /// <param name="output">The output to write the rendered HTML to.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div"; // Replace <bootstrap-modal> with <div>
            output.Attributes.SetAttribute("class", "modal fade");
            output.Attributes.SetAttribute("id", Id);
            output.Attributes.SetAttribute("tabindex", "-1");
            output.Attributes.SetAttribute("role", "dialog");
            output.Attributes.SetAttribute("aria-labelledby", $"{Id}Label");
            output.Attributes.SetAttribute("aria-hidden", "true");

            var dialogClasses = "modal-dialog";
            if (Centered) dialogClasses += " modal-dialog-centered";
            if (Scrollable) dialogClasses += " modal-dialog-scrollable";

            switch (Size)
            {
                case ModalSize.Small:
                    dialogClasses += " modal-sm";
                    break;
                case ModalSize.Large:
                    dialogClasses += " modal-lg";
                    break;
                case ModalSize.Default:
                default:
                    break;
            }

            var innerHtml = $@"
                <div class='{dialogClasses}' role='document'>
                    <div class='modal-content'>
                        <div class='modal-header'>
                            <h5 class='modal-title' id='{Id}Label'>{Title}</h5>";

                            if (ShowCloseButton)
                            {
                                innerHtml += @"
                            <button type='button' class='btn-close' data-bs-dismiss='modal' aria-label='Close'></button>";
                            }

                            innerHtml += @"
                        </div>
                        <div class='modal-body'>";

                            // Add whatever is inside the <bootstrap-modal> tag
                            innerHtml += output.GetChildContentAsync().Result.GetContent();

                            innerHtml += @"
                        </div>
                        <div class='modal-footer'>
                            <button type='button' class='btn btn-secondary' data-bs-dismiss='modal'>Close</button>
                        </div>
                    </div>
                </div>";

            output.Content.SetHtmlContent(innerHtml);
        }
    }
}
