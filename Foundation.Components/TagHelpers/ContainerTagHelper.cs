using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-container")]
    public class ContainerTagHelper : BaseTagHelper
    {
        public bool Border { get; set; }

        public bool Centered { get; set; }

        public bool MainContainer { get; set; }


        public string? Margin { get; set; }

        public string? Padding { get; set; } = "300";

        public SizeTypeEmum Size { get; set; } = SizeTypeEmum.lg;

        public string? Tag {  get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "border", Border);
            AddAttributeIfNotNull(output, "centered", Centered);
            AddAttributeIfNotNull(output, "main-container", MainContainer);
            AddAttributeIfNotNull(output, "margin", Margin);
            AddAttributeIfNotNull(output, "padding", Padding);
            AddAttributeIfNotNull(output, "size", Size);
            AddAttributeIfNotNull(output, "tag", Tag);
            base.Process(context, output);
        }

    }
}
