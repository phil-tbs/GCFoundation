using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Attributes;
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

        private DataTypeAttribute? DataTypeAttribute { get; set; }

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

            DataTypeAttribute = property.GetCustomAttribute<DataTypeAttribute>();

            output.TagName = "gcds-input";
            output.TagMode = TagMode.StartTagAndEndTag;
            string fieldName = For.Name;
            string label = GetLocalizedLabel(property);
            string hint = GetLocalizedHint(property);

            output.Attributes.SetAttribute("name", fieldName);
            output.Attributes.SetAttribute("label", label);
            output.Attributes.SetAttribute("hint", hint);

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

    }
}
