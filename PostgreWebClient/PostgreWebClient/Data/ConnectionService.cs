using Npgsql;
using PostgreWebClient.Abstractions;

namespace PostgreWebClient.Data;

public class ConnectionService : IConnectionService
{
    private NpgsqlConnection? _connection;
    public void Connect(string connectionString)
    {
        _connection = new NpgsqlConnection(connectionString);
        _connection.Open();
    }
}