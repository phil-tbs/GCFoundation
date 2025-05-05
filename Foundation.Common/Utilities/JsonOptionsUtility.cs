using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Foundation.Common.Utilities
{
    /// <summary>
    /// Provides cached instances of <see cref="JsonSerializerOptions"/> to avoid repeated allocations.
    /// Use these options to maintain consistent JSON serialization behavior across the application.
    /// </summary>
    public static class JsonOptionsUtility
    {
        /// <summary>
        /// Gets a <see cref="JsonSerializerOptions"/> instance with <see cref="JsonNamingPolicy.CamelCase"/> 
        /// to serialize property names using camelCase. This is commonly used in APIs and front-end JSON rendering.
        /// </summary>
        public static readonly JsonSerializerOptions CamelCase = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Gets the default <see cref="JsonSerializerOptions"/> instance with no special settings.
        /// Useful when serialization must preserve PascalCase or default .NET behavior.
        /// </summary>
        public static readonly JsonSerializerOptions Default = new JsonSerializerOptions();
    }
}
