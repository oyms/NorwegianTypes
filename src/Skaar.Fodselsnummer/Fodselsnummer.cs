using Skaar.TypeSupport.Contracts;
using Skaar.TypeSupport.Serialization;
using Skaar.TypeSupport.Utils;
using Skaar.Utils;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Skaar;

/// <summary>
/// This type represents a Norwegian identity number
/// (Fødselsnummer)
/// </summary>
/// <remarks>
/// <seealso href="https://en.wikipedia.org/wiki/National_identity_number_(Norway)"/>
/// <seealso href="https://www.skatteetaten.no/en/person/national-registry/identitetsnummer/fodselsnummer/"/>
/// </remarks>
[JsonConverter(typeof(ParsableJsonConverter<Fodselsnummer>))]
[TypeConverter(typeof(ParsableTypeConverter<Fodselsnummer>))]
public readonly struct Fodselsnummer :
    IParsable<Fodselsnummer>,
    ICanBeValid
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string? _value;

    private Fodselsnummer(string? value)
    {
        _value = StringUtils.RemoveWhitespace(value);
        Type = NummerParser.ParseIdNummer(_value);
    }

    public NummerType Type { get; }
    
    public static Fodselsnummer Parse(string s, IFormatProvider? provider = null)
    {
        if (!TryParse(s, provider, out var result) && !result.IsValid)
        {
            throw new FormatException("String is not a valid id-number.");
        }

        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Fodselsnummer result)
    {
        result = new Fodselsnummer(s);
        return result.IsValid;
    }

    public bool IsValid => Type != NummerType.Invalid;

    public override string ToString() => _value ?? string.Empty;
}