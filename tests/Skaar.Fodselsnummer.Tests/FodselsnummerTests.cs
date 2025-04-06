using Shouldly;
using Skaar.Contracts;
using System.Globalization;
using Xunit;

namespace Skaar.FodselsnummerTests;

public class FodselsnummerTests(ITestContextAccessor testContext)
{
    [Theory]
    [InlineData("07035514478")]
    [InlineData("2809 8838726")]
    [InlineData("23072388054")]
    [InlineData("07102635463")]
    [InlineData("081019 70753")]
    [InlineData("11122039597")]
    [InlineData("1811 19 02 867")]
    [InlineData("14106138320")]
    [InlineData("13101879636")]
    [InlineData("30076814992")]
    public void TryParse_WithValidNumber_IsValid(string number)
    {
        Fodselsnummer.TryParse(number, CultureInfo.InvariantCulture, out var result).ShouldBeTrue();
        result.IsValid.ShouldBeTrue();
    }
    
    [Theory]
    [InlineData("131018x9636")]
    [InlineData("30076814392")]
    public void TryParse_WithInvalidNumber_IsNotValid(string number)
    {
        Fodselsnummer.TryParse(number, CultureInfo.InvariantCulture, out var result).ShouldBeFalse();
        result.IsValid.ShouldBeFalse();
    }

    [Fact]
    public void TryParse_WithNull_IsNotValid()
    {
        Fodselsnummer.TryParse(null, null, out var result).ShouldBeFalse();
        result.IsValid.ShouldBeFalse();
    }
    
    [Fact]
    public void CreateNew_OnIdNummer_CreatesValidFodselsnummer()
    {
        var @out = testContext.Current.TestOutputHelper!; 
        const int n = 10;
        for (int i = 0; i < n; i++)
        {
            var result = Fodselsnummer.CreateNew();
            @out.WriteLine(result.ToString());
            result.IsValid.ShouldBeTrue();
        }
    }

    [Theory]
    [InlineData("04042735308", "04-04-1927")]
    [InlineData("08023617952", "08-02-1936")]
    public void BirthDate_FromNumber_ReturnsDate(string number, string expectedDate)
    {
        var target = Fodselsnummer.CreateNew(number);
        var date = DateOnly.ParseExact(expectedDate, "dd-MM-yyyy");
        target.BirthDate.ShouldBe(date);
    }
    
    [Theory]
    [InlineData("04042735308", Gender.Male)]
    [InlineData("07114507835", Gender.Female)]
    [InlineData("invalid", Gender.Undefined)]
    public void Gender_FromNumber_ReturnsGender(string number, Gender expected)
    {
        var target = Fodselsnummer.CreateNew(number);
        target.Gender.ShouldBe(expected);
    }

    [Fact]
    public void ConvertToIdNumber_WithNumber_ToStringIsEqual()
    {
        var input = Fodselsnummer.CreateNew();
        var result = (IdNumber)input;
        result.ToString().ShouldBe(input.ToString());
    }
    
}