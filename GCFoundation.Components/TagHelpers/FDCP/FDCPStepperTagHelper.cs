using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;
using System.Text;

namespace GCFoundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// A tag helper that renders a step indicator/progress component for multi-step processes.
    /// Displays numbered steps with labels and indicates completed, active, and upcoming steps.
    /// </summary>
    /// <remarks>
    /// Usage example:
    /// <code>
    /// &lt;fdcp-stepper current-step=&quot;2&quot; step-labels=&quot;@(new[] { &quot;Personal Info&quot;, &quot;Review&quot;, &quot;Submit&quot; })&quot;&gt;
    /// &lt;/fdcp-stepper&gt;
    /// </code>
    /// </remarks>
    [HtmlTargetElement("fdcp-stepper")]
    public class FDCPStepperTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the current active step number (1-based index).
        /// Steps before this number are marked as completed, the current step is marked as active,
        /// and steps after this number are marked as incomplete.
        /// </summary>
        /// <value>Default value is 1</value>
        public int CurrentStep { get; set; } = 1;

        /// <summary>
        /// Gets or sets the collection of labels for each step in the process.
        /// The number of labels determines the total number of steps displayed.
        /// </summary>
        /// <value>An empty list by default</value>
        public IEnumerable<string> StepLabels { get; set; } = new List<string>();

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
