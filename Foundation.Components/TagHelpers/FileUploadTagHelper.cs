using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    /// <summary>
    /// Represents a custom tag helper for rendering a file upload input field.
    /// This tag helper generates a file input element with support for labels, file type filtering, multiple file uploads, and more.
    /// </summary>
    public class FileUploadTagHelper : BaseFormComponentTagHelper
    {
        /// <summary>
        /// The label to display next to the file upload input field.
        /// </summary>
        public required string Label { get; set; }

        /// <summary>
        /// The unique identifier for the file upload input element.
        /// </summary>
        public required string UploaderId { get; set; }

        /// <summary>
        /// A comma-separated list of allowed file types (e.g., "image/png, image/jpeg").
        /// If not specified, any file type can be uploaded.
        /// </summary>
        public string? Accept { get; set; }

        /// <summary>
        /// Specifies whether the file input allows multiple files to be selected.
        /// If true, the user can select more than one file at once.
        /// Default is false (single file upload).
        /// </summary>
        public bool Multiple { get; set; }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
        }
    }
}
