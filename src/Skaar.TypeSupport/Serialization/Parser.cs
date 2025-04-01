using Skaar.TypeSupport.Contracts;

namespace Skaar.TypeSupport.Serialization;

public static class Parser
{
    public static T SafeParse<T>(string? value, IFormatProvider? provider = null) where T : IParsable<T>, ICanBeValid
    {
        T.TryParse(value, provider, out var result);
        return result!;
    }
}