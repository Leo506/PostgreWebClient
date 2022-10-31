using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Database;

public class CommandService : ICommandService
{
    private readonly IExecutorFactory _factory;

    public CommandService(IExecutorFactory factory)
    {
        _factory = factory;
    }

    public QueryModel ExecuteCommand(QueryModel query, NpgsqlConnection connection)
    {
        var result = new QueryModel()
        {
            Headers = new List<string>(),
            Rows = new List<List<object>>(),
            QueryText = query.QueryText
        };

        try
        {
            var executor = _factory.GetExecutor(query.QueryText, connection);
            var table = executor.Execute();

            if (table.Columns != null) result.Headers.AddRange(table.Columns);

            if (table.Rows == null) return result;
            
            foreach (var row in table.Rows)
            {
                result.Rows.Add(row);
            }

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