using Shouldly;
using System.Globalization;
using Xunit;

namespace Skaar.FodselsnummerTests;

public class DufNummerTests(ITestContextAccessor testContext)
{
    [Theory]
    [InlineData("200205705502")]
    [InlineData("200900595509")]
    public void TryParse_WithValidNumber_IsValid(string number)
    {
        DufNummer.TryParse(number, CultureInfo.InvariantCulture, out var result).ShouldBeTrue();
        result.IsValid.ShouldBeTrue();
    }
    
    [InlineData("131018x9636")]
    [InlineData("30076814392")]
    [Theory]
    public void TryParse_WithInvalidNumber_IsNotValid(string number)
    {
        DufNummer.TryParse(number, CultureInfo.InvariantCulture, out var result).ShouldBeFalse();
        result.IsValid.ShouldBeFalse();
    }
    
    [Fact]
    public void TryParse_WithNull_IsNotValid()
    {
        DufNummer.TryParse(null, null, out var result).ShouldBeFalse();
        result.IsValid.ShouldBeFalse();
    }
    
    [Fact]
    public void CreateNew_CreatesValidDNummer()
    {
        var @out = testContext.Current.TestOutputHelper!; 
        const int n = 10;
        for (int i = 0; i < n; i++)
        {
            var result = DufNummer.CreateNew();
            @out.WriteLine(result.ToString());
            result.IsValid.ShouldBeTrue();
        }
    }
    
    [Fact]
    public void ConvertToIdNumber_WithNumber_ToStringIsEqual()
    {
        var input = DufNummer.CreateNew();
        var result = (IdNumber)input;
        result.ToString().ShouldBe(input.ToString());
    }
}