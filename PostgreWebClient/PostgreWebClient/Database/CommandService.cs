using Calabonga.OperationResults;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Factories;
using PostgreWebClient.Models;

namespace PostgreWebClient.Database;

public class CommandService : ICommandService
{
    public OperationResult<Table> ExecuteCommand(QueryModel query, NpgsqlConnection connection)
    {
        var result = OperationResult.CreateResult<Table>();

        try
        {
            var executor = NpgsqlExecutorFactory.GetExecutor(query.QueryText, connection);
            result.Result = executor.Execute();
        }
        catch (Exception e)
        {
            result.AddError(e);
        }

        return result;
    }
}