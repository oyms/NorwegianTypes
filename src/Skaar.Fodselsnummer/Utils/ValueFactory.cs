using Skaar.Contracts;

namespace Skaar.Utils;

internal static class ValueFactory
{
    public static string CreateNew(NummerType type, DateOnly date, Gender gender) =>
        type switch
        {
            NummerType.Fodselsnummer => CreateFodselsnummer(date, date.ToString("ddMMyy"), gender),
            NummerType.DNummer => CreateDNummer(date, gender),
            NummerType.DufNummer => CreateDufNummer(date),
            _ => throw new NotSupportedException("Type not supported")
        };

    private static string CreateFodselsnummer(DateOnly date, string encodedDate, Gender gender)
    {
        var individualPart = GetIndiviualNumber(date.Year, gender);

        var number = $"{encodedDate}{individualPart:000}";
        if (ValueParser.TryGetChecksum(NummerType.Fodselsnummer, number.Select(c => c - '0').ToArray(), out int checksum))
        {
            return $"{number}{checksum:00}";
        }

        return CreateFodselsnummer(date, encodedDate, gender);
    }

    private static string CreateDNummer(DateOnly date, Gender gender)
    {
        var encodedDate = $"{date.Day + 40:00}{date:MMyy}";
        return CreateFodselsnummer(date, encodedDate, gender);
    }

    private static string CreateDufNummer(DateOnly date)
    {
        var firstPart = $"{date:yyyy}{Random.Shared.Next(1, 999999):000000}";
        return $"{firstPart}{ValueParser.GetDufNummerControlDigits(firstPart):00}";
    }

    private static int GetIndiviualNumber(int year, Gender gender)
    {
        int GetNumberInRange(int min, int max)
        {
            var number = Random.Shared.Next(min, max);
            if (gender == Gender.Female && number % 2 == 1)
            {
                return GetNumberInRange(min, max);
            }
            return number;
        }

        return year switch
        {
            < 1900 => GetNumberInRange(500, 749),
            >= 1940 and < 2000 => GetNumberInRange(900, 999),
            < 2000 => GetNumberInRange(000, 499),
            _ => GetNumberInRange(500, 999)
        };
    }
    
}