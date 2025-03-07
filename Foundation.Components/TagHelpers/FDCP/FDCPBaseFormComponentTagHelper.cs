using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using Foundation.Components.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.FDCP
{
    public abstract class FDCPBaseFormComponentTagHelper : TagHelper
    {
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

        protected PropertyInfo? PropertyInfo
        {
            get
            {
                return For.Metadata.ContainerType.GetProperty(For.Metadata.PropertyName);
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
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

            output.Attributes.SetAttribute("lang", LanguageUtilitiy.GetCurrentApplicationLanguage());
            
            if (required)
            {
                output.Attributes.SetAttribute("required", required);
            }


        }

        protected string GetLocalizedLabel(PropertyInfo property)
        {
            var displayAttr = property.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.GetName() ?? property.Name;
        }

        protected string GetLocalizedHint(PropertyInfo property)
        {
            var displayAttr = property.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.GetDescription() ?? string.Empty;
        }

    }
}
