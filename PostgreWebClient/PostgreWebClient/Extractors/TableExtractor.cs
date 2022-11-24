using System.Data;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Extractors;

public class TableExtractor : ITableExtractor
{
    public Table ExtractTable(IDataReader? reader)
    {
        if (reader is null)
            throw new ArgumentNullException(nameof(reader));
        
        var table = new Table
        {
            Columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList(),
            Rows = new List<List<object>>()
        };

        while (reader.Read())
        {
            table.Rows.Add(Enumerable.Range(0, reader.FieldCount).Select(i => reader[i]).ToList());
        }

        return table;
    }
}