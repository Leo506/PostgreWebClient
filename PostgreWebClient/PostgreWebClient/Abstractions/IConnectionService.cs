using Calabonga.OperationResults;
using Npgsql;
using PostgreWebClient.Models;

namespace PostgreWebClient.Abstractions;

public interface IConnectionService
{
    public ConnectionCollection Connections { get; protected set; }
    OperationResult<bool> Connect(string key, string connectionString);
}