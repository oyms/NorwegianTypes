namespace Skaar.TypeSupport.Utils;

public static class Mod11
{
    public static bool TryGetChecksumDigit(ReadOnlySpan<char> value, Span<int> weights, out char checksum)
    {
        if (value.Length != weights.Length)
        {
            checksum = '\0';
            return false;
        }

        int sum = 0;
        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];
            if (c is < '0' or > '9')
            {
                checksum = '\0';
                return false;
            }

            int digit = c - '0';
            sum += digit * weights[i];
        }

        int control = sum % 11;
        if (control > 0)
        {
            control = 11 - control;
        }

        if (control == 10)
        {
            checksum = '\0';
            return false;
        }

        checksum = (char)('0' + control);
        return true;
    }
}