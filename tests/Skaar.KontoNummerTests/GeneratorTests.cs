using Shouldly;
using Xunit;

namespace Skaar.KontonummerTests;

public class GeneratorTests(ITestContextAccessor contextAccessor)
{
    [Fact]
    public void CreateNew_WhenCalled_ReturnsAValidNumber()
    {
        var @out = contextAccessor.Current.TestOutputHelper!;
        for (int i = 0; i < 100; i++)
        {
            var result = Kontonummer.CreateNew();
            result.IsValid.ShouldBeTrue();
            @out.WriteLine($"{result.ToString(KontonummerFormatting.Spaces)} {result.Bank}");
        }
    }
}