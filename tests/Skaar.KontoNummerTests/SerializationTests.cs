using Shouldly;
using Xunit;

namespace Skaar.KontonummerTests;

public class SerializationTests
{
    [Theory]
    [InlineData(KontonummerFormatting.None, "38361226620")]
    [InlineData(KontonummerFormatting.Periods, "3836.12.26620")]
    [InlineData(KontonummerFormatting.Spaces, "3836 12 26620")]
    [InlineData(KontonummerFormatting.IbanPrint, "NO61\u00a03836\u00a01226\u00a0620")]
    [InlineData(KontonummerFormatting.IbanScreen, "NO6138361226620")]
    public void ToString_WithFormatting_ReturnsCorrectFormat(KontonummerFormatting format, string expected)
    {
        var target = Kontonummer.Parse("38361226620");
        target.ToString(format).ShouldBe(expected);
    }

    [Fact]
    public void ToString_InvalidNumber_ReturnsNumbers()
    {
        Kontonummer.CreateNew("1 2 3 4").ToString().ShouldBe("1234");
    }

}