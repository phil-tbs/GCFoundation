using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GCFoundation.Components.TagHelpers.GCDS
{
    /// <summary>
    /// A base class for form component tag helpers, providing common properties and functionality for form inputs.
    /// </summary>
    public abstract class BaseFormComponentTagHelper : BaseTagHelper
    {
        /// <summary>
        /// The model expression associated with the input field.
        /// </summary>
        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; } = default!;

        /// <summary>
        /// The <see cref="ViewContext"/> for the current view.
        /// </summary>
        [ViewContext]
        public ViewContext ViewContext { get; set; } = default!;

        /// <summary>
        /// The name of the input field.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Indicates whether the component is disabled.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// The error message associated with the input field, if any.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Indicates whether the input field is required.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// The event on which validation occurs. Defaults to "blur".
        /// </summary>
        public string ValidateOn { get; set; } = "blur";

        /// <summary>
        /// A hint providing additional information on how to answer the input.
        /// </summary>
        public string? Hint { get; set; }

        /// <summary>
        /// The value of the input field.
        /// </summary>
        public string? Value { get; set; }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (For != null)
            {
                Name ??= For.Name;
                Value ??= For.Model?.ToString() ?? "";
                Required = Required || For.Metadata.ValidatorMetadata.OfType<RequiredAttribute>().Any();
                RetrieveLocalizedProperties();
            }

            AddAttributeIfNotNull(output, "name", Name);
            AddAttributeIfNotNull(output, "disabled", Disabled);
            AddAttributeIfNotNull(output, "error-message", ErrorMessage);
            AddAttributeIfNotNull(output, "required", Required);
            AddAttributeIfNotNull(output, "validate-on", ValidateOn);
            AddAttributeIfNotNull(output, "hint", Hint);
            AddAttributeIfNotNull(output, "lang", Lang);
            AddAttributeIfNotNull(output, "value", Value);

            base.Process(context, output);
        }

        /// <summary>
        /// Retrieves localized properties, such as the hint text for the input field.
        /// </summary>
        private void RetrieveLocalizedProperties()
        {
            var propertyInfo = For.Metadata.ContainerType?.GetProperty(For.Name);
            if (propertyInfo == null) return;

            Hint = GetLocalizedHint(propertyInfo);

        }

        /// <summary>
        /// Gets the localized hint text from the <see cref="DisplayAttribute"/> of the specified property.
        /// </summary>
        /// <param name="property">The property to retrieve the localized hint for.</param>
        /// <returns>The localized hint text, or an empty string if not available.</returns>
        protected static string GetLocalizedHint(PropertyInfo property)
        {
            var displayAttr = property.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.GetDescription() ?? string.Empty;
        }

    }
}
