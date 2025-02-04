using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    public abstract class BaseFromComponentTagHelper : BaseTagHelper
    {
        /// <summary>
        /// Name of the input
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// If the component is disabled
        /// </summary>
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// The error messages
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// If the input is required
        /// </summary>
        public bool Required { get; set; } = false;

        /// <summary>
        /// Wich event the validation append
        /// </summary>
        public string ValidateOn { get; set; } = "blur";

        /// <summary>
        /// Information on how anwser the input
        /// </summary>
        public string? Hint { get; set; }

        /// <summary>
        /// Language of the input
        /// </summary>
        public LanguageEnum Lan { get; set; }

        /// <summary>
        /// Value of the input
        /// </summary>
        public string? Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "name", Name);
            AddAttributeIfNotNull(output, "disabled", Disabled);
            AddAttributeIfNotNull(output, "error-message", ErrorMessage);
            AddAttributeIfNotNull(output, "required", Required);
            AddAttributeIfNotNull(output, "validate-on", ValidateOn);
            AddAttributeIfNotNull(output, "hint", Hint);
            AddAttributeIfNotNull(output, "lan", Lan);
            AddAttributeIfNotNull(output, "value", Value);

            base.Process(context, output);
        }
    }
}
