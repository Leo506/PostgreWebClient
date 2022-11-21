using Calabonga.OperationResults;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Models;

namespace PostgreWebClient.Database;

public class ConnectionService : IConnectionService
{
    private IConnectionMaker _connectionMaker;
    private ConnectionCollection _connections;

    ConnectionCollection IConnectionService.Connections
    {
        get => _connections;
        set => _connections = value;
    }

    public ConnectionService(IConnectionMaker connectionMaker)
    {
        _connectionMaker = connectionMaker;
        _connections = new ConnectionCollection();
    }

    public OperationResult<bool> Connect(string key, string connectionString)
    {
        var result = OperationResult.CreateResult<bool>();
        try
        {
            var connection = _connectionMaker.MakeConnection(connectionString);
            _connections.Add(key, new DbConnectionModel(connection));
        }
        catch (Exception e)
        {
            result.AddError(e);
        }

        return result;
    }
}