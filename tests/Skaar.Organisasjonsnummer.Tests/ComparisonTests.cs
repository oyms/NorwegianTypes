using Shouldly;
using Xunit;

namespace Skaar.OrganisasjonsnummerTests;

public class ComparisonTests
{
    [Fact]
    public void Equals_ForEquivalentNumbers_ReturnsTrue()
    {
        var a = Organisasjonsnummer.Parse("932423898");
        var b = Organisasjonsnummer.Parse("932 423 898");
        
        a.Equals(b).ShouldBeTrue();
        (a == b).ShouldBeTrue();
        a.CompareTo(b).ShouldBe(0);
        (a > b).ShouldBeFalse();
        (a >= b).ShouldBeTrue();
        (a < b).ShouldBeFalse();
        (a <= b).ShouldBeTrue();
    }   
    
    [Fact]
    public void Equals_ForDifferentNumbers_ReturnsFalse()
    {
        var a = Organisasjonsnummer.Parse("929894472");
        var b = Organisasjonsnummer.Parse("841348192");
        
        a.Equals(b).ShouldBeFalse();
        (a != b).ShouldBeTrue();
        a.CompareTo(b).ShouldBe(1);
        b.CompareTo(a).ShouldBe(-1);
        (a > b).ShouldBeTrue();
        (a >= b).ShouldBeTrue();
        (a < b).ShouldBeFalse();
        (a <= b).ShouldBeFalse();
    }
}