namespace Skaar.TypeSupport.Utils;

public static class Mod11
{
    public static bool TryGetChecksumDigit(string? value, int[] weights, out char checksum)
    {
        if (value is null || value.Length != weights.Length || !value.All(char.IsDigit))
        {
            checksum = '\0';
            return false;
        }
        var digits = value.Select(c => c - '0').ToArray();
        var sum = digits.Take(10).Select((i, n) => i * weights[n]).Sum();
        var control = sum % 11;
        if (control > 0) control = 11 - control;
        if (control == 10)
        {
            checksum = '\0';
            return false;
        }

        checksum = control.ToString().Single();
        return true;
    }
}