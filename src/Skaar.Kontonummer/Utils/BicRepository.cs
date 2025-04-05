using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Reflection;

namespace Skaar.Utils;

internal static class BicRepository
{
    private static IDictionary<string, Bank>? _banks;
    public static IDictionary<string, Bank> Banks => _banks ??= ReadBicList();
    private static IDictionary<string, Bank> ReadBicList()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("Skaar.Resources.Norwegian-BIC-IBAN-table.csv");
        if (stream == null) throw new InvalidOperationException("Cannot find the csv file.");
        using var reader = new StreamReader(stream);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";", HasHeaderRecord = true };
        using var csv = new CsvReader(reader, config);
        var records = csv.GetRecords<BicRecord>()
            .ToDictionary(r => r.Identifier, r => new Bank(r.Bic, r.Bank));
        return records;
    }

    public static Bank Lookup(string id)
    {
        if(Banks.TryGetValue(id, out var record)) return record;
        return Bank.Undefined;
    }

    public static string GetRandomId()
    {
        var all = Banks.Keys;
        var index = Random.Shared.Next(all.Count - 1);
        return all.Skip(index).First();
    }
}