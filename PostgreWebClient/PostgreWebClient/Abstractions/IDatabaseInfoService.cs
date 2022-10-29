using Npgsql;
using PostgreWebClient.Models;

namespace PostgreWebClient.Abstractions;

public interface IDatabaseInfoService
{
    DatabaseInfo GetDatabaseInfo(NpgsqlConnection connection);
}