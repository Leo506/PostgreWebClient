using System.Data;

namespace PostgreWebClient.Abstractions;

public interface IConnectionMaker
{
    IDbConnection MakeConnection(string connectionString);
}