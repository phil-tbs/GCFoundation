using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Attributes;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-input", Attributes = "for")]
    public class InputTagHelper : BaseFormComponentTagHelper
    {
        /// <summary>
        /// Input ID (auto-derived from For.Name if not set)
        /// </summary>
        public required string InputId { get; set; }

        /// <summary>
        /// Label for the input (auto-derived from DisplayName attribute if not set)
        /// </summary>
        public required string Label { get; set; }

        /// <summary>
        /// Autocomplete behavior
        /// </summary>
        public AutocompleteTypeEnum Autocomplete { get; set; } = AutocompleteTypeEnum.off;

        /// <summary>
        /// Whether to hide the label
        /// </summary>
        public bool HideLabel { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Auto-derive InputId and Label if not provided
            if (For != null)
            {
                InputId ??= For.Name;
                Label ??= For.Metadata.DisplayName ?? For.Name;


                RetrieveLocalizedProperties();
            }

            AddAttributeIfNotNull(output, "input-id", InputId);
            AddAttributeIfNotNull(output, "label", Label);
            AddAttributeIfNotNull(output, "hide-label", HideLabel);
            AddAttributeIfNotNull(output, "autocomplete", Autocomplete);

            base.Process(context, output);
        }

        private void RetrieveLocalizedProperties()
        {
            var propertyInfo = For.Metadata.ContainerType?.GetProperty(For.Name);
            if (propertyInfo == null) return;

            // Use GetLocalizedLabel to retrieve the label
            Label = GetLocalizedLabel(propertyInfo);
        }

        protected string GetLocalizedLabel(PropertyInfo property)
        {
            var displayAttr = property.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.GetName() ?? property.Name;
        }

    }
}
