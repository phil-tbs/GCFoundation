using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-error-message")]
    public class ErrorMessageTagHelper : BaseTagHelper
    {
        public required string MessageId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "message-id", MessageId);
            base.Process(context, output);
        }
    }
}
