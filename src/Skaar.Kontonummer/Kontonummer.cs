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
/// This type represents a Norwegian bank account number.
/// </summary>
/// <remarks>
/// <seealso href="https://no.wikipedia.org/wiki/Kontonummer"/>
/// <seealso href="https://www.bits.no/en/document/standard-for-kontonummer-i-norsk-banknaering-ver10/"/>
/// </remarks>
[JsonConverter(typeof(ParsableJsonConverter<Kontonummer>))]
[TypeConverter(typeof(ParsableTypeConverter<Kontonummer>))]
[DebuggerDisplay("{ToString(KontonummerFormatting.Periods)}")]
public readonly struct Kontonummer :
    IParsable<Kontonummer>,
    ISafeParsable<Kontonummer>,
    ICanBeValid,
    IEquatable<Kontonummer>,
    IComparable<Kontonummer>,
    IComparisonOperators<Kontonummer,Kontonummer, bool>,
    IRandomValueFactory<Kontonummer>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ReadOnlyMemory<char> _value;
    
    private Kontonummer(string? value)
    {
        _value = StringUtils.RemoveNonDigits(value);
        IsValid = ValueParser.ValidateNumber(_value.Span);
    }
    
    public static Kontonummer Parse(string s, IFormatProvider? provider = null)
    {
        if (!TryParse(s, provider, out var result) && !result.IsValid)
        {
            throw new FormatException("String is not a valid id-number.");
        }

        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Kontonummer result)
    {
        result = new Kontonummer(s);
        return result.IsValid;
    }
    
    public static Kontonummer CreateNew(string? value, IFormatProvider? provider = null) => Parser.SafeParse<Kontonummer>(value, provider);
    
    [MemberNotNullWhen(true, nameof(_value))]
    public bool IsValid { get; }

    public override string ToString() => ToString(KontonummerFormatting.None);

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
    
    public bool Equals(Kontonummer other) => _value.Span.SequenceEqual(other._value.Span);

    public static Kontonummer CreateNew() => CreateNew(Factory.GenerateRandom());

    public override bool Equals(object? obj) => obj is Kontonummer other && Equals(other);

    public override int GetHashCode() => _value.GetHashCode();

    public static bool operator ==(Kontonummer left, Kontonummer right) => left.Equals(right);

    public static bool operator !=(Kontonummer left, Kontonummer right) => !left.Equals(right);

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