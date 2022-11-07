using System.Data;
using PostgreWebClient.Models;

namespace PostgreWebClient.Abstractions;

public interface ICommandExecutor
{
    Table Execute(string query, IDbConnection connection);
}