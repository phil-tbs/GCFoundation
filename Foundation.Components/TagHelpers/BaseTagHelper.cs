using Foundation.Common.Utilities;
using Foundation.Components.Enums;
using Foundation.Components.Utilities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    /// <summary>
    /// Provides common functionality for custom TagHelpers in the Foundation.Components library.
    /// Includes utility methods to conditionally add HTML attributes to the output.
    /// </summary>
    public abstract class BaseTagHelper : TagHelper
    {
        /// <summary>
        /// Gets the current language based on the result of <see cref="LanguageUtility.IsEnglish"/>.
        /// Returns <c>Language.en</c> if English is detected; otherwise, returns <c>Language.fr</c>.
        /// </summary>
        public static Language Lang
        {
            get
            {
                return (LanguageUtility.IsEnglish() ? Language.en : Language.fr);
            }
        }

        /// <summary>
        /// Adds an HTML attribute to the <see cref="TagHelperOutput"/> if the provided value is not null.
        /// For enum and boolean values, the value is converted to a lowercase invariant string.
        /// </summary>
        /// <param name="output">The <see cref="TagHelperOutput"/> object representing the tag helper's output.</param>
        /// <param name="attributeName">The name of the HTML attribute to add.</param>
        /// <param name="attributeValue">The value of the HTML attribute. If null, the attribute is not added.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="output"/> is null.</exception>
        protected static void AddAttributeIfNotNull(TagHelperOutput output, string attributeName, object? attributeValue)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            // Add the attribute only if the value is not null or default
            if (attributeValue != null)
            {
                if (attributeValue.GetType().IsEnum || attributeValue.GetType() == typeof(bool))
                {
#pragma warning disable CA1308 // Normalize strings to uppercase
                    output.Attributes.SetAttribute(attributeName, attributeValue.ToString()?.ToLowerInvariant());
#pragma warning restore CA1308 // Normalize strings to uppercase
                }
                else
                {

                    if (!string.IsNullOrEmpty(attributeValue.ToString()))
                    {
                        output.Attributes.SetAttribute(attributeName, attributeValue.ToString());
                    }
                }


            }
        }

        /// <summary>
        /// Adds an HTML attribute to the <see cref="TagHelperOutput"/> if the provided value is not null.
        /// The value is converted to kebab-case using <see cref="CaseUtility.ConvertToKebabCase(string)"/>.
        /// </summary>
        /// <param name="output">The <see cref="TagHelperOutput"/> object representing the tag helper's output.</param>
        /// <param name="attributeName">The name of the HTML attribute to add.</param>
        /// <param name="attributeValue">The value of the HTML attribute. If null, the attribute is not added.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="output"/> is null.</exception>
        protected static void AddAttributeIfNotNullWithCaseConversion(TagHelperOutput output, string attributeName, object? attributeValue)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            // Add the attribute only if the value is not null or default
            if (attributeValue != null)
            {
                string attributeValueString = attributeValue.ToString() ?? string.Empty; // Ensure it's not null
                output.Attributes.SetAttribute(attributeName, CaseUtility.ConvertToKebabCase(attributeValueString));
            }
        }

    }
}
