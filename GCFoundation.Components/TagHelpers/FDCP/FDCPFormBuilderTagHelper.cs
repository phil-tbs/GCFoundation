using System.Globalization;
using System.Text;
using GCFoundation.Common.Utilities;
using GCFoundation.Components.Models.FormBuilder;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace GCFoundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// TagHelper for rendering a dynamic form builder using the GC Design System components.
    /// Generates form markup based on the provided <see cref="FormDefinition"/> model.
    /// </summary>
    [HtmlTargetElement("fdcp-form-builder", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FDCPFormBuilderTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the form definition used to generate the form UI.
        /// </summary>
        public required FormDefinition Form { get; set; }

        /// <summary>
        /// Options for serializing JSON property names in camel case.
        /// </summary>
        private static readonly JsonSerializerSettings CamelCaseSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = { new StringEnumConverter(new CamelCaseNamingStrategy()) }
        };

        /// <summary>
        /// Options for serializing enums as integers.
        /// </summary>
        private static readonly JsonSerializerSettings DependencySerializerSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        /// <summary>
        /// Processes the tag helper and generates the HTML output for the form builder.
        /// </summary>
        /// <param name="context">The context for the tag helper.</param>
        /// <param name="output">The output for the tag helper.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            output.TagName = "div";
            output.Attributes.SetAttribute("class", "gc-form");

            var content = new StringBuilder();
            BuildFormContent(content);
            output.Content.SetHtmlContent(content.ToString());
        }

        private void BuildFormContent(StringBuilder content)
        {
            // Form wrapper
            content.AppendFormat(CultureInfo.InvariantCulture,
                "<form action='{0}' method='{1}' class='gc-form'>",
                Form.Action, Form.Methode);

            // Error summary component
            content.AppendLine("<gcds-error-summary></gcds-error-summary>");

            // Form sections
            foreach (var section in Form.Sections)
            {
                content.AppendLine(CultureInfo.InvariantCulture, $@"<gcds-fieldset 
                    fieldset-id='{section.Title}' 
                    legend='{section.Title}' 
                    legend-size='h3' 
                    hint='{section.Hint}'>");

                foreach (var question in section.Questions)
                {
                    content.AppendLine(RenderQuestion(question));
                }

                content.AppendLine("</gcds-fieldset>");
            }

            // Submit button
            content.AppendLine(CultureInfo.InvariantCulture, $@"<gcds-button 
                type='submit' 
                button-role='primary'>
                {Form.SubmithButtonText}
            </gcds-button>");

            content.AppendLine("</form>");
        }

        private static string RenderQuestion(FormQuestion question)
        {
            string language = LanguageUtility.GetCurrentApplicationLanguage();
            string isRequired = question.IsRequired ? "required" : "";

            // Base attributes that all components should have
            string baseAttributes = $@"
                id='{question.Id}'
                lang='{language}'
                {isRequired}";

            // Add dependencies attribute if question has dependencies
            if (question.Dependencies?.Any() == true)
            {
                var serializedDeps = JsonConvert.SerializeObject(question.Dependencies, DependencySerializerSettings);
                baseAttributes += $@" data-dependencies='{serializedDeps}'";
            }

            // Add validation rules if present
            if (question.ValidationRules?.Any() == true)
            {
                var validationRules = question.ValidationRules.Select(rule => new
                {
                    type = rule.Type.ToString().ToLowerInvariant(),
                    pattern = rule.Pattern,
                    min = rule.Min,
                    max = rule.Max,
                    errorMessages = rule.ErrorMessages
                });
                var serializedRules = JsonConvert.SerializeObject(validationRules, CamelCaseSettings);
                baseAttributes += $@" data-validation-rules='{serializedRules}'";

                if (question.ValidateOnBlur)
                {
                    baseAttributes += @" validate-on-blur";
                }
            }

            // Common attributes for all input types
            string commonAttributes = $@"
                name='{question.Id}'
                label='{question.Label}'
                hint='{question.Hint}'
                {baseAttributes}";

            return $@"<div class='gc-form-group'>{question.Type switch
            {
                QuestionType.Text => $@"<gcds-input 
                    type='text'
                    input-id='{question.Id}'
                    {commonAttributes}>
                </gcds-input>",

                QuestionType.Email => $@"<gcds-input 
                    type='email'
                    input-id='{question.Id}'
                    {commonAttributes}>
                </gcds-input>",

                QuestionType.Password => $@"<gcds-input 
                    type='password'
                    input-id='{question.Id}'
                    {commonAttributes}>
                </gcds-input>",

                QuestionType.Url => $@"<gcds-input 
                    type='url'
                    input-id='{question.Id}'
                    {commonAttributes}>
                </gcds-input>",

                QuestionType.Number => $@"<gcds-input 
                    type='number'
                    input-id='{question.Id}'
                    {commonAttributes}>
                </gcds-input>",

                QuestionType.Radio => BuildRadioGroup(question, language, commonAttributes),

                QuestionType.Checkbox => BuildCheckboxes(question, language, commonAttributes),

                QuestionType.Dropdown => $@"<gcds-select
                    select-id='{question.Id}'
                    default-value='Select option'
                    {commonAttributes}>
                    {BuildOptions(question.Options)}
                </gcds-select>",

                QuestionType.TextArea => $@"<gcds-textarea 
                    textarea-id='{question.Id}'
                    rows='{question.Size ?? 3}'
                    {commonAttributes}>
                    {question.Value ?? ""}
                </gcds-textarea>",

                QuestionType.Date => $@"<gcds-date-input
                    legend='{question.Label}'
                    name='{question.Id}'
                    format='{question.Format ?? "full"}'
                    value='{question.Value ?? ""}'
                    {baseAttributes}>
                </gcds-date-input>",

                QuestionType.FileUpload => $@"<gcds-input 
                    type='file'
                    input-id='{question.Id}'
                    {commonAttributes}>
                </gcds-input>",

                _ => throw new ArgumentException($"Unsupported question type: {question.Type}")
            }}</div>";
        }

        private static string BuildRadioGroup(FormQuestion question, string lang, string commonAttributes)
        {
            // Convert options to the required format for gcds-radios
            var options = question.Options?.Select(option => new
            {
                id = $"{question.Id}_{option.Id}",
                label = option.Label,
                value = option.Value,
                //@checked = (option.Value?.ToString() == question.Value?.ToString()),
                //hint = option.Hint,
            });

            var optionsJson = JsonConvert.SerializeObject(options, CamelCaseSettings);

            return $@"<gcds-radios
                name='{question.Id}'
                legend='{question.Label}'
                legend-size='h3'
                options='{optionsJson}'
                {(question.IsRequired ? "required" : "")}
                {(!string.IsNullOrEmpty(question.ErrorMessage) ? $@"error-message=""{question.ErrorMessage}""" : "")}
                {(!string.IsNullOrEmpty(question.Hint) ? $@"hint=""{question.Hint}""" : "")}
                lang='{lang}'
                id='{question.Id}'>
            </gcds-radios>";
        }

        private static string BuildOptions(IEnumerable<QuestionOption>? options)
        {
            if (options == null) return string.Empty;

            var sb = new StringBuilder();
            foreach (var option in options)
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"<option value='{option.Value}'>{option.Label}</option>");
            }
            return sb.ToString();
        }

        private static string BuildCheckboxes(FormQuestion question, string lang, string commonAttributes)
        {
            ArgumentNullException.ThrowIfNull(question.Options, nameof(question.Options));

            // Convert selected values to array of strings for value attribute
            var selectedValues = question.Value is IEnumerable<object> values
                ? values.Select(v => v.ToString()).ToArray()
                : Array.Empty<string>();

            // Convert options to the required format for gcds-checkboxes
            var options = question.Options.Select(option => new
            {
                id = $"{question.Id}_{option.Id}",
                label = option.Label,
                value = option.Value,
                //@checked = selectedValues.Contains(option.Value?.ToString()),
                //hint = option.Hint,
            });

            var optionsJson = JsonConvert.SerializeObject(options, CamelCaseSettings);

            // For multiple checkboxes case
            return $@"<gcds-checkboxes
                name='{question.Id}'
                legend='{question.Label}'
                {(!string.IsNullOrEmpty(question.LegendSize) ? $@"legend-size=""{question.LegendSize}""" : "legend-size=\"h3\"")}
                options='{optionsJson}'
                {(question.IsRequired ? "required" : "")}
                {(!string.IsNullOrEmpty(question.ErrorMessage) ? $@"error-message=""{question.ErrorMessage}""" : "")}
                {(!string.IsNullOrEmpty(question.Hint) ? $@"hint=""{question.Hint}""" : "")}
                validate-on='blur'
                lang='{lang}'
                {commonAttributes}>
            </gcds-checkboxes>";
        }

        /// <summary>
        /// Returns an HTML attribute string if the value is not null; otherwise, returns an empty string.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        /// <returns>The attribute string or empty if value is null.</returns>
        private static string AttributeIfNotNull(string name, string? value)
            => value is not null ? $" {name}='{value}'" : string.Empty;
    }
}
