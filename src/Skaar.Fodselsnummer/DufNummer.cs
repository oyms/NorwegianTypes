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
/// (DUF-nummer)
/// </summary>
/// <remarks>
/// <seealso href="https://www.udi.no/en/word-definitions/duf-number/"/>
/// </remarks>
[JsonConverter(typeof(ParsableJsonConverter<DufNummer>))]
[TypeConverter(typeof(ParsableTypeConverter<DufNummer>))]
public readonly struct DufNummer :
    IIdNumber,
    IParsable<DufNummer>,
    ISafeParsable<DufNummer>,
    IEquatable<DufNummer>,
    IComparable<DufNummer>,
    IRandomValueFactory<DufNummer>,
    IComparisonOperators<DufNummer, DufNummer, bool>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string? _value;

    private DufNummer(string? value)
    {
        _value = StringUtils.RemoveWhitespace(value);
        IsValid = NummerParser.IsDufNummer(_value);
    }
    
    public bool IsValid { get; }
    public static DufNummer Parse(string s, IFormatProvider? provider)
    {
        if (!TryParse(s, provider, out var result) && !result.IsValid)
        {
            throw new FormatException("String is not a valid duf-number.");
        }

        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out DufNummer result)
    {
        result = new DufNummer(s);
        return result.IsValid;
    }

    public static DufNummer CreateNew(string? value, IFormatProvider? provider = null) => Parser.SafeParse<DufNummer>(value, provider);
    public static DufNummer CreateNew()
    {
        var date = DateOnly.FromDateTime(new DateTime(1940, 1, 1) + TimeSpan.FromDays(Random.Shared.Next(365 * 100)));
        var gender = Gender.Undefined;
        return CreateNew(NummerFactory.CreateNew(NummerType.DufNummer ,date, gender));
    }

    public override string ToString() => _value ?? string.Empty;

    public bool Equals(DufNummer other) => _value == other._value;

    public override bool Equals(object? obj) => obj is DufNummer other && Equals(other);

    public override int GetHashCode() => (_value != null ? _value.GetHashCode() : 0);

    public static bool operator ==(DufNummer left, DufNummer right) => left.Equals(right);

    public static bool operator !=(DufNummer left, DufNummer right) => !left.Equals(right);

    public int CompareTo(DufNummer other)
    {
        var isValidComparison = IsValid.CompareTo(other.IsValid);
        if (isValidComparison != 0)
        {
            return isValidComparison;
        }

        return string.Compare(_value, other._value, StringComparison.Ordinal);
    }

    public static bool operator <(DufNummer left, DufNummer right) => left.CompareTo(right) < 0;

    public static bool operator >(DufNummer left, DufNummer right) => left.CompareTo(right) > 0;

    public static bool operator <=(DufNummer left, DufNummer right) => left.CompareTo(right) <= 0;

    public static bool operator >=(DufNummer left, DufNummer right) => left.CompareTo(right) >= 0;
}