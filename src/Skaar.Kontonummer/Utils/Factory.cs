namespace Skaar.Utils;

internal static class Factory
{
    public static string GenerateRandom()
    {
        var bank = BicRepository.GetRandomId();
        var accountType = Random.Shared.Next(1, 89);
        var accountNumber = Random.Shared.Next(0, 9999);
        var number = $"{bank}{accountType:00}{accountNumber:0000}";
        if(!ValueParser.TryGetControlDigit(number, out var controlDigit))
        {
            return GenerateRandom();
        }
        return number + controlDigit;
    }
}