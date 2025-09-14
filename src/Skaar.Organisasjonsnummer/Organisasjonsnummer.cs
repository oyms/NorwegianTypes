using Skaar.TypeSupport.Contracts;
using Skaar.TypeSupport.Serialization;
using Skaar.TypeSupport.Utils;
using Skaar.Utils;
using Skaar.ValueType;
using System.Diagnostics;
using System.Numerics;

namespace Skaar;

/// <summary>
/// This type represents a Norwegian organisation number.
/// Every legal entity in Norway is assigned such a number.
/// </summary>
/// <remarks>
/// <seealso href="https://www.brreg.no/en/about-us-2/our-registers/about-the-central-coordinating-register-for-legal-entities-ccr/about-the-organisation-number/?nocache=1743537931351"/>
/// </remarks>
[DebuggerDisplay("{ToString(OrganisasjonsnummerFormatting.WithSpaces)}")]
[ValueType]
public readonly partial struct Organisasjonsnummer : 
    ISpanFormattable,
    IComparable<Organisasjonsnummer>,
    IComparisonOperators<Organisasjonsnummer, Organisasjonsnummer, bool>,
    ICanBeValid,
    IHasLength,
    IRandomValueFactory<Organisasjonsnummer>,
    ISafeParsable<Organisasjonsnummer>
{
    private partial bool ValueIsValid(ReadOnlySpan<char> value) => ValidationRules.ValidateNumber(value);
    private partial ReadOnlySpan<char> Clean(ReadOnlySpan<char> value) => Helper.Clean.RemoveNonDigits(value);
    
    /// <summary>
    /// Creates a new random valid value.
    /// </summary>
    public static Organisasjonsnummer CreateNew() => ValidationRules.GenerateRandom();

    /// <summary>
    /// Parses the string (as with <see cref="Parse(string, IFormatProvider)"/>
    /// but will not throw expcetion when invalid.
    /// <seealso cref="IsValid"/>
    /// </summary>
    public static Organisasjonsnummer CreateNew(string? value, IFormatProvider? provider = null) 
        => Parser.SafeParse<Organisasjonsnummer>(value, provider);

    

    /// <summary>
    /// Returns the inner value formatted.
    /// </summary>
    /// <param name="formatting">Determines the format of the output.</param>
    /// <remarks>If inner value is <c>null</c>, an empty string is returned.</remarks>
    public string ToString(OrganisasjonsnummerFormatting formatting)
    {
        if (!IsValid) return ToString();
        return formatting switch
        {
            OrganisasjonsnummerFormatting.WithSpaces => ValidationRules.FormatNumberWithSpacing(this),
            OrganisasjonsnummerFormatting.OrgIdFormat => $"NO-BRC-{_value}",
            _ => ToString()
        };
    }
    
    public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider) =>
        StringUtils.TryFormatIgnoringFormatting(_value, destination, out charsWritten, format, provider);
    
    public int CompareTo(Organisasjonsnummer other) => StringUtils.MemoryCompare(_value, other._value);

    public static bool operator >(Organisasjonsnummer left, Organisasjonsnummer right) => left.CompareTo(right) > 0;

    public static bool operator >=(Organisasjonsnummer left, Organisasjonsnummer right) => left.CompareTo(right) >= 0;

    public static bool operator <(Organisasjonsnummer left, Organisasjonsnummer right) => left.CompareTo(right) < 0;

    public static bool operator <=(Organisasjonsnummer left, Organisasjonsnummer right) => left.CompareTo(right) <= 0;
}