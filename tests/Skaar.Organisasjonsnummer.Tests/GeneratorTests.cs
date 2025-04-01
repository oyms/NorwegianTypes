using Shouldly;
using Xunit;

namespace Skaar.OrganisasjonsnummerTests;

public class GeneratorTests(ITestContextAccessor contextAccessor)
{
    [Fact]
    public void CreateNew_WhenCalled_ReturnsAValidNumber()
    {
        var @out = contextAccessor.Current.TestOutputHelper;
        for (int i = 0; i < 100; i++)
        {
            var result = Organisasjonsnummer.CreateNew();
            result.IsValid.ShouldBeTrue();
            @out.WriteLine(result.ToString(OrganisasjonsnummerFormatting.WithSpaces));
        }
    }
}