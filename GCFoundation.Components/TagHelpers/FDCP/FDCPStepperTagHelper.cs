using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCFoundation.Components.TagHelpers.FDCP
{
    [HtmlTargetElement("fdcp-stepper")]
    public class FDCPStepperTagHelper : TagHelper
    {

        public int CurrentStep { get; set; } = 1;


        public IEnumerable<string> StepLabels { get; set; } = new List<string>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output);

            output.TagName = "div";
            var html = new StringBuilder();

            html.AppendLine("<gcds-heading tag='h2'>Current step</gcds-heading>");
            html.AppendLine("<div class='fdcp-stepper'>");
            for (int i = 0; i < StepLabels.Count(); i++)
            {
                string label = StepLabels.ElementAt(i);
                string stepClass = (i + 1) < CurrentStep
                    ? "completed"
                    : (i + 1) == CurrentStep
                        ? "active"
                        : "incomplete";

                html.AppendLine(CultureInfo.InvariantCulture, $"<div class='fdcp-step {stepClass}'>");
                html.AppendLine(CultureInfo.InvariantCulture, $"<div class='fdcp-step-circle'>{(i + 1)}</div>");
                html.AppendLine(CultureInfo.InvariantCulture, $"<div class='fdcp-step-label'>{label}</div>");
                html.AppendLine("</div>");
            }
            html.AppendLine("</div>");
            output.Content.SetHtmlContent(html.ToString());
        }

    }
}
