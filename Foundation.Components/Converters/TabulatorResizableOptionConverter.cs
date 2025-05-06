using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Foundation.Components.Enum;

namespace Foundation.Components.Converters
{
    /// <summary>
    /// A custom JSON converter for <see cref="TabulatorResizableOption"/> to support flexible input formats.
    /// Converts boolean and string values to the corresponding enum options and vice versa.
    /// </summary>
    public class TabulatorResizableOptionConverter: JsonConverter<TabulatorResizableOption>
    {
        /// <summary>
        /// Reads and converts a JSON value to a <see cref="TabulatorResizableOption"/>.
        /// Accepts <c>true</c>, <c>false</c>, <c>"header"</c>, and <c>"cell"</c> as valid inputs.
        /// </summary>
        /// <param name="reader">The reader to read the JSON value from.</param>
        /// <param name="typeToConvert">The type of the object to convert.</param>
        /// <param name="options">The serializer options to use.</param>
        /// <returns>The corresponding <see cref="TabulatorResizableOption"/> value.</returns>
        /// <exception cref="JsonException">Thrown if the input token is not a recognized resizable option.</exception>
        public override TabulatorResizableOption Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.True => TabulatorResizableOption.True,
                JsonTokenType.False => TabulatorResizableOption.False,
                JsonTokenType.String when reader.GetString() == "header" => TabulatorResizableOption.Header,
                JsonTokenType.String when reader.GetString() == "cell" => TabulatorResizableOption.Cell,
                _ => throw new JsonException("Invalid resizable value")
            };
        }

        /// <summary>
        /// Writes a <see cref="TabulatorResizableOption"/> as a JSON value.
        /// Outputs a boolean for <c>True</c> and <c>False</c>, or a string for <c>Header</c> and <c>Cell</c>.
        /// </summary>
        /// <param name="writer">The writer to output the JSON value to.</param>
        /// <param name="value">The <see cref="TabulatorResizableOption"/> value to write.</param>
        /// <param name="options">The serializer options to use.</param>
        public override void Write(Utf8JsonWriter writer, TabulatorResizableOption value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);

            switch (value)
            {
                case TabulatorResizableOption.True:
                    writer.WriteBooleanValue(true);
                    break;
                case TabulatorResizableOption.False:
                    writer.WriteBooleanValue(false);
                    break;
                case TabulatorResizableOption.Header:
                    writer.WriteStringValue("header");
                    break;
                case TabulatorResizableOption.Cell:
                    writer.WriteStringValue("cell");
                    break;
            }
        }
    }
}
