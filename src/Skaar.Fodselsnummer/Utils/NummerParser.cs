namespace Skaar.Utils;

internal static class NummerParser
{
    private static readonly int[] FNrWeights1 = [3, 7, 6, 1, 8, 9, 4, 5, 2];
    private static readonly int[] FNrWeights2 = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
    public static NummerType ParseIdNummer(string? value)
    {
        if (string.IsNullOrEmpty(value)) return NummerType.Invalid;
        if (IsFodselsnummer(value)) return NummerType.Fodselsnummer;
        if (IsDNummer(value)) return NummerType.DNummer;
        return NummerType.Invalid;
    }

    private static bool IsFodselsnummer(string fnr) => ValidateControlNumber(fnr) && ValidateBirthDate(fnr, out _);

    private static bool IsDNummer(string value)
    {
        if (value.Length != 11 || !value.All(char.IsDigit))
            return false;
        var firstDigit = (value[0] - '0');
        if (firstDigit > 2 && firstDigit <= 6)
        {
            return ValidateControlNumber(value) && ValidateBirthDate(firstDigit - 4 + value[1..], out _);
        }
        return false;
    }

    private static bool ValidateControlNumber(string number)
    {
        if (number.Length != 11 || !number.All(char.IsDigit))
            return false;

        int[] digits = number.Select(c => c - '0').ToArray();

        // Første kontrollsiffer
        int sum1 = digits.Take(9).Select((d, i) => d * FNrWeights1[i]).Sum();
        int k1 = 11 - (sum1 % 11);
        if (k1 == 11) k1 = 0;
        if (k1 == 10 || k1 != digits[9]) return false;

        // Andre kontrollsiffer
        int sum2 = digits.Take(10).Select((d, i) => d * FNrWeights2[i]).Sum();
        int k2 = 11 - (sum2 % 11);
        if (k2 == 11) k2 = 0;
        if (k2 == 10 || k2 != digits[10]) return false;
        return true;
    }

    public static bool ValidateBirthDate(string number, out DateOnly date)
    {
        // Valider fødselsdato (DDMMYY)
        string datePart = number.Substring(0, 6);
        string individPart = number.Substring(6, 3);

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
        {
            date = DateOnly.MinValue;
            return false;
        }

        string fullDate = $"{datePart.Substring(0, 2)}.{datePart.Substring(2, 2)}.{fullYear}";
        return DateOnly.TryParse(fullDate, out date);
    }
}