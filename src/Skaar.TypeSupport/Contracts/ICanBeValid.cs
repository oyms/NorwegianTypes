namespace Skaar.TypeSupport.Contracts;

/// <summary>
/// The object has a property indicating if the value it represents is valid.
/// </summary>
public interface ICanBeValid
{
    /// <summary>
    /// <c>true</c> when the value this object represents is valid,
    /// determined by the internal validation rules.
    /// </summary>
    bool IsValid { get; }
}