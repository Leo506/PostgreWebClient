using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Database;

public class CommandService : ICommandService
{
    public QueryModel ExecuteCommand(QueryModel query, NpgsqlConnection connection)
    {
        var cmd = new NpgsqlCommand(query.QueryText, connection);
        var result = new QueryModel()
        {
            Headers = new List<string>(),
            Rows = new List<List<object>>(),
            QueryText = query.QueryText
        };

        try
        {
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                reader.Dispose();
                return new QueryModel()
                {
                    QueryText = query.QueryText
                };
            }


            foreach (var column in reader.GetColumnSchema())
            {
                result.Headers.Add(column.ColumnName);
            }

            while (reader.Read())
            {
                var row = result.Headers.Select(header => reader[header]).ToList();
                result.Rows.Add(row);
            }

            reader.Close();
            reader.Dispose();

            return result;
        }
        catch (Exception e)
        {
            result.HasError = true;
            result.ErrorText = e.Message;
        }

        return result;
    }
}