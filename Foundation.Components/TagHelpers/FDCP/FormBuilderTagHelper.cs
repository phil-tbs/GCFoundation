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
using Newtonsoft.Json.Converters;

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
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            output.TagName = "div";
            output.Attributes.SetAttribute("class", "gc-form");

            var content = new StringBuilder();
            await BuildFormContentAsync(content);
            output.Content.SetHtmlContent(content.ToString());
        }

        private async Task BuildFormContentAsync(StringBuilder content)
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
                content.AppendLine($@"<gcds-fieldset 
                    fieldset-id='{section.Title}' 
                    legend='{section.Title}' 
                    hint='{section.Hint}'>");

                foreach (var question in section.Questions)
                {
                    content.AppendLine(await RenderQuestionAsync(question));
                }

                content.AppendLine("</gcds-fieldset>");
            }

            // Submit button
            content.AppendLine($@"<gcds-button 
                type='submit' 
                button-role='primary'>
                {Form.SubmithButtonText}
            </gcds-button>");

            content.AppendLine("</form>");
        }

        private async Task<string> RenderQuestionAsync(FormQuestion question)
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

                QuestionType.Radio => await BuildRadioGroupAsync(question, language, commonAttributes),

                QuestionType.Checkbox => await BuildCheckboxesAsync(question, language, commonAttributes),

                QuestionType.Dropdown => $@"<gcds-select
                    select-id='{question.Id}'
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

        private async Task<string> BuildRadioGroupAsync(FormQuestion question, string lang, string commonAttributes)
        {
            var options = question.Options?.Select(option => new
            {
                option.Id,
                option.Label,
                option.Hint,
                option.Value,
                Checked = (option.Value?.ToString() == question.Value?.ToString())
            });

            var optionsJson = JsonConvert.SerializeObject(options, CamelCaseSettings);

            return $@"
                <gcds-radio-group
                    radio-id='{question.Id}'
                    options='{optionsJson}'
                    {commonAttributes}>
                </gcds-radio-group>";
        }

        private string BuildOptions(IEnumerable<QuestionOption>? options)
        {
            if (options == null) return string.Empty;

            var sb = new StringBuilder();
            foreach (var option in options)
            {
                sb.AppendLine($"<option value='{option.Value}'>{option.Label}</option>");
            }
            return sb.ToString();
        }

        private async Task<string> BuildCheckboxesAsync(FormQuestion question, string lang, string commonAttributes)
        {
            ArgumentNullException.ThrowIfNull(question.Options, nameof(question.Options));
            var selectedValues = question.Value is IEnumerable<object> values
                ? values.Select(v => v.ToString()).ToHashSet()
                : new HashSet<string>();

            // Handle single checkbox case
            if (question.Options.Count() == 1)
            {
                var option = question.Options.First();
                return $@"<gcds-checkbox
                    checkbox-id=""{question.Id}""
                    name=""{question.Id}""
                    label=""{option.Label}""
                    value=""{option.Value}""
                    hint=""{question.Hint}""
                    {(question.IsRequired ? "required" : "")}
                    {(selectedValues.Contains(option.Value?.ToString()) ? "checked" : "")}
                    lang=""{lang}"">
                </gcds-checkbox>";
            }

            // Handle multiple checkboxes case
            var checkboxes = new StringBuilder();
            foreach (var option in question.Options)
            {
                checkboxes.AppendLine(CultureInfo.InvariantCulture ,$@"
                    <gcds-checkbox
                        checkbox-id=""{question.Id}_{option.Id}""
                        name=""{question.Id}""
                        label=""{option.Label}""
                        value=""{option.Value}""
                        hint=""{option.Hint}""
                        {(selectedValues.Contains(option.Value?.ToString()) ? "checked" : "")}
                        lang=""{lang}"">
                    </gcds-checkbox>");
            }

            return $@"<gcds-fieldset
                fieldset-id=""{question.Id}-group""
                legend=""{question.Label}""
                hint=""{question.Hint}""
                {(question.IsRequired ? "required" : "")}
                lang=""{lang}"">
                {checkboxes}
            </gcds-fieldset>";
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
