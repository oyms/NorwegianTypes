namespace Skaar.Utils;

internal static class NummerFactory
{
    public static string CreateNew(NummerType type, DateOnly date, Gender gender) =>
        type switch
        {
            NummerType.Fodselsnummer => CreateFodselsnummer(date, date.ToString("ddMMyy"), gender),
            _ => throw new NotSupportedException("Type not supported")
        };

    private static string CreateFodselsnummer(DateOnly date, string encodedDate, Gender gender)
    {
        var individualPart = GetIndiviualNumber(date.Year, gender);

        var number = $"{encodedDate}{individualPart:000}";
        if (NummerParser.TryGetChecksum(NummerType.Fodselsnummer, number.Select(c => c - '0').ToArray(), out int checksum))
        {
            return $"{number}{checksum:00}";
        }

        return CreateFodselsnummer(date, encodedDate, gender);
    }

    private static int GetIndiviualNumber(int year, Gender gender)
    {
        int GetNumberInRange(int min, int max, Gender gender)
        {
            var number = Random.Shared.Next(min, max);
            if (gender == Gender.Female && number % 2 == 1)
            {
                return GetNumberInRange(min, max, gender);
            }
            return number;
        }

        return year switch
        {
            < 1900 => GetNumberInRange(500, 749, gender),
            >= 1940 and < 2000 => GetNumberInRange(900, 999, gender),
            < 2000 => GetNumberInRange(000, 499, gender),
            _ => GetNumberInRange(500, 999, gender)
        };
    }
    
}