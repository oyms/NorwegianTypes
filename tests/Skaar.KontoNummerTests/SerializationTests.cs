using Shouldly;
using Xunit;

namespace Skaar.KontonummerTests;

public class SerializationTests(ITestContextAccessor testContext)
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

    [Fact]
    public void Serialize_WithSystemJson_CanDeserializeIntoSame()
    {
        var source = new SerializationTarget(Kontonummer.CreateNew());
        var json = System.Text.Json.JsonSerializer.Serialize(source);
        testContext.Current.TestOutputHelper!.WriteLine(json);
        var deserialized = System.Text.Json.JsonSerializer.Deserialize<SerializationTarget>(json);
        deserialized.ShouldBe(source);
    }
    
    [Fact]
    public void Serialize_WithNewtonsoft_CanDeserializeIntoSame()
    {
        var source = new SerializationTarget(Kontonummer.CreateNew());
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(source);
        testContext.Current.TestOutputHelper!.WriteLine(json);
        var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializationTarget>(json);
        deserialized.ShouldBe(source);
    }

}

file record SerializationTarget(Kontonummer Kontonummer);