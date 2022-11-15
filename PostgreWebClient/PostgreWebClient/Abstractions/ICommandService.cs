using Calabonga.OperationResults;
using Npgsql;
using PostgreWebClient.Models;

namespace PostgreWebClient.Abstractions;

public interface ICommandService
{
    Table ExecuteCommand(string query, NpgsqlConnection connection);
}