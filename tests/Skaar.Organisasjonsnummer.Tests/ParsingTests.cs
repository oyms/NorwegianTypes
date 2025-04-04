using Shouldly;
using System.Globalization;
using Xunit;

namespace Skaar.OrganisasjonsnummerTests;

public class ParsingTests
{
    [Theory]
    [InlineData("925068551")]
    [InlineData("931309080")]
    [InlineData("989 529 196")]
    [InlineData("934     3 28 2  80")]
    [InlineData("NO-BRC-968253980")]
    public void TryParse_WithValidNumbers_IsValidIsFalse(string orgnr)
    {
        Organisasjonsnummer.TryParse(orgnr, CultureInfo.InvariantCulture, out var result).ShouldBeTrue();
        result.IsValid.ShouldBeTrue();
    }    
    
    [Theory]
    [InlineData("9250685510")]
    [InlineData("111222333")]
    [InlineData("93130908")]
    [InlineData("989 529-196")]
    [InlineData("934xx3x28x2x80")]
    public void TryParse_WithInvalidNumbers_IsValidIsFalse(string orgnr)
    {
        Organisasjonsnummer.TryParse(orgnr, CultureInfo.InvariantCulture, out var result).ShouldBeFalse();
        result.IsValid.ShouldBeFalse();
    }
}