namespace GCFoundation.Components.Models.FormBuilder
{
    /// <summary>
    /// Represents a dependency relationship between form questions, 
    /// specifying an action to take when a target question has a specific value.
    /// </summary>
    public class QuestionDependency
    {
        /// <summary>
        /// Gets or sets the ID of the question that triggers this dependency.
        /// </summary>
        public required string SourceQuestionId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the question that this dependency targets.
        /// </summary>
        public required string TargetQuestionId { get; set; }

        /// <summary>
        /// Gets or sets the value of the target question that triggers the dependency action.
        /// </summary>
        public required string TriggerValue { get; set; }

        /// <summary>
        /// Gets or sets the action to perform when the dependency is triggered.
        /// </summary>
        public DependencyAction Action { get; set; }

        /// <summary>
        /// Gets or sets the value to set when the <see cref="DependencyAction.SetValue"/> action is triggered.
        /// </summary>
        public string? SetValue { get; set; }
    }


    /// <summary>
    /// Defines the possible actions that can be taken when a question dependency is triggered.
    /// </summary>
    public enum DependencyAction
    {
        /// <summary>
        /// Make the dependent question required.
        /// </summary>
        Require,
        /// <summary>
        /// Show the dependent question.
        /// </summary>
        Show,
        /// <summary>
        /// Hide the dependent question.
        /// </summary>
        Hide,
        /// <summary>
        /// Enable the dependent question.
        /// </summary>
        Enable,
        /// <summary>
        /// Disable the dependent question.
        /// </summary>
        Disable,
        /// <summary>
        /// Clear the value of the dependent question.
        /// </summary>
        ClearValue,
        /// <summary>
        /// Set the value of the dependent question.
        /// </summary>
        SetValue
    }

}
