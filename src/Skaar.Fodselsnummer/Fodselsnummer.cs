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
/// (Fødselsnummer)
/// </summary>
/// <remarks>
/// <seealso href="https://en.wikipedia.org/wiki/National_identity_number_(Norway)"/>
/// <seealso href="https://www.skatteetaten.no/en/person/national-registry/identitetsnummer/fodselsnummer/"/>
/// </remarks>
[JsonConverter(typeof(ParsableJsonConverter<Fodselsnummer>))]
[TypeConverter(typeof(ParsableTypeConverter<Fodselsnummer>))]
public readonly struct Fodselsnummer :
    IIdNumber,
    IParsable<Fodselsnummer>,
    ISafeParsable<Fodselsnummer>,
    IEquatable<Fodselsnummer>,
    IComparable<Fodselsnummer>,
    IRandomValueFactory<Fodselsnummer>,
    IComparisonOperators<Fodselsnummer, Fodselsnummer, bool>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string? _value;

    private Fodselsnummer(string? value)
    {
        _value = StringUtils.RemoveWhitespace(value);
        IsValid = NummerParser.IsFodselsnummer(_value);
    }
    
    public DateOnly BirthDate 
    {
        get
        {
            if(!IsValid || !NummerParser.ValidateBirthDate(_value!, out var date)) return default;
            return date;
        }
    }

    public Gender Gender
    {
        get
        {
            if(!IsValid) return default;
            var genderDigit = _value![8] - '0';
            return genderDigit % 2 == 0 ? Gender.Female : Gender.Male;
        }
    }
    
    public bool IsValid { get; }
    public static Fodselsnummer Parse(string s, IFormatProvider? provider)
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

    public static Fodselsnummer CreateNew(string? value, IFormatProvider? provider = null) => Parser.SafeParse<Fodselsnummer>(value, provider);
    public static Fodselsnummer CreateNew()
    {
        var date = DateOnly.FromDateTime(new DateTime(1940, 1, 1) + TimeSpan.FromDays(Random.Shared.Next(365 * 100)));
        var gender = Gender.Undefined;
        return CreateNew(NummerFactory.CreateNew(NummerType.Fodselsnummer ,date, gender));
    }

    public override string ToString() => _value ?? string.Empty;

    public bool Equals(Fodselsnummer other) => _value == other._value;

    public override bool Equals(object? obj) => obj is Fodselsnummer other && Equals(other);

    public override int GetHashCode() => (_value != null ? _value.GetHashCode() : 0);

    public static bool operator ==(Fodselsnummer left, Fodselsnummer right) => left.Equals(right);

    public static bool operator !=(Fodselsnummer left, Fodselsnummer right) => !left.Equals(right);

    public int CompareTo(Fodselsnummer other)
    {
        var isValidComparison = IsValid.CompareTo(other.IsValid);
        if (isValidComparison != 0)
        {
            return isValidComparison;
        }

        return string.Compare(_value, other._value, StringComparison.Ordinal);
    }

    public static bool operator <(Fodselsnummer left, Fodselsnummer right) => left.CompareTo(right) < 0;

    public static bool operator >(Fodselsnummer left, Fodselsnummer right) => left.CompareTo(right) > 0;

    public static bool operator <=(Fodselsnummer left, Fodselsnummer right) => left.CompareTo(right) <= 0;

    public static bool operator >=(Fodselsnummer left, Fodselsnummer right) => left.CompareTo(right) >= 0;
}