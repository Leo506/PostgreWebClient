using System.Data;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Executors;

namespace PostgreWebClient.Factories;

public class NpgsqlExecutorFactory : IExecutorFactory
{
    public ICommandExecutor GetExecutor(string query, IDbConnection connection)
    {
        return new NpgsqlCommandExecutor(query,
            connection as NpgsqlConnection ?? throw new InvalidOperationException());
    }
}