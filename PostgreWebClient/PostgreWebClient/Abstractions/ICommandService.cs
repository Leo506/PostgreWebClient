using Calabonga.OperationResults;
using Npgsql;
using PostgreWebClient.Models;

namespace PostgreWebClient.Abstractions;

public interface ICommandService
{
    OperationResult<Table> ExecuteCommand(QueryModel query, NpgsqlConnection connection);
}