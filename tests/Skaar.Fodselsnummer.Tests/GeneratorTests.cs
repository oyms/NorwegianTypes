using Shouldly;
using Skaar.Contracts;
using Xunit;

namespace Skaar.FodselsnummerTests;

public class GeneratorTests(ITestContextAccessor testContext)
{
    [Fact]
    public void CreateNew_OnIdNummer_CreatesValidFodselsnummer()
    {
        var @out = testContext.Current.TestOutputHelper!; 
        const int n = 10;
        for (int i = 0; i < n; i++)
        {
            var result = IdNumber.CreateNew();
            @out.WriteLine(result.ToString());
            result.Type.ShouldBe(NummerType.Fodselsnummer);
        }
    }
}