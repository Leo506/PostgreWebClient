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

    public OperationResult<Table> ExecuteCommand(string query, NpgsqlConnection connection)
    {
        var result = OperationResult.CreateResult<Table>();

        try
        {
            result.Result = _executor.Execute(query, connection);
        }
        catch (Exception e)
        {
            result.AddError(e);
        }

        return result;
    }
}