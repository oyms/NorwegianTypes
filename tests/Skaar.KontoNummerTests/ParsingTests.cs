using Shouldly;
using System.Globalization;
using Xunit;

namespace Skaar.KontonummerTests;

public class ParsingTests(ITestContextAccessor testContextAccessor)
{
    [Theory]
    [InlineData("89531040148")]
    [InlineData("86662218148")]
    [InlineData("49318705959")]
    public void TryParse_WithValidValue_IsValidIsTrue(string value)
    {
        Kontonummer.TryParse(value, CultureInfo.InvariantCulture, out var result).ShouldBeTrue();
        testContextAccessor.Current.TestOutputHelper!.WriteLine(result.ToString()!);
        result.IsValid.ShouldBeTrue();
    }
}