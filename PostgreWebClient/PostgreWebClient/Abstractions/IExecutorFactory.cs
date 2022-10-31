using System.Data;

namespace PostgreWebClient.Abstractions;

public interface IExecutorFactory
{
    ICommandExecutor GetExecutor(string query, IDbConnection connection);
}