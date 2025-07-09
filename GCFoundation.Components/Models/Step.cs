using System;

namespace GCFoundation.Components.Models
{
    /// <summary>  
    /// Defines display modes for steps in a stepper component.  
    /// </summary>  
    public enum StepDisplayMode
    {
        /// <summary>  
        /// Display the step as a number.  
        /// </summary>  
        Number,

        /// <summary>  
        /// Display the step as an icon.  
        /// </summary>  
        Icon
    }

    /// <summary>
    /// Represents a step within a stepper component that can display numbers or icons
    /// and tracks the step's state and display properties.
    /// </summary>
    public class Step
    {
        /// <summary>
        /// Gets or sets the numerical position of the step in the sequence.
        /// </summary>
        public int StepNumber { get; set; }

        /// <summary>
        /// Gets or sets the text label describing the step.
        /// </summary>
        public required string Label { get; set; }

        /// <summary>
        /// Gets or sets whether the step should be hidden from display.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets how the step should be displayed (as a number or icon).
        /// Defaults to Number display mode.
        /// </summary>
        public StepDisplayMode DisplayMode { get; set; } = StepDisplayMode.Number;

        /// <summary>
        /// Gets or sets the HTML for the FontAwesome icon to display when the step is completed.
        /// Example: "&lt;i class='fa fa-check'&gt;&lt;/i&gt;"
        /// </summary>
        public string? CompletedIconHtml { get; set; }

        /// <summary>
        /// Gets or sets the HTML for the FontAwesome icon to display when the step is in progress.
        /// </summary>
        public string? InProgressIconHtml { get; set; }

        /// <summary>
        /// Gets or sets the HTML for the FontAwesome icon to display when the step hasn't been started.
        /// </summary>
        public string? NotStartedIconHtml { get; set; }

        /// <summary>
        /// Gets or sets the URL for navigation when the step is clicked.
        /// </summary>
        public string? LinkUrl { get; set; }

        /// <summary>
        /// Gets or sets whether this step should be rendered as a clickable link.
        /// </summary>
        public bool IsLink { get; set; }

        /// <summary>
        /// Generates the HTML display content for the step based on its current state and display mode.
        /// </summary>
        /// <param name="currentStep">The current active step number in the sequence.</param>
        /// <returns>HTML string representing the step's display content. Returns empty string if the step is hidden.</returns>
        public string GetDisplayHtml(int currentStep)
        {
            if (IsHidden)
                return string.Empty;

            if (DisplayMode == StepDisplayMode.Icon)
            {
                return GetStatusByCurrentStep(currentStep) switch
                {
                    "completed" => !string.IsNullOrEmpty(CompletedIconHtml) ? CompletedIconHtml : StepNumber.ToString(),
                    "active" => !string.IsNullOrEmpty(InProgressIconHtml) ? InProgressIconHtml : StepNumber.ToString(),
                    _ => !string.IsNullOrEmpty(NotStartedIconHtml) ? NotStartedIconHtml : StepNumber.ToString()
                };
            }
            return StepNumber.ToString();
        }

        /// <summary>
        /// Determines the status of this step relative to the current step in the sequence.
        /// </summary>
        /// <param name="currentStep">The current active step number in the sequence.</param>
        /// <returns>
        /// Returns one of three status strings:
        /// - "completed": for steps before the current step
        /// - "active": for the current step
        /// - "incomplete": for steps after the current step
        /// </returns>
        public string GetStatusByCurrentStep(int currentStep)
        {
            if (StepNumber < currentStep)
                return "completed";
            else if (StepNumber == currentStep)
                return "active";
            else
                return "incomplete";
        }
    }
}
