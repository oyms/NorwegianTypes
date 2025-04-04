using Shouldly;
using System.Diagnostics;
using System.Globalization;
using Xunit;

namespace Skaar.FodselsnummerTests;

public class DNummerTests(ITestContextAccessor testContext)
{
    [Theory]
    [InlineData("41070571224")]
    [InlineData("420580 91663")]
    [InlineData("50089890141")]
    public void TryParse_WithValidNumber_IsValid(string number)
    {
        DNummer.TryParse(number, CultureInfo.CurrentCulture, out var result).ShouldBeTrue();
        result.IsValid.ShouldBeTrue();
    }
    
    [InlineData("131018x9636")]
    [InlineData("30076814392")]
    [Theory]
    public void TryParse_WithInvalidNumber_IsNotValid(string number)
    {
        DNummer.TryParse(number, CultureInfo.CurrentCulture, out var result).ShouldBeFalse();
        result.IsValid.ShouldBeFalse();
    }
    
    [Fact]
    public void CreateNew_OnIdNummer_CreatesValidDNummer()
    {
        var @out = testContext.Current.TestOutputHelper; 
        const int n = 10;
        for (int i = 0; i < n; i++)
        {
            var result = DNummer.CreateNew();
            @out.WriteLine(result.ToString());
            if(!result.IsValid) Debugger.Break();
            result.IsValid.ShouldBeTrue();
        }
    }

    [Theory]
    [InlineData("44076895624", "04-07-1968")]
    [InlineData("65043642558", "25-04-1936")]
    [InlineData("08023617952", "01-01-0001")]
    public void BirthDate_FromNumber_ReturnsDate(string number, string expectedDate)
    {
        var target = DNummer.CreateNew(number);
        var date = DateOnly.ParseExact(expectedDate, "dd-MM-yyyy");
        target.BirthDate.ShouldBe(date);
    }
    
    [Theory]
    [InlineData("56125190125", Gender.Male)]
    [InlineData("57097293009", Gender.Female)]
    [InlineData("invalid", Gender.Undefined)]
    public void Gender_FromNumber_ReturnsGender(string number, Gender expected)
    {
        var target = DNummer.CreateNew(number);
        target.Gender.ShouldBe(expected);
    }

    [Fact]
    public void ConvertToIdNumber_WithNumber_ToStringIsEqual()
    {
        var input = DNummer.CreateNew();
        var result = (IdNumber)input;
        result.ToString().ShouldBe(input.ToString());
    }
}