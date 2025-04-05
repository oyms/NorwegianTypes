using Shouldly;
using System.Text.Json;
using Xunit;

namespace Skaar.OrganisasjonsnummerTests;

public class SerializingTests
{
    [Fact]
    public void Deserialize_FromString_ReturnsValue()
    {
        var json = """
        {
           "OrgNr": "841348192"
        }
        """;
        var result = JsonSerializer.Deserialize<TargetType>(json);
        result!.OrgNr.ToString().ShouldBe("841348192");
    }

    [Fact]
    public void Serialize_FromValue_SerializesAsString()
    {
        var expected = "932555409";
        var json = JsonSerializer.Serialize(new TargetType(Organisasjonsnummer.Parse(expected)));
        var deserialized = JsonSerializer.Deserialize<TargetTypeWithString>(json);
        deserialized!.OrgNr.ShouldBe(expected);
    }

    [Fact]
    public void ToString_WhenValid_WithFormatting_ReturnsFormattedString()
    {
        var target = Organisasjonsnummer.Parse("84 134 8 19 2");
        target.ToString().ShouldBe("841348192");
        target.ToString(OrganisasjonsnummerFormatting.None).ShouldBe("841348192");
        target.ToString(OrganisasjonsnummerFormatting.WithSpaces).ShouldBe("841 348 192");
        target.ToString(OrganisasjonsnummerFormatting.OrgIdFormat).ShouldBe("NO-BRC-841348192");
    }
}

file record TargetType(Organisasjonsnummer OrgNr);
file record TargetTypeWithString(string OrgNr);