namespace Skaar.Utils;

internal static class ValueParser
{
    private static readonly int[] Weights = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
    
    /// <summary>
    /// Value is already trimmed/cleaned of spacing
    /// </summary>
    public static bool ValidateNumber(string? value)
    {
        if (value is null) return false;
        if (value.Length != 11 || !value.All(char.IsDigit)) return false;
        if (!TryGetControlDigit(value[..10], out var control))
        {
            return false;
        }
        return control == value.Last();
    }

    public static bool TryGetControlDigit(string value, out char result)
    {
        if (value.Length != 10 || !value.All(char.IsDigit))
        {
            result = '0';
            return false;
        }
        var digits = value.Select(c => c - '0').ToArray();
        var sum = digits.Take(10).Select((i, n) => i * Weights[n]).Sum();
        var control = sum % 11;
        if (control > 0) control = 11 - control;
        if (control == 10)
        {
            result = '0';
            return false;
        }
        result = control.ToString().Single();
        return true;
    }

    public static string GetIbanNumber(string accountNumber)
    {
        var rearranged = accountNumber + "232400";
        var numeric = System.Numerics.BigInteger.Parse(rearranged); 
        var checksum = (int)(98 - (numeric % 97));
        return $"NO{checksum:00}{accountNumber}";
    }

    public static AccountType GetAccountType(string accountNumber)
    {
        var accountSeries = int.Parse(accountNumber[4..6]);
        if(accountSeries == 0) return AccountType.Settlement;
        if (accountSeries >= 90) return AccountType.Internal;
        return AccountType.CustomerAccount;
    }
}
