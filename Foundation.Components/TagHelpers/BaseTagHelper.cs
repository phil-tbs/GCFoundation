using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Utilities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    public abstract class BaseTagHelper : TagHelper
    {
        protected void AddAttributeIfNotNull(TagHelperOutput output, string attributeName, object? attributeValue)
        {
            // Add the attribute only if the value is not null or default
            if (attributeValue != null && !attributeValue.Equals(default))
            {
                if (attributeValue.GetType().IsEnum || attributeValue.GetType() == typeof(bool))
                {
                    output.Attributes.SetAttribute(attributeName, attributeValue.ToString()?.ToLower());
                }
                else
                {
                    output.Attributes.SetAttribute(attributeName, attributeValue.ToString());
                }

                
            }
        }

        protected void AddAttributeIfNotNullWithCaseConversion(TagHelperOutput output, string attributeName, object? attributeValue)
        {
            // Add the attribute only if the value is not null or default
            if (attributeValue != null && !attributeValue.Equals(default))
            {
                string attributeValueString = attributeValue.ToString() ?? string.Empty; // Ensure it's not null
                output.Attributes.SetAttribute(attributeName, CaseUtility.ConvertToKebabCase(attributeValueString));
            }
        }
    }
}
