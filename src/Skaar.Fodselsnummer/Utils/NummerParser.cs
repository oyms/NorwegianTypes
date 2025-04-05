using Skaar.Contracts;

namespace Skaar.Utils;

internal static class NummerParser
{
    private static readonly int[] FNrWeights1 = [3, 7, 6, 1, 8, 9, 4, 5, 2];
    private static readonly int[] FNrWeights2 = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
    private static readonly int[] DufNrWeights = [4, 6, 3, 2, 4, 6, 3, 2, 7, 5];
    public static NummerType ParseIdNummer(string? value)
    {
        if (string.IsNullOrEmpty(value)) return NummerType.Invalid;
        if (IsFodselsnummer(value)) return NummerType.Fodselsnummer;
        if (IsDNummer(value)) return NummerType.DNummer;
        if (IsDufNummer(value)) return NummerType.DufNummer;
        return NummerType.Invalid;
    }

    public static bool TryGetChecksum(NummerType nummerType,int[] digits, out int result)
    {
        switch (nummerType)
        {
            case NummerType.Fodselsnummer:
            case NummerType.DNummer:
                {
                    var sum1 = digits.Take(9).Select((d, i) => d * FNrWeights1[i]).Sum();
                    var k1 = 11 - (sum1 % 11);
                    if (k1 == 11) k1 = 0;
                    if (k1 == 10)
                    {
                        result = 0;
                        return false;
                    };
                    var sum2 = digits.Append(k1).Select((d, i) => d * FNrWeights2[i]).Sum();
                    var k2 = 11 - (sum2 % 11);
                    if (k2 == 11) k2 = 0;
                    if (k2 == 10)
                    {
                        result = 0;
                        return false;
                    }

                    result = k1 * 10 + k2;
                    return true;
                }
            default: throw new NotSupportedException();
        }
    }

    public static bool IsFodselsnummer(string? number) => number is not null && ValidateControlNumber(number) && ValidateBirthDate(number, out _);

    public static bool IsDNummer(string? number)
    {
        if (number is null) return false;
        if (number.Length != 11 || !number.All(char.IsDigit))
            return false;
        var firstDigit = (number[0] - '0');
        if (firstDigit > 3 && firstDigit <= 7)
        {
            return ValidateControlNumber(number) && ValidateBirthDate(firstDigit - 4 + number[1..], out _);
        }
        return false;
    }

    public static bool IsDufNummer(string? number)
    {
        if (number is null) return false;
        if (number.Length != 12 || !number.All(char.IsDigit))
            return false;
        var calculatedControlNumber = GetDufNummerControlDigits(number[..10]);
        var controlNumber = int.Parse(number[10..]);
        return controlNumber == calculatedControlNumber;
    }

    public static int GetDufNummerControlDigits(string number)
    {
        if (number.Length != 10) throw new ArgumentException("Wrong length", nameof(number));
        var digits = number.Select(c => c - '0').ToArray();
        var sum = digits[..10].Select((d, i) => d * DufNrWeights[i]).Sum();
        return sum % 11;
    }

    private static bool ValidateControlNumber(string number)
    {
        if (number.Length != 11 || !number.All(char.IsDigit))
            return false;

        var digits = number.Select(c => c - '0').ToArray();

        // First control
        var sum1 = digits.Take(9).Select((d, i) => d * FNrWeights1[i]).Sum();
        var k1 = 11 - (sum1 % 11);
        if (k1 == 11) k1 = 0;
        if (k1 == 10 || k1 != digits[9]) return false;

        // Second control
        var sum2 = digits.Take(10).Select((d, i) => d * FNrWeights2[i]).Sum();
        var k2 = 11 - (sum2 % 11);
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

        if (individNr < 500) //1900-1999
        {
            fullYear = year + 1900;
        }
        else if (individNr <= 749) //1854-1899
        {
            if (year >= 54)
            {
                fullYear = 1800 + year;
            }
            else
            {
                fullYear = 2000 + year;
            }
        }
        else if (individNr >= 900)
        {
            if (year >= 40)
            {
                fullYear = 1900 + year;
            }
            else
            {
                fullYear = 2000 + year;
            }
        }
        else if (individNr >= 500 && year <= 39)
        {
            fullYear = 2000 + year;
        }
        else
        {
            date = DateOnly.MinValue;
            return false;
        }

        string fullDate = $"{datePart.Substring(0, 2)}.{datePart.Substring(2, 2)}.{fullYear}";
        return DateOnly.TryParseExact(fullDate, "dd.MM.yyyy", out date);
    }
}