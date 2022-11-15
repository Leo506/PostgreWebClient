using Calabonga.OperationResults;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Database;

public class CommandService : ICommandService
{
    private readonly ICommandExecutor _executor;

    public CommandService(ICommandExecutor executor)
    {
        _executor = executor;
    }

    public Table ExecuteCommand(string query, NpgsqlConnection connection)
    {
        var result = new Table();

        try
        {
            var table = _executor.Execute(query, connection);
            if (table.Equals(Table.Empty))
                result = new Table()
                {
                    Columns = new List<string>() { "Query", "Result" },
                    Rows = new List<List<object>>()
                    {
                        new() { query, "Success" }
                    }
                };
            else
                result = table;
        }
        catch (Exception e)
        {
            result = new Table()
            {
                Columns = new List<string>() { "Query", "Result", "Reason" },
                Rows = new List<List<object>>()
                {
                    new() { query, "Failed", e.Message }
                }
            };
        }

        return result;
    }
}