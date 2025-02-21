using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("fd-feild", Attributes = "for" )]
    public class FoundationFieldTagHelper : TagHelper
    {
        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; } = default!;

        [ViewContext]
        public ViewContext ViewContext { get; set; } = default!;

        private DataTypeAttribute? DataTypeAttribute { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if (For == null)
            {
                output.SuppressOutput();
                return;
            }

            PropertyInfo property = For.Metadata.ContainerType.GetProperty(For.Metadata.PropertyName);
            if (property == null)
            {
                output.SuppressOutput();
                return;
            }

            DataTypeAttribute = property.GetCustomAttribute<DataTypeAttribute>();

            // Determine which GC Design System component to use
            string gcdsComponent = GetGcdsComponent(property);
            string fieldName = For.Name;
            string label = GetLocalizedLabel(property);
            string hint = GetLocalizedHint(property);
            string errorMessage = GetValidationMessage(property);
            string inputType = GetInputType(property);

            // Generate the component tag dynamically
            output.TagName = gcdsComponent;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.SetAttribute("name", fieldName);
            output.Attributes.SetAttribute("label", label);
            output.Attributes.SetAttribute("input-id", fieldName);
            output.Attributes.SetAttribute("hint", hint);
            output.Attributes.SetAttribute("type", inputType);

            if(DataTypeAttribute != null && DataTypeAttribute.DataType == DataType.Date)
            {
                output.Attributes.SetAttribute("format", "full");
            }

            //if (!string.IsNullOrEmpty(errorMessage))
            //{
            //    output.Attributes.SetAttribute("error-message", errorMessage);
            //}

            if (property.GetCustomAttribute<RequiredAttribute>() != null)
            {
                output.Attributes.SetAttribute("required", "true");
            }
        }

        private string GetGcdsComponent(PropertyInfo property)
        {
            if (property.PropertyType == typeof(bool))
                return "gcds-checkbox";

            if (DataTypeAttribute != null)
            {
                switch (DataTypeAttribute.DataType)
                {
                    case DataType.EmailAddress:
                        return "gcds-input";
                    case DataType.Password:
                        return "gcds-input";
                    case DataType.Url:
                        return "gcds-input";
                    case DataType.PhoneNumber:
                        return "gcds-input";
                    case DataType.Date:
                        return "gcds-date-input";
                    case DataType.MultilineText:
                        return "gcds-textarea";
                    case DataType.Text:
                    default:
                        return "gcds-input";
                }
            }

            return "gcds-input";
        }

        private string GetInputType(PropertyInfo property)
        {

            if (DataTypeAttribute != null)
            {
                return DataTypeAttribute.DataType switch
                {
                    DataType.EmailAddress => "email",
                    DataType.Password => "password",
                    DataType.Url => "url",
                    _ => "text"
                };
            }

            // If no DataType attribute is set, determine type from property type
            if (property.PropertyType == typeof(int) ||
                property.PropertyType == typeof(decimal) ||
                property.PropertyType == typeof(double) ||
                property.PropertyType == typeof(float))
            {
                return "number";
            }

            return "text";
        }

        private string GetLocalizedLabel(PropertyInfo property)
        {
            var displayAttr = property.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.GetName() ?? property.Name;
        }

        private string GetLocalizedHint(PropertyInfo property)
        {
            var displayAttr = property.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.GetDescription() ?? string.Empty;
        }

        private string GetValidationMessage(PropertyInfo property)
        {
            var requiredAttr = property.GetCustomAttribute<RequiredAttribute>();
            if (requiredAttr != null)
            {
                return string.Format("{0} is required.", GetLocalizedLabel(property));
            }

            var minLengthAttr = property.GetCustomAttribute<MinLengthAttribute>();
            if (minLengthAttr != null)
            {
                return string.Format("{0} must be at least {1} characters.", GetLocalizedLabel(property), minLengthAttr.Length);
            }

            var maxLengthAttr = property.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLengthAttr != null)
            {
                return string.Format("{0} cannot exceed {1} characters.", GetLocalizedLabel(property), maxLengthAttr.Length);
            }

            var regexAttr = property.GetCustomAttribute<RegularExpressionAttribute>();
            if (regexAttr != null)
            {
                return regexAttr.ErrorMessage ?? "Invalid format.";
            }

            return string.Empty;
        }

    }
}
