using System.Text.RegularExpressions;

namespace Skaar;

internal static partial class Utils
{
    /// <remarks>
    /// <seealso href="https://www.brreg.no/en/about-us-2/our-registers/about-the-central-coordinating-register-for-legal-entities-ccr/about-the-organisation-number/?nocache=1743537931351"/>
    /// <seealso href="https://no.wikipedia.org/wiki/Organisasjonsnummer"/>
    /// <seealso href="https://info.altinn.no/starte-og-drive/starte/registrering/organisasjonsnummer/"/>
    /// </remarks>
    public static bool ValidateNumber(string? number)
    {
        if (number == null || number.Length != 9 || !number.All(char.IsDigit))
        {
            return false;
        }

        var weights = new[] { 3, 2, 7, 6, 5, 4, 3, 2 };
        int sum = 0;

        for (int i = 0; i < 8; i++)
        {
            sum += (number[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = remainder == 0 ? 0 : 11 - remainder;

        return checkDigit != 10 && checkDigit == number[8] - '0';
    }

    public static Organisasjonsnummer GenerateRandom()
    {
            var random = new Random();
            int[] weights = { 3, 2, 7, 6, 5, 4, 3, 2 };

            while (true)
            {
                int[] digits = new int[9];

                // Lag 8 tilfeldige sifre
                for (int i = 0; i < 8; i++)
                {
                    digits[i] = random.Next(0, 10);
                }

                // Regn ut kontrollsiffer
                int sum = 0;
                for (int i = 0; i < 8; i++)
                {
                    sum += digits[i] * weights[i];
                }

                int remainder = sum % 11;
                int checkDigit = 11 - remainder;

                if (checkDigit == 11) checkDigit = 0;
                if (checkDigit == 10) continue; // ugyldig, prøv på nytt

                digits[8] = checkDigit;

                return Organisasjonsnummer.Parse(string.Concat(digits));
            }
    }
    
    public static string? RemoveWhitespace(string? rawValue)
    {
        if (rawValue == null)
        {
            return rawValue;
        }

        return RemoveSpacesPattern().Replace(rawValue, "");
    }

    public static string FormatNumberWithSpacing(Organisasjonsnummer number) => InsertSpacesPattern().Replace(number.ToString(), "$1\u00A0");


    [GeneratedRegex(@"NO\-BRC\-|\s+")]
    private static partial Regex RemoveSpacesPattern();
    [GeneratedRegex(@"(\d{3})(?=\d)")]
    private static partial Regex InsertSpacesPattern();
}