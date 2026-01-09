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
    private bool ValueIsValid(ReadOnlySpan<char> value) => ValidationRules.ValidateNumber(value);
    private ReadOnlySpan<char> Clean(ReadOnlySpan<char> value) => Helper.Clean.RemoveNonDigits(value);
    
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

    /// <summary>
    /// Formats the number according to the specified format.
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
    /// <term>"S"</term>
    /// <description>Triples interspaced with non-breaking space.</description>
    /// </item>
    /// <item>
    /// <term>"O"</term>
    /// <description><see href="https://org-id.guide/list/NO-BRC"/></description>
    /// </item>
    /// </list>
    /// </param>
    /// <param name="formatProvider">Not in use</param>
    /// <returns>A formatted string</returns>
    /// <remarks>Returns default when <paramref name="format"/> is unknown.</remarks>
    /// <remarks>Returns the inner value if value is invalid.</remarks>
    /// <seealso cref="OrganisasjonsnummerFormatting"/>
    /// <example>
    /// <code language="csharp">
    /// value.ToString("G", CultureInfo.InvariantCulture); // "968253980"
    /// value.ToString("S", CultureInfo.InvariantCulture); // "894 961 902"
    /// value.ToString("O", CultureInfo.InvariantCulture); // "NO-BRC-972417920"
    /// </code>
    /// </example>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (!IsValid) return ToString();
        return format switch
        {
            "S" => ToString(OrganisasjonsnummerFormatting.WithSpaces),
            "O" => ToString(OrganisasjonsnummerFormatting.OrgIdFormat),
            _ => ToString()
        };
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider) =>
        StringUtils.TryFormatIgnoringFormatting(_value, destination, out charsWritten, format, provider);
    
    public int CompareTo(Organisasjonsnummer other) => StringUtils.MemoryCompare(_value, other._value);

    public static bool operator >(Organisasjonsnummer left, Organisasjonsnummer right) => left.CompareTo(right) > 0;

    public static bool operator >=(Organisasjonsnummer left, Organisasjonsnummer right) => left.CompareTo(right) >= 0;

    public static bool operator <(Organisasjonsnummer left, Organisasjonsnummer right) => left.CompareTo(right) < 0;

    public static bool operator <=(Organisasjonsnummer left, Organisasjonsnummer right) => left.CompareTo(right) <= 0;
}