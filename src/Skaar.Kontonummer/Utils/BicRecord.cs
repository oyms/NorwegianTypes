using CsvHelper.Configuration.Attributes;

namespace Skaar.Utils;

internal record BicRecord
{
    [Name("Bank identifier")]
    public required string Identifier { get; init; }
    [Name("BIC")]
    public required string Bic { get; init; }
    public required string Bank { get; init; }
}