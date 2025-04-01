using System.ComponentModel.DataAnnotations;

namespace Skaar.TypeSupport.Contracts;

/// <summary>
/// Decorate the model if the value is implementing <see cref="ICanBeValid"/>.
/// When validating the model, the value of <see cref="ICanBeValid.IsValid"/> is checked.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Class, AllowMultiple = true)]
public class MustBeValidAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is ICanBeValid canBeValid)
        {
            if (!canBeValid.IsValid) return new ValidationResult(ErrorMessage ?? "The value is not valid");
            else return ValidationResult.Success;
        }

        throw new InvalidOperationException("The type does not implement ICanBeValid");
    }
}