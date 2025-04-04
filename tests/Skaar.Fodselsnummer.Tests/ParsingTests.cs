using Shouldly;
using Skaar.Contracts;
using System.Globalization;
using Xunit;

namespace Skaar.FodselsnummerTests;

public class ParsingTests
{
    [Theory]
    [InlineData("07035514478")]
    [InlineData("28098838726")]
    [InlineData("23072388054")]
    [InlineData("07102635463")]
    [InlineData("081019 70753")]
    [InlineData("11122039597")]
    [InlineData("1811 19 02 867")]
    [InlineData("14106138320")]
    [InlineData("13101879636")]
    [InlineData("30076814992")]
    public void TryParse_WithValidFodselsnummer_IsValidOfTypeFodselsnummer(string number)
    {
        IdNumber.TryParse(number, CultureInfo.InvariantCulture, out var result).ShouldBeTrue();
        result.IsValid.ShouldBe(true);
        result.Type.ShouldBe(NummerType.Fodselsnummer);
    }
    
    [Theory]
    [InlineData("41041714032")]
    [InlineData("41093341501")]
    [InlineData("42041010376")]
    [InlineData("42043417112")]
    [InlineData("42079638336")]
    [InlineData("420999980 02")]
    [InlineData("42116298443")]
    [InlineData("42124536957")]
    [InlineData("42128 99 8688")]
    [InlineData("43026142033")]
    [InlineData("43043543149")]
    [InlineData("43100833132")]
    public void TryParse_WithValidDNummer_IsValidOfTypeDNummer(string number)
    {
        IdNumber.TryParse(number, CultureInfo.InvariantCulture, out var result).ShouldBeTrue();
        result.IsValid.ShouldBe(true);
        result.Type.ShouldBe(NummerType.DNummer);
    }
    
    [InlineData("200205705502")]
    [InlineData("200900595509")]
    [Theory]
    public void TryParse_WithValidDufNummer_IsValidOfTypeDufNummer(string number)
    {
        var result = IdNumber.CreateNew(number, CultureInfo.InvariantCulture);
        result.IsValid.ShouldBe(true);
        result.Type.ShouldBe(NummerType.DufNummer);
    }
}