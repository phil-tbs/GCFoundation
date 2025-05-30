using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Common.Utilities;
using Foundation.Components.Models.FormBuilder;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Foundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// TagHelper for rendering a dynamic form builder using the GC Design System components.
    /// Generates form markup based on the provided <see cref="FormDefinition"/> model.
    /// </summary>
    [HtmlTargetElement("fdcp-form-builder", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormBuilderTagHelper : TagHelper
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
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            Converters = { new Newtonsoft.Json.Converters.StringEnumConverter(new CamelCaseNamingStrategy()) }
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

            content.Append($"<form action='{Form.Action}' method='{Form.Methode}'>");

            // Add Errors summary
            content.Append("<gcds-error-summary></gcds-error-summary>");

            foreach (var section in Form.Sections)
            {
                content.AppendLine(CultureInfo.InvariantCulture, $"<gcds-fieldset fieldset-id='{section.Title}' legend='{section.Title}' hint='{section.Hint}'>");

                foreach (var question in section.Questions)
                {
                    content.AppendLine(BuildQuestionMarkup(question));
                }

                content.AppendLine("</gcds-fieldset>");
            }

            content.AppendLine("<div id='test-dynamic'></div>");
            content.Append($"<gcds-button type='submit'>{Form.SubmithButtonText}</gcds-button>");


            content.Append("</form>");

            output.Content.SetHtmlContent(content.ToString());
        }

        /// <summary>
        /// Builds the HTML markup for a single form question based on its type.
        /// </summary>
        /// <param name="question">The form question to render.</param>
        /// <returns>The HTML markup for the question.</returns>
        private static string BuildQuestionMarkup(FormQuestion question)
        {
            string language = LanguageUtility.GetCurrentApplicationLanguage();
            string isRequired = question.IsRequired ? "required" : "";
            string? hint = question.Hint;
            string label = question.Label;
            string questionId = question.Id;

            // Old:
            //string dependenciesAttr = question.Dependencies != null && question.Dependencies.Any()
            //    ? $" data-dependencies='{JsonSerializer.Serialize(question.Dependencies, CamelCaseOptions)}'"
            //    : "";
            // New:
            string dependenciesAttr = question.Dependencies != null && question.Dependencies.Any()
                ? $" data-dependencies='{JsonConvert.SerializeObject(question.Dependencies, CamelCaseSettings)}'"
                : "";


            return question.Type switch
            {
                QuestionType.Text or QuestionType.Email or QuestionType.Number or QuestionType.Url or QuestionType.Password
                    => BuildInput(question.Type.ToString().ToLowerInvariant(), questionId, label, hint, language, isRequired),

                QuestionType.Radio
                    => BuildRadioGroup(question, language, isRequired, dependenciesAttr),

                QuestionType.Checkbox
                    => BuildCheckboxes(question, language, isRequired, dependenciesAttr),

                QuestionType.Dropdown
                    => BuildDropdown(question, language, isRequired, dependenciesAttr),

                QuestionType.TextArea
                    => BuildTextArea(question, language, isRequired),

                QuestionType.Date
                    => BuildDateInput(question, language, isRequired),

                _ => string.Empty
            };
        }

        /// <summary>
        /// Builds the HTML markup for a standard input element.
        /// </summary>
        /// <param name="type">The input type (e.g., "text", "email").</param>
        /// <param name="id">The input ID.</param>
        /// <param name="label">The input label.</param>
        /// <param name="hint">The input hint (optional).</param>
        /// <param name="lang">The language code.</param>
        /// <param name="isRequired">The required attribute string.</param>
        /// <returns>The HTML markup for the input element.</returns>
        private static string BuildInput(string type, string id, string label, string? hint, string lang, string isRequired)
            => $"<gcds-input type='{type}' input-id='{id}' label='{label}' hint='{hint}' lang='{lang}' {isRequired}></gcds-input>";

        /// <summary>
        /// Builds the HTML markup for a radio group question.
        /// </summary>
        /// <param name="question">The form question to render as a radio group.</param>
        /// <param name="lang">The language code for the rendered markup.</param>
        /// <param name="isRequired">The required attribute string to indicate if the field is mandatory.</param>
        /// <param name="dependenciesAttr">
        /// The HTML attribute containing serialized dependency metadata, or an empty string if there are no dependencies.
        /// </param>
        /// <returns>The HTML markup for the radio group, including dependency metadata if applicable.</returns>
        private static string BuildRadioGroup(FormQuestion question, string lang, string isRequired, string dependenciesAttr)
            => $@"
                <gcds-radio-group
                    name='{question.Label}'
                    options='{JsonConvert.SerializeObject(question.Options, CamelCaseSettings)}'
                    lang='{lang}'
                    {isRequired}
                    {dependenciesAttr}
                >
                </gcds-radio-group>";

        /// <summary>
        /// Builds the HTML markup for a set of checkbox options.
        /// </summary>
        /// <param name="question">The form question.</param>
        /// <param name="lang">The language code.</param>
        /// <param name="isRequired">The required attribute string.</param>
        /// <param name="dependenciesAttr">
        /// The HTML attribute containing serialized dependency metadata, or an empty string if there are no dependencies.
        /// </param>
        /// <returns>The HTML markup for the checkboxes.</returns>
        private static string BuildCheckboxes(FormQuestion question, string lang, string isRequired, string dependenciesAttr)
        {
            ArgumentNullException.ThrowIfNull(question.Options, nameof(question.Options));
            var sb = new StringBuilder();
            foreach (var option in question.Options)
            {
                sb.AppendLine($@"
                    <gcds-checkbox
                        checkbox-id='{option.Id}'
                        label='{option.Label}'
                        name='{question.Id}'" +
                    AttributeIfNotNull("hint", option.Hint) +
                    $@" lang='{lang}'
                        {isRequired}
                        {dependenciesAttr}
                    >
                    </gcds-checkbox>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Builds the HTML markup for a dropdown/select question.
        /// </summary>
        /// <param name="question">The form question.</param>
        /// <param name="lang">The language code.</param>
        /// <param name="isRequired">The required attribute string.</param>
        /// <param name="dependenciesAttr">
        /// The HTML attribute containing serialized dependency metadata, or an empty string if there are no dependencies.
        /// </param>
        /// <returns>The HTML markup for the dropdown.</returns>
        private static string BuildDropdown(FormQuestion question, string lang, string isRequired, string dependenciesAttr)
        {
            ArgumentNullException.ThrowIfNull(question.Options, nameof(question.Options));

            var sb = new StringBuilder();
            sb.AppendLine($@"
                <gcds-select
                    select-id='{question.Id}'
                    label='{question.Label}'" +
                AttributeIfNotNull("hint", question.Hint) +
                $@" lang='{lang}'
                    {isRequired}
                    {dependenciesAttr}
                >");
            foreach (var option in question.Options)
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"<option value='{option.Value}'>{option.Label}</option>");
            }
            sb.AppendLine("</gcds-select>");
            return sb.ToString();
        }

        /// <summary>
        /// Builds the HTML markup for a textarea question.
        /// </summary>
        /// <param name="question">The form question.</param>
        /// <param name="lang">The language code.</param>
        /// <param name="isRequired">The required attribute string.</param>
        /// <returns>The HTML markup for the textarea.</returns>
        private static string BuildTextArea(FormQuestion question, string lang, string isRequired)
            => $@"<gcds-textarea 
                    textarea-id='{question.Id}' 
                    label='{question.Label}' 
                    name='{question.Id}' 
                    {AttributeIfNotNull("hint", question.Hint)}
                    {isRequired} 
                    lang='{lang}' 
                    rows='{question.Size}'>
                </gcds-textarea>";

        /// <summary>
        /// Builds the HTML markup for a date input question using the GC Design System date input component.
        /// </summary>
        /// <param name="question">The form question.</param>
        /// <param name="lang">The language code.</param>
        /// <param name="isRequired">The required attribute string.</param>
        /// <returns>The HTML markup for the date input.</returns>
        private static string BuildDateInput(FormQuestion question, string lang, string isRequired)
        {
            string format = question.Format ?? "full";
            return $"<gcds-date-input legend='{question.Label}' name='{question.Id}' format='{format}' lang='{lang}' {isRequired}></gcds-date-input>";
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
