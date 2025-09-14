using Skaar.Contracts;
using Skaar.TypeSupport.Contracts;
using Skaar.TypeSupport.Serialization;
using Skaar.TypeSupport.Utils;
using Skaar.Utils;
using Skaar.ValueType;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Skaar;

/// <summary>
/// This type represents a Norwegian identity number
/// (Fødselsnummer, D-nummer, DUF-nummer)
/// </summary>
/// <remarks>
/// <seealso href="https://en.wikipedia.org/wiki/National_identity_number_(Norway)"/>
/// </remarks>
[ValueType]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct IdNumber :
    IIdNumber,
    ISafeParsable<IdNumber>,
    ISpanFormattable,
    IHasLength,
    IComparable<IdNumber>,
    IComparisonOperators<IdNumber, IdNumber, bool>,
    IRandomValueFactory<IdNumber>
{

    private IdNumber(ReadOnlySpan<char> value)
    {
        _value = Clean(value).ToArray();
        Type = ValueParser.ParseIdNummer(_value.Span);
    }
    
    #pragma warning disable CS8826 // Throwaway parameter
    private partial bool ValueIsValid(ReadOnlySpan<char> _) => Type != NummerType.Invalid;
    private partial ReadOnlySpan<char> Clean(ReadOnlySpan<char> value) => Helper.Clean.RemoveNonDigits(value);

    public NummerType Type { get; }
    
    public static IdNumber CreateNew(string? value, IFormatProvider? provider = null) => Parser.SafeParse<IdNumber>(value, provider);
        
    public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider) =>
        StringUtils.TryFormatIgnoringFormatting(_value, destination, out charsWritten, format, provider);

    public static IdNumber CreateNew()
    {
        var date = DateOnly.FromDateTime(new DateTime(1940, 1, 1) + TimeSpan.FromDays(Random.Shared.Next(365 * 100)));
        var gender = Gender.Undefined;
        return CreateNew(ValueFactory.CreateNew(NummerType.Fodselsnummer ,date, gender));
    }

    public int CompareTo(IdNumber other) => StringUtils.MemoryCompare(_value, other._value);

    public static bool operator >(IdNumber left, IdNumber right) => left.CompareTo(right) > 0;

    public static bool operator >=(IdNumber left, IdNumber right) => left.CompareTo(right) >= 0;

    public static bool operator <(IdNumber left, IdNumber right) => left.CompareTo(right) < 0;

    public static bool operator <=(IdNumber left, IdNumber right) => left.CompareTo(right) <= 0;

    public static explicit operator IdNumber(Fodselsnummer fodselsnummer) => CreateNew(fodselsnummer.ToString());
    public static explicit operator IdNumber(DNummer dNummer) => CreateNew(dNummer.ToString());
    public static explicit operator IdNumber(DufNummer dufNummer) => CreateNew(dufNummer.ToString());
}