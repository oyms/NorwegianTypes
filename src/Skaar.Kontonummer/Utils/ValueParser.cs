using Skaar.TypeSupport.Utils;

namespace Skaar.Utils;

internal static class ValueParser
{
    private static readonly int[] Weights = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
    
    /// <summary>
    /// Value is already trimmed/cleaned of spacing
    /// </summary>
    public static bool ValidateNumber(ReadOnlySpan<char> value)
    {
        if (value.Length != 11) return false;
        if (!TryGetControlDigit(value[..10], out var control))
        {
            return false;
        }
        return control == value[10];
    }

    public static bool TryGetControlDigit(ReadOnlySpan<char> value, out char result) => Mod11.TryGetChecksumDigit(value, Weights, out result);

    public static string GetIbanNumber(ReadOnlySpan<char> accountNumber)
    {
        var rearranged = accountNumber.ToString() + "232400";
        var numeric = System.Numerics.BigInteger.Parse(rearranged); 
        var checksum = (int)(98 - (numeric % 97));
        return $"NO{checksum:00}{accountNumber}";
    }

    public static AccountType GetAccountType(ReadOnlySpan<char> accountNumber)
    {
        var accountSeries = int.Parse(accountNumber[4..6]);
        if(accountSeries == 0) return AccountType.Settlement;
        if (accountSeries >= 90) return AccountType.Internal;
        return AccountType.CustomerAccount;
    }
}
