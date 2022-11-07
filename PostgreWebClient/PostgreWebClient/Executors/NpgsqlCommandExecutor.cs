using System.Data;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Executors;

public class NpgsqlCommandExecutor : ICommandExecutor
{

    public Table Execute(string query, IDbConnection connection)
    {
        var cmd = new NpgsqlCommand(query, connection as NpgsqlConnection);
        var reader = cmd.ExecuteReader();

        var table = new Table()
        {
            Columns = new List<string>(),
            Rows = new List<List<object>>()
        };

        foreach (var column in reader.GetColumnSchema())
        {
            table.Columns.Add(column.ColumnName);
        }

        while (reader.Read())
        {
            var row = table.Columns.Select(column => reader[column]).ToList();
            table.Rows.Add(row);
        }
        
        reader.Close();
        reader.Dispose();

        return table;
    }
}