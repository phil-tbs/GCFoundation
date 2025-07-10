using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;
using System.Text;
using GCFoundation.Components.Models;

namespace GCFoundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// A tag helper that renders a step indicator/progress component for multi-step processes.
    /// Displays numbered steps with labels and indicates completed, active, and upcoming steps.
    /// </summary>
    /// <remarks>
    /// Usage example:
    /// <code>
    /// &lt;fdcp-stepper current-step=&quot;2&quot; steps=&quot;@(new[] { new Step { StepNumber = 1, Status = StepStatus.Completed }, new Step { StepNumber = 2, Status = StepStatus.InProgress }, new Step { StepNumber = 3, Status = StepStatus.NotStarted } })&quot;&gt;
    /// &lt;/fdcp-stepper&gt;
    /// </code>
    /// </remarks>
    [HtmlTargetElement("fdcp-stepper")]
    public class FDCPStepperTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the current active step number (1-based index).
        /// </summary>
        public int CurrentStep { get; set; } = 1;

        /// <summary>
        /// Gets or sets the collection of steps for the process.
        /// </summary>
        public IEnumerable<StepperStep> Steps { get; set; } = new List<StepperStep>();

        /// <summary>
        /// Processes the tag helper and generates the HTML output for the stepper component.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">The output that will be rendered by the tag helper.</param>
        /// <exception cref="ArgumentNullException">Thrown when output is null.</exception>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output);

            output.TagName = "div";
            var html = new StringBuilder();

            html.AppendLine("<gcds-heading tag='h2'>Current step</gcds-heading>");
            html.AppendLine("<div class='fdcp-stepper'>");

            foreach (var step in Steps)
            {
                if (step.IsHidden || string.IsNullOrWhiteSpace(step.Label))
                    continue;

                string stepClass = step.GetStatusByCurrentStep(CurrentStep);

                html.AppendLine(CultureInfo.InvariantCulture, $"<div class='fdcp-step {stepClass}'>");

                string circleContent = step.GetDisplayHtml(CurrentStep);

                if (step.IsLink && !string.IsNullOrEmpty(step.LinkUrl))
                {
                    html.AppendLine(CultureInfo.InvariantCulture, $"<div class='fdcp-step-circle'>{circleContent}</div>");
                    html.AppendLine(CultureInfo.InvariantCulture, $"<div class='fdcp-step-label'><gcds-link href='{step.LinkUrl}'>{step.Label}</gcds-link></div>");
                }
                else
                {
                    html.AppendLine(CultureInfo.InvariantCulture, $"<div class='fdcp-step-circle'>{circleContent}</div>");
                    html.AppendLine(CultureInfo.InvariantCulture, $"<div class='fdcp-step-label'>{step.Label}</div>");
                }

                html.AppendLine("</div>");
            }

            html.AppendLine("</div>");
            output.Content.SetHtmlContent(html.ToString());
        }
    }
}
