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
    ISpanParsable<DufNummer>,
    ISafeParsable<DufNummer>,
    ISpanFormattable,
    IHasLength,
    IEquatable<DufNummer>,
    IComparable<DufNummer>,
    IRandomValueFactory<DufNummer>,
    IComparisonOperators<DufNummer, DufNummer, bool>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ReadOnlyMemory<char> _value;

    private DufNummer(ReadOnlySpan<char> value)
    {
        _value = StringUtils.RemoveNonDigits(value);
        IsValid = ValueParser.IsDufNummer(_value.Span);
    }
    
    [MemberNotNullWhen(true, nameof(_value))]
    public bool IsValid { get; }
    public static DufNummer Parse(string s, IFormatProvider? provider = null)
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
    
    public static DufNummer Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        if (!TryParse(s, provider, out var result) && !result.IsValid)
        {
            throw new FormatException("String is not a valid duf-number.");
        }

        return result;
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DufNummer result)
    {
        result = new DufNummer(s);
        return result.IsValid;
    }

    public static DufNummer CreateNew(string? value, IFormatProvider? provider = null) => Parser.SafeParse<DufNummer>(value, provider);
    public static DufNummer CreateNew()
    {
        var date = DateOnly.FromDateTime(new DateTime(1940, 1, 1) + TimeSpan.FromDays(Random.Shared.Next(365 * 100)));
        var gender = Gender.Undefined;
        return CreateNew(ValueFactory.CreateNew(NummerType.DufNummer ,date, gender));
    }
    public int Length => _value.Length;
    public override string ToString() => _value.ToString();
    
    public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider) =>
        StringUtils.TryFormatIgnoringFormatting(_value, destination, out charsWritten, format, provider);

    public bool Equals(DufNummer other) => _value.Span.SequenceEqual(other._value.Span);

    public override bool Equals(object? obj) => obj is DufNummer other && Equals(other);

    public override int GetHashCode() => _value.GetHashCode();

    public static bool operator ==(DufNummer left, DufNummer right) => left.Equals(right);

    public static bool operator !=(DufNummer left, DufNummer right) => !left.Equals(right);

    public int CompareTo(DufNummer other)
    {
        var isValidComparison = IsValid.CompareTo(other.IsValid);
        if (isValidComparison != 0)
        {
            return isValidComparison;
        }

        return StringUtils.MemoryCompare(_value, other._value);
    }

    public static bool operator <(DufNummer left, DufNummer right) => left.CompareTo(right) < 0;

    public static bool operator >(DufNummer left, DufNummer right) => left.CompareTo(right) > 0;

    public static bool operator <=(DufNummer left, DufNummer right) => left.CompareTo(right) <= 0;

    public static bool operator >=(DufNummer left, DufNummer right) => left.CompareTo(right) >= 0;
}