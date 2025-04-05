using Skaar.TypeSupport.Utils;
using System.Text.RegularExpressions;

namespace Skaar.Utils;

internal static partial class ValidationRules
{
    private static readonly int[] Weights = [3, 2, 7, 6, 5, 4, 3, 2];
    
    /// <remarks>
    /// <seealso href="https://www.brreg.no/en/about-us-2/our-registers/about-the-central-coordinating-register-for-legal-entities-ccr/about-the-organisation-number/?nocache=1743537931351"/>
    /// <seealso href="https://no.wikipedia.org/wiki/Organisasjonsnummer"/>
    /// <seealso href="https://info.altinn.no/starte-og-drive/starte/registrering/organisasjonsnummer/"/>
    /// </remarks>
    public static bool ValidateNumber(string? number)
    {
        if (number == null || 
            number.Length != 9 || 
            !number.All(char.IsDigit) 
            || !Mod11.TryGetChecksumDigit(number[..8], Weights, out var checksum))
        {
            return false;
        }
        
        return checksum == number.Last();
    }

    public static Organisasjonsnummer GenerateRandom()
    {
        var randomNumber = Random.Shared.Next(0, 99999999).ToString("00000000");
        if (!Mod11.TryGetChecksumDigit(randomNumber, Weights, out var checksum))
        {
            return GenerateRandom();
        }

        return Organisasjonsnummer.Parse($"{randomNumber}{checksum}");
    }

    public static string FormatNumberWithSpacing(Organisasjonsnummer number) => InsertSpacesPattern().Replace(number.ToString(), "$1\u00A0");


    [GeneratedRegex(@"(\d{3})(?=\d)")]
    private static partial Regex InsertSpacesPattern();
}