using Skaar.TypeSupport.Contracts;
using Skaar.TypeSupport.Serialization;
using Skaar.TypeSupport.Utils;
using Skaar.ValueType;
using System.Security.AccessControl;

namespace Skaar;

/// <summary>
/// This type represents a Norwegian car registration number.
/// </summary>
/// <seealso href="https://no.wikipedia.org/wiki/Kjennemerke_for_motorkj%C3%B8ret%C3%B8y_i_Norge"/>
[ValueType]
public readonly partial struct Registreringsnummer :
    ISpanFormattable,
    ICanBeValid,
    ISafeParsable<Registreringsnummer>
{
    private ReadOnlySpan<char> Clean(ReadOnlySpan<char> value) => Helper.Clean.RemoveNonLettersOrDigits(value);
    
    public string ToString(string? format, IFormatProvider? formatProvider) => ToString();
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider) =>
        StringUtils.TryFormatIgnoringFormatting(_value, destination, out charsWritten, format, provider);

    public static Registreringsnummer CreateNew(string? value, IFormatProvider? provider = null) => 
        Parser.SafeParse<Registreringsnummer>(value, provider);
}