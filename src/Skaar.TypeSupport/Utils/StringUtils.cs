using System.Text.RegularExpressions;

namespace Skaar.TypeSupport.Utils;

public static partial class StringUtils
{
    public static string? RemoveWhitespace(string? rawValue)
    {
        if (rawValue == null)
        {
            return rawValue;
        }

        return RemoveSpacesPattern().Replace(rawValue, "");
    }

    public static string? RemoveNonDigits(string? rawValue)
    {
        if (rawValue == null)
        {
            return rawValue;
        }
        return RemoveNonDigitsPattern().Replace(rawValue, "");
    }
    
    [GeneratedRegex(@"\s+")]
    private static partial Regex RemoveSpacesPattern();
    
    [GeneratedRegex(@"\D+")]
    private static partial Regex RemoveNonDigitsPattern();
}