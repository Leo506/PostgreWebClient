using Calabonga.OperationResults;
using Npgsql;
using PostgreWebClient.Models;

namespace PostgreWebClient.Abstractions;

public interface IDatabaseInfoService
{
    OperationResult<DatabaseInfo> GetDatabaseInfo(NpgsqlConnection connection);
}