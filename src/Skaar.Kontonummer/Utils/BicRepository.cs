using Skaar.TypeSupport.Utils;
using System.Reflection;

namespace Skaar.Utils;

internal static class BicRepository
{
    private static readonly LazyAndWeakReference<IDictionary<string, Bank>> BicList = new(ReadBicList);
    private static IDictionary<string, Bank> Banks => BicList.Value;
    private static IDictionary<string, Bank> ReadBicList()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("Skaar.Resources.Norwegian-BIC-IBAN-table.csv");
        if (stream == null) throw new InvalidOperationException("Cannot find the csv file.");
        using var reader = new StreamReader(stream);
        // Columns: Bank identifier;BIC;Bank. The first line is the header.
        reader.ReadLine();
        var records = new Dictionary<string, Bank>();
        while (reader.ReadLine() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var fields = line.Split(';');
            if (fields.Length != 3) throw new InvalidDataException($"Invalid line in the csv file: '{line}'");
            records.Add(fields[0], new Bank(fields[1], fields[2]));
        }
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