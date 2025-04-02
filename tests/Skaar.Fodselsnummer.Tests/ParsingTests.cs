using Shouldly;
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
    public void TryParse_WithValidFodselsnummer_IsValidIsTrue(string number)
    {
        Fodselsnummer.TryParse(number, CultureInfo.CurrentCulture, out var result).ShouldBeTrue();
        result.IsValid.ShouldBe(true);
        result.Type.ShouldBe(NummerType.Fodselsnummer);
    }
}