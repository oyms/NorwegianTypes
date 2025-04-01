namespace Skaar.TypeSupport.Contracts;

/// <summary>
/// Defines a type that can parse a string "safely", and return a value.
/// </summary>
public interface ISafeParsable<out TSelf> where TSelf : ISafeParsable<TSelf>, IParsable<TSelf>, ICanBeValid
{
    /// <summary>
    /// A simpler way to call <see cref="IParsable{TSelf}.TryParse"/>
    /// when you can accept an invalid value.
    /// </summary>
    static abstract TSelf CreateNew(string? value, IFormatProvider? provider = null);
}