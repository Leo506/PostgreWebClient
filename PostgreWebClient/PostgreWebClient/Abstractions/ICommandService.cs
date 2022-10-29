using Npgsql;
using PostgreWebClient.Models;

namespace PostgreWebClient.Abstractions;

public interface ICommandService
{
    QueryModel ExecuteCommand(QueryModel query, NpgsqlConnection connection);
}