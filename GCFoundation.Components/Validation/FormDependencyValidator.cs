using GCFoundation.Components.Models.FormBuilder;
using System.ComponentModel.DataAnnotations;

namespace GCFoundation.Components.Validation
{
    /// <summary>
    /// Validates form dependencies on the server side to ensure data integrity.
    /// </summary>
    public class FormDependencyValidator
    {
        /// <summary>
        /// The form definition containing sections and questions.
        /// </summary>
        private readonly FormDefinition _form;

        /// <summary>
        /// The dictionary containing form data keyed by question ID.
        /// </summary>
        private readonly Dictionary<string, object?> _formData;

        /// <summary>
        /// The list of validation results collected during validation.
        /// </summary>
        private readonly List<ValidationResult> _validationResults;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormDependencyValidator"/> class.
        /// </summary>
        /// <param name="form">The form definition to validate.</param>
        /// <param name="formData">The form data to validate against dependencies.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="form"/> or <paramref name="formData"/> is null.</exception>
        public FormDependencyValidator(FormDefinition form, Dictionary<string, object?> formData)
        {
            _form = form ?? throw new ArgumentNullException(nameof(form));
            _formData = formData ?? throw new ArgumentNullException(nameof(formData));
            _validationResults = new List<ValidationResult>();
        }

        /// <summary>
        /// Validates all form dependencies and returns validation results.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{ValidationResult}"/> containing any validation errors found.
        /// </returns>
        public IEnumerable<ValidationResult> Validate()
        {
            foreach (var section in _form.Sections)
            {
                foreach (var question in section.Questions)
                {
                    ValidateQuestionDependencies(question);
                }
            }

            return _validationResults;
        }

        /// <summary>
        /// Validates the dependencies for a specific question.
        /// </summary>
        /// <param name="question">The question to validate dependencies for.</param>
        private void ValidateQuestionDependencies(FormQuestion question)
        {
            if (question.Dependencies == null || !question.Dependencies.Any()) return;

            foreach (var dependency in question.Dependencies)
            {
                // Find the source question that triggers this dependency
                var sourceQuestion = FindQuestionById(dependency.SourceQuestionId);
                if (sourceQuestion == null) continue;

                // Get the current value of the source question
                var sourceValue = _formData.GetValueOrDefault(dependency.SourceQuestionId);
                var targetValue = _formData.GetValueOrDefault(dependency.TargetQuestionId);

                bool conditionMet = EvaluateCondition(dependency, sourceValue);

                // Validate based on the dependency action
                ValidateDependencyAction(dependency, conditionMet, targetValue, question);
            }
        }

        /// <summary>
        /// Finds a question in the form by its unique identifier.
        /// </summary>
        /// <param name="questionId">The ID of the question to find.</param>
        /// <returns>The <see cref="FormQuestion"/> if found; otherwise, <c>null</c>.</returns>
        private FormQuestion? FindQuestionById(string questionId)
        {
            return _form.Sections
                .SelectMany(s => s.Questions)
                .FirstOrDefault(q => q.Id == questionId);
        }

        /// <summary>
        /// Evaluates whether the dependency condition is met based on the source value.
        /// </summary>
        /// <param name="dependency">The dependency to evaluate.</param>
        /// <param name="sourceValue">The value of the source question.</param>
        /// <returns><c>true</c> if the condition is met; otherwise, <c>false</c>.</returns>
        private static bool EvaluateCondition(QuestionDependency dependency, object? sourceValue)
        {
            var triggerValue = dependency.TriggerValue;

            // Simple equals comparison since we don't have condition in the model anymore
            return string.Equals(sourceValue?.ToString(), triggerValue?.ToString(),
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Validates the dependency action and adds a validation result if the dependency is not satisfied.
        /// </summary>
        /// <param name="dependency">The dependency being validated.</param>
        /// <param name="conditionMet">Whether the dependency condition is met.</param>
        /// <param name="targetValue">The value of the target question.</param>
        /// <param name="question">The target question.</param>
        private void ValidateDependencyAction(QuestionDependency dependency, bool conditionMet,
            object? targetValue, FormQuestion question)
        {
            switch (dependency.Action)
            {
                case DependencyAction.Require when conditionMet &&
                    (targetValue == null || string.IsNullOrWhiteSpace(targetValue.ToString())):
                    _validationResults.Add(new ValidationResult(
                        $"The field {question.Label} is required based on your other answers.",
                        new[] { dependency.TargetQuestionId }));
                    break;

                case DependencyAction.Show when conditionMet &&
                    (targetValue == null || string.IsNullOrWhiteSpace(targetValue.ToString())):
                    // For 'show' dependencies, we might want to validate that the field has a value when shown
                    if (question.IsRequired)
                    {
                        _validationResults.Add(new ValidationResult(
                            $"The field {question.Label} is required when shown.",
                            new[] { dependency.TargetQuestionId }));
                    }
                    break;

                case DependencyAction.SetValue when conditionMet && dependency.SetValue != null &&
                    !string.Equals(targetValue?.ToString(), dependency.SetValue.ToString(),
                    StringComparison.OrdinalIgnoreCase):
                    _validationResults.Add(new ValidationResult(
                        $"The field {question.Label} must have the value {dependency.SetValue} based on your other answers.",
                        new[] { dependency.TargetQuestionId }));
                    break;
            }
        }
    }
}