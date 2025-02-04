using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    public class FileUploadTagHelper : BaseFromComponentTagHelper
    {
        public required string Label { get; set; }

        public required string UploaderId { get; set; }

        public string? Accept { get; set; }

        public bool Multiple { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
        }
    }
}
