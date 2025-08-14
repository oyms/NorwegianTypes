using Skaar.TypeSupport.Contracts;
using Skaar.TypeSupport.Serialization;
using Skaar.TypeSupport.Utils;
using Skaar.Utils;
using Skaar.ValueType;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Skaar;

/// <summary>
/// This type represents a Norwegian bank account number.
/// </summary>
/// <remarks>
/// <seealso href="https://no.wikipedia.org/wiki/Kontonummer"/>
/// <seealso href="https://www.bits.no/en/document/standard-for-kontonummer-i-norsk-banknaering-ver10/"/>
/// </remarks>
[DebuggerDisplay("{ToString(KontonummerFormatting.Periods)}")]
[ValueType]
public readonly partial struct Kontonummer : ISafeParsable<Kontonummer>,
    ISpanFormattable,
    ICanBeValid,
    IHasLength,
    IComparable<Kontonummer>,
    IComparisonOperators<Kontonummer,Kontonummer, bool>,
    IRandomValueFactory<Kontonummer>
{
    
    private partial bool ValueIsValid(ReadOnlySpan<char> value) => ValueParser.ValidateNumber(value);
    private partial ReadOnlySpan<char> Clean(ReadOnlySpan<char> value) => Helper.Clean.RemoveNonDigits(value);
   
    
    public static Kontonummer CreateNew(string? value, IFormatProvider? provider = null) => Parser.SafeParse<Kontonummer>(value, provider);
    
    [MemberNotNullWhen(true, nameof(_value))]

    public string ToString(KontonummerFormatting formatting)
    {
        var that = this;
        string FormatIban()
        {
            var iban = that.Iban;
            return $"{iban[..4]} {iban[4..8]} {iban[8..12]} {iban[12..]}";
        }
        if (!IsValid) return _value.ToString();
        return formatting switch
        {
            KontonummerFormatting.None => _value.ToString(),
            KontonummerFormatting.Periods => $"{_value[..4]}.{_value[4..6]}.{_value[6..]}",
            KontonummerFormatting.Spaces => $"{_value[..4]} {_value[4..6]} {_value[6..]}",
            KontonummerFormatting.IbanScreen => Iban,
            KontonummerFormatting.IbanPrint => FormatIban(),
            _ => throw new ArgumentException("Unknown formatting")
        };
    }
    
    public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider) =>
        StringUtils.TryFormatIgnoringFormatting(_value, destination, out charsWritten, format, provider);

    /// <summary>
    /// IBAN (International Bank Account Number)
    /// <seealso href="https://en.wikipedia.org/wiki/International_Bank_Account_Number"/>
    /// </summary>
    public string Iban
    {
        get
        {
            if (!IsValid) return string.Empty;
            return ValueParser.GetIbanNumber(_value.Span);
        }
    }

    /// <summary>
    /// The type, based on the account series
    /// </summary>
    public AccountType AccountType => IsValid ? ValueParser.GetAccountType(_value.Span) : AccountType.Undefined;
    
    /// <summary>
    /// The bank holding the account (based on a lookup in the bic registry)
    /// </summary>
    public Bank Bank => IsValid ? BicRepository.Lookup(_value.Span[..4].ToString()) : Bank.Undefined;
    
    public static Kontonummer CreateNew() => CreateNew(Factory.GenerateRandom());

    public int CompareTo(Kontonummer other)
    {
        var isValidComparison = IsValid.CompareTo(other.IsValid);
        if (isValidComparison != 0)
        {
            return isValidComparison;
        }

        return StringUtils.MemoryCompare(_value, other._value);
    }

    public static bool operator <(Kontonummer left, Kontonummer right) => left.CompareTo(right) < 0;

    public static bool operator >(Kontonummer left, Kontonummer right) => left.CompareTo(right) > 0;

    public static bool operator <=(Kontonummer left, Kontonummer right) => left.CompareTo(right) <= 0;

    public static bool operator >=(Kontonummer left, Kontonummer right) => left.CompareTo(right) >= 0;
}