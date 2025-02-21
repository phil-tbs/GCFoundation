using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LocalizedFieldMetadataAttribute: Attribute
    {
        private readonly string? _labelKey;
        private readonly string? _hintKey;
        private readonly Type _resourceType;

        public LocalizedFieldMetadataAttribute(Type resourceType, string? labelKey = null, string? hintKey = null)
        {
            _resourceType = resourceType;
            _labelKey = labelKey;
            _hintKey = hintKey;
        }


        public string? Label => GetResourceValue(_labelKey);
        public string? Hint => GetResourceValue(_hintKey);

        private string? GetResourceValue(string? key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            var resourceManager = new ResourceManager(_resourceType);
            return resourceManager.GetString(key, CultureInfo.CurrentUICulture);
        }
    }
}
