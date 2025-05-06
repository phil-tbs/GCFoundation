using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Utilities
{
    /// <summary>
    /// Provides utility methods for manipulating string cases, including conversion to kebab case.
    /// </summary>
    public static class CaseUtility
    {
        /// <summary>
        /// Converts the input string from camel case or Pascal case to kebab case (e.g., "CamelCase" -> "camel-case").
        /// Kebab case uses hyphens to separate words and all letters are converted to lowercase.
        /// </summary>
        /// <param name="input">The input string to convert. It can be in camel case, Pascal case, or any string with uppercase letters.</param>
        /// <returns>A string in kebab case format. If the input is null or empty, it returns the original input.</returns>
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
