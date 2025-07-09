using GCFoundation.Common.Utilities;
using GCFoundation.Components.TagHelpers.GCDS;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;

namespace GCFoundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// A base class for form components that provides functionality for binding model properties, 
    /// performing validation, and adding common attributes like labels and hints to the HTML output.
    /// </summary>
    public abstract class FDCPBaseFormComponentTagHelper : BaseTagHelper
    {
        /// <summary>
        /// Options for serializing JSON property names in camel case.
        /// </summary>
        protected static readonly JsonSerializerOptions CamelCaseOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Binds the tag helper to a model property, enabling validation and data binding.
        /// </summary>
        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; } = default!;


        /// <summary>
        /// Injects the current ViewContext to access ModelState for validation.
        /// </summary>
        [ViewContext]
        public ViewContext ViewContext { get; set; } = default!;

        /// <summary>
        /// Retrieves the <see cref="DataTypeAttribute"/> for the property if available.
        /// </summary>
        protected DataTypeAttribute? DataTypeAttribute
        {
            get
            {
                if (PropertyInfo == null)
                {
                    return null;
                }
                return PropertyInfo.GetCustomAttribute<DataTypeAttribute>();
            }
        }

        /// <summary>
        /// Retrieves the <see cref="PropertyInfo"/> for the model property bound to this tag helper.
        /// </summary>
        protected PropertyInfo? PropertyInfo
        {
            get
            {
                PropertyInfo? propertyInfo = null;

                if (!string.IsNullOrEmpty(For.Metadata.PropertyName))
                {
                    propertyInfo = For.Metadata.ContainerType?.GetProperty(For.Metadata.PropertyName);
                }
                return propertyInfo;
            }
        }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            if (For == null)
            {
                output.SuppressOutput();
                return;
            }

            PropertyInfo? property = For.Metadata.ContainerType?.GetProperty(For.Metadata.PropertyName ?? string.Empty);
            if (property == null)
            {
                output.SuppressOutput();
                return;
            }

            output.TagName = "gcds-input";
            output.TagMode = TagMode.StartTagAndEndTag;
            string fieldName = For.Name;

            string label = GetLocalizedLabel(property);
            string hint = GetLocalizedHint(property);
            bool required = For.Metadata.ValidatorMetadata.OfType<RequiredAttribute>().Any();
            string fieldValue = For.Model?.ToString() ?? string.Empty; // Retrieve the model value


            output.Attributes.SetAttribute("value", fieldValue);
            output.Attributes.SetAttribute("name", fieldName);
            output.Attributes.SetAttribute("label", label);
            output.Attributes.SetAttribute("hint", hint);

            output.Attributes.SetAttribute("lang", LanguageUtility.GetCurrentApplicationLanguage());

            if (required)
            {
                output.Attributes.SetAttribute("required", required);
            }


        }

        /// <summary>
        /// Retrieves the localized label for a property, falling back to the property name if no label is provided.
        /// </summary>
        /// <param name="property">The property to retrieve the label for.</param>
        /// <returns>The localized label for the property.</returns>
        protected static string GetLocalizedLabel(PropertyInfo property)
        {
            ArgumentNullException.ThrowIfNull(property, nameof(property));

            var displayAttr = property.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.GetName() ?? property.Name;
        }

        /// <summary>
        /// Retrieves the localized hint for a property, falling back to an empty string if no hint is provided.
        /// </summary>
        /// <param name="property">The property to retrieve the hint for.</param>
        /// <returns>The localized hint for the property.</returns>
        protected static string GetLocalizedHint(PropertyInfo property)
        {
            var displayAttr = property.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.GetDescription() ?? string.Empty;
        }

    }
}
