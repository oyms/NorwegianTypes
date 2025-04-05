namespace Skaar;

/// <summary>
/// A record from the BIC lookup table
/// <seealso href="https://www.bits.no/document/iban/"/>
/// </summary>
/// <param name="Bic">BIC</param>
/// <param name="Name">Bank name</param>
public record Bank(string Bic, string Name)
{
    public static Bank Undefined => new (string.Empty, "Unknown");
}