using System.Data;
using Npgsql;
using PostgreWebClient.Abstractions;
using PostgreWebClient.Executors;

namespace PostgreWebClient.Factories;

public static class NpgsqlExecutorFactory
{ public static ICommandExecutor? Executor { get; set; } = null;

    public static ICommandExecutor GetExecutor(string query, IDbConnection connection)
    {
        return Executor ?? new NpgsqlCommandExecutor(query,
            connection as NpgsqlConnection ?? throw new InvalidOperationException());
    }
}