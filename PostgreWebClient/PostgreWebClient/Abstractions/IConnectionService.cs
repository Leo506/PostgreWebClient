using Npgsql;

namespace PostgreWebClient.Abstractions;

public interface IConnectionService
{
    public Dictionary<string, NpgsqlConnection> Connections { get; protected set; }
    void Connect(string key, string connectionString);
}