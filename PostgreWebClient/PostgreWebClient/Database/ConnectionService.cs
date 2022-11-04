using Calabonga.OperationResults;
using Npgsql;
using PostgreWebClient.Abstractions;

namespace PostgreWebClient.Database;

public class ConnectionService : IConnectionService
{
    private IConnectionMaker _connectionMaker;
    private Dictionary<string, NpgsqlConnection> _connections;

    Dictionary<string, NpgsqlConnection> IConnectionService.Connections
    {
        get => _connections;
        set => _connections = value;
    }

    public ConnectionService(IConnectionMaker connectionMaker)
    {
        _connectionMaker = connectionMaker;
        _connections = new Dictionary<string, NpgsqlConnection>();
    }

    public OperationResult<bool> Connect(string key, string connectionString)
    {
        var result = OperationResult.CreateResult<bool>();
        try
        {
            _connections.Add(key, (_connectionMaker.MakeConnection(connectionString) as NpgsqlConnection)!);
        }
        catch (Exception e)
        {
            result.AddError(e);
        }

        return result;
    }
}