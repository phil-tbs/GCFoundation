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
        /// Only applicable when <see cref="Type"/> is <see cref="ValidationRuleType.Regex"/>.
        /// </summary>
        public string? Pattern { get; set; }

        /// <summary>
        /// Gets or sets the minimum value for numeric validation.
        /// Only applicable when <see cref="Type"/> is <see cref="ValidationRuleType.MinValue"/> or <see cref="ValidationRuleType.MinLength"/>.
        /// </summary>
        public decimal? Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum value for numeric validation.
        /// Only applicable when <see cref="Type"/> is <see cref="ValidationRuleType.MaxValue"/> or <see cref="ValidationRuleType.MaxLength"/>.
        /// </summary>
        public decimal? Max { get; set; }

        /// <summary>
        /// Gets or sets the localized error messages.
        /// The key is the language code (e.g., "en", "fr"), and the value is the error message.
        /// </summary>
        public Dictionary<string, string> ErrorMessages { get; } = new();

        /// <summary>
        /// Validates the provided value against this rule.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns><c>true</c> if validation passes; otherwise, <c>false</c>.</returns>
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
            ArgumentNullException.ThrowIfNull(language, nameof(language));
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
        /// <summary>
        /// The value is required.
        /// </summary>
        Required,
        /// <summary>
        /// The value must match a regular expression.
        /// </summary>
        Regex,
        /// <summary>
        /// The value must be a valid email address.
        /// </summary>
        Email,
        /// <summary>
        /// The value must have a minimum length.
        /// </summary>
        MinLength,
        /// <summary>
        /// The value must have a maximum length.
        /// </summary>
        MaxLength,
        /// <summary>
        /// The value must be greater than or equal to a minimum value.
        /// </summary>
        MinValue,
        /// <summary>
        /// The value must be less than or equal to a maximum value.
        /// </summary>
        MaxValue
    }
}