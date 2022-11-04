using Calabonga.OperationResults;
using Npgsql;

namespace PostgreWebClient.Abstractions;

public interface IConnectionService
{
    public Dictionary<string, NpgsqlConnection> Connections { get; protected set; }
    OperationResult<bool> Connect(string key, string connectionString);
}