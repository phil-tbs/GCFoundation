﻿using GCFoundation.Components.Enums;
using GCFoundation.Components.Resources;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;
using System.Text;

namespace GCFoundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// Renders a standalone modal component.
    /// Use &lt;fdcp-modal&gt; in your Razor views to generate a modal dialog.
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
        public bool Scrollable { get; set; }

        /// <summary>
        /// Sets the size of the modal. Default, Small, or Large.
        /// </summary>
        public ModalSize Size { get; set; } = ModalSize.Default;

        /// <summary>
        /// Determines if a close ("×") button is shown in the modal header.
        /// </summary>
        public bool ShowCloseButton { get; set; } = true;

        /// <summary>
        /// Determines whether the modal will have a static backdrop (prevents closing by clicking outside the modal).
        /// </summary>
        public bool IsStaticBackdrop { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "fdcp-modal");
            output.Attributes.SetAttribute("id", Id);
            output.Attributes.SetAttribute("tabindex", "-1");
            output.Attributes.SetAttribute("aria-labelledby", $"{Id}Label");
            output.Attributes.SetAttribute("aria-hidden", "true");
            output.Attributes.SetAttribute("role", "dialog");
            
            if (IsStaticBackdrop)
            {
                output.Attributes.SetAttribute("data-static", "true");
            }

            var dialogClasses = new List<string> { "fdcp-modal__dialog" };
            if (Centered) dialogClasses.Add("fdcp-modal__dialog--centered");
            if (Scrollable) dialogClasses.Add("fdcp-modal__dialog--scrollable");
            if (Size == ModalSize.Small) dialogClasses.Add("fdcp-modal__dialog--sm");
            else if (Size == ModalSize.Large) dialogClasses.Add("fdcp-modal__dialog--lg");

            var childContent = await output.GetChildContentAsync().ConfigureAwait(true);
            var html = childContent.GetContent();

            var sb = new StringBuilder();
            sb.AppendLine("<div class='fdcp-modal__backdrop'></div>");
            sb.AppendLine(CultureInfo.InvariantCulture, $"<div class='{string.Join(" ", dialogClasses)}' role='document'>");
            sb.AppendLine("  <div class='fdcp-modal__content'>");
            sb.AppendLine("    <div class='fdcp-modal__header'>");
            sb.AppendLine(CultureInfo.InvariantCulture, $"      <h5 class='fdcp-modal__title' id='{Id}Label'>{Title}</h5>");
            if (ShowCloseButton)
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"      <button type='button' class='fdcp-modal__close' aria-label='{Modal.Modal_Close}'>&times;</button>");
            }
            sb.AppendLine("    </div>");
            sb.AppendLine(html);
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
