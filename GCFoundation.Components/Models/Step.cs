using System;

namespace GCFoundation.Components.Models
{
    public enum StepDisplayMode
    {
        Number,
        Icon
    }

    public class Step
    {
        public int StepNumber { get; set; }
        public required string Label { get; set; }
        public bool IsHidden { get; set; }
        public StepDisplayMode DisplayMode { get; set; } = StepDisplayMode.Number;

        // FontAwesome icon HTML (e.g., "<i class='fa fa-check'></i>")
        public string? CompletedIconHtml { get; set; }
        public string? InProgressIconHtml { get; set; }
        public string? NotStartedIconHtml { get; set; }

        // Link for navigation
        public string? LinkUrl { get; set; }
        public bool IsLink { get; set; }


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
