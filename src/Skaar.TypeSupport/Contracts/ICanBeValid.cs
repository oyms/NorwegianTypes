namespace Skaar.TypeSupport.Contracts;

/// <summary>
/// The object has a property indicating if the value it represents is valid.
/// </summary>
/// <remarks>
/// In other words; this type represents a value that may have failed validation.
/// The value may still be represented.
/// </remarks>
public interface ICanBeValid
{
    /// <summary>
    /// <c>true</c> when the value this object represents is valid,
    /// determined by the internal validation rules.
    /// </summary>
    bool IsValid { get; }
}