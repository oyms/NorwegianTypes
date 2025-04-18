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
/// This type represents a Norwegian D-number
/// (D-nummer)
/// </summary>
/// <remarks>
/// <seealso href="https://www.skatteetaten.no/en/person/national-registry/identitetsnummer/d-nummer/"/>
/// </remarks>
[JsonConverter(typeof(ParsableJsonConverter<DNummer>))]
[TypeConverter(typeof(ParsableTypeConverter<DNummer>))]
public readonly struct DNummer :
    IIdNumber,
    ISpanParsable<DNummer>,
    ISafeParsable<DNummer>,
    ISpanFormattable,
    IHasLength,
    IEquatable<DNummer>,
    IComparable<DNummer>,
    IRandomValueFactory<DNummer>,
    IComparisonOperators<DNummer, DNummer, bool>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ReadOnlyMemory<char> _value;

    private DNummer(ReadOnlySpan<char> value)
    {
        _value = StringUtils.RemoveNonDigits(value);
        IsValid = ValueParser.IsDNummer(_value.Span);
    }
    
    public DateOnly BirthDate 
    {
        get
        {
            if (!IsValid) return default;
            var day = int.Parse(_value.Span[..2]) - 40;
            if (ValueParser.ValidateBirthDate($"{day:00}{_value[2..]}", out var result))
            {
                return result;
            }

            return default;
        }
    }

    public Gender Gender
    {
        get
        {
            if(!IsValid) return default;
            var genderDigit = _value.Span[8] - '0';
            return genderDigit % 2 == 0 ? Gender.Female : Gender.Male;
        }
    }
    
    [MemberNotNullWhen(true, nameof(_value))]
    public bool IsValid { get; }
    public static DNummer Parse(string s, IFormatProvider? provider = null)
    {
        if (!TryParse(s, provider, out var result) && !result.IsValid)
        {
            throw new FormatException("String is not a valid d-number.");
        }

        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out DNummer result)
    {
        result = new DNummer(s);
        return result.IsValid;
    }
    
    public static DNummer Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        if (!TryParse(s, provider, out var result) && !result.IsValid)
        {
            throw new FormatException("String is not a valid d-number.");
        }

        return result;
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DNummer result)
    {
        result = new DNummer(s);
        return result.IsValid;
    }

    public static DNummer CreateNew(string? value, IFormatProvider? provider = null) => Parser.SafeParse<DNummer>(value, provider);
    public static DNummer CreateNew()
    {
        var date = DateOnly.FromDateTime(new DateTime(1940, 1, 1) + TimeSpan.FromDays(Random.Shared.Next(365 * 100)));
        var gender = Gender.Undefined;
        return CreateNew(ValueFactory.CreateNew(NummerType.DNummer, date, gender));
    }
    public int Length => _value.Length;
    public override string ToString() => _value.ToString();
        
    public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider) =>
        StringUtils.TryFormatIgnoringFormatting(_value, destination, out charsWritten, format, provider);

    public bool Equals(DNummer other) => _value.Span.SequenceEqual(other._value.Span);

    public override bool Equals(object? obj) => obj is DNummer other && Equals(other);

    public override int GetHashCode() => _value.GetHashCode();

    public static bool operator ==(DNummer left, DNummer right) => left.Equals(right);

    public static bool operator !=(DNummer left, DNummer right) => !left.Equals(right);

    public int CompareTo(DNummer other)
    {
        var isValidComparison = IsValid.CompareTo(other.IsValid);
        if (isValidComparison != 0)
        {
            return isValidComparison;
        }

        return StringUtils.MemoryCompare(_value, other._value);
    }

    public static bool operator <(DNummer left, DNummer right) => left.CompareTo(right) < 0;

    public static bool operator >(DNummer left, DNummer right) => left.CompareTo(right) > 0;

    public static bool operator <=(DNummer left, DNummer right) => left.CompareTo(right) <= 0;

    public static bool operator >=(DNummer left, DNummer right) => left.CompareTo(right) >= 0;
}