using Skaar.TypeSupport.Contracts;
using Skaar.ValueType;

namespace Skaar;

/// <summary>
/// This type represents a Norwegian car registration number.
/// </summary>
/// <seealso href="https://no.wikipedia.org/wiki/Kjennemerke_for_motorkj%C3%B8ret%C3%B8y_i_Norge"/>
[ValueType]
public readonly partial struct Registreringsnummer :
    ICanBeValid
{
    private ReadOnlySpan<char> Clean(ReadOnlySpan<char> value) => Helper.Clean.RemoveNonLettersOrDigits(value);
}