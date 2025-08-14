using Skaar.Contracts;
using Skaar.TypeSupport.Contracts;
using Skaar.TypeSupport.Serialization;
using Skaar.TypeSupport.Utils;
using Skaar.Utils;
using Skaar.ValueType;
using System.Numerics;

namespace Skaar;

/// <summary>
/// This type represents a Norwegian identity number
/// (DUF-nummer)
/// </summary>
/// <remarks>
/// <seealso href="https://www.udi.no/en/word-definitions/duf-number/"/>
/// </remarks>
[ValueType]
public readonly partial struct DufNummer :
    IIdNumber,
    ISafeParsable<DufNummer>,
    ISpanFormattable,
    IHasLength,
    IComparable<DufNummer>,
    IRandomValueFactory<DufNummer>,
    IComparisonOperators<DufNummer, DufNummer, bool>
{
    
    private partial bool ValueIsValid(ReadOnlySpan<char> value) => ValueParser.IsDufNummer(value);

    private partial ReadOnlySpan<char> Clean(ReadOnlySpan<char> value) => Helper.Clean.RemoveNonDigits(value);
    
    public static DufNummer CreateNew(string? value, IFormatProvider? provider = null) => Parser.SafeParse<DufNummer>(value, provider);
    public static DufNummer CreateNew()
    {
        var date = DateOnly.FromDateTime(new DateTime(1940, 1, 1) + TimeSpan.FromDays(Random.Shared.Next(365 * 100)));
        var gender = Gender.Undefined;
        return CreateNew(ValueFactory.CreateNew(NummerType.DufNummer ,date, gender));
    }
    
    public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider) =>
        StringUtils.TryFormatIgnoringFormatting(_value, destination, out charsWritten, format, provider);
    

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