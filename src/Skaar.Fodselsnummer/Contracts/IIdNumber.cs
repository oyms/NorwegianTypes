using Skaar.TypeSupport.Contracts;

namespace Skaar.Contracts;

public interface IIdNumber : ICanBeValid
{
    /// <summary>
    /// Returns the value as string.
    /// </summary>
    /// <remarks>When value is invalid and null, an empty string is returned.</remarks>
    string ToString();
}