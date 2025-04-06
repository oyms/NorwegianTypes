using Skaar.Contracts;
using Skaar.TypeSupport.Utils;
using System.Diagnostics.CodeAnalysis;

namespace Skaar.Utils;

internal static class ValueParser
{
    private static readonly int[] FNrWeights1 = [3, 7, 6, 1, 8, 9, 4, 5, 2];
    private static readonly int[] FNrWeights2 = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
    private static readonly int[] DufNrWeights = [4, 6, 3, 2, 4, 6, 3, 2, 7, 5];
    public static NummerType ParseIdNummer(ReadOnlySpan<char> value)
    {
        if (value.Length == 0) return NummerType.Invalid;
        if (IsFodselsnummer(value)) return NummerType.Fodselsnummer;
        if (IsDNummer(value)) return NummerType.DNummer;
        if (IsDufNummer(value)) return NummerType.DufNummer;
        return NummerType.Invalid;
    }

    public static bool TryGetChecksumForFodselsnummer(ReadOnlySpan<char> number, [NotNullWhen(true)] out string? checksum)
    {
        if (number.Length != 9)
        {
            checksum = null;
            return false;
        }

        if (Mod11.TryGetChecksumDigit(number, FNrWeights1, out var checksum1))
        {
            Span<char> buffer = stackalloc char[10]; 

            number.CopyTo(buffer.Slice(0, 9));
            buffer[9] = checksum1;

            if (Mod11.TryGetChecksumDigit(buffer, FNrWeights2, out var checksum2))
            {
                checksum = $"{checksum1}{checksum2}";
                return true;
            }
        }

        checksum = null;
        return false;
    }

    public static bool IsFodselsnummer(ReadOnlySpan<char> number) => ValidateFnrControlNumber(number) && ValidateBirthDate(number, out _);

    public static bool IsDNummer(ReadOnlySpan<char> number)
    {
        if (number.Length != 11)
        {
            return false;
        }
        var firstDigit = (number[0] - '0');
        if (firstDigit is > 3 and <= 7)
        {
            Span<char> buffer = stackalloc char[11];
            number.CopyTo(buffer);
            buffer[0] = (char) ((firstDigit - 4) + '0');
            
            
            return ValidateFnrControlNumber(number) && ValidateBirthDate(buffer, out _);
        }
        return false;
    }

    public static bool IsDufNummer(ReadOnlySpan<char> number)
    {
        if (number.Length != 12)
        {
            return false;
        }
        var calculatedControlNumber = GetDufNummerControlDigits(number[..10]);
        var controlNumber = int.Parse(number[10..]);
        return controlNumber == calculatedControlNumber;
    }

    public static int GetDufNummerControlDigits(ReadOnlySpan<char> number)
    {
        if (number.Length != DufNrWeights.Length) throw new ArgumentException("Wrong length", nameof(number));
        var sum = 0;
        for (int i = 0; i < number.Length; i++)
        {
            sum += (number[i] - '0') * DufNrWeights[i];
        }
        return sum % 11;
    }

    private static bool ValidateFnrControlNumber(ReadOnlySpan<char> number)
    {
        if (number.Length != 11)
        {
            return false;
        }

        if (!Mod11.TryGetChecksumDigit(number[..9], FNrWeights1, out var checksum1) || checksum1 != number[9])
        {
            return false;
        }
        
        if (!Mod11.TryGetChecksumDigit(number[..10], FNrWeights2, out var checksum2) || checksum2 != number[10])
        {
            return false;
        }

        return true;
    }

    public static bool ValidateBirthDate(ReadOnlySpan<char> number, out DateOnly date)
    {
        var datePart = number[..6];
        var individPart = number[6..9];

        int individNr = int.Parse(individPart);
        int year = int.Parse(datePart[4..6]);
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
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        else if (individNr >= 500 && year <= 39)
        {
            fullYear = 2000 + year;
        }
        else
        {
            date = DateOnly.MinValue;
            return false;
        }

        Span<char> fulldate = stackalloc char[10];
        datePart[..2].CopyTo(fulldate);
        fulldate[2] = '.';
        datePart[2..4].CopyTo(fulldate.Slice(3));
        fulldate[5] = '.';
        fulldate[6] = (char) ('0' + fullYear / 1000 % 10);
        fulldate[7] = (char) ('0' + fullYear / 100 % 10);
        fulldate[8] = (char) ('0' + fullYear / 10 % 10);
        fulldate[9] = (char) ('0' + fullYear / 1 % 10);
        
        return DateOnly.TryParseExact(fulldate, "dd.MM.yyyy", out date);
    }
}