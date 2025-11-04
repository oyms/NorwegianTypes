using Shouldly;
using System.Globalization;
using Xunit;

namespace Skaar.BilRegNrTests;

public class ParsingTests
{
    [Theory]
    [InlineData("DN 96650")]
    public void TryParse_WithValidValues_IsValidReturnsTrue(string regNr)
    {
        Registreringsnummer.TryParse(regNr, CultureInfo.InvariantCulture, out var result).ShouldBeTrue();
        result.IsValid.ShouldBeTrue();
    }
    
        
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(" - + ")]
    public void TryParse_WithNoValidCharacters_IsNotValid(string? regNr)
    {
        Registreringsnummer.TryParse(regNr, CultureInfo.InvariantCulture, out var result).ShouldBeFalse();
        result.IsValid.ShouldBeFalse();
    }
}