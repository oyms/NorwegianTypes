using Shouldly;
using Skaar.TypeSupport.Contracts;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Skaar.OrganisasjonsnummerTests;

public class ValidationTests(ITestContextAccessor testContext)
{
    [Fact]
    public void ValidateModel_InvalidNumber_ReturnsFalse()
    {
        var @out = testContext.Current.TestOutputHelper!;
        var orgNr = Organisasjonsnummer.CreateNew("xxxx");
        var model = new TargetType(orgNr);

        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        Validator.TryValidateObject(model, context, results, validateAllProperties: true).ShouldBeFalse();
        foreach (var result in results)
        {
            @out.WriteLine(result.ErrorMessage ?? "No error");
        }
    }
}

// ReSharper disable once NotAccessedPositionalProperty.Local
file record TargetType([property:MustBeValid(ErrorMessage = "Not a valid org nr")]Organisasjonsnummer OrgNr);