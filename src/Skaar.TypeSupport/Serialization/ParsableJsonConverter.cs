using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Skaar.TypeSupport.Serialization;

/// <summary>
/// Parses any object of type implementing <see cref="IParsable{TSelf}"/>
/// to and from <see cref="string"/>.
/// </summary>
public class ParsableJsonConverter<T> : JsonConverter<T> where T : IParsable<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) 
        => T.Parse(reader.GetString()!, CultureInfo.InvariantCulture);

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteStringValue(value.ToString());
    }
}