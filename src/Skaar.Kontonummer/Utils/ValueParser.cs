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
        var digits = value.Select(c => c - '0').ToArray();
        var sum = digits.Take(10).Select((i, n) => i * Weights[n]).Sum();
        var control = sum % 11;
        if (control > 0) control = 11 - control;

        return control != 10 && control == digits.Last();
    }

    public static string GetIbanNumber(string accountNumber)
    {
        var rearranged = accountNumber + "232400";
        var numeric = System.Numerics.BigInteger.Parse(rearranged); 
        var checksum = (int)(98 - (numeric % 97));
        return $"NO{checksum:00}{accountNumber}";
    } 
}
