using Npgsql;
using PostgreWebClient.Abstractions;

namespace PostgreWebClient.Data;

public class ConnectionService : IConnectionService
{
    private Dictionary<string, NpgsqlConnection> _connections;

    Dictionary<string, NpgsqlConnection> IConnectionService.Connections
    {
        get => _connections;
        set => _connections = value;
    }

    public ConnectionService() => _connections = new Dictionary<string, NpgsqlConnection>();

    public void Connect(string key, string connectionString)
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        _connections.Add(key, connection);
    }
}