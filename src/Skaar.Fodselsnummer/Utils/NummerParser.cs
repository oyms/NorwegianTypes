namespace Skaar.Utils;

internal static class NummerParser
{
    private static readonly int[] fNrWeights1 = [3, 7, 6, 1, 8, 9, 4, 5, 2];
    private static readonly int[] fNrWeights2 = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
    public static NummerType ParseIdNummer(string? value)
    {
        if (string.IsNullOrEmpty(value)) return NummerType.Invalid;
        if (IsFodselsnummer(value)) return NummerType.Fodselsnummer;
        return NummerType.Invalid;
    }

    private static bool IsFodselsnummer(string fnr)
    {
        if (fnr.Length != 11 || !fnr.All(char.IsDigit))
            return false;

        int[] digits = fnr.Select(c => c - '0').ToArray();

        // Første kontrollsiffer
        int sum1 = digits.Take(9).Select((d, i) => d * fNrWeights1[i]).Sum();
        int k1 = 11 - (sum1 % 11);
        if (k1 == 11) k1 = 0;
        if (k1 == 10 || k1 != digits[9]) return false;

        // Andre kontrollsiffer
        int sum2 = digits.Take(10).Select((d, i) => d * fNrWeights2[i]).Sum();
        int k2 = 11 - (sum2 % 11);
        if (k2 == 11) k2 = 0;
        if (k2 == 10 || k2 != digits[10]) return false;

        // Valider fødselsdato (DDMMYY)
        string datePart = fnr.Substring(0, 6);
        string individPart = fnr.Substring(6, 3);

        // Hent fødselsår ut fra individnummer
        int individNr = int.Parse(individPart);
        int year = int.Parse(datePart.Substring(4, 2));
        int fullYear;

        if (individNr >= 000 && individNr <= 499)
            fullYear = 1900 + year;
        else if (individNr >= 500 && individNr <= 749 && year >= 54)
            fullYear = 1800 + year;
        else if ((individNr >= 500 && individNr <= 999 && year <= 39))
            fullYear = 2000 + year;
        else if (individNr >= 900 && individNr <= 999 && year >= 40)
            fullYear = 1900 + year;
        else
            return false;

        string fullDate = $"{datePart.Substring(0, 2)}.{datePart.Substring(2, 2)}.{fullYear}";
        return DateTime.TryParse(fullDate, out _);
    }
}