using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Executors;

public class NpgsqlCommandExecutor : ICommandExecutor
{
    private readonly string _query;
    private readonly NpgsqlConnection _connection;

    public NpgsqlCommandExecutor(string query, NpgsqlConnection connection)
    {
        _query = query;
        _connection = connection;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Table Execute()
    {
        var cmd = new NpgsqlCommand(_query, _connection);
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