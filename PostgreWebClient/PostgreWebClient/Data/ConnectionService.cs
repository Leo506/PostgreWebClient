using Npgsql;
using PostgreWebClient.Abstractions;

namespace PostgreWebClient.Data;

public class ConnectionService : IConnectionService
{
    private Dictionary<string, NpgsqlConnection>? _connections;
    
    public void Connect(string key, string connectionString)
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        _connections ??= new Dictionary<string, NpgsqlConnection>();

        _connections.Add(key, connection);
    }
}