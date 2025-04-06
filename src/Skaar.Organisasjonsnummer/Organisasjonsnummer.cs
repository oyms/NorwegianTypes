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
/// This type represents a Norwegian organisation number.
/// Every legal entity in Norway is assigned such a number.
/// </summary>
/// <remarks>
/// <seealso href="https://www.brreg.no/en/about-us-2/our-registers/about-the-central-coordinating-register-for-legal-entities-ccr/about-the-organisation-number/?nocache=1743537931351"/>
/// </remarks>
[JsonConverter(typeof(ParsableJsonConverter<Organisasjonsnummer>))]
[TypeConverter(typeof(ParsableTypeConverter<Organisasjonsnummer>))]
[DebuggerDisplay("{ToString(OrganisasjonsnummerFormatting.WithSpaces)}")]
public readonly struct Organisasjonsnummer : 
    ISpanParsable<Organisasjonsnummer>,
    ISpanFormattable,
    IEquatable<Organisasjonsnummer>, 
    IComparable<Organisasjonsnummer>,
    IComparisonOperators<Organisasjonsnummer, Organisasjonsnummer, bool>,
    ICanBeValid,
    IHasLength,
    IRandomValueFactory<Organisasjonsnummer>,
    ISafeParsable<Organisasjonsnummer>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ReadOnlyMemory<char> _value;

    private Organisasjonsnummer(ReadOnlySpan<char> value)
    {
        _value = StringUtils.RemoveNonDigits(value);
        IsValid = ValidationRules.ValidateNumber(_value.Span);
    }

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
    /// Parses a string to a value.
    /// </summary>
    /// <param name="s">The string containing the org number.</param>
    /// <param name="provider">A format provider.</param>
    /// <returns>A valid value.</returns>
    /// <exception cref="FormatException">
    /// Throws when the result is invalid.
    /// (<see cref="IsValid"/> is <c>false</c>)
    /// </exception>
    public static Organisasjonsnummer Parse(string s, IFormatProvider? provider = null)
    {
        if (!TryParse(s, provider, out var result) && !result.IsValid)
        {
            throw new FormatException("String is not a valid Organisasjonsnummer.");
        }

        return result;
    }

    /// <inheritdoc cref="IParsable{TSelf}"/>
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Organisasjonsnummer result)
    {
        result = new Organisasjonsnummer(s);
        return result.IsValid;
    }
    
    public static Organisasjonsnummer Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        if (!TryParse(s, provider, out var result) && !result.IsValid)
        {
            throw new FormatException("String is not a valid Organisasjonsnummer.");
        }

        return result;
    }

    /// <inheritdoc cref="IParsable{TSelf}"/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Organisasjonsnummer result)
    {
        result = new Organisasjonsnummer(s);
        return result.IsValid;
    }

    /// <summary>
    /// Returns the inner value.
    /// </summary>
    /// <remarks>If inner value is <c>null</c>, an empty string is returned.</remarks>
    public override string ToString() => _value.ToString();

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

    public override int GetHashCode() => HashCode.Combine(_value);

    public override bool Equals(object? obj) => obj is Organisasjonsnummer other && Equals(other);
    public bool Equals(Organisasjonsnummer other) => _value.Span.SequenceEqual(other._value.Span);
    public int CompareTo(Organisasjonsnummer other) => StringUtils.MemoryCompare(_value, other._value);

    public static bool operator ==(Organisasjonsnummer left, Organisasjonsnummer right) => left.Equals(right);

    public static bool operator !=(Organisasjonsnummer left, Organisasjonsnummer right) => !(left == right);

    public static bool operator >(Organisasjonsnummer left, Organisasjonsnummer right) => left.CompareTo(right) > 0;

    public static bool operator >=(Organisasjonsnummer left, Organisasjonsnummer right) => left.CompareTo(right) >= 0;

    public static bool operator <(Organisasjonsnummer left, Organisasjonsnummer right) => left.CompareTo(right) < 0;

    public static bool operator <=(Organisasjonsnummer left, Organisasjonsnummer right) => left.CompareTo(right) <= 0;
    
    /// <summary>
    /// Returns true if the value is valid.
    /// </summary>
    /// <seealso href="https://www.brreg.no/en/about-us-2/our-registers/about-the-central-coordinating-register-for-legal-entities-ccr/about-the-organisation-number/?nocache=1743537931351"/>
    
    [MemberNotNullWhen(true, nameof(_value))]
    public bool IsValid { get; }

    public int Length => _value.Length;
}