using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Foundation.Components.Models.FormBuilder
{
    /// <summary>
    /// Represents a dependency relationship between form questions, 
    /// specifying an action to take when a target question has a specific value.
    /// </summary>
    public class QuestionDependency
    {
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
        [JsonConverter(typeof(StringEnumConverter))]  
        public DependencyAction Action { get; set; }
    }


    /// <summary>
    /// Defines the possible actions that can be taken when a question dependency is triggered.
    /// </summary>
    public enum DependencyAction
    {
        /// <summary>
        /// Show the dependent question.
        /// </summary>
        Show,

        /// <summary>
        /// Hide the dependent question.
        /// </summary>
        Hide,

        /// <summary>
        /// Make the dependent question required.
        /// </summary>
        Require,

        /// <summary>
        /// Remove the dependent question.
        /// </summary>
        Remove
    }

}
