using Shouldly;
using System.Globalization;
using Xunit;

namespace Skaar.KontonummerTests;

public class ParsingTests(ITestContextAccessor testContextAccessor)
{
    [Theory]
    [InlineData("89531040148")]
    [InlineData("86662218148")]
    [InlineData("49318705959")]
    [InlineData("49310005958")]
    [InlineData("49319015956")]
    public void TryParse_WithValidValue_IsValidIsTrue(string value)
    {
        Kontonummer.TryParse(value, CultureInfo.InvariantCulture, out var result).ShouldBeTrue();
        testContextAccessor.Current.TestOutputHelper!.WriteLine(result.ToString());
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("09445101173", AccountType.CustomerAccount)]
    [InlineData("094437878", AccountType.Undefined)]
    [InlineData("49310005958", AccountType.Settlement)]
    [InlineData("49319015956", AccountType.Internal)]
    public void AccountType_FromValidNumber_HasCorrectValue(string number, AccountType expected)
    {
        var target = Kontonummer.CreateNew(number);
        target.AccountType.ShouldBe(expected);
    }

    [Theory]
    [InlineData("55035395554", "DNBANOKK", "DNB Bank ASA")]
    [InlineData("32445395551", "HAUGNO21", "Haugesund Sparebank")]
    [InlineData("98645395557", "SHEDNO22", "SpareBank 1 Østlandet")]
    public void Bank_WithNumber_ReturnsCorrectNameAndBic(string number, string bic, string name)
    {
        var target = Kontonummer.Parse(number, CultureInfo.InvariantCulture);
        var bank = target.Bank;
        bank.Bic.ShouldBe(bic);
        bank.Name.ShouldBe(name);
    }
}