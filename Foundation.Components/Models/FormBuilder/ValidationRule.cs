using System.Text.RegularExpressions;

namespace Foundation.Components.Models.FormBuilder
{
    /// <summary>
    /// Represents a validation rule for a form question.
    /// </summary>
    public class ValidationRule
    {
        /// <summary>
        /// Gets or sets the type of validation rule.
        /// </summary>
        public ValidationRuleType Type { get; set; }

        /// <summary>
        /// Gets or sets the regex pattern for custom validation.
        /// Only applicable when Type is Regex.
        /// </summary>
        public string? Pattern { get; set; }

        /// <summary>
        /// Gets or sets the minimum value for numeric validation.
        /// Only applicable when Type is MinValue or MinLength.
        /// </summary>
        public decimal? Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum value for numeric validation.
        /// Only applicable when Type is MaxValue or MaxLength.
        /// </summary>
        public decimal? Max { get; set; }

        /// <summary>
        /// Gets or sets the localized error messages.
        /// Key is the language code (e.g., "en", "fr"), value is the error message.
        /// </summary>
        public Dictionary<string, string> ErrorMessages { get; set; } = new();

        /// <summary>
        /// Validates the provided value against this rule.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if validation passes, false otherwise.</returns>
        public bool Validate(string? value)
        {
            if (string.IsNullOrEmpty(value) && Type != ValidationRuleType.Required)
                return true;

            return Type switch
            {
                ValidationRuleType.Required => !string.IsNullOrWhiteSpace(value),
                ValidationRuleType.Regex => !string.IsNullOrEmpty(Pattern) && 
                                          Regex.IsMatch(value ?? "", Pattern),
                ValidationRuleType.Email => Regex.IsMatch(value ?? "", 
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"),
                ValidationRuleType.MinLength => (value?.Length ?? 0) >= Min,
                ValidationRuleType.MaxLength => (value?.Length ?? 0) <= Max,
                ValidationRuleType.MinValue => decimal.TryParse(value, out var num) && 
                                             num >= Min,
                ValidationRuleType.MaxValue => decimal.TryParse(value, out var num) && 
                                             num <= Max,
                _ => true
            };
        }

        /// <summary>
        /// Gets the error message for the specified language.
        /// </summary>
        /// <param name="language">The language code (e.g., "en", "fr").</param>
        /// <returns>The localized error message or a default message if not found.</returns>
        public string GetErrorMessage(string language)
        {
            return ErrorMessages.TryGetValue(language.ToLowerInvariant(), out var message)
                ? message
                : ErrorMessages.TryGetValue("en", out var defaultMessage)
                    ? defaultMessage
                    : "Invalid value";
        }
    }

    /// <summary>
    /// Defines the types of validation rules available.
    /// </summary>
    public enum ValidationRuleType
    {
        Required,
        Regex,
        Email,
        MinLength,
        MaxLength,
        MinValue,
        MaxValue
    }
} 