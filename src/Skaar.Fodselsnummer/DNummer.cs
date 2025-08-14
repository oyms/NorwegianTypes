using Skaar.Contracts;
using Skaar.TypeSupport.Contracts;
using Skaar.TypeSupport.Serialization;
using Skaar.TypeSupport.Utils;
using Skaar.Utils;
using Skaar.ValueType;
using System.Numerics;

namespace Skaar;

/// <summary>
/// This type represents a Norwegian D-number
/// (D-nummer)
/// </summary>
/// <remarks>
/// <seealso href="https://www.skatteetaten.no/en/person/national-registry/identitetsnummer/d-nummer/"/>
/// </remarks>
[ValueType]
public readonly partial struct DNummer :
    IIdNumber,
    ISafeParsable<DNummer>,
    ISpanFormattable,
    IHasLength,
    IComparable<DNummer>,
    IRandomValueFactory<DNummer>,
    IComparisonOperators<DNummer, DNummer, bool>
{
    

    private partial bool ValueIsValid(ReadOnlySpan<char> value) => ValueParser.IsDNummer(value);

    private partial ReadOnlySpan<char> Clean(ReadOnlySpan<char> value) => Helper.Clean.RemoveNonDigits(value);


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
    
    public static DNummer CreateNew(string? value, IFormatProvider? provider = null) => Parser.SafeParse<DNummer>(value, provider);
    public static DNummer CreateNew()
    {
        var date = DateOnly.FromDateTime(new DateTime(1940, 1, 1) + TimeSpan.FromDays(Random.Shared.Next(365 * 100)));
        var gender = Gender.Undefined;
        return CreateNew(ValueFactory.CreateNew(NummerType.DNummer, date, gender));
    }
        
    public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider) =>
        StringUtils.TryFormatIgnoringFormatting(_value, destination, out charsWritten, format, provider);

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