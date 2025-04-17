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
    public class TabulatorResizableOptionConverter: JsonConverter<TabulatorResizableOption>
    {
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

        public override void Write(Utf8JsonWriter writer, TabulatorResizableOption value, JsonSerializerOptions options)
        {
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
