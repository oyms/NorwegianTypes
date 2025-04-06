namespace Skaar.TypeSupport.Utils;

public static class StringUtils
{
    public static int MemoryCompare(ReadOnlyMemory<char> a, ReadOnlyMemory<char> b)
    {
        var spanA = a.Span;
        var spanB = b.Span;
    
        int minLength = Math.Min(spanA.Length, spanB.Length);
    
        for (int i = 0; i < minLength; i++)
        {
            int cmp = spanA[i].CompareTo(spanB[i]);
            if (cmp != 0)
            {
                return cmp;
            }
        }
        return spanA.Length.CompareTo(spanB.Length);
    }
    
    public static ReadOnlyMemory<char> RemoveNonDigits(ReadOnlySpan<char> rawValue)
    {
        var buffer = rawValue.Length <= 256 ? stackalloc char[rawValue.Length] : new char[rawValue.Length];

        int j = 0;
        for (int i = 0; i < rawValue.Length; i++)
        {
            if (char.IsDigit(rawValue[i]))
            {
                buffer[j++] = rawValue[i];
            }
        }
        var result = new char[j];
        buffer[..j].CopyTo(result);
        return result;
    }

    public static bool TryFormatIgnoringFormatting(ReadOnlyMemory<char> source, Span<char> destination,
        out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (!format.IsEmpty && format is not "G")
        {
            throw new FormatException("Only general format 'G' is supported.");
        }
        var span = source.Span;

        if (span.Length > destination.Length)
        {
            charsWritten = 0;
            return false; // Not enough space
        }

        span.CopyTo(destination);
        charsWritten = span.Length;
        return true;
    }
}