using Skaar.Contracts;
using Skaar.TypeSupport.Contracts;
using Skaar.TypeSupport.Serialization;
using Skaar.TypeSupport.Utils;
using Skaar.Utils;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Skaar;

/// <summary>
/// This type represents a Norwegian identity number
/// (Fødselsnummer, D-nummer, DUF-nummer)
/// </summary>
/// <remarks>
/// <seealso href="https://en.wikipedia.org/wiki/National_identity_number_(Norway)"/>
/// </remarks>
[JsonConverter(typeof(ParsableJsonConverter<IdNumber>))]
[TypeConverter(typeof(ParsableTypeConverter<IdNumber>))]
public readonly struct IdNumber :
    IIdNumber,
    IParsable<IdNumber>,
    ISafeParsable<IdNumber>,
    IEquatable<IdNumber>,
    IComparable<IdNumber>,
    IComparisonOperators<IdNumber, IdNumber, bool>,
    IRandomValueFactory<IdNumber>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string? _value;

    private IdNumber(string? value)
    {
        _value = StringUtils.RemoveWhitespace(value);
        Type = NummerParser.ParseIdNummer(_value);
    }

    public NummerType Type { get; }
    
    public static IdNumber Parse(string s, IFormatProvider? provider = null)
    {
        if (!TryParse(s, provider, out var result) && !result.IsValid)
        {
            throw new FormatException("String is not a valid id-number.");
        }

        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out IdNumber result)
    {
        result = new IdNumber(s);
        return result.IsValid;
    }

    public static IdNumber CreateNew(string? value, IFormatProvider? provider = null) => Parser.SafeParse<IdNumber>(value, provider);

    public bool IsValid => Type != NummerType.Invalid;

    public override string ToString() => _value ?? string.Empty;

    public bool Equals(IdNumber other) => Type == other.Type && _value == other._value;

    public static IdNumber CreateNew()
    {
        var date = DateOnly.FromDateTime(new DateTime(1940, 1, 1) + TimeSpan.FromDays(Random.Shared.Next(365 * 100)));
        var gender = Gender.Undefined;
        return CreateNew(NummerFactory.CreateNew(NummerType.Fodselsnummer ,date, gender));
    }

    public override bool Equals(object? obj)
    {
        return obj is IdNumber other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_value, (int)Type);
    }

    public int CompareTo(IdNumber other) => string.Compare(_value, other._value, StringComparison.OrdinalIgnoreCase);

    public static bool operator ==(IdNumber left, IdNumber right) => left.Equals(right);

    public static bool operator !=(IdNumber left, IdNumber right) => !(left == right);

    public static bool operator >(IdNumber left, IdNumber right) => left.CompareTo(right) > 0;

    public static bool operator >=(IdNumber left, IdNumber right) => left.CompareTo(right) >= 0;

    public static bool operator <(IdNumber left, IdNumber right) => left.CompareTo(right) < 0;

    public static bool operator <=(IdNumber left, IdNumber right) => left.CompareTo(right) <= 0;

    public static explicit operator IdNumber(Fodselsnummer fodselsnummer) => CreateNew(fodselsnummer.ToString());
}