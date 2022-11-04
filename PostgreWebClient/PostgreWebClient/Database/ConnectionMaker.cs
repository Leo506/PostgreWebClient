using System.Data;
using Npgsql;
using PostgreWebClient.Abstractions;

namespace PostgreWebClient.Database;

public class ConnectionMaker : IConnectionMaker
{
    public IDbConnection MakeConnection(string connectionString)
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        return connection;
    }
}