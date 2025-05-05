using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Utilities
{
    public static class CaseUtility
    {
        /// <summary>
        /// Change input with camel case to kebab case
        /// </summary>
        /// <param name="input">string to changed</param>
        /// <returns>Changed string</returns>
        public static string ConvertToKebabCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var builder = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsUpper(c))
                {
                    builder.Append('-');
                    builder.Append(char.ToLower(c, CultureInfo.InvariantCulture));
                }
                else
                {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }
    }
}
