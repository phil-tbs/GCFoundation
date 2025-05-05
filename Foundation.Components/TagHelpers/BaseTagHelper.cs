using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Utilities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    public abstract class BaseTagHelper : TagHelper
    {
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
                    output.Attributes.SetAttribute(attributeName, attributeValue.ToString());
                }

                
            }
        }

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
