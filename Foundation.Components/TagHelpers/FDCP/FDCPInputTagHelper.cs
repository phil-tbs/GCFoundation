using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Foundation.Components.Attributes;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// A tag helper for rendering input elements for different data types (e.g., text, date, checkbox, text area). 
    /// It supports automatic binding to model properties and validation, and it dynamically chooses the appropriate input tag based on the property type.
    /// </summary>
    [HtmlTargetElement("fdcp-input", Attributes = "for")]
    public class FDCPInputTagHelper : FDCPBaseFormComponentTagHelper
    {
        private enum TagType
        {
            input,
            date,
            checkbox,
            textArea
        }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));
            // Call base class to handle label, hint, and errors
            base.Process(context, output);

            if (this.PropertyInfo == null)
            {
                output.SuppressOutput();
                return;
            }

            TagType tagType = GetTagType();

            BuildByTagType(context, output, tagType);
        }

        /// <summary>
        /// Builds the HTML tag based on the tag type (input, date, checkbox, textArea).
        /// </summary>
        /// <param name="context">The context of the tag helper.</param>
        /// <param name="output">The output for the tag helper content.</param>
        /// <param name="tagType">The tag type representing the type of input element to create.</param>
        private void BuildByTagType(TagHelperContext context, TagHelperOutput output, TagType tagType)
        {
            output.TagName = GetTagNameByInputType(tagType);

            if (tagType == TagType.checkbox)
            {
                output.Attributes.SetAttribute("checkbox-id", For.Name);
            }
            else if (tagType == TagType.textArea)
            {
                output.Attributes.SetAttribute("textarea-id", For.Name);
            }
            else if (tagType == TagType.date)
            {
                if (this.PropertyInfo == null)
                {
                    throw new InvalidOperationException("Missing proprities");
                }

                DateFormatAttribute? formatAttr = this.PropertyInfo.GetCustomAttribute<DateFormatAttribute>();
                string label = GetLocalizedLabel(this.PropertyInfo);

                output.Attributes.SetAttribute("legend", label);
                output.Attributes.SetAttribute("format", (formatAttr != null) ? formatAttr.Format : "full");
            }
            else
            {
                string gcdsType = GetInputType();
                output.Attributes.SetAttribute("type", gcdsType);
                output.Attributes.SetAttribute("input-id", For.Name);
            }

        }

        /// <summary>
        /// Retrieves the appropriate HTML tag name based on the input type.
        /// </summary>
        /// <param name="inputType">The type of the input (e.g., input, date, checkbox, textArea).</param>
        /// <returns>The HTML tag name as a string.</returns>
        private static string? GetTagNameByInputType(TagType inputType)
        {
            switch (inputType)
            {
                case TagType.input:
                    return "gcds-input";
                case TagType.date:
                    return "gcds-date-input";
                case TagType.checkbox:
                    return "gcds-checkbox";
                case TagType.textArea:
                    return "gcds-textarea";
                default:
                    return null;

            }
        }

        /// <summary>
        /// Determines the appropriate tag type (e.g., input, date, checkbox) based on the property type.
        /// </summary>
        /// <returns>The tag type corresponding to the model property type.</returns>
        private TagType GetTagType()
        {
            if (this.PropertyInfo != null && this.PropertyInfo.PropertyType == typeof(bool))
                return TagType.checkbox;

            if (DataTypeAttribute != null)
            {

                switch (DataTypeAttribute.DataType)
                {
                    case DataType.Date:
                        return TagType.date;
                    case DataType.MultilineText:
                        return TagType.textArea;
                    default:
                        return TagType.input;
                }
            }

            return TagType.input;
        }

        /// <summary>
        /// Determines the input type (e.g., text, email, password) based on the data type or property type.
        /// </summary>
        /// <returns>The input type as a string (e.g., "text", "email", "password").</returns>
        private string GetInputType()
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

            // Ensure PropertyInfo is not null before accessing its PropertyType
            if (this.PropertyInfo == null)
            {
                // Return a default value or handle this case appropriately
                return "text";
            }

            Type propertyType = this.PropertyInfo.PropertyType;
            // Converting to regular type (no nullable) for comparaison
            Type underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (underlyingType == typeof(int) ||
                underlyingType == typeof(decimal) ||
                underlyingType == typeof(double) ||
                underlyingType == typeof(float))
            {
                return "number";
            }

            return "text";
        }
    }
}
