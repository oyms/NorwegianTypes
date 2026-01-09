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
    
    private bool ValueIsValid(ReadOnlySpan<char> value) => ValueParser.ValidateNumber(value);
    private ReadOnlySpan<char> Clean(ReadOnlySpan<char> value) => Helper.Clean.RemoveNonDigits(value);
   
    
    public static Kontonummer CreateNew(string? value, IFormatProvider? provider = null) => Parser.SafeParse<Kontonummer>(value, provider);
    
    [MemberNotNullWhen(true, nameof(_value))]

    
    public string ToString(KontonummerFormatting formatting)
    {
        if (!IsValid) return ToString();
        return formatting switch
        {
            KontonummerFormatting.None => ToString(KontonummerFormats.General, null),
            KontonummerFormatting.Periods => ToString(KontonummerFormats.Periods, null),
            KontonummerFormatting.Spaces => ToString(KontonummerFormats.NonBreakingSpaces, null),
            KontonummerFormatting.IbanScreen => ToString(KontonummerFormats.IbanScreen, null),
            KontonummerFormatting.IbanPrint => ToString(KontonummerFormats.IbanPrint, null),
            _ => throw new ArgumentException("Unknown formatting")
        };
    }

    /// <summary>
    /// Formats the Kontonummer according to the specified format.
    /// </summary>
    /// <param name="format">
    /// Supported formats:
    /// <list type="table">
    /// <listheader><term>Format</term><description>Description</description></listheader>
    /// <item>
    /// <term>"G"</term>
    /// <description>General format (default).</description>
    /// </item>
    /// <item>
    /// <term>"N"</term>
    /// <description>Non-breaking spaces as separators.</description>
    /// </item>
    /// <item>
    /// <term>"P"</term>
    /// <description>Periods as separators.</description>
    /// </item>
    /// <item>
    /// <term>"S"</term>
    /// <description>IBAN electronic format</description>
    /// </item>
    /// <item>
    /// <term>"I"</term>
    /// <description>IBAN print format.</description>
    /// </item>
    /// </list>
    /// </param>
    /// <param name="formatProvider">Not in use</param>
    /// <returns>A formatted string</returns>
    /// <remarks>Returns default when <paramref name="format"/> is unknown.</remarks>
    /// <remarks>Returns the inner value if value is invalid.</remarks>
    /// <seealso cref="KontonummerFormats"/>
    /// <seealso cref="KontonummerFormatting"/>
    /// <example>
    /// <code language="csharp">
    /// value.ToString("G", CultureInfo.InvariantCulture); // "00000000000"
    /// value.ToString("N", CultureInfo.InvariantCulture); // "0000 00 00000"
    /// value.ToString("P", CultureInfo.InvariantCulture); // "0000.00.00000"
    /// value.ToString("S", CultureInfo.InvariantCulture); // "NO0000000000000"
    /// value.ToString("I", CultureInfo.InvariantCulture); // "NO00 0000 0000 000"
    /// </code>
    /// </example>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var that = this;
        string FormatIban()
        {
            var iban = that.Iban;
            return $"{iban[..4]} {iban[4..8]} {iban[8..12]} {iban[12..]}";
        }
        if (!IsValid) return ToString();
        return format switch
        {
            KontonummerFormats.NonBreakingSpaces => $"{_value[..4]} {_value[4..6]} {_value[6..]}",
            KontonummerFormats.Periods => $"{_value[..4]}.{_value[4..6]}.{_value[6..]}",
            KontonummerFormats.IbanScreen => Iban,
            KontonummerFormats.IbanPrint => FormatIban(),
            _ => ToString()
        };
    }

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