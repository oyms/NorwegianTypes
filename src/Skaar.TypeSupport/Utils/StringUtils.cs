using System.Text.RegularExpressions;

namespace Skaar.TypeSupport.Utils;

public static partial class StringUtils
{
    public static string? RemoveNonDigits(string? rawValue)
    {
        if (rawValue == null)
        {
            return rawValue;
        }
        return RemoveNonDigitsPattern().Replace(rawValue, "");
    }
    
    [GeneratedRegex(@"\D+")]
    private static partial Regex RemoveNonDigitsPattern();
}