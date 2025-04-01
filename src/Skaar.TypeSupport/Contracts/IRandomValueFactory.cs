namespace Skaar.TypeSupport.Contracts;

/// <summary>
/// Defines a type that can create random instances of itself.
/// </summary>
public interface IRandomValueFactory<out TSelf> where TSelf : IRandomValueFactory<TSelf>
{
    /// <summary>
    /// Create a new random value.
    /// </summary>
    static abstract TSelf CreateNew();
}